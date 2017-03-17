using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace PLWPF
{
    public class Tools
    {
        public static System.Media.SoundPlayer SoundWhip     = new System.Media.SoundPlayer(Sounds.Whip);
        public static System.Media.SoundPlayer SoundFailed   = new System.Media.SoundPlayer(Sounds.Trombone);
        public static System.Media.SoundPlayer SoundClick    = new System.Media.SoundPlayer(Sounds.Click);
        public static System.Media.SoundPlayer SoundHover    = new System.Media.SoundPlayer(Sounds.Hover);
        public static System.Media.SoundPlayer SoundSuccsses = new System.Media.SoundPlayer(Sounds.HolyGrenade);
        public static System.Media.SoundPlayer SoundMoney    = new System.Media.SoundPlayer(Sounds.Coins);
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    using (System.IO.Stream stream = client.OpenRead("http://www.google.com")) { return true; }
                }
            }
            catch
            {
                return false;
            }
        }
        public static void mail(string name = "Mr. Mysterious", string body = "I have no body")
        {
            var fromAddress = new MailAddress("jctlev16@gmail.com", name);
            var toAddress = new MailAddress("neriat@gmail.com", "Neria");
            const string fromPassword = "pruhheywpf";
            const string subject = "מצאתי באג";

            System.Net.Mail.SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            System.Net.Mail.MailMessage message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };
            smtp.Send(message);
        }
    }
}
