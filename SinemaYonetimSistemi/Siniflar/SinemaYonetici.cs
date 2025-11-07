using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinemaYonetimSistemi.Siniflar
{
    internal class SinemaYonetici
    {
        public int _yeniFilmID = Properties.Settings.Default.sonFilmID;
        public int _yeniSalonID = Properties.Settings.Default.sonSalonID;

        public List<Film> _filmler;

        public List<Salon> _salonlar;

        public SinemaYonetici() 
        {
            _filmler = FilmleriOku();
            _salonlar = SalonlariOku();
        }

        public List<Film> FilmleriOku()
        {
            var filmler = new List<Film>();
            try
            {
                string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
                string dosyaYolu = Path.Combine(kayitDizini, "filmler.txt");                
                if (File.Exists(dosyaYolu) == true)
                {
                    using (var streamOkuyucu = new StreamReader(dosyaYolu, Encoding.UTF8))
                    {
                        string satir = string.Empty;
                        Film film = new Film();
                        while ((satir = streamOkuyucu.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(satir))
                                continue;
                            var satirParcalari = satir.Split(';', StringSplitOptions.RemoveEmptyEntries);
                            foreach (var parca in satirParcalari)
                            {
                                string ozellikAdi = parca.Split('=', 2)[0];
                                string ozellikDeger = parca.Split('=', 2)[1];
                                if (ozellikAdi == "FilmID")
                                {
                                    film.FilmID = Convert.ToInt32(ozellikDeger);
                                }
                                if (ozellikAdi == "FilmAdi")
                                {
                                    film.FilmAdi = ozellikDeger;
                                }
                                if (ozellikAdi == "FilmFiyat")
                                {
                                    film.FilmFiyat = Convert.ToDouble(ozellikDeger);
                                }
                                if (ozellikAdi == "FilmKategori")
                                {
                                    film.FilmKategori = ozellikDeger;
                                }
                                if (ozellikAdi == "FilmSure")
                                {
                                    film.FilmSure = Convert.ToInt32(ozellikDeger);
                                }
                                if (ozellikAdi == "FilmImdb")
                                {
                                    film.FilmImdb = Convert.ToInt32(ozellikDeger);
                                }
                                if (ozellikAdi == "FilmFotograf")
                                {
                                    film.FilmFotograf = ozellikDeger;
                                }
                                if (ozellikDeger == "FilmSalon")
                                {
                                    //
                                }
                                if (ozellikAdi == "SalonID")
                                {
                                    //
                                }
                                if (ozellikAdi == "SalonAdi")
                                {
                                    //
                                }
                                if (ozellikAdi == "SalonSiraSayisi")
                                {
                                    //
                                }
                                if (ozellikAdi == "SalonSiraKoltukSayisi")
                                {
                                    //
                                }
                            }
                            filmler.Add(film);
                        }
                    }
                }
                _filmler = filmler;
            }
            catch  { }
            return filmler;
        }

        public List<int> FilmSeansOku(int filmID)
        {
            var seanslar = new List<int>();
            try
            {
                string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
                string dosyaYolu = Path.Combine(kayitDizini, "seanslar.txt");
                if (File.Exists(dosyaYolu) == true)
                {
                    using (var streamOkuyucu = new StreamReader(dosyaYolu, Encoding.UTF8))
                    {
                        string satir = string.Empty;                       
                        while ((satir = streamOkuyucu.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(satir))
                                continue;
                            var satirParcalari = satir.Split(';', StringSplitOptions.RemoveEmptyEntries);
                            Console.WriteLine(satirParcalari);
                            ///
                        }
                    }
                }
            }
            catch { }
            return seanslar;
        }
        public bool FilmEkle(Film yeniFilm)
        {
            try
            {
                string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
                string dosyaYolu = Path.Combine(kayitDizini, "filmler.txt");
                Directory.CreateDirectory(kayitDizini);
                using (var streamYazici = new StreamWriter(dosyaYolu, append: true, Encoding.UTF8))
                {
                    string yeniSatir = "FilmID=" + _yeniFilmID.ToString() + ";FilmAdi=" + yeniFilm.FilmAdi.ToString() + ";FilmFiyat=" + yeniFilm.FilmFiyat.ToString() + ";FilmKategori=" + yeniFilm.FilmKategori
                        + ";FilmSure=" + yeniFilm.FilmSure.ToString() + ";FilmImdb=" + yeniFilm.FilmImdb.ToString() + ";FilmFotograf=" + yeniFilm.FilmFotograf.ToString() + ";";
                    streamYazici.WriteLine(yeniSatir);
                }
                if (FilmSeansEkle(_yeniFilmID, yeniFilm.FilmSeanslar))
                {
                    FilmleriOku();
                    Properties.Settings.Default.sonFilmID++;
                    _yeniFilmID = Properties.Settings.Default.sonFilmID;
                    Properties.Settings.Default.Save(); 
                    return true;
                }
                else
                    return false;
            }
            catch { 
                return false;
            }
        }
        public bool FilmSeansEkle(int yeniFilmID, List<string> yeniFilmSeanslar)
        {
            try
            {
                string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
                string dosyaYolu = Path.Combine(kayitDizini, "seanslar.txt");
                Directory.CreateDirectory(kayitDizini);
                using (var streamYazici = new StreamWriter(dosyaYolu, append: true, Encoding.UTF8))
                {
                    string yeniSatir = "FilmID=" + yeniFilmID.ToString();
                    for (int i = 0; i < yeniFilmSeanslar.Count; i++)
                    {
                        yeniSatir += ";" + yeniFilmSeanslar[i];
                    }
                    streamYazici.WriteLine(yeniSatir);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Salon> SalonlariOku()
        {
            var salonlar = new List<Salon>();
            try
            {
                string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
                string dosyaYolu = Path.Combine(kayitDizini, "salonlar.txt");
                if (File.Exists(dosyaYolu) == true)
                {
                    using (var streamOkuyucu = new StreamReader(dosyaYolu, Encoding.UTF8))
                    {
                        string satir = string.Empty;
                        while ((satir = streamOkuyucu.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(satir))
                                continue;
                            Salon salon = new Salon();
                            var satirParcalari = satir.Split(';', StringSplitOptions.RemoveEmptyEntries);
                            foreach (var parca in satirParcalari)
                            {
                                string ozellikAdi = parca.Split('=', 2)[0];
                                string ozellikDeger = parca.Split('=', 2)[1];
                                if (ozellikAdi == "SalonID")
                                {
                                    salon.SalonID = Convert.ToInt32(ozellikDeger);
                                }
                                if (ozellikAdi == "SalonAdi")
                                {
                                    salon.SalonAdi = ozellikDeger;
                                }
                                if (ozellikAdi == "SalonSiraSayisi")
                                {
                                    salon.SalonSiraSayisi = Convert.ToInt32(ozellikDeger);
                                }
                                if (ozellikAdi == "SalonSiraKoltukSayisi")
                                {
                                    salon.SalonSiraKoltukSayisi = Convert.ToInt32(ozellikDeger);
                                }                               
                            }
                            salonlar.Add(salon);
                        }
                    }
                }
                _salonlar = salonlar;
            }
            catch { }
            return salonlar;
        }
        public bool SalonEkle(Salon yeniSalon)
        {
            try
            {
                string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
                string dosyaYolu = Path.Combine(kayitDizini, "salonlar.txt");
                Directory.CreateDirectory(kayitDizini);
                using (var streamYazici = new StreamWriter(dosyaYolu, append: true, Encoding.UTF8))
                {
                    string yeniSatir = "SalonID=" + _yeniSalonID.ToString() + ";SalonAdi=" + yeniSalon.SalonAdi.ToString() + ";SalonSiraSayisi=" + yeniSalon.SalonSiraSayisi.ToString() + ";SalonSiraKoltukSayisi=" + yeniSalon.SalonSiraKoltukSayisi + ";";
                    streamYazici.WriteLine(yeniSatir);
                }
                SalonlariOku();
                Properties.Settings.Default.sonSalonID++;
                _yeniSalonID = Properties.Settings.Default.sonSalonID;
                Properties.Settings.Default.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public bool SalonSil(int salonID)
        {
            try
            {
                string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
                string dosyaYolu = Path.Combine(kayitDizini, "salonlar.txt");
                if (File.Exists(dosyaYolu) == true)
                {
                    var satirlar = File.ReadAllLines(dosyaYolu).ToList();
                    satirlar.RemoveAll(line =>
                    {
                        if (string.IsNullOrWhiteSpace(line)) return false;
                        var parcalar = line.Split(';', StringSplitOptions.RemoveEmptyEntries);

                        foreach (var parca in parcalar)
                        {
                            var kv = parca.Split('=', 2);
                            if (kv.Length == 2 && kv[0].Trim() == "SalonID")
                            {
                                return Convert.ToInt32(kv[1].Trim()) == salonID;
                            }
                        }
                        return false;
                    });
                    File.WriteAllLines(dosyaYolu, satirlar);
                    SalonlariOku();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { 
                return false;
            }
        }
    }
}
