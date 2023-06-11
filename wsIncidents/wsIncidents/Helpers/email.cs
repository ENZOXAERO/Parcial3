using System;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Linq;
using System.Net.Mail;
using wsIncidents.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace wsIncidents.Helpers {

    internal class email {

        private static bool state = false;

        private static string path = globals.path;

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="codinc"></param>
        /// <returns></returns>
        public static async Task<bool> send(int codinc) {

            DataTable emails = new DataTable();

            var data = new List<Tuple<string,string>>();

            using(connection con = new connection()) {
                emails = con.get("man_sel_inc_incidents_group",new System.Collections.Hashtable() { { "@codinc",codinc } });
                data.Add(new Tuple<string,string>("{type}",emails.Rows[0]["typ_description"].ToString()));
                data.Add(new Tuple<string,string>("{user}",emails.Rows[0]["usr_name"].ToString()));
                data.Add(new Tuple<string,string>("{dateTime}",emails.Rows[0]["dateTime"].ToString()));
                data.Add(new Tuple<string,string>("{description}",emails.Rows[0]["inc_description"].ToString()));
            }

            try {

                using(SmtpClient smtp = new SmtpClient()) {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential() {
                        UserName = "enzoxaero.info@gmail.com",
                        Password = "bpjmukzzjepszzmu"
                    };

                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    using(MailMessage message = new MailMessage()) {
                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(readTemplate(data),Encoding.UTF8,"text/html");

                        message.From = new MailAddress("enzoxaero.info@gmail.com","Dev");
                        
                        for(int x = 0; x < emails.Rows.Count; x++) {
                            message.To.Add(emails.Rows[x]["emp_email"].ToString());
                        }
                        
                        message.AlternateViews.Add(htmlView);
                        message.Subject = "Información de incidencia";
                        message.IsBodyHtml = true;
                        message.Priority = MailPriority.High;
                        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                        smtp.SendCompleted += Smtp_SendCompleted;
                        await smtp.SendMailAsync(message);

                        return state;
                    }
                }

            } catch(Exception) {
                return state;
            }

        }

        private static void Smtp_SendCompleted(object sender,System.ComponentModel.AsyncCompletedEventArgs e) {
            if(e.Cancelled) {
                state = false;
                return;
            }
            if(e.Error != null) {
                state = false;
                return;
            }
            state = true;
        }

        private static string readTemplate(List<Tuple<string,string>> data) {
            return read(Path.Combine(@$"{path}\emailTemplate","notification.html"),data);
        }

        private static IEnumerable<string> readLines(string template) {

            using(StreamReader reader = new StreamReader(template,Encoding.UTF8)) {
                string line;
                while((line = reader.ReadLine()) != null) {
                    yield return line;
                }
            }

            yield break;
        }

        private static string read(string template,List<Tuple<string,string>> data) {
            StringBuilder stringBuilder = new StringBuilder();
            int count = 0;
            readLines(template).ToList().ForEach(delegate (string row) {
                if(count < data.Count) {
                    if(row.Contains(data[count].Item1)) {
                        stringBuilder.AppendLine(row.Replace(data[count].Item1,data[count].Item2));
                        count++;
                        return;
                    }
                }
                stringBuilder.AppendLine(row);
            });
            return stringBuilder.ToString();
        }
  
    }
}
