using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using task.app.Services;

namespace task.app.Pages
{
    public partial class LogoutView
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IDataService DataService { get; set; }
        private async Task Yes()
        {
            await DataService.Logout();
            NavigationManager.NavigateTo("/", true);
        }

        private void No()
        {
            NavigationManager.NavigateTo("/", true);
        }
    }
}
