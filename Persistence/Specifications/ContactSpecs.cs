using System;
using System.Linq;
using System.Linq.Expressions;
using CrudDatastore;

namespace Persistence.Specifications
{
    public class ContactSpecs : Specification<Entities.Contact>
    {
        private ContactSpecs(Expression<Func<Entities.Contact, bool>> predicate)
            : base(predicate)
        { }

        public static ContactSpecs Get(int id)
        {
            return new ContactSpecs(e => e.ContactId == id);
        }

        public static ContactSpecs GetCallList()
        {
            return new ContactSpecs(e => e.Phones != null && e.Phones.Any(p => p.Type == "home"));
        }

        public static ContactSpecs GetAll()
        {
            return new ContactSpecs(e => true);
        }
    }
}

