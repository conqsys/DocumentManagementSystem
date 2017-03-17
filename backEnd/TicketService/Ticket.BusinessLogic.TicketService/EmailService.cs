using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
//using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticket.BusinessLogic.Common;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Ticket.DataAccess.TicketService;
using System.Reflection;
using Ticket.Base.Entities;

namespace Ticket.BusinessLogic.TicketService
{
    public enum EmailEventType
    {
        RegistrationEmailToUser = 1,
        RegistrationEmailToAdmin = 2,
        ActivationEmailToUser = 3,
        ContactEmailToUser = 4,
        ContactEmailToAdmin = 5
    }

    public class EmailService
    {
        public IConfiguration _configuration;
        public EmailTemplateRepository emailTemplateRepository;
        public EmailService(IConfiguration configuration, EmailTemplateRepository repository)
        {
            this._configuration = configuration;
            this.emailTemplateRepository = repository;
        }

        public bool SendEmail(EmailEventType eventType, object dataTableOrDTO, string to = "", string toName = "")
        {
            IEmailTemplate template = this.emailTemplateRepository.GetTemplate((int)eventType);

            if (to == "")
                to = template.DefaultMailToId;

            if (toName == "")
                toName = template.DefaultMailToName;

            object dataRow = dataTableOrDTO;
            if (dataTableOrDTO is DataTable)
            {
                dataRow = ((DataTable)dataTableOrDTO).Rows.FirstOrDefault();
            }

            string htmlBody = this.ParseEmailTemplate(template.DefaultMailBody, dataRow);

            bool result = SendMail(to, toName, template.DefaultMailFromId, template.DefaultMailFromName, template.DefaultMailCC, "", template.DefaultMailSubject, htmlBody);
            return result;

        }

        public bool SendEmail(int eventType, DataTable dt, string to = "", string toName = "")
        {
            return this.SendEmail(eventType, dt, to, toName);
        }

        public bool SendMail(string to, string toName, string from, string fromName, string cc, string bcc, string subject, string mailBody)
        {
            try
            {
                //Task.Run(() =>
                //{
                //});

                string mailDomain = this._configuration["mailDomain"];
                string userName = this._configuration["mailUserName"];
                string password = this._configuration["mailPassword"];

                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(fromName, from));
                emailMessage.To.Add(new MailboxAddress(toName, to));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = mailBody };

                using (SmtpClient client = new SmtpClient())
                {
                    client.LocalDomain = mailDomain;

                    client.ServerCertificateValidationCallback = ((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) =>
                    {
                        return true;
                    });

                    client.Connect(mailDomain, 465, SecureSocketOptions.SslOnConnect);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(new System.Net.NetworkCredential(userName, password));
                    client.Send(emailMessage);
                    client.Disconnect(true);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        public string[] GetColumnNames(DataRow dr)
        {
            var columnNames = (from x in dr.Table.Columns
                               select x.ColumnName).ToArray();
            return columnNames;
        }

        public string[] GetColumnNames(TypeInfo typeOfObject)
        {
            var properties = typeOfObject.GetProperties();

            var columnNames = (from x in properties
                               select x.Name).ToArray();
            return columnNames;
        }

        private string FindTokenValue(string foundToken, string finalBody, object dataObject)
        {
            TypeInfo type = dataObject.GetType().GetTypeInfo();
            var columnNames = GetColumnNames(type);
            string tokenValue = "";

            if (dataObject is DataRow)
            {
                tokenValue = FindTokenValue(foundToken, finalBody, (DataRow)dataObject);
                return tokenValue;
            }

            for (var i = 0; i < columnNames.Length; i++)
            {
                if (columnNames[i] == foundToken)
                {
                    tokenValue = type.GetProperty(columnNames[i]).GetValue(dataObject).ToString();
                    break;
                }
            }

            return tokenValue;
        }



        private string FindTokenValue(string foundToken, string finalBody, DataRow dr)
        {
            string tokenValue = "";
            for (var i = 0; i < dr.FieldCount; i++)
            {
                if (dr._dataTable.Columns[i].ColumnName == foundToken)
                {
                    tokenValue = dr._data[i].ToString();
                    break;
                }
            }

            return tokenValue;
        }

        public string ParseEmailTemplate(string templateBody, object dr)
        {
            try
            {
                string finalBody = templateBody;

                while (true)
                {
                    int findIndex = finalBody.IndexOf("{");
                    int nextIndex = -1;
                    string foundToken = "";
                    string tokenValue = "";
                    if (findIndex >= 0)
                    {
                        nextIndex = finalBody.IndexOf("}", findIndex + 1);
                        if (nextIndex >= 0)
                        {
                            foundToken = finalBody.Substring(findIndex + 1, nextIndex - findIndex - 1);
                            tokenValue = FindTokenValue(foundToken, finalBody, dr);
                            if (string.IsNullOrEmpty(tokenValue)) tokenValue = "Field Not Found";
                            finalBody = finalBody.Replace("{" + foundToken + "}", tokenValue);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                return finalBody;
            }
            catch
            {
                return "";
            }
        }

    }
}
