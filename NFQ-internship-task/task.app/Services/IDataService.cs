using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using task.shared;

namespace task.app.Services
{
    public interface IDataService
    {
        Task Login(string username, string password);
        Task Logout();
        Task<IEnumerable<Appointment>> GetAppointments();
        Task<Appointment> AddAppointment();
        Task<bool> CancelAppointment(string reservationCode);
        Task<bool> StartAppointment(string reservationCode);
        Task<bool> EndAppointment(string reservationCode);
        Task<Appointment> NewAppointment();
        Task<IEnumerable<Appointment>> GetDisplayBoardAppointments();
        Task<TimeSpan> CheckAppointment(string reservationCode);
        Task<bool> IsAuthenticated();
    }
}
