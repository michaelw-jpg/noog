using Microsoft.AspNetCore.Mvc;
using Noog_api.Helpers;
using Noog_api.Models;
using Noog_api.Services.IServices;


namespace Noog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService<ApplicationUser> _userService;

        public UserController(IUserService<ApplicationUser> userService)
        {
            _userService = userService;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<List<ApplicationUser>>> GetAllUsers()
        {
            var users = await _userService.AllUsersAsync();
            return (users);
        }

        // GET api/<UserController>/5
        [HttpGet("byEmail/{id}")]
        public async Task<ActionResult<ApplicationUser>> GetByEmail(string email)
        {
            var user = await _userService.FindByEmailAsync(email);
            return (user);
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
