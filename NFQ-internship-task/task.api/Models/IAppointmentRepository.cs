using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using task.shared;

namespace task.api.Models
{
    public interface IAppointmentRepository
    {
        IEnumerable<Appointment> GetAppointments(string username);
        IEnumerable<Appointment> GetAppointments();
        void NewAppointment(Appointment appointment);
        bool CancelAppointment(string reservationCode);
        bool StartAppointment(string reservationCode);
        bool EndAppointment(string reservationCode);
        TimeSpan CheckAppointmentTimeLeft(string reservationCode);
    }
}
