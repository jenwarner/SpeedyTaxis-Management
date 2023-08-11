using SpeedyTaxis.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService.Models;

namespace SpeedyTaxis.Processors
{
    class TrainingManagerProcessor
    {
        public static async Task<bool> ProcessTraining(Training training)
        {
            return await TrainingManagerRepository.SaveTraining(training);
        }
        public static async Task<bool> ProcessDeleteTraining(int id)
        {
            return await TrainingManagerRepository.DeleteTraining(id);
        }
        public static async Task<bool> ProcessUpdateTraining(Training training)
        {
            return await TrainingManagerRepository.UpdateTraining(training);
        }

        public static async Task<bool> ProcessTrainingFromDriverLicenceID(Training training, Driver driver)
        {
            return await TrainingManagerRepository.SaveTrainingFromDriverLicenceID(training, driver);
        }
    }
}
