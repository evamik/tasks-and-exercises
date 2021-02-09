using System;
using System.Collections;
using System.Collections.Generic;
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

        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> GetAppointments()
        {
            return Ok(_appointmentRepository.GetAppointments());
        }

        [HttpGet("{id}")]
        public ActionResult<Appointment> GetAppointmentById(int id)
        {
            return _appointmentRepository.GetAppointment(id);
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
    }
}
