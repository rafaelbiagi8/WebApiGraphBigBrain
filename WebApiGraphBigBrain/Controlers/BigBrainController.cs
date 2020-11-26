using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiGraphBigBrain.models;
using WebApiGraphBigBrain.Services;
using User = WebApiGraphBigBrain.models.User;

namespace WebApiGraphBigBrain.Controlers
{
    [ApiController]
    [Route("users")]
    public class BigBrainController : ControllerBase
    {
        [HttpGet]
        [Route("list/{filter}/{value}")]
        public async Task<ActionResult<List<Microsoft.Graph.User>>> Get(string filter, string value)
        {
            try
            {
                return Ok(await getUser(filter, value));
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<List<Microsoft.Graph.User>>> Get()
        { 
            try
            {
                return Ok(await getUser());
                //return await getUser();
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        private async Task<String> getUser(string filter=null, string value=null)
        {
            Users users = new Users();

            users.resources = new List<User>();
            GraphServiceClient client = await BigGraphClient.GetGraphServiceClient();

            IGraphServiceUsersCollectionPage userList;
            if (!String.IsNullOrEmpty(filter) && !String.IsNullOrEmpty(value)) {

                String query = "";

                if (filter.Equals("StartsGivenName"))
                {
                    query = "startswith(givenName, '" + value  + "')";
                }else if (filter.Equals("ExactlysStartsGivenName"))
                {
                    query = "displayName, '" + value + "'";
                }

                userList = await client.Users.Request()
                    .Filter(query)
                    .GetAsync();
            }
            else
            {
                userList = await client.Users.Request().GetAsync();
            }

            foreach (var user in userList)
            {
                var objUser = Handler.UserProperty(user);
                users.resources.Add(objUser);
            }

            users.totalResults = users.resources.Count;

            //return JsonSerializer.Serialize(userList);
            return JsonConvert.SerializeObject(userList, Formatting.Indented);
        }
    }
}
