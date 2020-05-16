using Appointo_BE.DTOs;
using Appointo_BE.HubConfig;
using Appointo_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Appointo_BE.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HairdressersController : ControllerBase
    {
        private readonly IHairdresserRepository _hairdresserRepository;
        private readonly IHubContext<AppointmentHub> _hub;

        public HairdressersController(IHairdresserRepository repo, IHubContext<AppointmentHub> hub)
        {
            _hairdresserRepository = repo;
            _hub = hub;
        }

        #region Hairdressers
        // GET: api/Hairdressers
        /// <summary>
        /// Get all hairdressers
        /// </summary>
        /// <param name="name">Filter by name</param>
        /// <returns>An array of hairdressers</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Hairdresser>> GetHairdressers(string name = null)
        {
            if (string.IsNullOrEmpty(name))
                return Ok(_hairdresserRepository.GetAll());
            else return Ok(_hairdresserRepository.GetBy(name));
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

            return Ok(hairdresser);
        }

        /// <summary>
        /// Add a new hairdresser
        /// </summary>
        /// <param name="hairdresser">The object of the hairdresser</param>
        /// <returns>The new hairdresser</returns>
        //[HttpPost]
        //public ActionResult<Hairdresser> PostHairdresser(HairdresserDTO hairdresser)
        //{
        //    IList<WorkDay> workDays = new List<WorkDay>();
        //    workDays.Add(new WorkDay(DayOfWeek.Monday, hairdresser.WorkDays.Monday));
        //    workDays.Add(new WorkDay(DayOfWeek.Tuesday, hairdresser.WorkDays.Tuesday));
        //    workDays.Add(new WorkDay(DayOfWeek.Wednesday, hairdresser.WorkDays.Wednesday));
        //    workDays.Add(new WorkDay(DayOfWeek.Thursday, hairdresser.WorkDays.Thursday));
        //    workDays.Add(new WorkDay(DayOfWeek.Friday, hairdresser.WorkDays.Friday));
        //    workDays.Add(new WorkDay(DayOfWeek.Saturday, hairdresser.WorkDays.Saturday));
        //    workDays.Add(new WorkDay(DayOfWeek.Sunday, hairdresser.WorkDays.Sunday));

        //    Hairdresser hairdresserToCreate = new Hairdresser(hairdresser.Name, hairdresser.Email, hairdresser.Treatments, workDays);

        //    foreach (var i in hairdresser.Treatments.ToList())
        //        hairdresserToCreate.AddTreatment(new Treatment(i.Name, new TimeSpan(i.Duration.Hours, i.Duration.Minutes, i.Duration.Seconds), i.Category, i.Price));

        //    _hairdresserRepository.Add(hairdresserToCreate);
        //    _hairdresserRepository.SaveChanges();

        //    return CreatedAtAction(nameof(GetHairdresser), new { hairdresserToCreate.Id }, hairdresserToCreate);
        //}

        /// <summary>
        /// Modify a hairdresser
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <param name="hairdresser">The object of the hairdresser</param>
        /// <returns>The modified hairdresser</returns>
        [HttpPut("{id}")]
        public ActionResult<Hairdresser> PutHairdresser(int id, Hairdresser hairdresser)
        {
            if (id != hairdresser.Id)
                return BadRequest();

            _hairdresserRepository.Update(hairdresser);
            _hairdresserRepository.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Delete a hairdresser
        /// </summary>
        /// <param name="id">The id of the hairdresser to be deleted</param>
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

        [HttpPost("{id}/availabletimes")]
        public ActionResult<IList<DateTime>> GetAvailableTimes(int id, DateTime date, IEnumerable<Treatment> selectedTreatments)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            IList<Treatment> treatments = new List<Treatment>();
            foreach (Treatment tr in selectedTreatments)
                treatments.Add(hairdresser.GetTreatment(tr.Id));

            if (treatments.Count < 1)
                return BadRequest();

            return Ok(hairdresser.GiveAvailableTimesOnDate(date, treatments));
        }

        #region Appointments

        /// <summary>
        /// Get all appointments of a haidresser
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <returns>An array of appointments</returns>
        //[HttpGet("{id}/appointments")]
        //public ActionResult<IEnumerable<Appointment>> GetAppointments(int id)
        //{
        //    Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

        //    if (hairdresser == null)
        //        return NotFound();

        //    return Ok(hairdresser.Appointments.Select(a => new AppointmentDTO() { StartMoment = a.StartMoment, Treatments = a.Treatments.Select(tr => tr.Treatment).ToList() }).ToList());
        //}

        /// <summary>
        /// Get an appointment of a haidresser
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <param name="appointmentId">The id of the appointment</param>
        /// <returns>The appointment</returns>
        //[HttpGet("{id}/appointments/{appointmentId}")]
        //public ActionResult<Appointment> GetAppointment(int id, int appointmentId)
        //{
        //    Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

        //    if (hairdresser == null)
        //        return NotFound();

        //    Appointment appointment = hairdresser.GetAppointment(appointmentId);

        //    if (appointment == null)
        //        return NotFound();

        //    return Ok(appointment);
        //}

        /// <summary>
        /// Add a new appointment to a hairdresser
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <param name="appointment">The object of the appointment</param>
        /// <returns>The new appointment</returns>
        [HttpPost("{id}/appointments")]
        public ActionResult<AppointmentDTO> PostAppointment(int id, AppointmentDTO appointment)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            if (appointment.StartMoment < DateTime.Now)
                return BadRequest();

            IList<Treatment> treatments = new List<Treatment>();
            foreach (Treatment tr in appointment.Treatments)
                treatments.Add(hairdresser.GetTreatment(tr.Id));

            Appointment appointmentToCreate = new Appointment(treatments, appointment.StartMoment, appointment.Firstname, appointment.Lastname);

            bool result = hairdresser.AddAppointment(appointmentToCreate);

            if (result == false)
                return BadRequest();

            _hairdresserRepository.SaveChanges();

            _hub.Clients.All.SendAsync("appointments", hairdresser.Appointments);

            return Ok(new AppointmentDTO() { StartMoment = appointmentToCreate.StartMoment, Treatments = appointmentToCreate.Treatments.Select(tr => tr.Treatment).ToList() }); // CreatedAtAction() not possible --> bug
        }

        /// <summary>
        /// Delete an appointment of a hairdresser
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <param name="appointmentId">The id of the appointment to be deleted</param>
        //[HttpDelete("{id}/appointments/{appointmentId}")]
        //public IActionResult DeleteAppointment(int id, int appointmentId)
        //{
        //    Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

        //    if (hairdresser == null)
        //        return NotFound();


        //    Appointment appointment = hairdresser.GetAppointment(appointmentId);

        //    hairdresser.RemoveAppointment(appointment);

        //    _hairdresserRepository.SaveChanges();

        //    return NoContent();
        //}

        #endregion

        #region Treatments

        /// <summary>
        /// Get all treatments of a hairdresser
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <returns>An array of treatments</returns>
        [HttpGet("{id}/treatments")]
        public ActionResult<IEnumerable<Treatment>> GetTreatments(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            return Ok(hairdresser.Treatments);
        }

        /// <summary>
        /// Get all treatments of a hairdresser
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <returns>An array of treatments</returns>
        [Authorize]
        [HttpGet("treatments")]
        public ActionResult<IEnumerable<Treatment>> GetTreatmentsWithoutId()
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            return Ok(hairdresser.Treatments);
        }

        /// <summary>
        /// Get a treatment of a hairdresser
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <param name="treatmentId">The id of the treatment</param>
        /// <returns>The treatment</returns>
        [HttpGet("{id}/treatments/{treatmentId}")]
        public ActionResult<Treatment> GetTreatment(int id, int treatmentId)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            Treatment treatment = hairdresser.GetTreatment(treatmentId);

            if (treatment == null)
                return NotFound();

            return Ok(treatment);
        }

        [HttpGet("treatments/{id}")]
        public ActionResult<Treatment> GetTreatmentWithoutId(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            return GetTreatment(hairdresser.Id, id);
        }

        /// <summary>
        /// Add a new treatment to a hairdresser
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <param name="treatment">The object of the treatment</param>
        /// <returns>The new treatment</returns>
        [HttpPost("{id}/treatments")]
        public ActionResult<Treatment> PostTreatment(int id, TreatmentDTO treatment)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);

            if (hairdresser == null)
                return NotFound();

            Treatment treatmenToCreate = new Treatment(treatment.Name, new TimeSpan(treatment.Duration.Hours, treatment.Duration.Minutes, treatment.Duration.Seconds), treatment.Category, treatment.Price);

            hairdresser.AddTreatment(treatmenToCreate);

            _hairdresserRepository.SaveChanges();

            return Ok(treatmenToCreate); // CreatedAtAction() not possible --> bug
        }

        /// <summary>
        /// Modify a treatment of a hairdresser
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <param name="treatmentId">The id of the treatment</param>
        /// <param name="treatment">The object of the treatment</param>
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

        /// <summary>
        /// Delete a treatment of a hairdresser
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <param name="treatmentId">The id of the treatment to be deleted</param>
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