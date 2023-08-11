using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService.Models;
using WebService.Processors;

namespace WebService.Controllers
{
    public class QualificationController : ApiController
    {
        // GET: api/Qualification
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Qualification/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("SaveQualification")]
        // POST api/Qualification
        public bool SaveQualification(Qualification qualification)
        {
            if (qualification == null)
            {
                return false;
            }

            return QualificationProcessor.ProcessQualification(qualification);
        }

        [HttpPost]
        [Route("UpdateQualification")]
        // POST api/Qualification
        public bool UpdateQualification(Qualification qualification)
        {
            if (qualification == null)
            {
                return false;
            }

            return QualificationProcessor.ProcessUpdateQualification(qualification);
        }

        // PUT: api/Qualification/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        [Route("DeleteQualification/id/{id}")]
        // DELETE api/values/5
        public bool DeleteQualification(int id)
        {
            if (id == 0)
            {
                return false;
            }

            return QualificationProcessor.ProcessDeleteQualification(id);
        }
    }
}
