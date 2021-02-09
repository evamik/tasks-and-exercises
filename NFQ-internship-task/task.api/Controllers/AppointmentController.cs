using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using task.api.Models;
using task.shared;

namespace task.api.Controllers
{
    [Route("api/[controller]")]
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
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> GetAppointments()
        {
            return Ok(_appointmentRepository.GetAppointments(User.Identity.Name));
        }

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

        [HttpPut("cancel/{reservationCode}")]
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
    }
}
