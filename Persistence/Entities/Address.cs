using System;
using CrudDatastore;

namespace Persistence.Entities
{
    public class Address : EntityBase
    {
        public int AddressId { get; set; }
        public int ContactId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}

