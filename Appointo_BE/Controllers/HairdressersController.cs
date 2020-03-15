using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointo_BE.Model;
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
    }
}