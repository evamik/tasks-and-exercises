using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using task.shared;

namespace task.api.Models
{
    public class AppointmentSpecialistRepository : IAppointmentSpecialistRepository
    {
        private readonly ApiDbContext _apiDbContext;
        private readonly ILogger<AppointmentSpecialistRepository> _logger;

        public AppointmentSpecialistRepository(ApiDbContext apiDbContext, ILogger<AppointmentSpecialistRepository> logger)
        {
            _apiDbContext = apiDbContext;
            _logger = logger;
        }

        public AppointmentSpecialist GetAppointmentSpecialist(string id)
        {
            return _apiDbContext.AppointmentSpecialists.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<AppointmentSpecialist> GetAppointmentSpecialists()
        {
            return _apiDbContext.AppointmentSpecialists.Include(a => a.AvailableTimePeriods);
        }
    }
}
