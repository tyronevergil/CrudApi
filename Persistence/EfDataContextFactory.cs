using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CrudDatastore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;

namespace Persistence
{
    public class EfDataContextFactory : IDataContextFactory
    {
        private string _connectionString;

        public EfDataContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataContextBase CreateDataContext()
        {
            return new DataContext(new EfUnitOfWork(_connectionString));
        }
    }

    internal class EfUnitOfWork : DbContext, IUnitOfWork
    {
        private string _connectionString;
        private readonly IDictionary<Type, object> _dataQueries = new Dictionary<Type, object>();

        public event EventHandler<EntityEventArgs> EntityMaterialized;
        public event EventHandler<EntityEventArgs> EntityCreate;
        public event EventHandler<EntityEventArgs> EntityUpdate;
        public event EventHandler<EntityEventArgs> EntityDelete;

        public EfUnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Contact>()
                .HasKey(c => c.ContactId);

            modelBuilder.Entity<Entities.Contact>()
                .HasOne(c => c.Name)
                .WithOne()
                .HasForeignKey<Entities.Name>(n => n.ContactId);

            modelBuilder.Entity<Entities.Contact>()
                .HasOne(c => c.Address)
                .WithOne()
                .HasForeignKey<Entities.Address>(a => a.ContactId);

            modelBuilder.Entity<Entities.Contact>()
                .HasMany(c => c.Phones)
                .WithOne()
                .HasForeignKey(p => p.ContactId);

            modelBuilder.Entity<Entities.Name>()
                .HasKey(n => n.NameId);

            modelBuilder.Entity<Entities.Address>()
                .HasKey(a => a.AddressId);

            modelBuilder.Entity<Entities.Phone>()
                .HasKey(p => p.PhoneId);

            modelBuilder.Entity<Entities.Contact>()
                .ToTable("Contacts");

            modelBuilder.Entity<Entities.Name>()
                .ToTable("Names");

            modelBuilder.Entity<Entities.Address>()
                .ToTable("Addresses");

            modelBuilder.Entity<Entities.Phone>()
                .ToTable("Phones");

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(_connectionString);

            optionsBuilder.ReplaceService<IEntityMaterializerSource, UnitOfWorkEntityMaterializerSource>();
        }

        public void Execute(string command, params object[] parameters)
        {
            this.Database.ExecuteSqlRaw(command, parameters);
        }

        public IDataQuery<T> Read<T>() where T : EntityBase
        {
            if (!_dataQueries.Any())
            {
                var entityMaterializerSource = this.GetService<IEntityMaterializerSource>() as UnitOfWorkEntityMaterializerSource;
                if (entityMaterializerSource != null)
                {
                    entityMaterializerSource.SetObjectMaterialize(
                        (entity) =>
                        {
                            EntityMaterialized?.Invoke(this, new EntityEventArgs(entity));
                            return entity;
                        });
                }
            }

            var entityType = typeof(T);
            if (_dataQueries.ContainsKey(entityType))
                return (IDataQuery<T>)_dataQueries[entityType];

            var dataQuery = new DataQuery<T>(new DbSetQueryAdapter<T>(this));
            _dataQueries.Add(entityType, dataQuery);

            return dataQuery;
        }

        public void MarkNew<T>(T entity) where T : EntityBase
        {
            Set<T>().Add(entity);
        }

        public void MarkModified<T>(T entity) where T : EntityBase
        {
            var entry = Entry(entity);
            if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                entry.State = EntityState.Modified;
        }

        public void MarkDeleted<T>(T entity) where T : EntityBase
        {
            var entry = Entry(entity);
            if (entry.State == EntityState.Detached)
                Set<T>().Attach(entity);

            Set<T>().Remove(entity);
        }

        public void Commit()
        {
            ChangeTracker.DetectChanges();

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        EntityCreate?.Invoke(this, new EntityEventArgs(entry.Entity));
                        break;
                    case EntityState.Modified:
                        EntityUpdate?.Invoke(this, new EntityEventArgs(entry.Entity));
                        break;
                    case EntityState.Deleted:
                        EntityDelete?.Invoke(this, new EntityEventArgs(entry.Entity));
                        break;
                }
            }

            SaveChanges();
        }
    }

    internal class UnitOfWorkEntityMaterializerSource : EntityMaterializerSource
    {
        private Func<object, object> _materializeObject;

        public UnitOfWorkEntityMaterializerSource(EntityMaterializerSourceDependencies dependencies)
            : base(dependencies)
        { }

        public void SetObjectMaterialize(Func<object, object> materializeObject)
        {
            _materializeObject = materializeObject;
        }

        public override Expression CreateMaterializeExpression(IEntityType entityType, string entityInstanceName, Expression materializationContextExpression)
        {
            var baseExpression = base.CreateMaterializeExpression(entityType, entityInstanceName, materializationContextExpression);
            if (typeof(EntityBase).IsAssignableFrom(entityType.ClrType))
            {
                var blockExpressions = new List<Expression>(((BlockExpression)baseExpression).Expressions);
                var instanceVariable = blockExpressions.Last() as ParameterExpression;

                var materializeObjectExpression = _materializeObject.Target == null ?
                    Expression.Call(_materializeObject.Method, instanceVariable) :
                    Expression.Call(Expression.Constant(_materializeObject.Target), _materializeObject.Method, instanceVariable);

                blockExpressions.RemoveAt(blockExpressions.Count - 1);
                blockExpressions.Add(materializeObjectExpression);

                return Expression.Block(new[] { instanceVariable }, blockExpressions);
            }

            return baseExpression;
        }
    }

    internal class DbSetQueryAdapter<T> : DelegateQueryAdapter<T> where T : EntityBase
    {
        public DbSetQueryAdapter(DbContext dbContext)
            : base
            (
                /* readExpressionTrigger */
                (predicate) =>
                {
                    return dbContext.Set<T>().Where(predicate)
                        .ToList().AsQueryable();
                },

                /* readCommandTrigger */
                (command, parameters) =>
                {
                    return dbContext.Set<T>().FromSqlRaw(command, parameters)
                        .ToList().AsQueryable();
                }
            )
        { }
    }
}

