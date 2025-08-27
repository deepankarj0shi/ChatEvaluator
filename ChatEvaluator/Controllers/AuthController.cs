
using ChatEvaluator.Models;
using ChatEvaluator.SharedObject.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChatEvaluator.Dal.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace ChatEvaluator.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        protected ResponseData _response;
        private readonly IJwtTokenManagement _jwtTokenManagement;


        public AuthController(IJwtTokenManagement jwtTokenManagement)
        {
            _jwtTokenManagement = jwtTokenManagement;
            _response = new();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest model)
        {
            int id = await new Bal.AccountRepository().RegisterUser(model);
            if(id > 0)
            {
                _response.Message = "Registration successfull!Please Login";
                return Ok(_response);
            }
            _response.Status = false;
            _response.Message = "Error";
            return BadRequest(_response.Message);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest model)
        {
            Users user = await new Bal.AccountRepository().GetUserByName(model.Username);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    var token = _jwtTokenManagement.GenerateToken(user);
                    LoginResponse response = new LoginResponse()
                    {
                        LoginRequest= new LoginRequest { Username=user.Username, Password=user.Password},
                        JwtToken= token
                    };
                    await SignInUser(response);

                    _response.Status = true;
                    _response.Result = response;
                    return Ok(_response);
                }
            }
            _response.Status = false;
            _response.Message = "Username or password is incorrect";
            return BadRequest(_response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            _jwtTokenManagement.ClearToken();
            _response.Status = true;
            _response.Result = "Logged out successfully!";
            return Ok(_response);
        }
        private async Task SignInUser(LoginResponse model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.JwtToken);

            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            foreach (var claim in jwt.Claims)
            {
                identity.AddClaim(claim);
            }
            var principal = new ClaimsPrincipal(identity);
            HttpContext.User = principal;
        }
    }
}
