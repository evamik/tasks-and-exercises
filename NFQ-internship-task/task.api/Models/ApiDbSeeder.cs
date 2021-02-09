using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using task.shared;

namespace task.api.Models
{
    public class ApiDbSeeder
    {
        private readonly ApiDbContext _ctx;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppointmentSpecialist> _userManager;

        public ApiDbSeeder(ApiDbContext ctx, IWebHostEnvironment env, UserManager<AppointmentSpecialist> userManager)
        {
            _ctx = ctx;
            _env = env;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await _ctx.Database.EnsureCreatedAsync();

            if (!_ctx.AppointmentSpecialists.Any())
            {
                var filepath = Path.Combine(_env.ContentRootPath, "Models/appointmentSpecialists.json");
                var json = await File.ReadAllTextAsync(filepath);
                var appointmentSpecialists = JsonConvert.DeserializeObject<IEnumerable<AppointmentSpecialist>>(json);
                foreach (var spec in appointmentSpecialists)
                {
                    var result = await _userManager.CreateAsync(spec, $"{spec.FirstName}{spec.LastName}1.");
                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException($"Could not create new specialist in seeder. {result.Errors}");
                    }
                }

                await _ctx.SaveChangesAsync();
            }
        }
    }
}
