using System;
using CrudDatastore;

namespace Persistence.Entities
{
    public class Phone : EntityBase
    {
        public int PhoneId { get; set; }
        public int ContactId { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
    }
}

