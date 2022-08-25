using System;
using CrudDatastore;

namespace Persistence
{
    internal class DataContext : DataContextBase
    {
        public DataContext(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }
    }
}

