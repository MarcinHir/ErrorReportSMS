using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorReportSMS.Core.Domains
{
    public class Error
    {
        public int Id { get; set; }
        public string Massage { get; set; }
        public DateTime Date { get; set; }
    }
}
