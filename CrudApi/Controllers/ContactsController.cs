using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.Specifications;

namespace CrudApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private IDataContextFactory _dataContextFactory;

        public ContactsController(IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
        }

        [HttpPost]
        public IActionResult Add(ContactEntryModel contact)
        {
            if (contact.IsValid())
            {
                var entity = contact.ToEntity();

                using (var context = _dataContextFactory.CreateDataContext())
                {
                    context.Add(entity);
                    context.SaveChanges();

                    return StatusCode(201, new ContactResponseModel(entity));
                }
            }

            return StatusCode(400);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Edit(int id, ContactEntryModel contact)
        {
            if (contact.IsValid())
            {
                using (var context = _dataContextFactory.CreateDataContext())
                {
                    var entity = context.FindSingle(ContactSpecs.Get(id));
                    if (entity != null)
                    {
                        contact.UpdateEntity(entity);

                        context.Update(entity);
                        context.SaveChanges();

                        return StatusCode(202);
                    }
                }

                return StatusCode(204);
            }

            return StatusCode(400);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            using (var context = _dataContextFactory.CreateDataContext())
            {
                var entity = context.FindSingle(ContactSpecs.Get(id));
                if (entity != null)
                {
                    context.Delete(entity);
                    context.SaveChanges();

                    return StatusCode(202);
                }
            }

            return StatusCode(204);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            using (var context = _dataContextFactory.CreateDataContext())
            {
                var entity = context.FindSingle(ContactSpecs.Get(id));
                if (entity != null)
                {

                    return StatusCode(200, ContactModel.ToContact(entity));
                }
            }

            return StatusCode(204);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            using (var context = _dataContextFactory.CreateDataContext())
            {
                var entities = context.Find(ContactSpecs.GetAll());
                return StatusCode(200, ContactModel.ToContacts(entities));
            }
        }

        [HttpGet]
        [Route("call-list")]
        public IActionResult GetCallList()
        {
            using (var context = _dataContextFactory.CreateDataContext())
            {
                var entities = context.Find(ContactSpecs.GetCallList());
                return StatusCode(200, CallContactModel.ToCallList(entities)
                    .OrderBy(c => c.Name.Last)
                    .ThenBy(c => c.Name.First));
            }
        }
    }
}