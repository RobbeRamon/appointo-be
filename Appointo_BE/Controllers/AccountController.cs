using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Appointo_BE.DTOs;
using Appointo_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Appointo_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHairdresserRepository _hairdresserRepository;
        private readonly IConfiguration _config;

        public AccountController(
          SignInManager<IdentityUser> signInManager,
          UserManager<IdentityUser> userManager,
          IHairdresserRepository hairdresserRepository,
          IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _hairdresserRepository = hairdresserRepository;
            _config = config;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model">the login details</param>
        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<ActionResult<String>> CreateToken(LoginDTO model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.Email);

        //    if (user != null)
        //    {
        //        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        //        if (result.Succeeded)
        //        {
        //            string token = GetToken(user);
        //            return Created("", token); //returns only the token                    
        //        }
        //    }
        //    return BadRequest();
        //}

        /// <summary>
        /// Register a user
        /// </summary>
        /// <param name="model">the user details</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<String>> Register(RegisterHairdresserDTO hairdresser)
        {
            IdentityUser user = new IdentityUser { UserName = hairdresser.Email, Email = hairdresser.Email };
            IList<WorkDay> workDays = new List<WorkDay>();
            workDays.Add(new WorkDay(DayOfWeek.Monday, hairdresser.WorkDays.Monday));
            workDays.Add(new WorkDay(DayOfWeek.Tuesday, hairdresser.WorkDays.Tuesday));
            workDays.Add(new WorkDay(DayOfWeek.Wednesday, hairdresser.WorkDays.Wednesday));
            workDays.Add(new WorkDay(DayOfWeek.Thursday, hairdresser.WorkDays.Thursday));
            workDays.Add(new WorkDay(DayOfWeek.Friday, hairdresser.WorkDays.Friday));
            workDays.Add(new WorkDay(DayOfWeek.Saturday, hairdresser.WorkDays.Saturday));
            workDays.Add(new WorkDay(DayOfWeek.Sunday, hairdresser.WorkDays.Sunday));

            Hairdresser hairdresserToCreate = new Hairdresser(hairdresser.Name, hairdresser.Email, hairdresser.Treatments.Select(tr => new Treatment(tr.Name, new TimeSpan(tr.Duration.Hours, tr.Duration.Minutes, tr.Duration.Seconds), tr.Category, tr.Price)).ToList(), workDays);

            foreach (var i in hairdresser.Treatments.ToList())
                hairdresserToCreate.AddTreatment(new Treatment(i.Name, new TimeSpan(i.Duration.Hours, i.Duration.Minutes, i.Duration.Seconds), i.Category, i.Price));

            var result = await _userManager.CreateAsync(user, hairdresser.Password);

            if (result.Succeeded)
            {
                _hairdresserRepository.Add(hairdresserToCreate);
                _hairdresserRepository.SaveChanges();
                string token = GetToken(user);
                return Created("", token);
            }
            return BadRequest();
        }

        /// <summary>
        /// Checks if an email is available as username
        /// </summary>
        /// <returns>true if the email is not registered yet</returns>
        /// <param name="email">Email.</param>/
        [AllowAnonymous]
        [HttpGet("checkusername")]
        public async Task<ActionResult<Boolean>> CheckAvailableUserName(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            return user == null;
        }

        private String GetToken(IdentityUser user)
        {
            // Create the token
            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, user.Email),
              new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              null, null,
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
