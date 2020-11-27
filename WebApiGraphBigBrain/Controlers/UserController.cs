using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebApiGraphBigBrain.Interfaces;
using WebApiGraphBigBrain.models;
using User = WebApiGraphBigBrain.models.User;

namespace WebApiGraphBigBrain.Controlers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("list/{filter}/{value}")]
        public async Task<ActionResult<List<Microsoft.Graph.User>>> Get(string filter, string value)
        {
            try
            {
                return Ok(await _userService.GetUser(filter, value));
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                    return BadRequest();
                return NotFound();
            }
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<List<Microsoft.Graph.User>>> Get()
        { 
            try
            {
                return Ok(await _userService.GetUser());
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                    return BadRequest();

                return NotFound();
            }
        }

        [HttpGet]
        [Route("listMap")]
        public async Task<ActionResult<List<Microsoft.Graph.User>>> GetMapper()
        {
            try
            {
                return Ok(new Users { resources = _mapper.Map<List<User>>(await _userService.GetUserMap()) });
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                    return BadRequest();

                return NotFound();
            }
        }


    }
}
