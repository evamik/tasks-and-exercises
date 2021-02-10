using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using task.shared;

namespace task.app.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var currentSpecialist = await _httpClient.GetFromJsonAsync<AppointmentSpecialist>("api/account");

            if (currentSpecialist?.UserName == null)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var claim = new Claim(ClaimTypes.Name, currentSpecialist.UserName);
            var claimsIdentity = new ClaimsIdentity(new[] { claim }, "serverAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationState(claimsPrincipal);

        }
    }
}
