using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using task.shared;

namespace task.api.Models
{
    public interface IAppointmentRepository
    {
        IEnumerable<Appointment> GetAppointments();
        Appointment GetAppointment(int id);
        void NewAppointment(Appointment appointment);
    }
}
