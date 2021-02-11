using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using task.app.Services;
using task.shared;

namespace task.app.Pages
{
    public partial class DisplayBoardView
    {
        public IEnumerable<Appointment> Appointments { get; set; }
        [Inject]
        public IDataService DataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var timer = new Timer(new TimerCallback(_ =>
            {
                InvokeAsync(async () =>
                {
                    Appointments = (await DataService.GetDisplayBoardAppointments()).ToList();
                    StateHasChanged();
                });
            }), null, 0, 5000);
        }
    }
}
