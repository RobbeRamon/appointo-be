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
        /// Get the logged in hairdresser
        /// </summary>
        /// <returns>The hairdresser</returns>
        [Authorize]
        [HttpGet("loggedInUser")]
        public ActionResult<Hairdresser> GetHairdresserWithoutId()
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            return Ok(hairdresser);
        }

        #endregion

        /// <summary>
        /// Get the avaiable times for an appointment
        /// </summary>
        /// <param name="id">The id of the hairdresser</param>
        /// <param name="date">The date of the appointment</param>
        /// <param name="selectedTreatments">The selected treatments</param>
        /// <returns></returns>
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

            _hub.Clients.All.SendAsync("appointments", hairdresser.Appointments.Select(a => new AppointmentDTO() { Id = a.Id, Firstname = a.Firstname, Lastname = a.Lastname, StartMoment = a.StartMoment, Treatments = a.Treatments.Select(tr => tr.Treatment).ToList() }).ToList());

            return Ok(new AppointmentDTO() { StartMoment = appointmentToCreate.StartMoment, Treatments = appointmentToCreate.Treatments.Select(tr => tr.Treatment).ToList() }); // CreatedAtAction() not possible --> bug
        }

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
        /// Get all treatments of the logged in hairdresser
        /// </summary>
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

        /// <summary>
        /// Get a treatment of the logged in hairdresser
        /// </summary>
        /// <param name="id">The id of the treatment</param>
        /// <returns>The treatment</returns>
        [Authorize]
        [HttpGet("treatments/{id}")]
        public ActionResult<Treatment> GetTreatmentWithoutId(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            return GetTreatment(hairdresser.Id, id);
        }

        #endregion
    }
}