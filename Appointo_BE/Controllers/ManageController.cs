using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Appointo_BE.DTOs;
using Appointo_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Modify a hairdresser
        /// </summary>
        /// <param name="hairdresser">The object of the hairdresser</param>
        /// <returns>The modified hairdresser</returns>
        [HttpPut("Hairdressers")]
        public ActionResult<Hairdresser> PutHairdresser(Hairdresser hairdresser)
        {
            Hairdresser hairdresser2 = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            if (hairdresser.Id != hairdresser2.Id)
                return BadRequest();

            hairdresser2.Name = hairdresser.Name;

            _hairdresserRepository.Update(hairdresser2);
            _hairdresserRepository.SaveChanges();

            return Ok(hairdresser2);
        }

        /// <summary>
        /// Deletes the logged in hairdresser
        /// </summary>
        [HttpDelete("Hairdressers/{id}")]
        public IActionResult DeleteHairdresser()
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            _hairdresserRepository.Delete(hairdresser);
            _hairdresserRepository.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Get all the appointments of the logged in hairdresser
        /// </summary>
        /// <returns>List of appointments</returns>
        [HttpGet("Appointments")]
        public ActionResult<IEnumerable<Appointment>> GetAppointments()
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            return Ok(hairdresser.Appointments.Select(a => new AppointmentDTO() {Id = a.Id, Firstname = a.Firstname,Lastname = a.Lastname, StartMoment = a.StartMoment, Treatments = a.Treatments.Select(tr => tr.Treatment).ToList() }).ToList());
        }

        /// <summary>
        /// Get an appointment of the logged in hairdresser
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The appointment</returns>
        [HttpGet("Appointments/{id}")]
        public ActionResult<AppointmentDTO> GetAppointment(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            Appointment appointment = hairdresser.GetAppointment(id);

            if (appointment == null)
                return NotFound();

            return Ok(new AppointmentDTO() { Id = appointment.Id, Firstname = appointment.Firstname, Lastname = appointment.Lastname, StartMoment = appointment.StartMoment, Treatments = appointment.Treatments.Select(tr => tr.Treatment).ToList() });
        }

        /// <summary>
        /// Delete an appointment of the logged in hairdresser
        /// </summary>
        /// <param name="id"></param>
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

        /// <summary>
        /// Add a new treatment to the logged in hairdresser
        /// </summary>
        /// <param name="treatment">The treatment object</param>
        /// <returns>The created treatment</returns>
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

        /// <summary>
        /// Update a treatment of the logged in haidresser
        /// </summary>
        /// <param name="id">treatment id</param>
        /// <param name="treatment">treatment object</param>
        [HttpPut("Treatments/{id}")]
        public ActionResult<Treatment> PutTreatment(int id, TreatmentDTO treatment)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            if (id != treatment.Id)
                return BadRequest();

            Treatment treatment2 = new Treatment(treatment.Name, new TimeSpan(treatment.Duration.Hours, treatment.Duration.Minutes, treatment.Duration.Seconds), treatment.Category, treatment.Price) { Id = treatment.Id};

            bool result = hairdresser.UpdateTreatment(treatment2);

            if (result == false)
                return NotFound();

            _hairdresserRepository.SaveChanges();
            return NoContent();
        }
        
        /// <summary>
        /// Delete a treatment of a hairdresser
        /// </summary>
        /// <param name="id">The treatment id</param>
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

        /// <summary>
        /// Get the opening hours of the logged in haridresser
        /// </summary>
        /// <returns>The workdays of the hairdreser</returns>
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
        
        /// <summary>
        /// Add a new workday to the logged in hairdresser
        /// </summary>
        /// <param name="workDays">The workday object</param>
        /// <returns>The workdays of the hairdresser</returns>
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

            return Ok(hairdresser.OpeningHours.WorkDays.Select(wd => new WorkDayDTO((int)wd.Day, wd.Hours.ToList())).ToList());
        }

        /// <summary>
        /// Upload a new banner
        /// </summary>
        [HttpPost("UploadBanner"), DisableRequestSizeLimit]
        public IActionResult UploadBanner()
        {

            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            var file = Request.Form.Files[0];
            bool result = AddFile(file, hairdresser, true);

            if (result)
            {
                return Ok();
            } else
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Upload a card image
        /// </summary>
        [Authorize]
        [HttpPost("UploadCardImage"), DisableRequestSizeLimit]
        public ActionResult<string> UploadCardImage()
        {

            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            var file = Request.Form.Files[0];
            bool result = AddFile(file, hairdresser, false);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        private bool AddFile(IFormFile file, Hairdresser hairdresser, bool banner)
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave + "/" + User.Identity.Name))
                {
                    Directory.CreateDirectory(pathToSave + "/" + User.Identity.Name);
                }

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    folderName = Path.Combine("Resources", "Images", User.Identity.Name);
                    pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);


                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    if (banner)
                    {
                        hairdresser.BannerPath = dbPath;
                    } else
                    {
                        hairdresser.CardImagePath = dbPath;
                    }


                    _hairdresserRepository.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}