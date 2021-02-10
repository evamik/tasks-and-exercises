using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using task.app.Services;
using task.shared;

namespace task.app.Pages
{
    public partial class AppointmentsView
    {
        public IEnumerable<Appointment> Appointments { get; set; }
        [Inject]
        public IDataService DataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Appointments = (await DataService.GetAppointments()).ToList();
        }

        private async Task StartAppointment(Appointment appointment)
        {
            if(await DataService.StartAppointment(appointment.ReservationCode))
                appointment.Status = AppointmentStatus.Active;
        }

        private async Task EndAppointment(Appointment appointment)
        {
            if (await DataService.EndAppointment(appointment.ReservationCode))
                appointment.Status = AppointmentStatus.Ended;
        }

        private async Task CancelAppointment(Appointment appointment)
        {
            if (await DataService.CancelAppointment(appointment.ReservationCode))
                appointment.Status = AppointmentStatus.Cancelled;
        }
    }
}
