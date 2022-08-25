using System;
using CrudDatastore;

namespace Persistence.Entities
{
    public class Name : EntityBase
    {
        public int NameId { get; set; }
        public int ContactId { get; set; }
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
    }
}

