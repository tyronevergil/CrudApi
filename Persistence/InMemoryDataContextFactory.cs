using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CrudDatastore;
using Persistence.Entities;

namespace Persistence
{
    public class InMemoryDataList
    {
        public IList<Entities.Contact> Contacts = new List<Entities.Contact>();
        public IList<Entities.Name> Names = new List<Entities.Name>();
        public IList<Entities.Address> Addresses = new List<Entities.Address>();
        public IList<Entities.Phone> Phones = new List<Entities.Phone>();
    }

    public class InMemoryDataContextFactory : IDataContextFactory
    {
        private InMemoryDataList _dataList;

        public InMemoryDataContextFactory(InMemoryDataList dataList)
        {
            _dataList = dataList;
        }

        public DataContextBase CreateDataContext()
        {
            return new DataContext(new InMemoryUnitOfWork(_dataList));
        }
    }

    internal class InMemoryUnitOfWork : UnitOfWorkBase
    {
        private IDataStore<Entities.Contact> Contacts(IList<Entities.Contact> contacts)
        {
            return new DataStore<Entities.Contact>(
                new DelegateCrudAdapter<Entities.Contact>(
                    /* create */
                    (e) =>
                    {
                        e.ContactId = (contacts.Any() ? contacts.Max(i => i.ContactId) : 0) + 1;
                        contacts.Add(new Entities.Contact
                        {
                            ContactId = e.ContactId,
                            Email = e.Email
                        });
                    },

                    /* update */
                    (e) =>
                    {
                        var contact = contacts.FirstOrDefault(i => i.ContactId == e.ContactId);
                        if (contact != null)
                        {
                            contact.Email = e.Email;
                        }
                    },

                    /* delete */
                    (e) =>
                    {
                        var contact = contacts.FirstOrDefault(i => i.ContactId == e.ContactId);
                        if (contact != null)
                        {
                            contacts.Remove(contact);
                        }
                    },

                    /* read */
                    (predicate) =>
                    {
                        // expensive operation
                        return contacts
                            .Select(c => (Entities.Contact)((IDataNavigation)this).ResolveNavigationProperties(c))
                            .Where(predicate.Compile())
                            .AsQueryable();
                    }
                ));
        }

        private IDataStore<Entities.Name> Names(IList<Entities.Name> names)
        {
            return new DataStore<Entities.Name>(
                new DelegateCrudAdapter<Entities.Name>(
                    /* create */
                    (e) =>
                    {
                        e.NameId = (names.Any() ? names.Max(i => i.NameId) : 0) + 1;
                        names.Add(new Entities.Name
                        {
                            NameId = e.NameId,
                            ContactId = e.ContactId,
                            First = e.First,
                            Middle = e.Middle,
                            Last = e.Last
                        });
                    },

                    /* update */
                    (e) =>
                    {
                        var name = names.FirstOrDefault(i => i.NameId == e.NameId);
                        if (name != null)
                        {
                            name.First = e.First;
                            name.Middle = e.Middle;
                            name.Last = e.Last;
                        }
                    },

                    /* delete */
                    (e) =>
                    {
                        var name = names.FirstOrDefault(i => i.NameId == e.NameId);
                        if (name != null)
                        {
                            names.Remove(name);
                        }
                    },

                    /* read */
                    (predicate) =>
                    {
                        return names.Where(predicate.Compile()).AsQueryable();
                    }
                ));
        }

        private IDataStore<Entities.Address> Addresses(IList<Entities.Address> addresses)
        {
            return new DataStore<Entities.Address>(
                new DelegateCrudAdapter<Entities.Address>(
                    /* create */
                    (e) =>
                    {
                        e.AddressId = (addresses.Any() ? addresses.Max(i => i.AddressId) : 0) + 1;
                        addresses.Add(new Entities.Address
                        {
                            AddressId = e.AddressId,
                            ContactId = e.ContactId,
                            Street = e.Street,
                            City = e.City,
                            State = e.State,
                            Zip = e.Zip
                        });
                    },

                    /* update */
                    (e) =>
                    {
                        var address = addresses.FirstOrDefault(i => i.AddressId == e.AddressId);
                        if (address != null)
                        {
                            address.Street = e.Street;
                            address.City = e.City;
                            address.State = e.State;
                            address.Zip = e.Zip;
                        }
                    },

                    /* delete */
                    (e) =>
                    {
                        var address = addresses.FirstOrDefault(i => i.AddressId == e.AddressId);
                        if (address != null)
                        {
                            addresses.Remove(address);
                        }
                    },

                    /* read */
                    (predicate) =>
                    {
                        return addresses.Where(predicate.Compile()).AsQueryable();
                    }
                ));
        }

        private IDataStore<Entities.Phone> Phones(IList<Entities.Phone> phones)
        {
            return new DataStore<Entities.Phone>(
                new DelegateCrudAdapter<Entities.Phone>(
                    /* create */
                    (e) =>
                    {
                        e.PhoneId = (phones.Any() ? phones.Max(i => i.PhoneId) : 0) + 1;
                        phones.Add(new Entities.Phone
                        {
                            PhoneId = e.PhoneId,
                            ContactId = e.ContactId,
                            Number = e.Number,
                            Type = e.Type
                        });
                    },

                    /* update */
                    (e) =>
                    {
                        var phone = phones.FirstOrDefault(i => i.PhoneId == e.PhoneId);
                        if (phone != null)
                        {
                            phone.Number = e.Number;
                            phone.Type = e.Type;
                        }
                    },

                    /* delete */
                    (e) =>
                    {
                        var phone = phones.FirstOrDefault(i => i.PhoneId == e.PhoneId);
                        if (phone != null)
                        {
                            phones.Remove(phone);
                        }
                    },

                    /* read */
                    (predicate) =>
                    {
                        return phones.Where(predicate.Compile()).AsQueryable();
                    }
                ));
        }

        public InMemoryUnitOfWork(InMemoryDataList dataList)
        {
            this.Register(Contacts(dataList.Contacts))
                .Map(c => c.Name, (c, n) => c.ContactId == n.ContactId)
                .Map(c => c.Address, (c, a) => c.ContactId == a.ContactId)
                .Map(c => c.Phones, (c, p) => c.ContactId == p.ContactId);
            this.Register(Names(dataList.Names));
            this.Register(Addresses(dataList.Addresses));
            this.Register(Phones(dataList.Phones));
        }
    }
}

