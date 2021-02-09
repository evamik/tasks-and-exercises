using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using task.shared;

namespace task.api.Models
{
    public interface IAppointmentSpecialistRepository
    {
        IEnumerable<AppointmentSpecialist> GetAppointmentSpecialists();
        AppointmentSpecialist GetAppointmentSpecialist(int id);
    }
}
