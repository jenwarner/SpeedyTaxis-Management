using SpeedyTaxis.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService.Models;

namespace SpeedyTaxis.Processors
{
    class DriverManagerProcessor
    {
        public static async Task<bool> ProcessDriver(Driver driver)
        {
            return await DriverManagerRepository.SaveDriver(driver);
        }

        public static async Task<bool> ProcessDeleteDriver(int id)
        {
            return await DriverManagerRepository.DeleteDriver(id);
        }

        public static async Task<bool> ProcessUpdateDriver(Driver driver)
        {
            return await DriverManagerRepository.UpdateDriver(driver);
        }

        public static async Task<int> ProcessGetDriverIDWithName(Driver driver)
        {
            return await DriverManagerRepository.GetDriverIDWithName(driver);
        }
    }
}
