using AutoMapper;
using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGraphBigBrain.Interfaces;
using WebApiGraphBigBrain.models;

namespace WebApiGraphBigBrain.Services
{
    public class UserService : IUserService
    {
        private readonly IGraphService _graphService;
        private readonly IMapper _mapper;
        public UserService(IGraphService graphService, IMapper mapper)
        {
            _graphService = graphService;
            _mapper = mapper;
        }

        public async Task<string> GetUser(string filter = null, string value = null)
        {
            IGraphServiceClient client = await _graphService.GetGraphServiceClient();

            IGraphServiceUsersCollectionPage userList;
            if (!string.IsNullOrWhiteSpace(filter) && !string.IsNullOrWhiteSpace(value))
            {

                string query = string.Empty;

                if (filter.Equals("StartsGivenName"))
                {
                    query = "startswith(givenName, '" + value + "')";
                }
                else if (filter.Equals("ExactlysStartsGivenName"))
                {
                    query = "givenName, '" + value + "'";
                }

                userList = await client.Users.Request().Filter(query).GetAsync();
            }
            else
            {
                userList = await client.Users.Request().GetAsync();
            }

            return JsonConvert.SerializeObject(userList, Formatting.Indented);
        }

        public async Task<string> GetUserMap()
        {
            IGraphServiceClient client = await _graphService.GetGraphServiceClient();

            IGraphServiceUsersCollectionPage userList = await client.Users.Request().GetAsync();

            Users users = new Users();

            users.resources.AddRange(_mapper.Map<List<models.User>>(userList));
            
            return JsonConvert.SerializeObject(userList, Formatting.Indented);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
