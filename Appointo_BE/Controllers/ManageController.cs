using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
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

        [HttpGet("Appointments")]
        public ActionResult<IEnumerable<Appointment>> GetAppointments()
        {
            Hairdresser hairdresser = _hairdresserRepository.GetByEmail(User.Identity.Name);

            if (hairdresser == null)
                return NotFound();

            return Ok(hairdresser.Appointments.Select(a => new AppointmentDTO() {Id = a.Id, Firstname = a.Firstname,Lastname = a.Lastname, StartMoment = a.StartMoment, Treatments = a.Treatments.Select(tr => tr.Treatment).ToList() }).ToList());
        }

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

            return Ok(hairdresser.OpeningHours.WorkDays.Select(wd => new WorkDayDTO((int)wd.Day, wd.Hours.ToList())).ToList());
        }

        [Authorize]
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

            //try
            //{
            //    var file = Request.Form.Files[0];
            //    var folderName = Path.Combine("Resources", "Images");
            //    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            //    if (!Directory.Exists(pathToSave + "/" + User.Identity.Name))
            //    {
            //        Directory.CreateDirectory(pathToSave + "/" + User.Identity.Name);
            //    }

            //    if (file.Length > 0)
            //    {
            //        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            //        folderName = Path.Combine("Resources", "Images", User.Identity.Name);
            //        pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //        var fullPath = Path.Combine(pathToSave, fileName);
            //        var dbPath = Path.Combine(folderName, fileName);


            //        using (var stream = new FileStream(fullPath, FileMode.Create))
            //        {
            //            file.CopyTo(stream);
            //        }

            //        return Ok(fullPath);
            //    } else
            //    {
            //        return BadRequest();
            //    }
            //}
            //catch (Exception e)
            //{
            //    return BadRequest();
            //}
        }

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