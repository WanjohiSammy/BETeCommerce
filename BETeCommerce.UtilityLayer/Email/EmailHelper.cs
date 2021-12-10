using BETeCommerce.BunessEntities.Configurations;
using BETeCommerce.BunessEntities.DataEntities;
using BETeCommerce.BunessEntities.Requests;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BETeCommerce.UtilityLayer.Email
{
    public class EmailHelper : IEmailHelper
    {
        private readonly EmailConfig _emailConfig;

        public EmailHelper(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public EmailDataDto SendEmail(CheckoutItemsRequest request)
        {
            var emailHtml = FormEmail(request);
            var emailData = new EmailDataDto
            {
                ToAddress = request.BuyerEmail,
                EmailBody = emailHtml,
                FromAddress = _emailConfig.FromAddress,
                Subject = EmailConstants.EmailSubject,
                SentStatus = false
            };

            SendEmail(emailData);
            emailData.SentTime = DateTime.Now;
            emailData.SentStatus = true;

            return emailData;
        }

        #region Private
        private string FormEmail(CheckoutItemsRequest request)
        {
            var tablesRows = new StringBuilder();

            foreach(var item in request.CartItems)
            {
                var row = string.Format(EmailConstants.TableRow, 
                    item.ProductName, 
                    item.Quantity, 
                    item.Price);
                tablesRows.Append(row);
            }

            var emailFormed = EmailConstants.FullEmailTemplate
                .Replace(EmailConstants.OrderNumberKey, Guid.NewGuid().ToString())
                .Replace(EmailConstants.TotalKey, request.TotalPrice.ToString())
                .Replace(EmailConstants.TableRowsContentKey, tablesRows.ToString());


            //var emailFormed = string.Format(EmailConstants.FullEmailTemplate,
            //    Guid.NewGuid(),
            //    request.TotalPrice,
            //    tablesRows.ToString());

            return emailFormed;
        }

        private void SendEmail(EmailDataDto emailData)
        {
            using (SmtpClient client = new SmtpClient(EmailConstants.SmtpHost))
            {
                var mail = new MailMessage();
                mail.To.Add(emailData.ToAddress);
                mail.Subject = emailData.Subject;

                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(emailData.EmailBody, null, MediaTypeNames.Text.Html);
                mail.AlternateViews.Add(alternateView);
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(_emailConfig.FromAddress);

                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_emailConfig.FromAddress, _emailConfig.Password);
                client.Port = EmailConstants.SmtpPort;
                client.Host = EmailConstants.SmtpHost;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;

                client.Send(mail);
            }
            
        }
        #endregion

    }
}
