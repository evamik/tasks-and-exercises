using System;
using System.Collections.Generic;
using System.Text;

namespace task.shared
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime StartingTime { get; set; }
        public string ReservationCode { get; set; }
        public AppointmentStatus Status { get; set; }
        public int SpecialistId { get; set; }
        public AppointmentSpecialist Specialist { get; set; }
    }
}
