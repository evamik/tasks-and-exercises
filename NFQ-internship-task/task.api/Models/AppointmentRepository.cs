using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using task.shared;

namespace task.api.Models
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApiDbContext _apiDbContext;
        private readonly ILogger<AppointmentRepository> _logger;

        public AppointmentRepository(ApiDbContext apiDbContext, ILogger<AppointmentRepository> logger)
        {
            _apiDbContext = apiDbContext;
            _logger = logger;
        }

        public Appointment GetAppointment(int id)
        {
            return _apiDbContext.Appointments.FirstOrDefault(a => a.AppointmentId == id);
        }

        public void NewAppointment(Appointment appointment)
        {
            var specialists = _apiDbContext.AppointmentSpecialists
                .Include(a => a.Appointments)
                .Include(a => a.AvailableTimePeriods)
                .Where(a => a.AvailableTimePeriods.Count > 0)
                .ToList();
            var soonest = DateTime.MinValue;
            string specialistId = null;
            foreach (var spec in specialists)
            {
                var bookTime = DateTime.MinValue;
                var soonestCancelled = spec.Appointments
                    .Where(a => a.Status == AppointmentStatus.Cancelled && a.StartingTime >= DateTime.Now)
                    .OrderByDescending(a => a.StartingTime)
                    .LastOrDefault();
                var latestAppointment = spec.Appointments
                    .Where(a => a.Status < AppointmentStatus.Cancelled)
                    .OrderByDescending(a => a.StartingTime)
                    .FirstOrDefault();
                DateTime date = DateTime.Now;
                var availableTimes = spec.AvailableTimePeriods
                    .OrderByDescending(a => a.Start)
                    .Reverse();
                if (latestAppointment != null && latestAppointment.StartingTime.AddMinutes(30) > date)
                    date = latestAppointment.StartingTime.AddMinutes(30);
                var nowInFormat = new DateTime(1, 1, 1);
                var now = DateTime.Now;
                nowInFormat = nowInFormat.AddDays(now.TimeOfDay.TotalDays);
                if (availableTimes.FirstOrDefault(a => a.Start <= nowInFormat && a.End >= nowInFormat.AddMinutes(30)) != null)
                    if (spec.Appointments.Count(a => a.Status >= AppointmentStatus.Cancelled && ((a.StartingTime < now && a.StartingTime.AddMinutes(30) > now) || (a.StartingTime > now && a.StartingTime < now.AddMinutes(30)))) == 2)
                        date = DateTime.Now;

                if (soonestCancelled != null && soonestCancelled.StartingTime < date)
                    bookTime = soonestCancelled.StartingTime;
                else
                    foreach (var t in availableTimes)
                    {
                        var newTime = CurrentWeek(date, t.Start);
                        if (CurrentWeek(date, t.End).AddMinutes(-30) >= date)
                        {
                            if (newTime > date)
                            {
                                bookTime = newTime;
                                break;
                            }

                            bookTime = date;
                            break;
                        }
                        else if (newTime > date)
                        {
                            bookTime = newTime;
                            break;
                        }
                    }

                if (bookTime == DateTime.MinValue)
                    bookTime = NextWeek(date, availableTimes.First().Start);

                if (soonest == DateTime.MinValue || bookTime < soonest)
                {
                    soonest = bookTime;
                    specialistId = spec.Id;
                }
            }
            appointment.SpecialistId = specialistId;
            appointment.StartingTime = soonest;
            _apiDbContext.Appointments.Add(appointment);
            foreach (var app in _apiDbContext.Appointments
                .Where(a => a.SpecialistId == specialistId
                            && a.Status == AppointmentStatus.Cancelled
                            && ((a.StartingTime >= appointment.StartingTime
                                 && a.StartingTime <= appointment.StartingTime.AddMinutes(30)) || (a.StartingTime < appointment.StartingTime
                                && a.StartingTime.AddMinutes(30) > appointment.StartingTime))))
            {
                app.Status = AppointmentStatus.CancelledButTaken;
            }
            _apiDbContext.SaveChanges();
            appointment.ReservationCode = $"s{specialistId.Substring(0,5)}a{appointment.AppointmentId}";
            _apiDbContext.SaveChanges();
        }

        private static DateTime NextWeek(DateTime from, DateTime date)
        {
            var today = from.Date;
            var daysToAdd = (7 - (int)today.DayOfWeek);
            return today.AddDays(daysToAdd + date.TimeOfDay.TotalDays + (int)date.DayOfWeek); ;
        }

        private static DateTime CurrentWeek(DateTime from, DateTime date)
        {
            var today = from.Date;
            var daysToAdd = -(int)today.DayOfWeek;
            return today.AddDays(daysToAdd + date.TimeOfDay.TotalDays + (int)date.DayOfWeek); ;
        }

        public IEnumerable<Appointment> GetAppointments(string specialistId)
        {
             return _apiDbContext.Appointments.Where(a => a.SpecialistId == specialistId);
        }

        public bool CancelAppointment(string reservationCode)
        {
            var appointment = _apiDbContext.Appointments.FirstOrDefault(a => a.ReservationCode.Equals(reservationCode.ToLower()));
            if (appointment == null || appointment.Status != AppointmentStatus.Waiting)
                return false;

            appointment.Status = AppointmentStatus.Cancelled;
            _apiDbContext.SaveChanges();
            return true;
        }
    }
}
