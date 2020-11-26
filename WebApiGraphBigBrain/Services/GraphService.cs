using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using WebApiGraphBigBrain.Interfaces;
using WebApiGraphBigBrain.models;

namespace WebApiGraphBigBrain.Services
{
    public class GraphService : IGraphService
    {
        private static GraphServiceClient graphClient;
        private static IConfiguration configuration;

        private static string appId;
        private static string secret;
        private static string tenantId;
        private static string instance;
        private static string resource;
        private static string endPoint;
        private static string authority;

        static GraphService()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            SetADOptions();
        }

        private static void SetADOptions()
        {
            var adOptions = new AzureAD();
            configuration.Bind("AzureAD", adOptions);

            appId = adOptions.AppId;
            secret = adOptions.Secret;
            tenantId = adOptions.TenantId;
            instance = adOptions.Instance;
            resource = adOptions.GraphResource;
            endPoint = $"{adOptions.GraphResource}{adOptions.GraphResourceEndPoint}";
            authority = $"{instance}{tenantId}";
        }

        public async Task<IGraphServiceClient> GetGraphServiceClient()
        {
            var delegateAuthProvider = await GetAuthProvider();
            graphClient = new GraphServiceClient(endPoint, delegateAuthProvider);

            return graphClient;
        }


        private static async Task<IAuthenticationProvider> GetAuthProvider()
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(authority);
            ClientCredential clientCredencial = new ClientCredential(appId, secret);

            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(resource, clientCredencial);
            var token = authenticationResult.AccessToken;

            var delegateAuthProvider = new DelegateAuthenticationProvider((requestMessage) =>
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token.ToString());
                return Task.FromResult(0);
            });

            return delegateAuthProvider;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
