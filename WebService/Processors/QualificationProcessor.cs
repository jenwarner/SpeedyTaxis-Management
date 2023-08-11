using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using WebService.Repositories;

namespace WebService.Processors
{
    public class QualificationProcessor
    {
        public static bool ProcessQualification(Qualification qualification)
        {
            // processing / validating / formatting
            return QualificationRepository.AddQualificationToDB(qualification);

        }
        public static bool ProcessDeleteQualification(int id)
        {
            return QualificationRepository.DeleteQualificationFromDBWithID(id);
        }
        public static bool ProcessUpdateQualification(Qualification qualification)
        {
            // processing / validating / formatting
            return QualificationRepository.UpdateQualificationToDB(qualification);
        }
    }
}