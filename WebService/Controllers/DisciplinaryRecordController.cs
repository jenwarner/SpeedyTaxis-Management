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
    public class DisciplinaryRecordController : ApiController
    {
        // GET: api/DisciplinaryRecord
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DisciplinaryRecord/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("SaveDisciplinaryRecord")]
        // POST api/DisciplinaryRecord
        public bool SaveDisciplinaryRecord(DisciplinaryRecord disciplinaryRecord)
        {
            if (disciplinaryRecord == null)
            {
                return false;
            }

            return DisciplinaryRecordProcessor.ProcessDisciplinaryRecord(disciplinaryRecord);
        }

        [HttpPost]
        [Route("UpdateDisciplinaryRecord")]
        // POST api/DisciplinaryRecord
        public bool UpdateDisciplinaryRecord(DisciplinaryRecord disciplinaryRecord)
        {
            if (disciplinaryRecord == null)
            {
                return false;
            }

            return DisciplinaryRecordProcessor.ProcessUpdateDisciplinaryRecord(disciplinaryRecord);
        }

        // PUT: api/DisciplinaryRecord/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        [Route("DeleteDisciplinaryRecord/id/{id}")]
        // DELETE api/DisciplinaryRecord/5
        public bool DeleteDisciplinaryRecord(int id)
        {
            if (id == 0)
            {
                return false;
            }

            return DisciplinaryRecordProcessor.ProcessDeleteDisciplinaryRecord(id);
        }
    }
}
