using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using task.app.Services;
using task.shared;

namespace task.app.Pages
{
    public partial class AppointmentOverview
    {
        private Appointment _bookedAppointment;
        [Inject]
        public IDataService DataService { get; set; }

        private async Task BookAppointment()
        {
            _bookedAppointment = await DataService.NewAppointment();
            System.Console.WriteLine("test");
        }
    }
}
