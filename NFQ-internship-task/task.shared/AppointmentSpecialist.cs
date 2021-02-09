using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace task.shared
{
    public class AppointmentSpecialist : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<AvailableTimePeriod> AvailableTimePeriods { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
