using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public class LogData
    {
        public string StartOfJourney { get; set; }
        public string EndOfJourney { get; set; }
        public string LengthOfJourney { get; set; }
        //public string DestinationCurrency { get; set; }
        //public double JourneyResult { get; set; }
        //public double DayResult { get; set; }
        public string StartOfDay { get; set; }
        public string EndOfDay { get; set; }
        public string LengthOfDay { get; set; }
        public string AmountOfHoursWorked { get; set; }
        public DateTime TimeOfLogCalculation { get; set; }
    }
}