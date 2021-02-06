using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using task.shared;

namespace task.api.Models
{
    public class ApiDbContext : DbContext
    {
        public DbSet<AppointmentSpecialist> Specialists { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
