using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using task.api.Models;
using task.shared;

namespace task.api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(IAppointmentRepository appointmentRepository, ILogger<AppointmentController> logger)
        {
            _appointmentRepository = appointmentRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{reservationCode}")]
        public ActionResult<TimeSpan> CheckAppointmentTimeLeft(string reservationCode)
        {
            return Ok(_appointmentRepository.CheckAppointmentTimeLeft(reservationCode));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> GetAppointments()
        {
            return Ok(_appointmentRepository.GetAppointments(User.Identity.Name));
        }

        [HttpGet("displayboard")]
        public ActionResult<IEnumerable<Appointment>> GetDisplayBoardAppointments()
        {
            return Ok(_appointmentRepository.GetAppointments());
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult NewAppointment(Appointment appointment)
        {
            try
            {
                _appointmentRepository.NewAppointment(appointment);
                appointment.Specialist = null;
                return Created($"/api/appointment/{appointment.AppointmentId}", appointment);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to book an appointment: {e}");
            }

            return BadRequest($"Failed to book an appointment");
        }

        [AllowAnonymous]
        [HttpPut("{reservationCode}/cancel")]
        public IActionResult CancelAppointment(string reservationCode)
        {
            try
            {
                if(_appointmentRepository.CancelAppointment(reservationCode))
                    return Ok();
                return BadRequest("Appointment doesn't exist or is already cancelled");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to cancel appointment: {e}");
            }

            return BadRequest($"Failed to cancel appointment");
        }

        [HttpPut("{reservationCode}/start")]
        public IActionResult StartAppointment(string reservationCode)
        {
            try
            {
                if (_appointmentRepository.StartAppointment(reservationCode))
                    return Ok();
                return BadRequest("Failed to start an appointment");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to start an appointment: {e}");
            }

            return BadRequest($"Failed to start an appointment");
        }

        [HttpPut("{reservationCode}/end")]
        public IActionResult EndAppointment(string reservationCode)
        {
            try
            {
                if (_appointmentRepository.EndAppointment(reservationCode))
                    return Ok();
                return BadRequest("Failed to end an appointment");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to end an appointment: {e}");
            }

            return BadRequest($"Failed to end an appointment");
        }
    }
}
