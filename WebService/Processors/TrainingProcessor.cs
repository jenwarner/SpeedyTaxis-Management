using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using WebService.Repositories;

namespace WebService.Processors
{
    public class TrainingProcessor
    {
        public static bool ProcessTraining(Training training, Driver driver)
        {
            // processing / validating / formatting
            return TrainingRepository.AddTrainingToDBFromDriverLicenceID(training, driver);

        }

        public static bool ProcessTraining(Training training)
        {
            // processing / validating / formatting
            return TrainingRepository.AddTrainingToDB(training);
        }
        public static bool ProcessDeleteTraining(int id)
        {
            return TrainingRepository.DeleteTrainingFromDBWithID(id);
        }
        public static bool ProcessUpdateTraining(Training training)
        {
            // processing / validating / formatting
            return TrainingRepository.UpdateTrainingToDB(training);
        }
    }
}