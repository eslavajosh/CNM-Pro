using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Sabio.Data.Providers;
using Sabio.Models.AppSettings;
using Sabio.Models.Requests.ContactUs;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class ContactUsService : IContactUsService
    {
        public IDataProvider _data = null;

        private IWebHostEnvironment _environment = null;

        private AppKeys _apiKey = null;


        public ContactUsService(IOptions<AppKeys> appKeys, IWebHostEnvironment environment, IDataProvider data)
        {
            
            _apiKey = appKeys.Value;
            _environment = environment;
            _data = data;
        }

        public async Task ContactUsEmail(ContactUsAddRequest model)
        {
            EmailAddress from = new EmailAddress(model.Email);
            string subject = $"{model.Subject}";
            EmailAddress to = new EmailAddress(_apiKey.DomainEmail);
            string firstname = $"{model.Name}";
            string lastname = $"{model.SurName}";
            string plainTextContent = $"{model.Message}";
            string path = _environment.WebRootPath + "/EmailTemplates/EmailContact.html";
            string htmlContent = File.ReadAllText(path);
            htmlContent = htmlContent.Replace("{{userName}}", $"{firstname} {lastname}");
            htmlContent = htmlContent.Replace("{{userMessage}}", $"{model.Message}");

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await SendEmail(msg);
        }

        private async Task<Response> SendEmail(SendGridMessage msg)
        {
            var apiKey = _apiKey.SendGridAppKey;
            SendGridClient client = new SendGridClient(apiKey);
            Response response = await client.SendEmailAsync(msg);
            return response;
        }
    }
}
