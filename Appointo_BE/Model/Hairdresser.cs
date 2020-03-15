using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Model
{
    public class Hairdresser
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Treatment> Treatments { get; set; }
        public IList<Appointment> Appointments { get; set; }

        #endregion
    }
}
