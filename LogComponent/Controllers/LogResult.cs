using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogComponent.Controllers
{
    public class LogResult
    {
        public double AmountOfHoursWorked { get; set; }
        public double LengthOfJourney { get; set; }
        //public string DestinationCurrency { get; set; }
        public double JourneyResult { get; set; }
        public double DayResult { get; set; }
        public DateTime TimeOfConversion { get; set; }
        public DateTime StartOfDay { get; set; }
        public DateTime EndOfDay { get; set; }
        public DateTime StartOfJourney { get; set; }
        public DateTime EndOfJourney { get; set; }

    }
}