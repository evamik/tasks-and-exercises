using System;
using System.Collections.Generic;
using System.Text;

namespace task.shared
{
    public class AppointmentSpecialist
    {
        public int AppointmentSpecialistId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<AvailableTimePeriod> AvailableTimePeriods { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
