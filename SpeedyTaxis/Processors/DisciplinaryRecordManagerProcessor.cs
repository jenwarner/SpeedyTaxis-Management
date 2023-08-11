using SpeedyTaxis.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService.Models;

namespace SpeedyTaxis.Processors
{
    class DisciplinaryRecordManagerProcessor
    {
        public static async Task<bool> ProcessDisciplinaryRecord(DisciplinaryRecord disciplinaryRecord)
        {
            return await DisciplinaryRecordManagerRepository.SaveDisciplinaryRecord(disciplinaryRecord);
        }
        public static async Task<bool> ProcessDeleteDisciplinaryRecord(int id)
        {
            return await DisciplinaryRecordManagerRepository.DeleteDisciplinaryRecord(id);
        }
        public static async Task<bool> ProcessUpdateDisciplinaryRecord(DisciplinaryRecord disciplinaryRecord)
        {
            return await DisciplinaryRecordManagerRepository.UpdateDisciplinaryRecord(disciplinaryRecord);
        }
    }
}
