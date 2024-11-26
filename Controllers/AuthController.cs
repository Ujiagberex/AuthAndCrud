using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.IService;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;

        public AuthController(IAuth auth)
        {
            _auth = auth;
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserDTO addUser)
        {
            var response = await _auth.CreateAccount(addUser);
            return Ok(response);
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LogInDTO login)
        {
            var response = await _auth.LogInAccount(login);
            return Ok(response);
        }


    }
}
