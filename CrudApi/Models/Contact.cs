using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Persistence.Entities;

namespace CrudApi.Models
{
    public class ContactEntryModel
    {
        private static IEnumerable<string> _validPhoneTypes = new [] { "home", "work", "mobile" };

        public NameModel Name { get; set; }
        public AddressModel Address { get; set; }
        public IEnumerable<PhoneModel> Phone { get; set; }
        public string Email { get; set; }

        public bool IsValid()
        {
            if (this.Phone != null)
            {
                return this.Phone.All(p => _validPhoneTypes.Contains(p.Type));
            }

            return true;
        }

        public Contact ToEntity()
        {
            var entity = new Contact
            {
                Email = this.Email
            };

            if (this.Name != null)
            {
                entity.Name = new Name
                {
                    First = this.Name.First,
                    Middle = this.Name.Middle,
                    Last = this.Name.Last
                };
            }

            if (this.Address != null)
            {
                entity.Address = new Address
                {
                    Street = this.Address.Street,
                    City = this.Address.City,
                    State = this.Address.State,
                    Zip = this.Address.Zip
                };
            }

            if (this.Phone != null && this.Phone.Any())
            {
                entity.Phones = this.Phone
                    .Select(p => new Phone
                        {
                            Number = p.Number,
                            Type = p.Type
                        })
                    .ToList();
            }
            else
            {
                entity.Phones = new List<Phone>();
            }

            return entity;
        }

        public void UpdateEntity(Contact entity)
        {
            entity.Email = this.Email;

            if (this.Name != null)
            {
                entity.Name = entity.Name ?? new Name();
                entity.Name.First = this.Name.First;
                entity.Name.Middle = this.Name.Middle;
                entity.Name.Last = this.Name.Last;
            }
            else
            {
                if (entity.Name != null)
                {
                    entity.Name.First = null;
                    entity.Name.Middle = null;
                    entity.Name.Last = null;
                }
            }

            if (this.Address != null)
            {
                entity.Address = entity.Address ?? new Address();
                entity.Address.Street = this.Address.Street;
                entity.Address.City = this.Address.City;
                entity.Address.State = this.Address.State;
                entity.Address.Zip = this.Address.Zip;
            }
            else
            {
                if (entity.Address != null)
                {
                    entity.Address.Street = null;
                    entity.Address.City = null;
                    entity.Address.State = null;
                    entity.Address.Zip = null;
                }
            }

            if (this.Phone != null && this.Phone.Any())
            {
                if (entity.Phones != null)
                {
                    foreach (var delPhone in entity.Phones.ToList())
                    {
                        entity.Phones.Remove(delPhone);
                    }

                    foreach(var addPhone in this.Phone)
                    {
                        entity.Phones.Add(new Phone 
                        {
                            Number = addPhone.Number,
                            Type = addPhone.Type
                        });
                    }
                }
                else
                {
                    entity.Phones = this.Phone
                        .Select(p => new Phone
                        {
                            Number = p.Number,
                            Type = p.Type
                        })
                        .ToList();
                }
            }
            else
            {
                entity.Phones = new List<Phone>();
            }
        }
    }

    public class ContactModel : ContactEntryModel
    {
        public int Id { get; set; }

        public static ContactModel ToContact(Contact entity)
        {
            var contact = new ContactModel
            {
                Id = entity.ContactId,
                Email = entity.Email
            };

            contact.Name = new NameModel();
            if (entity.Name != null)
            {
                contact.Name.First = entity.Name.First;
                contact.Name.Middle = entity.Name.Middle;
                contact.Name.Last = entity.Name.Last;
            }

            contact.Address = new AddressModel();
            if (entity.Address != null)
            {
                contact.Address.Street = entity.Address.Street;
                contact.Address.City = entity.Address.City;
                contact.Address.State = entity.Address.State;
                contact.Address.Zip = entity.Address.Zip;
            }

            if (entity.Phones != null && entity.Phones.Any())
            {
                contact.Phone = entity.Phones
                    .Select(p => new PhoneModel
                    {
                        Number = p.Number,
                        Type = p.Type
                    })
                    .ToList();
            }
            else
            {
                contact.Phone = Enumerable.Empty<PhoneModel>();
            }

            return contact;
        }

        public static IEnumerable<ContactModel> ToContacts(IEnumerable<Contact> entities)
        {
            return entities
                .Select(c => ToContact(c))
                .ToList();
        }
    }

    public class ContactResponseModel
    {
        public ContactResponseModel(Contact entity)
        {
            Id = entity.ContactId;
        }

        public int Id { get; set; }
    }
}

