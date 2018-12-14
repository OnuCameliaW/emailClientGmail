namespace SendEmailClient
{
    using System.IO;
    using System.Threading;

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Gmail.v1;
    using Google.Apis.Services;
    using Google.Apis.Util.Store;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        private static readonly string ApplicationName = TextResources.gmailApplicationName;

        private static readonly string[] Scopes = { GmailService.Scope.GmailReadonly };

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            InitializeEmailClientGmail();
        }

        private static void InitializeEmailClientGmail()
        {
            UserCredential credential;
            using (var stream = new FileStream(TextResources.credentialsFile, FileMode.Open, FileAccess.Read))
            {
                var credPath = TextResources.tokenFile;
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    TextResources.gmailUsername,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            var service = new GmailService(
                new BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = ApplicationName
                    });

            service.Users.Labels.List(TextResources.randomUserId);
        }
    }
}