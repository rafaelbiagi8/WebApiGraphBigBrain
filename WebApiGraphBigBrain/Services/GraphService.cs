using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using WebApiGraphBigBrain.Interfaces;
using WebApiGraphBigBrain.models;

namespace WebApiGraphBigBrain.Services
{
    public class GraphService : IGraphService
    {
        private GraphServiceClient _graphClient;
        private AzureAD Options { get; set; }

        public GraphService(IConfiguration configuration)
        {
            Options = new AzureAD();
            new ConfigureFromConfigurationOptions<AzureAD>(configuration.GetSection("AzureAD"))
                .Configure(Options);
        }

        public async Task<IGraphServiceClient> GetGraphServiceClient()
        {
            var delegateAuthProvider = await GetAuthProvider();
            _graphClient = new GraphServiceClient($"{Options.GraphResource}{Options.GraphResourceEndPoint}", delegateAuthProvider);

            return _graphClient;
        }


        private async Task<IAuthenticationProvider> GetAuthProvider()
        {
            AuthenticationContext authenticationContext = new AuthenticationContext($"{Options.Instance}{Options.TenantId}");
            ClientCredential clientCredencial = new ClientCredential(Options.AppId, Options.Secret);

            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(Options.GraphResource, clientCredencial);
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
