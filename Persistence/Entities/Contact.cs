using System;
using System.Collections.Generic;
using CrudDatastore;

namespace Persistence.Entities
{
    public class Contact : EntityBase
    {
        public int ContactId { get; set; }
        public virtual Name Name { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
        public string Email { get; set; }
    }
}

