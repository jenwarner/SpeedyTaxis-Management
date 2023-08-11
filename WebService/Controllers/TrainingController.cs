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
    public class TrainingController : ApiController
    {
        // GET: api/Training
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Training/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("SaveTraining")]
        // POST api/Training
        public bool SaveTraining(Training training)
        {
            if (training == null)
            {
                return false;
            }

            return TrainingProcessor.ProcessTraining(training);
        }

        [HttpPost]
        [Route("SaveTrainingFromDriverLicenceID/id/{id}")]
        // POST api/Training
        public bool SaveTrainingFromDriverLicenceID(Training training, Driver driver)
        {
            if (training == null)
            {
                return false;
            }

            return TrainingProcessor.ProcessTraining(training, driver);
        }

        [HttpPost]
        [Route("UpdateTraining")]
        // POST api/Training
        public bool UpdateTraining(Training training)
        {
            if (training == null)
            {
                return false;
            }

            return TrainingProcessor.ProcessUpdateTraining(training);
        }

        // PUT: api/Training/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        [Route("DeleteTraining/id/{id}")]
        // DELETE api/Training/5
        public bool DeleteTraining(int id)
        {
            if (id == 0)
            {
                return false;
            }

            return TrainingProcessor.ProcessDeleteTraining(id);
        }
    }
}
