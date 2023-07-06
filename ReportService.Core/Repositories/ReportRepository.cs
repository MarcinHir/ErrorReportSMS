using ReportService.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Core.Repositories
{
    public class ReportRepository
    {
        public Report GetLastNotSentReport()
        {
            //pobieranie z bazy danych
            return new Report
            {
                Id = 1,
                Title = "R/1/2023",
                Date = new DateTime(2023, 1, 1, 12, 00, 00),
                Position = new List<ReportPosition>
                {
                    new ReportPosition
                    {
                        Id = 1,
                        ReportId = 1,
                        Title = " Tytuł 1",
                        Description = "Opis 1",
                        Value = 12.30M
                    },
                    new ReportPosition
                    {
                        Id = 2,
                        ReportId = 2,
                        Title = " Tytuł 2",
                        Description = "Opis 2",
                        Value = 57.60M
                    },
                    new ReportPosition
                    {
                        Id = 3,
                        ReportId = 3,
                        Title = " Tytuł 3",
                        Description = "Opis 3",
                        Value = 63.50M
                    },
                }
            };
        }

        public void RaportIsSend(Report report)
        {
            report.IsSend = true;
            //zapis w bazie danych
        }
    }
}
