using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using task.shared;

namespace task.api.Models
{
    public class ApiDbSeeder
    {
        private readonly ApiDbContext _ctx;
        private readonly IWebHostEnvironment _env;

        public ApiDbSeeder(ApiDbContext ctx, IWebHostEnvironment env)
        {
            _ctx = ctx;
            _env = env;
        }

        public void Seed()
        {
            _ctx.Database.EnsureCreated();

            if (!_ctx.AppointmentSpecialists.Any())
            {
                var filepath = Path.Combine(_env.ContentRootPath, "Models/appointmentSpecialists.json");
                var json = File.ReadAllText(filepath);
                var appointmentSpecialists = JsonConvert.DeserializeObject<IEnumerable<AppointmentSpecialist>>(json);
                _ctx.AppointmentSpecialists.AddRange(appointmentSpecialists);

                _ctx.SaveChanges();
            }
        }
    }
}
