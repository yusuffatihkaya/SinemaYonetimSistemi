using SinemaYonetimSistemi.Interfaceler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinemaYonetimSistemi.Siniflar
{
    public class Film : IFilm
    {
        public int FilmID { get; set; }
        public string FilmAdi { get; set; }
        public double FilmFiyat { get; set; }
        public string FilmKategori { get; set; }
        public int FilmSure { get; set; }
        public int FilmImdb { get; set; }
        public List<string> FilmSeanslar { get; set; }
        public string FilmFotograf { get; set; }
        public ISalon FilmSalon { get; set; }
        public int SalonID { get; set; }
        public string SalonAdi { get; set; }
        public int SalonSiraSayisi { get; set; }
        public int SalonSiraKoltukSayisi { get; set; }

        public Film()
        {
            //
        }

    }


}
