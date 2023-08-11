using SpeedyTaxis.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService.Models;

namespace SpeedyTaxis.Processors
{
    class QualificationManagerProcessor
    {
        public static async Task<bool> ProcessQualification(Qualification qualification)
        {
            return await QualificationManagerRepository.SaveQualification(qualification);
        }
        public static async Task<bool> ProcessDeleteQualification(int id)
        {
            return await QualificationManagerRepository.DeleteQualification(id);
        }
        public static async Task<bool> ProcessUpdateQualification(Qualification qualification)
        {
            return await QualificationManagerRepository.UpdateQualification(qualification);
        }
    }
}
