using ErrorReportSMS.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorReportSMS.Core.Repositories
{
    public class ErrorRepository
    {
        public string GetLastErrors(int IntervalInMinutes)
        {
            //pobieranie z bazy danych

            return "Błąd testowy 1";
        }
    }
}
