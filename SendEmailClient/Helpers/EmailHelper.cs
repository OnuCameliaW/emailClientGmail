namespace SendEmailClient.Helpers
{
    using System;
    using System.IO;
    using System.Net.Mail;
    using System.Threading;

    using DotLiquid;

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Gmail.v1;
    using Google.Apis.Gmail.v1.Data;
    using Google.Apis.Services;
    using Google.Apis.Util.Store;

    using MimeKit;

    using Encoding = System.Text.Encoding;

    public class EmailHelper
    {
        public static bool SendEmail(EmailDto email)
        {
            try
            {
                var templateFolderPath = Path.GetFullPath(TextResources.emailTemplateFile);

                if (File.Exists(templateFolderPath))
                {
                    var source = File.ReadAllText(templateFolderPath);
                    var mail = new MailMessage();
                    mail.Subject = TextResources.emailSubject;
                    //if (FluidTemplate.TryParse(source, out var template))
                    //{
                    //    var context = new TemplateContext();
                    //    context.MemberAccessStrategy.Register(email.EmailBody.GetType()); // Allows any public property of the model to be used
                    //    context.SetValue("emailBody", email.EmailBody);
                    //    mail.Body = template.Render(context);
                    //}

                    Template template = Template.Parse(source);
                    mail.Body = template.Render(Hash.FromAnonymousObject(new { emailBody = email.EmailBody }));

                    mail.From = new MailAddress(email.SentFrom);
                    mail.IsBodyHtml = true;
                    mail.To.Add(new MailAddress(email.SendTo));
                    var mimeMessage = MimeMessage.CreateFromMailMessage(mail);
                    var message = new Message();
                    message.Raw = Base64UrlEncode(mimeMessage.ToString());
                    var gmailService = InitilizeGmailService();
                    gmailService.Users.Messages.Send(message, TextResources.randomUserId).Execute();
                    return true;
                }
            }
            catch (Exception exception)
            {
                return false;
            }

            return false;
        }

        private static GmailService InitilizeGmailService()
        {
            UserCredential credential;
            string[] scopes = { GmailService.Scope.GmailSend };
            var applicationName = TextResources.gmailApplicationName;
            using (var stream = new FileStream(TextResources.credentialsFile, FileMode.Open, FileAccess.Read))
            {
                var credPath = TextResources.tokenSendFile;
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    TextResources.gmailUsername,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            var gmailService = new GmailService(
                new BaseClientService.Initializer { HttpClientInitializer = credential, ApplicationName = applicationName });
            return gmailService;
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes).Replace('+', '-').Replace('/', '_').Replace("=", string.Empty);
        }
    }
}