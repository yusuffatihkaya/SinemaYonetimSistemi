using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinemaYonetimSistemi.Interfaceler
{
    public interface ISalon
    {
        public int SalonID { get; set; }
        public string SalonAdi { get; set; }
        public int SalonSiraSayisi { get; set; }
        public int SalonSiraKoltukSayisi { get; set; }

    }
}
