using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using task.shared;

namespace task.app.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Login(string username, string password)
        {
            var loginJson = new StringContent(JsonSerializer.Serialize(new {username, password}), Encoding.UTF8,
                "application/json");

            await _httpClient.PostAsync("/api/account/login", loginJson);
        }

        public async Task Logout()
        {
            await _httpClient.GetAsync("api/account/logout");
        }

        public async Task<IEnumerable<Appointment>> GetAppointments()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Appointment>>(
                await _httpClient.GetStreamAsync($"api/appointment"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public Task<Appointment> AddAppointment()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CancelAppointment(string reservationCode)
        {
            var res = await _httpClient.PutAsync($"api/appointment/{reservationCode}/cancel", null);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> StartAppointment(string reservationCode)
        {
            var res = await _httpClient.PutAsync($"api/appointment/{reservationCode}/start", null);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> EndAppointment(string reservationCode)
        {
            var res = await _httpClient.PutAsync($"api/appointment/{reservationCode}/end", null);
            return res.IsSuccessStatusCode;
        }

        public async Task<Appointment> NewAppointment()
        {
            var res = await _httpClient.PostAsync("api/appointment/", new StringContent("{}", Encoding.UTF8, "application/json"));

            if (!res.IsSuccessStatusCode)
                return null;

            return await JsonSerializer.DeserializeAsync<Appointment>(await res.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<Appointment>> GetDisplayBoardAppointments()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Appointment>>(
                await _httpClient.GetStreamAsync($"api/appointment/displayboard"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<TimeSpan> CheckAppointment(string reservationCode)
        {
            string res = await _httpClient.GetStringAsync(($"api/appointment/{reservationCode}"));
            return TimeSpan.Parse(res.Replace("\"", string.Empty));
        }
    }
}
