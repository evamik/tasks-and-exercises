using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using task.api.Models;
using task.shared;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace task.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<AppointmentSpecialist> _signInManager;
        private readonly UserManager<AppointmentSpecialist> _userManager;
        private readonly IConfiguration _config;

        public AccountController(ILogger<AccountController> logger, 
            SignInManager<AppointmentSpecialist> signInManager, 
            UserManager<AppointmentSpecialist> userManager,
            IConfiguration config)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCredentials loginCredentials)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginCredentials.UserName,
                    loginCredentials.Password, true, false);
                if (result.Succeeded)
                    return Ok();
            }
            ModelState.AddModelError("", "Failed to login");
            return BadRequest();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("createToken")]
        public async Task<IActionResult> CreateToken([FromBody]LoginCredentials loginCredentials)
        {
            var user = await _userManager.FindByNameAsync(loginCredentials.UserName);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginCredentials.Password, false);

                if (result.Succeeded)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Id)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        _config["Tokens:Issuer"],
                        _config["Tokens:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: credentials);

                    var results = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };

                    return Created("", results);
                }
            }

            return BadRequest();
        }
    }
}
