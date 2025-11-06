using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinemaYonetimSistemi.Interfaceler
{
    public interface IFilm: ISalon
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
    }
}
