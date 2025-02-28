using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Practice.Model;
using Practice.Models;
using Practice.Repo;

namespace Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly Exam1Context _exam1;
        private readonly IUserRepo _userrepo;
        private readonly IConfiguration _configuration;
        public RegisterController(Exam1Context exam1, IUserRepo userrepo, IConfiguration configuration)
        {
            _exam1 = exam1;
            _userrepo = userrepo;
            _configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<Response<string>> Register([FromBody] Register register)
        {

            return await _userrepo.SingUp(register);

        }

        [HttpPost("Login")]
        public async Task<Response<string>> Login([FromBody] Login login)
        {

            var response = await _userrepo.SingIn(login);
            if (response.IsSuccess)
            {
                // Generate Token on successful login
                string token = GeneratedToken(login.email);
                response.Token = token;

            }
            return response;

        }
        private string GeneratedToken(string email)
        {
            var jwtkey = _configuration["Jwt:Key"];
            var jwtissuser = _configuration["Jwt:Issuer"];
            var jwtaudience = _configuration["Jwt:Audience"];
            //var jwtsubjecct = _configuration["Jwt:Subject"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,email),
                //new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };
            var token = new JwtSecurityToken(jwtissuser, jwtaudience, claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
