using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using task.app.Services;

namespace task.app.Pages
{
    public partial class LoginView
    {
        private string Username { get; set; }
        private string Password { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IDataService DataService { get; set; }


        private async Task Login()
        {
            await DataService.Login(Username, Password);
            NavigationManager.NavigateTo("/", true);
        }
    }
}
