using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using task.api.Models;
using task.shared;

namespace task.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
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
        public async Task<ActionResult<AppointmentSpecialist>> Login(LoginCredentials loginCredentials)
        {
            var user = await _userManager.FindByNameAsync(loginCredentials.UserName);
            await _signInManager.PasswordSignInAsync(loginCredentials.UserName,
                loginCredentials.Password, true, false);

            return await Task.FromResult(user);
        }

        [HttpGet]
        public async Task<ActionResult<AppointmentSpecialist>> GetUser()
        {
            AppointmentSpecialist user = new AppointmentSpecialist();
            if (User.Identity.IsAuthenticated)
            {
                user.UserName = User.FindFirstValue(ClaimTypes.Name);
            }

            return await Task.FromResult(user);
        }

        [HttpGet("logout")]
        public async Task<ActionResult<String>> Logout()
        {
            await _signInManager.SignOutAsync();
            return "Success";
        }
    }
}
