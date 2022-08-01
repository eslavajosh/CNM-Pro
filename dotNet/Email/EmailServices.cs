using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sabio.Data.Providers;
using SendGrid;
using SendGrid.Helpers.Mail;
using Newtonsoft.Json.Linq;
using Sabio.Models.AppSettings;
using Microsoft.Extensions.Options;
using Sabio.Services.Interfaces;
using Sabio.Models.Requests.Email;
using Microsoft.Extensions.Configuration;

namespace Sabio.Services.Email
{
    public class EmailServices : IEmailServices
    {
        public IDataProvider _data = null;

        private IWebHostEnvironment _environment = null;

        private AppKeys _apiKey = null;

        public EmailServices(IOptions<AppKeys> appKeys, IWebHostEnvironment environment, IDataProvider data)
        {
            _apiKey = appKeys.Value;
            _environment = environment;
            _data = data;
        }

        public async Task SendContactUsEmail(EmailAddRequest model) 
        {
            EmailAddress from = new EmailAddress(model.Email, "John Doe"); 
            string subject = $"{model.Subject}";
            EmailAddress to = new EmailAddress(_apiKey.DomainEmail); 
            string plainTextContent = $"{model.PlainText}";
            string path = _environment.WebRootPath + "/EmailTemplates/EmailContact.html";
            string htmlContent = File.ReadAllText(path);
            htmlContent = htmlContent.Replace("{{userMessage}}", $"{model.PlainText}"); 

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await SendEmail(msg);
        }

        public async Task SendConfirmEmail(EmailAddRequest model) 
        {
            EmailAddress from = new EmailAddress(_apiKey.DomainEmail, "CNM Pro Support Team");
            string subject = $"{model.Subject}";
            EmailAddress to = new EmailAddress(model.Email);
            string plainTextContent = $"{model.PlainText}";
            string path = _environment.WebRootPath + "/EmailTemplates/EmailThankYou.html";
            string htmlContent = File.ReadAllText(path);

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await SendEmail(msg);
        }

        public async void confirmEmail(string email, string token) 
        {
            string path = _environment.WebRootPath + "/EmailTemplates/VerifyEmail.html";
            SendGridMessage msg = new SendGridMessage()
            {
                From = new EmailAddress("limasi7444@tebyy.com", "CnmPro"),
                Subject = "Please confirm your email.",
                PlainTextContent = "Please confirm your email.",
                HtmlContent = File.ReadAllText(path).Replace("{{token}}", token)
            };
            msg.AddTo(new EmailAddress(email, "Cnm Received"));
            await SendEmail(msg);
        }

        private async Task<Response> SendEmail(SendGridMessage msg)
        {
            var apiKey = _apiKey.SendGridAppKey;
            SendGridClient client = new SendGridClient(apiKey);
            Response response = await client.SendEmailAsync(msg);
            return response;
        }

        public async Task SendResetPasswordEmail(string email, string token)
        {
            EmailAddress from = new EmailAddress(_apiKey.DomainEmail, "CNM Pro Support Team");
            string subject = "Reset your password";
            EmailAddress to = new EmailAddress(email, "User");
            string baseUrl = (_apiKey.HttpsUrl + "/changepassword?token=" + token + "&email=" + email);
            string path = _environment.WebRootPath + "/EmailTemplates/EmailResetPassword.html"; 
            string htmlContent = File.ReadAllText(path);
            htmlContent = htmlContent.Replace("{{userMessage}}", baseUrl);
            SendGridMessage msg = new SendGridMessage()
            {
                From = from,
                Subject = subject,
                PlainTextContent = "Reset Password",
                HtmlContent = htmlContent
            };
            msg.AddTo(to);
            await SendEmail(msg);
        }
    }
}
