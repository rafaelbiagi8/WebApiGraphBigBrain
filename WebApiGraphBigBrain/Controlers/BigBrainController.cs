using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiGraphBigBrain.Interfaces;
using WebApiGraphBigBrain.models;
using WebApiGraphBigBrain.Services;
using User = WebApiGraphBigBrain.models.User;

namespace WebApiGraphBigBrain.Controlers
{
    [ApiController]
    [Route("users")]
    public class BigBrainController : ControllerBase
    {
        private readonly IGraphService _graphService;
        private readonly IMapper _mapper;
        public BigBrainController(IGraphService graphService, IMapper mapper)
        {
            _graphService = graphService;
            _mapper = mapper;
        }

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

        [HttpGet]
        [Route("listMap")]
        public async Task<ActionResult<List<Microsoft.Graph.User>>> GetMapper()
        {
            try
            {
                Users users = new Users();
                users.resources = new List<User>();
                IGraphServiceClient client = await _graphService.GetGraphServiceClient();

                IGraphServiceUsersCollectionPage userList = await client.Users.Request().GetAsync();

                foreach (var user in userList)
                {
                    var objUser = _mapper.Map<User>(user);
                    users.resources.Add(objUser);
                }

                return Ok(users);
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
            IGraphServiceClient client = await _graphService.GetGraphServiceClient();

            IGraphServiceUsersCollectionPage userList;
            if (!String.IsNullOrEmpty(filter) && !String.IsNullOrEmpty(value)) {

                String query = "";

                if (filter.Equals("StartsGivenName"))
                {
                    query = "startswith(givenName, '" + value  + "')";
                }else if (filter.Equals("ExactlysStartsGivenName"))
                {
                    query = "givenName, '" + value + "'";
                }

                userList = await client.Users.Request()
                    .Filter(query)
                    .GetAsync();
            }
            else
            {
                userList = await client.Users.Request().GetAsync();
            }

            return JsonConvert.SerializeObject(userList, Formatting.Indented);
        }
    }
}
