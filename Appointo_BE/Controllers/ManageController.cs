using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Appointo_BE.DTOs;
using Appointo_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Appointo_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManageController : ControllerBase
    {
        private readonly IHairdresserRepository _hairdresserRepository;

        public ManageController(IHairdresserRepository repo)
        {
            this._hairdresserRepository = repo;
        }

        [HttpGet("appointments")]
        public ActionResult<IEnumerable<Appointment>> GetAppointments()
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            return Ok(hairdresser.Appointments.Select(a => new AppointmentDTO() { StartMoment = a.StartMoment, Treatments = a.Treatments.Select(tr => tr.Treatment).ToList() }).ToList());
        }

        [HttpGet("Appointments/{id}")]
        public ActionResult<Appointment> GetAppointment(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            Appointment appointment = hairdresser.GetAppointment(id);

            if (appointment == null)
                return NotFound();

            return Ok(appointment);
        }

        [HttpDelete("Appointments/{id}")]
        public IActionResult DeleteAppointemnt(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            Appointment appointment = hairdresser.GetAppointment(id);

            hairdresser.RemoveAppointment(appointment);

            _hairdresserRepository.SaveChanges();

            return NoContent();
        }

        [HttpPost("Treatments")]
        public ActionResult<Treatment> PostTreatment(TreatmentDTO treatment)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            Treatment treatmenToCreate = new Treatment(treatment.Name, new TimeSpan(treatment.Duration.Hours, treatment.Duration.Minutes, treatment.Duration.Seconds), treatment.Category, treatment.Price);

            hairdresser.AddTreatment(treatmenToCreate);

            _hairdresserRepository.SaveChanges();

            return Ok(treatmenToCreate); 
        }

        [HttpPut("Treatments/{id}")]
        public ActionResult<Treatment> PutTreatment(int id, Treatment treatment)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            if (id != treatment.Id)
                return BadRequest();

            bool result = hairdresser.UpdateTreatment(treatment);

            if (result == false)
                return NotFound();

            _hairdresserRepository.SaveChanges();
            return NoContent();
        }

        [HttpDelete("Treatments/{id}")]
        public ActionResult DeleteTreatment(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            Treatment treatment = hairdresser.GetTreatment(id);

            if (treatment == null)
                return NotFound();

            hairdresser.RemoveTreatment(treatment);
            _hairdresserRepository.SaveChanges();

            return NoContent();
        }

        [HttpGet("Workdays")]
        public ActionResult<List<WorkDay>> GetWorkDays()
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            List<WorkDayDTO> workDays = new List<WorkDayDTO>();
            
            foreach (WorkDay workDay in hairdresser.OpeningHours.WorkDays)
            {
                workDays.Add(new WorkDayDTO((int)workDay.Day, workDay.Hours.ToList()));
            }

            return Ok(workDays);
        }
        

        [HttpPost("Workdays")]
        public ActionResult<List<WorkDay>> PostWorkDays(IList<WorkDayDTO> workDays)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            if (workDays.Any(wd => wd.DayId > 6 || wd.DayId < 0))
                return BadRequest();

 
            foreach (WorkDayDTO workDay in workDays) {
                hairdresser.ChangeWorkDays(workDay.DayId, workDay.Hours);
            }

            _hairdresserRepository.SaveChanges();

            return Ok(hairdresser.OpeningHours.WorkDays);
        }
        
    }
}