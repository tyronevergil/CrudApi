using System.Collections.Generic;
using System.Linq;
using CrudApi.Controllers;
using CrudApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;
using Persistence.Entities;

namespace CrudApi.Tests
{
    [TestClass]
    public class ContactsControllerTests
    {
        [TestMethod]
        public void GIVEN_New_Valid_Contact_Information_WHEN_Invoke_Add_Operation_THEN_New_Contact_Information_Is_Recorded()
        {
            //
            var dataList = new InMemoryDataList();
            var dataContextFactory = new InMemoryDataContextFactory(dataList);
            var controller = new ContactsController(dataContextFactory);

            //
            var contact = new ContactEntryModel
            {
                Email = "tyrone.roson@gmail.com",
                Name = new NameModel
                {
                    First = "Tyrone",
                    Last = "Roson"
                },
                Address = new AddressModel
                {
                    Street = "Bernabe St.",
                    City = "Paranaque City",
                    State = "Metro Manila",
                    Zip = "1711"
                },
                Phone = new[]
                {
                    new PhoneModel
                    {
                        Number = "111-111-1111",
                        Type = "mobile"
                    },
                    new PhoneModel
                    {
                        Number = "222-111-1111",
                        Type = "work"
                    }
                }
            };


            // Act
            controller.Add(contact);

            // Assert
            Assert.AreEqual(1, dataList.Contacts.Count);
            Assert.AreEqual(1, dataList.Names.Count);
            Assert.AreEqual(1, dataList.Addresses.Count);
            Assert.AreEqual(2, dataList.Phones.Count);
        }

        [TestMethod]
        public void GIVEN_New_Invalid_Contact_Information_WHEN_Invoke_Add_Operation_THEN_New_Contact_Information_Is_Not_Recorded()
        {
            //
            var dataList = new InMemoryDataList();
            var dataContextFactory = new InMemoryDataContextFactory(dataList);
            var controller = new ContactsController(dataContextFactory);

            //
            var contact = new ContactEntryModel
            {
                Email = "tyrone.roson@gmail.com",
                Name = new NameModel
                {
                    First = "Tyrone",
                    Last = "Roson"
                },
                Address = new AddressModel
                {
                    Street = "Bernabe St.",
                    City = "Paranaque City",
                    State = "Metro Manila",
                    Zip = "1711"
                },
                Phone = new[]
                {
                    new PhoneModel
                    {
                        Number = "111-111-1111",
                        Type = "INVALID"
                    },
                    new PhoneModel
                    {
                        Number = "222-111-1111",
                        Type = "INVALID"
                    }
                }
            };


            // Act
            controller.Add(contact);

            // Assert
            Assert.AreEqual(0, dataList.Contacts.Count);
            Assert.AreEqual(0, dataList.Names.Count);
            Assert.AreEqual(0, dataList.Addresses.Count);
            Assert.AreEqual(0, dataList.Phones.Count);
        }

        [TestMethod]
        public void GIVEN_A_Valid_Contact_Id_And_Information_WHEN_Invoke_Edit_Operation_THEN_The_Contact_Information_Is_Updated()
        {
            //
            var dataList = new InMemoryDataList
            {
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        ContactId = 1,
                        Email = "troy.roson@gmail.com"
                    }
                },
                Names = new List<Name>
                {
                    new Name
                    {
                        ContactId = 1,
                        First = "Troy",
                        Last = "Roson"
                    }
                },
                Addresses = new List<Address>
                {
                    new Address
                    {
                        ContactId = 1,
                        Street = "Delight St.",
                        City = "Paranaque City",
                        State = "Metro Manila",
                        Zip = "1700"
                    }
                },
                Phones = new List<Phone>
                {
                    new Phone
                    {
                        ContactId = 1,
                        Number = "000-000-0000",
                        Type = "home"
                    }
                }
            };

            //
            var dataContextFactory = new InMemoryDataContextFactory(dataList);
            var controller = new ContactsController(dataContextFactory);

            //
            var contact = new ContactEntryModel
            {
                Email = "tyrone.roson@gmail.com",
                Name = new NameModel
                {
                    First = "Tyrone",
                    Last = "Roson"
                },
                Address = new AddressModel
                {
                    Street = "Bernabe St.",
                    City = "Paranaque City",
                    State = "Metro Manila",
                    Zip = "1711"
                },
                Phone = new[]
                {
                    new PhoneModel
                    {
                        Number = "111-111-1111",
                        Type = "mobile"
                    },
                    new PhoneModel
                    {
                        Number = "222-111-1111",
                        Type = "work"
                    }
                }
            };


            // Act
            controller.Edit(1, contact);

            // Assert
            Assert.AreEqual(1, dataList.Contacts.Count);
            Assert.AreEqual("tyrone.roson@gmail.com", dataList.Contacts.First().Email);

            Assert.AreEqual(1, dataList.Names.Count);
            Assert.AreEqual("Tyrone", dataList.Names.First().First);
            Assert.AreEqual("Roson", dataList.Names.First().Last);

            Assert.AreEqual(1, dataList.Addresses.Count);
            Assert.AreEqual("Bernabe St.", dataList.Addresses.First().Street);
            Assert.AreEqual("Paranaque City", dataList.Addresses.First().City);
            Assert.AreEqual("Metro Manila", dataList.Addresses.First().State);
            Assert.AreEqual("1711", dataList.Addresses.First().Zip);

            Assert.AreEqual(2, dataList.Phones.Count);
            Assert.AreEqual(true, dataList.Phones.All(p => p.Type != "home"));
        }

        [TestMethod]
        public void GIVEN_An_Invalid_Contact_Id_WHEN_Invoke_Edit_Operation_THEN_The_Contact_Information_Is_Not_Updated()
        {
            //
            var dataList = new InMemoryDataList
            {
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        ContactId = 1,
                        Email = "troy.roson@gmail.com"
                    }
                },
                Names = new List<Name>
                {
                    new Name
                    {
                        ContactId = 1,
                        First = "Troy",
                        Last = "Roson"
                    }
                },
                Addresses = new List<Address>
                {
                    new Address
                    {
                        ContactId = 1,
                        Street = "Delight St.",
                        City = "Paranaque City",
                        State = "Metro Manila",
                        Zip = "1700"
                    }
                },
                Phones = new List<Phone>
                {
                    new Phone
                    {
                        ContactId = 1,
                        Number = "000-000-0000",
                        Type = "home"
                    }
                }
            };

            //
            var dataContextFactory = new InMemoryDataContextFactory(dataList);
            var controller = new ContactsController(dataContextFactory);

            //
            var contact = new ContactEntryModel
            {
                Email = "tyrone.roson@gmail.com",
                Name = new NameModel
                {
                    First = "Tyrone",
                    Last = "Roson"
                },
                Address = new AddressModel
                {
                    Street = "Bernabe St.",
                    City = "Paranaque City",
                    State = "Metro Manila",
                    Zip = "1711"
                },
                Phone = new[]
                {
                    new PhoneModel
                    {
                        Number = "111-111-1111",
                        Type = "mobile"
                    },
                    new PhoneModel
                    {
                        Number = "222-111-1111",
                        Type = "work"
                    }
                }
            };


            // Act
            controller.Edit(2, contact);

            // Assert
            Assert.AreEqual(1, dataList.Contacts.Count);
            Assert.AreEqual("troy.roson@gmail.com", dataList.Contacts.First().Email);

            Assert.AreEqual(1, dataList.Names.Count);
            Assert.AreEqual("Troy", dataList.Names.First().First);
            Assert.AreEqual("Roson", dataList.Names.First().Last);

            Assert.AreEqual(1, dataList.Addresses.Count);
            Assert.AreEqual("Delight St.", dataList.Addresses.First().Street);
            Assert.AreEqual("Paranaque City", dataList.Addresses.First().City);
            Assert.AreEqual("Metro Manila", dataList.Addresses.First().State);
            Assert.AreEqual("1700", dataList.Addresses.First().Zip);

            Assert.AreEqual(1, dataList.Phones.Count);
            Assert.AreEqual(true, dataList.Phones.All(p => p.Type == "home"));
        }

        [TestMethod]
        public void GIVEN_A_Valid_Contact_Id_WHEN_Invoke_Delete_Operation_THEN_The_Contact_Information_Is_Deleted()
        {
            //
            var dataList = new InMemoryDataList
            {
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        ContactId = 1,
                        Email = "tyrone.roson@gmail.com"
                    }
                }
            };

            //
            var dataContextFactory = new InMemoryDataContextFactory(dataList);
            var controller = new ContactsController(dataContextFactory);

            // Act
            controller.Delete(1);

            // Assert
            Assert.AreEqual(0, dataList.Contacts.Count);
        }

        [TestMethod]
        public void GIVEN_An_Invalid_Contact_Id_WHEN_Invoke_Delete_Operation_THEN_The_Contact_Information_Is_Not_Deleted()
        {
            //
            var dataList = new InMemoryDataList
            {
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        ContactId = 1,
                        Email = "tyrone.roson@gmail.com"
                    }
                }
            };

            //
            var dataContextFactory = new InMemoryDataContextFactory(dataList);
            var controller = new ContactsController(dataContextFactory);

            // Act
            controller.Delete(2);

            // Assert
            Assert.AreEqual(1, dataList.Contacts.Count);
        }

        [TestMethod]
        public void GIVEN_A_Valid_Contact_Id_WHEN_Invoke_Get_Operation_THEN_The_Contact_Information_Is_Provided()
        {
            //
            var dataList = new InMemoryDataList
            {
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        ContactId = 1,
                        Email = "tyrone.roson@gmail.com"
                    }
                },
                Names = new List<Name>
                {
                    new Name
                    {
                        ContactId = 1,
                        First = "Tyrone",
                        Last = "Roson"
                    }
                },
                Addresses = new List<Address>
                {
                    new Address
                    {
                        ContactId = 1,
                        Street = "Bernabe St.",
                        City = "Paranaque City",
                        State = "Metro Manila",
                        Zip = "1711"
                    }
                },
                Phones = new List<Phone>
                {
                    new Phone
                    {
                        ContactId = 1,
                        Number = "000-000-0000",
                        Type = "home"
                    }
                }
            };

            //
            var dataContextFactory = new InMemoryDataContextFactory(dataList);
            var controller = new ContactsController(dataContextFactory);

            // Act
            var response = controller.Get(1);

            // Assert
            var contact = ((ObjectResult)response).Value as ContactModel;

            Assert.IsNotNull(contact);
            Assert.AreEqual("tyrone.roson@gmail.com", contact.Email);
            Assert.AreEqual("Tyrone", contact.Name.First);
            Assert.AreEqual("Roson", contact.Name.Last);
            Assert.AreEqual("Bernabe St.", contact.Address.Street);
            Assert.AreEqual("Paranaque City", contact.Address.City);
            Assert.AreEqual("Metro Manila", contact.Address.State);
            Assert.AreEqual("1711", contact.Address.Zip);

            Assert.AreEqual(1, contact.Phone.Count());
            Assert.AreEqual(true, contact.Phone.All(p => p.Type == "home"));
        }

        [TestMethod]
        public void GIVEN_An_Invalid_Contact_Id_WHEN_Invoke_Get_Operation_THEN_The_Contact_Information_Is_Not_Provided()
        {
            //
            var dataList = new InMemoryDataList
            {
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        ContactId = 1,
                        Email = "tyrone.roson@gmail.com"
                    }
                },
                Names = new List<Name>
                {
                    new Name
                    {
                        ContactId = 1,
                        First = "Tyrone",
                        Last = "Roson"
                    }
                },
                Addresses = new List<Address>
                {
                    new Address
                    {
                        ContactId = 1,
                        Street = "Bernabe St.",
                        City = "Paranaque City",
                        State = "Metro Manila",
                        Zip = "1711"
                    }
                },
                Phones = new List<Phone>
                {
                    new Phone
                    {
                        ContactId = 1,
                        Number = "000-000-0000",
                        Type = "home"
                    }
                }
            };

            //
            var dataContextFactory = new InMemoryDataContextFactory(dataList);
            var controller = new ContactsController(dataContextFactory);

            // Act
            var response = controller.Get(2);

            // Assert
            Assert.AreEqual(204, ((StatusCodeResult)response).StatusCode);
        }

        [TestMethod]
        public void GIVEN_Two_Contact_Records_WHEN_Invoke_GetAll_Operation_THEN_Two_Contact_Information_Are_Provided()
        {
            //
            var dataList = new InMemoryDataList
            {
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        ContactId = 1,
                        Email = "tyrone.roson@gmail.com"
                    },
                    new Contact
                    {
                        ContactId = 2,
                        Email = "troy.roson@gmail.com"
                    }
                },
                Names = new List<Name>
                {
                    new Name
                    {
                        ContactId = 1,
                        First = "Tyrone",
                        Last = "Roson"
                    },
                    new Name
                    {
                        ContactId = 2,
                        First = "Troy",
                        Last = "Roson"
                    }
                },
                Addresses = new List<Address>
                {
                    new Address
                    {
                        ContactId = 1,
                        Street = "Bernabe St.",
                        City = "Paranaque City",
                        State = "Metro Manila",
                        Zip = "1711"
                    },
                    new Address
                    {
                        ContactId = 2,
                        Street = "Delight St.",
                        City = "Paranaque City",
                        State = "Metro Manila",
                        Zip = "1700"
                    }
                },
                Phones = new List<Phone>
                {
                    new Phone
                    {
                        ContactId = 1,
                        Number = "000-000-0000",
                        Type = "home"
                    },
                    new Phone
                    {
                        ContactId = 2,
                        Number = "111-111-1111",
                        Type = "mobile"
                    },
                    new Phone
                    {
                        ContactId = 2,
                        Number = "222-111-1111",
                        Type = "work"
                    }
                }
            };

            //
            var dataContextFactory = new InMemoryDataContextFactory(dataList);
            var controller = new ContactsController(dataContextFactory);

            // Act
            var response = controller.GetAll();

            // Assert
            var contacts = ((ObjectResult)response).Value as IEnumerable<ContactModel>;

            Assert.IsNotNull(contacts);
            Assert.AreEqual(2, contacts.Count());
        }

        [TestMethod]
        public void GIVEN_Two_Contact_Records_And_One_With_Home_Phone_Information_WHEN_Invoke_GetCallList_Operation_THEN_One_CallContact_Information_Is_Provided()
        {
            //
            var dataList = new InMemoryDataList
            {
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        ContactId = 1,
                        Email = "tyrone.roson@gmail.com"
                    },
                    new Contact
                    {
                        ContactId = 2,
                        Email = "troy.roson@gmail.com"
                    }
                },
                Names = new List<Name>
                {
                    new Name
                    {
                        ContactId = 1,
                        First = "Tyrone",
                        Last = "Roson"
                    },
                    new Name
                    {
                        ContactId = 2,
                        First = "Troy",
                        Last = "Roson"
                    }
                },
                Addresses = new List<Address>
                {
                    new Address
                    {
                        ContactId = 1,
                        Street = "Bernabe St.",
                        City = "Paranaque City",
                        State = "Metro Manila",
                        Zip = "1711"
                    },
                    new Address
                    {
                        ContactId = 2,
                        Street = "Delight St.",
                        City = "Paranaque City",
                        State = "Metro Manila",
                        Zip = "1700"
                    }
                },
                Phones = new List<Phone>
                {
                    new Phone
                    {
                        ContactId = 1,
                        Number = "000-000-0000",
                        Type = "home"
                    },
                    new Phone
                    {
                        ContactId = 2,
                        Number = "111-111-1111",
                        Type = "mobile"
                    },
                    new Phone
                    {
                        ContactId = 2,
                        Number = "222-111-1111",
                        Type = "work"
                    }
                }
            };

            //
            var dataContextFactory = new InMemoryDataContextFactory(dataList);
            var controller = new ContactsController(dataContextFactory);

            // Act
            var response = controller.GetCallList();

            // Assert
            var contacts = ((ObjectResult)response).Value as IEnumerable<CallContactModel>;

            Assert.IsNotNull(contacts);
            Assert.AreEqual(1, contacts.Count());
        }
    }
}