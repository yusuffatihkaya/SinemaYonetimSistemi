using SinemaYonetimSistemi.Interfaceler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinemaYonetimSistemi.Siniflar
{
    internal class Salon : ISalon
    {
        public int SalonID { get ; set ; }
        public string SalonAdi { get; set; }
        public int SalonSiraSayisi { get; set; }
        public int SalonSiraKoltukSayisi { get; set; }
    }
}
