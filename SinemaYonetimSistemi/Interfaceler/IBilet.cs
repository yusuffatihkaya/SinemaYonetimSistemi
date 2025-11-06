using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinemaYonetimSistemi.Interfaceler
{
    internal interface IBilet: IFilm, ISalon
    {
        public int BiletID { get; set; }
        public string MusteriAdi { get; set; }
        public string MusteriSoyadi { get; set; }
        public IFilm Film { get; set; }
        public ISalon Salon { get; set; }
        public string Koltuk { get; set; }
        public string Odeme { get; set; }
        public double Fiyat { get; set; }
    }
}
