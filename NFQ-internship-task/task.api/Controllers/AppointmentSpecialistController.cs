using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using task.api.Models;
using task.shared;

namespace task.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentSpecialistController : Controller
    {
        private readonly IAppointmentSpecialistRepository _appointmentSpecialistRepository;
        private readonly ILogger<AppointmentSpecialistController> _logger;

        public AppointmentSpecialistController(IAppointmentSpecialistRepository appointmentSpecialistRepository, ILogger<AppointmentSpecialistController> logger)
        {
            _appointmentSpecialistRepository = appointmentSpecialistRepository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AppointmentSpecialist>> GetAppointmentSpecialists()
        {
            try
            {
                _logger.LogInformation(_appointmentSpecialistRepository.GetAppointmentSpecialists().First().Login);
                return Ok(_appointmentSpecialistRepository.GetAppointmentSpecialists());
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get appointment specialists: {e}");
                return BadRequest("Failed to get appointment specialists");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<AppointmentSpecialist> GetAppointmentSpecialistById(int id)
        {
            try
            {
                _logger.LogInformation(_appointmentSpecialistRepository.GetAppointmentSpecialist(id).AppointmentSpecialistId.ToString());
                return _appointmentSpecialistRepository.GetAppointmentSpecialist(id);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get an appointment specialist: {e}");
                return BadRequest("Failed to get an appointment specialist");
            }
        }
    }
}
