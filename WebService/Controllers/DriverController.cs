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
    public class DriverController : ApiController
    {
        [HttpGet]
        [Route("GetDriverIDWithName/{firstName}/{surname}")]
        // GET api/values
        public bool GetDriverIDFromName(Driver driver)
        {
            if (driver == null)
            {
                return false;
            }
            return DriverProcessor.ProcessGetIDWithDriverName(driver);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("SaveDriver")]
        // POST api/values
        public bool SaveDriver(Driver driver)
        {
            if (driver == null)
            {
                return false;
            }

            return DriverProcessor.ProcessDriver(driver);
        }

        [HttpPost]
        [Route("UpdateDriver")]
        // POST api/values
        public bool UpdateDriver(Driver driver)
        {
            if (driver == null)
            {
                return false;
            }

            return DriverProcessor.ProcessUpdateDriver(driver);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        [Route("DeleteDriver/id/{id}")]
        // DELETE api/values/5
        public bool DeleteDriver(int id)
        {
            if (id == 0)
            {
                return false;
            }

            return DriverProcessor.ProcessDeleteDriver(id);
        }
    }
}
