using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using task.app.Services;
using task.shared;

namespace task.app.Pages
{
    public partial class TimeLeftView
    {
        [Inject]
        public IDataService DataService { get; set; }

        private TimeSpan _timeLeft;
        private string _reservationCode;
        private bool _wrongCode = false;
        private bool showTime = false;

        private async Task Check(string reservationCode = "")
        {
            _wrongCode = false;
            var timeLeft = await DataService.CheckAppointment(reservationCode);
            if (timeLeft.Seconds > 0)
            {
                _timeLeft = timeLeft;
                showTime = true;
            }
            else
                _wrongCode = true;
        }
    }
}
