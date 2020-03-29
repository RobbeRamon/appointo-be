﻿using System;
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

        #region Hairdressers
        // GET: api/Hairdressers
        /// <summary>
        /// Get all hairdressers
        /// </summary>
        /// <param name="name">Filter by name</param>
        /// <returns>Array of hairdressers</returns>
        [HttpGet]
        public IEnumerable<Hairdresser> GetHairdressers(string name = null)
        {
            if (string.IsNullOrEmpty(name))
                return _hairdresserRepository.GetAll();
            else return _hairdresserRepository.GetBy(name);
        }

        // GET: api/Hairdressers/1
        /// <summary>
        /// Get the hairdresser with the given id
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <returns>The hairdresser</returns>
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

        #endregion

        #region Appointments

        [HttpGet("{id}/appointments")]
        public ActionResult<IEnumerable<Appointment>> GetAppointments(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            return Ok(hairdresser.Appointments.Select(a => new AppointmentDTO() { StartMoment = a.StartMoment, Treatments = a.Treatments.Select(tr => tr.Treatment).ToList() }).ToList());
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
        public ActionResult<AppointmentDTO> PostAppointment(int id, AppointmentDTO appointment)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            IList<Treatment> treatments = new List<Treatment>();
            foreach (Treatment tr in appointment.Treatments)
                treatments.Add(hairdresser.GetTreatment(tr.Id));

            Appointment appointmentToCreate = new Appointment(treatments, appointment.StartMoment);

            bool result = hairdresser.AddAppointment(appointmentToCreate);

            if (result == false)
                return BadRequest();

            _hairdresserRepository.SaveChanges();


            return new AppointmentDTO() { StartMoment = appointmentToCreate.StartMoment, Treatments = appointmentToCreate.Treatments.Select(tr => tr.Treatment).ToList() };
        }



        [HttpDelete("{id}/appointments/{appointmentId}")]
        public IActionResult DeleteAppointment(int id, int appointmentId)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();


            Appointment appointment = hairdresser.GetAppointment(appointmentId);

            hairdresser.RemoveAppointment(appointment);

            _hairdresserRepository.SaveChanges();

            return NoContent();
        }

        #endregion

        #region Treatments

        [HttpGet("{id}/treatments")]
        public ActionResult<IEnumerable<Treatment>> GetTreatments(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            return Ok(hairdresser.Treatments);
        }

        [HttpGet("{id}/treatments/{treatmentId}")]
        public ActionResult<Treatment> GetTreatment(int id, int treatmentId)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            Treatment treatment = hairdresser.GetTreatment(treatmentId);

            if (treatment == null)
                return NotFound();

            return treatment;
        }

        [HttpPost("{id}/treatments")]
        public ActionResult<Treatment> PostTreatment(int id, TreatmentDTO treatment)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            Treatment treatmenToCreate = new Treatment(treatment.Name, new TimeSpan(treatment.Duration.Hours, treatment.Duration.Minutes, treatment.Duration.Seconds));

            hairdresser.AddTreatment(treatmenToCreate);

            _hairdresserRepository.SaveChanges();

            return treatmenToCreate;
        }

        [HttpPut("{id}/treatments/{treatmentId}")]
        public ActionResult<Treatment> PutTreatment(int id, int treatmentId, Treatment treatment)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            if (treatmentId != treatment.Id)
                return BadRequest();

            bool result = hairdresser.UpdateTreatment(treatment);

            if (result == false)
                return NotFound();

            _hairdresserRepository.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}/treatments/{treatmentId}")]
        public IActionResult DeleteTreatment(int id, int treatmentId)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            Treatment treatment = hairdresser.GetTreatment(treatmentId);

            if (treatment == null)
                return NotFound();

            hairdresser.RemoveTreatment(treatment);
            _hairdresserRepository.SaveChanges();

            return NoContent();
        }

        #endregion
    }
}   