using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebService.Models;
using WebService.Repositories;

namespace WebService.Processors
{
    public class DisciplinaryRecordProcessor
    {
        public static bool ProcessDisciplinaryRecord(DisciplinaryRecord disciplinaryRecord)
        {
            // processing / validating / formatting
            return DisciplinaryRecordRepository.AddDisciplinaryRecordToDB(disciplinaryRecord);

        }
        public static bool ProcessDeleteDisciplinaryRecord(int id)
        {
            return DisciplinaryRecordRepository.DeleteDisciplinaryRecordFromDBWithID(id);
        }
        public static bool ProcessUpdateDisciplinaryRecord(DisciplinaryRecord disciplinaryRecord)
        {
            // processing / validating / formatting
            return DisciplinaryRecordRepository.UpdateDisciplinaryRecordToDB(disciplinaryRecord);
        }
    }
}