using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointo_BE.DTOs;
using Appointo_BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointo_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HairdressersController : ControllerBase
    {
        private readonly IHairdresserRepository _hairdresserRepository;

        public HairdressersController(IHairdresserRepository repo)
        {
            _hairdresserRepository = repo;
        }

        [HttpGet]
        public IEnumerable<Hairdresser> GetHairdressers(string name = null, string location = null)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(location))
                return _hairdresserRepository.GetAll();
            else return _hairdresserRepository.GetBy(name, location);
        }

        [HttpGet("{id}")]
        public ActionResult<Hairdresser> GetHairdresser(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            return hairdresser;
        }

        [HttpPost]
        public ActionResult<Hairdresser> PostHairdresser(HairdresserDTO hairdresser)
        {
            IList<WorkDay> workDays = new List<WorkDay>();
            workDays.Add(new WorkDay(DayOfWeek.Monday, hairdresser.WorkDays.Monday));
            workDays.Add(new WorkDay(DayOfWeek.Tuesday, hairdresser.WorkDays.Tuesday));
            workDays.Add(new WorkDay(DayOfWeek.Wednesday, hairdresser.WorkDays.Wednesday));
            workDays.Add(new WorkDay(DayOfWeek.Thursday, hairdresser.WorkDays.Thursday));
            workDays.Add(new WorkDay(DayOfWeek.Friday, hairdresser.WorkDays.Friday));
            workDays.Add(new WorkDay(DayOfWeek.Saturday, hairdresser.WorkDays.Saturday));
            workDays.Add(new WorkDay(DayOfWeek.Sunday, hairdresser.WorkDays.Sunday));

            Hairdresser hairdresserToCreate = new Hairdresser(hairdresser.Name, hairdresser.Treatments, workDays);

            foreach (var i in hairdresser.Treatments)
                hairdresserToCreate.AddTreatment(new Treatment(i.Name, new TimeSpan(i.Duration.Hours, i.Duration.Minutes, i.Duration.Seconds)));

            _hairdresserRepository.Add(hairdresserToCreate);
            _hairdresserRepository.SaveChanges();

            return CreatedAtAction(nameof(GetHairdresser), new { hairdresserToCreate.Id }, hairdresserToCreate);
        }

        [HttpPut("{id}")]
        public ActionResult<Hairdresser> PutHairdresser(int id, Hairdresser hairdresser)
        {
            if (id != hairdresser.Id)
                return BadRequest();

            _hairdresserRepository.Update(hairdresser);
            _hairdresserRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteHairdresser(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            _hairdresserRepository.Delete(hairdresser);
            _hairdresserRepository.SaveChanges();

            return NoContent();
        }

        [HttpGet("{id}/appointments/{appointmentId}")]
        public ActionResult<Appointment> GetAppointment(int id, int appointmentId)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            Appointment appointment = hairdresser.GetAppointment(appointmentId);

            if (appointment == null)
                return NotFound();

            return appointment;
        }

        [HttpPost("{id}/appointments")]
        public ActionResult<Appointment> PostAppointment(int id, AppointmentDTO appointment)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            IList<Treatment> treatments = new List<Treatment>();
            foreach (int treatmentId in appointment.TreatmentIds)
                treatments.Add(hairdresser.GetTreatment(treatmentId));

            Appointment appointmentToCreate = new Appointment(treatments);

            bool result = hairdresser.AddAppointment(appointmentToCreate);

            if (result == false)
                return BadRequest();

            _hairdresserRepository.SaveChanges();

            return CreatedAtAction(nameof(Appointment), new { appointmentToCreate.Id }, appointmentToCreate);
        }
    }
}   