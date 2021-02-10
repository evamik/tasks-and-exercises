using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using task.app.Services;

namespace task.app.Pages
{
    public partial class CancelAppointmentView
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IDataService DataService { get; set; }

        private string _reservationCode;
        private bool _wrongCode = false;

        private async Task Cancel(string reservationCode = "")
        {
            _wrongCode = false;
            if (await DataService.CancelAppointment(reservationCode))
                NavigationManager.NavigateTo("/");
            else
                _wrongCode = true;
        }
    }
}
