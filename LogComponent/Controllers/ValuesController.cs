using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LogComponent.Controllers
{
    public class LogController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        //[RoutePrefix{"Convert/{startOfJourney}/{endOfJourney}/{startOfDay}/{endOfDay}"]
        // GET api/values/5
        public LogResult GetResults(DateTime startOfJourney, DateTime endOfJourney, DateTime startOfDay, DateTime endOfDay)
        {
            double journeyResult, dayResult;
            journeyResult = endOfJourney.Subtract(startOfJourney).TotalHours;

            dayResult = endOfDay.Subtract(startOfDay).TotalHours;

            return new LogResult { JourneyResult = journeyResult, DayResult = dayResult };
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        
    }
}