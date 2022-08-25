using System;
using Persistence.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CrudApi.Models
{
    public class CallContactModel
    {
        public NameModel Name { get; set; }
        public string Phone { get; set; }

        private static CallContactModel ToCallItem(Contact entity)
        {
            var callItem = new CallContactModel();

            callItem.Name = new NameModel();
            if (entity.Name != null)
            {
                callItem.Name.First = entity.Name.First;
                callItem.Name.Middle = entity.Name.Middle;
                callItem.Name.Last = entity.Name.Last;
            }


            if (entity.Phones != null && entity.Phones.Any())
            {
                var homeNumber = entity.Phones.FirstOrDefault(p => p.Type == "home");
                if (homeNumber != null)
                {
                    callItem.Phone = homeNumber.Number;
                }
            }

            return callItem;
        }

        public static IEnumerable<CallContactModel> ToCallList(IEnumerable<Contact> entities)
        {
            return entities
                .Select(c => ToCallItem(c))
                .ToList();
        }
    }
}

