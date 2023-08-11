using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService.Models;

namespace WebService.Controllers
{
    public class LogDataController : ApiController
    {
        [Route("LogData/{startOfJourney}/{endOfJourney}/{startOfDay}/{endOfDay}")]
        // GET api/values/09:00/10:00/09:00/18:00
        public LogData GetResults(DateTime startOfJourney, DateTime endOfJourney, DateTime startOfDay, DateTime endOfDay)
        {
            if(startOfJourney == null || endOfJourney == null)
            {
                return null;
            }
            if(startOfDay == null || endOfDay == null)
            {
                return null;
            }

            string lengthOfJourney, lengthOfDay;

            lengthOfJourney = endOfJourney.Subtract(startOfJourney).TotalMinutes + " minutes.";

            lengthOfDay = endOfDay.Subtract(startOfDay).TotalHours + " hours.";

            return new LogData { StartOfJourney = "Start of selected journey: " + startOfJourney.ToLongTimeString(), EndOfJourney = "End of selected journey: " + endOfJourney.ToLongTimeString(), LengthOfJourney = "Length of selected journey: " + lengthOfJourney, StartOfDay = "Start of working day: " + startOfDay.ToLongTimeString(), EndOfDay = "End of working day: " + endOfDay.ToLongTimeString(), LengthOfDay = "Length of working day: " + lengthOfDay, AmountOfHoursWorked = "Driver worked for " + lengthOfDay, TimeOfLogCalculation = DateTime.Now };
        }

        // GET: api/LogData/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LogData
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LogData/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LogData/5
        public void Delete(int id)
        {
        }
    }
}
