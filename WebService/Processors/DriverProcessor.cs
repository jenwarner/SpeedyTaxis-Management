using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using WebService.Repositories;

namespace WebService.Processors
{
    public class DriverProcessor
    {
        public static bool ProcessDriver(Driver driver)
        {
            // processing / validating / formatting
            return DriverRepository.AddDriverToDB(driver);
        }

        public static bool ProcessDeleteDriver(int id)
        {
            return DriverRepository.DeleteDriverFromDBWithID(id);
        }

        public static bool ProcessUpdateDriver(Driver driver)
        {
            // processing / validating / formatting
            return DriverRepository.UpdateDriverToDB(driver);
        }
        public static bool ProcessGetIDWithDriverName(Driver driver)
        {
            // processing / validating / formatting
            return DriverRepository.GetDriverIDFromDBWithName(driver);
        }
    }
}