using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinemaYonetimSistemi.Siniflar
{
    internal class Kontrol
    {
        public bool KontrolString(string gelenDeger, string gelenDegerIsim)
        {
            gelenDeger = gelenDeger.Trim();
            if(gelenDeger.Length < 3)
            {
                MessageBox.Show(gelenDegerIsim + " en az (3) karakter girilmelidir.");
                return false;
            }
            if(gelenDeger.Length > 50)
            {
                MessageBox.Show(gelenDegerIsim + " en fazla (50) karakter girilmelidir.");
                return false;
            }
            if (gelenDeger.IndexOf(";") != -1) {
                MessageBox.Show(gelenDegerIsim + " ';' karakter içeremez.");
                return false;
            }
            return true;
        }
    }
}
