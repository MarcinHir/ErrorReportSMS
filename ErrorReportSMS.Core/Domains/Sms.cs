using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorReportSMS.Core.Domains
{
    public class Sms
    {
        public int Id { get; set; }
        public string Sid { get; set; }
        public string Token { get; set; }
        public string PhoneFrom { get; set; }
        public string PhoneTo { get; set; }
        public string Message { get; set; }
    }
}
