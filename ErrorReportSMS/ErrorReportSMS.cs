using Cipher;
using ErrorReportSMS.Core;
using ErrorReportSMS.Core.Domains;
using ErrorReportSMS.Core.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ErrorReportSMS
{
    public partial class ErrorReportSMS : ServiceBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly StringCipher _stringCipher = new StringCipher("D9272B5C-C958-4A23-9C0E-9B812E3FE992");
        private int _intervalInMinutes;
        private Timer _timer;
        private ErrorRepository _errorRepository = new ErrorRepository();
        private Sms _sms;
        private const string NotEncryptedPasswordPrefix = "encrypt:";
        
        public ErrorReportSMS()
        {
            InitializeComponent();

            try
            {                
                _intervalInMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInMinutes"]);
                _timer = new Timer(_intervalInMinutes * 60000);
                _sms = new Sms
                {
                    Sid = ConfigurationManager.AppSettings["Sid"],
                    Token = DecryptSenderToken(),
                    PhoneFrom = ConfigurationManager.AppSettings["PhoneFrom"],
                    PhoneTo = ConfigurationManager.AppSettings["PhoneTo"]                  
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
        }
        private string DecryptSenderToken()
        {
            var encryptedToken = ConfigurationManager.AppSettings["Token"];
            if (encryptedToken.StartsWith(NotEncryptedPasswordPrefix))
            {
                encryptedToken = _stringCipher.
                    Encrypt(encryptedToken.Replace(NotEncryptedPasswordPrefix, ""));

                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configFile.AppSettings.Settings["Token"].Value = encryptedToken;
                configFile.Save();
            }

            return _stringCipher.Decrypt(encryptedToken);
        }

        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += DoWork;
            _timer.Start();
            Logger.Info("Service started .....");
        }
        private async void DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                await SendError();                
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }

        }
        private async Task SendError()
        {
            string errors = _errorRepository.GetLastErrors(_intervalInMinutes);

            if (errors == null || !errors.Any())
            {
                Logger.Info("Error not sent error null or none");
                return;
            }

            _sms.Message = errors;
            await Task.Run(() => SendSms(_sms));
            Logger.Info("Errors sent");
        }
        protected override void OnStop()
        {
            Logger.Info("Service stopped");
            NLog.LogManager.Shutdown();
        }
        public void SendSms(Sms sms)
        {
            try
            {
                TwilioClient.Init(sms.Sid, sms.Token);

                var message = MessageResource.Create(
                body: sms.Message,
                    from: new Twilio.Types.PhoneNumber(sms.PhoneFrom),
                    to: new Twilio.Types.PhoneNumber(sms.PhoneTo)
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
