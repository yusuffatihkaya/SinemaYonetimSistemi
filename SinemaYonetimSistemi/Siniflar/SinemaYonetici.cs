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
                        while ((satir = streamOkuyucu.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(satir))
                                continue;
                            Film film = new Film();
                            var satirParcalari = satir.Split(';', StringSplitOptions.RemoveEmptyEntries);
                            foreach (var parca in satirParcalari)
                            {
                                string ozellikAdi = parca.Split('=', 2)[0];
                                string ozellikDeger = parca.Split('=', 2)[1];
                                if (ozellikAdi == "FilmID")
                                {
                                    film.FilmID = Convert.ToInt32(ozellikDeger);
                                    film.FilmSeanslar = FilmSeansOku(film.FilmID);
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
        public List<string> FilmSeansOku(int filmID)
        {
            var seanslar = new List<string>();
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
                            for (int i = 0; i < satirParcalari.Length; i++)
                            {
                                int satirFilmID = Convert.ToInt32(satirParcalari[0].Split('=', 2)[1]);
                                if (satirFilmID == filmID && i > 0)
                                {
                                    seanslar.Add(satirParcalari[i]);
                                }
                            }
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
                        + ";FilmSure=" + yeniFilm.FilmSure.ToString() + ";FilmImdb=" + yeniFilm.FilmImdb.ToString() + ";FilmFotograf=" + yeniFilm.FilmFotograf.ToString() + ";FilmSalonID=-1";
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
                    string yeniSatir = "FilmID=" + _yeniFilmID.ToString();
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
        public bool FilmSil(int filmID)
        {
            try
            {
                string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
                string dosyaYolu = Path.Combine(kayitDizini, "filmler.txt");
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
                            if (kv.Length == 2 && kv[0].Trim() == "FilmID")
                            {
                                return Convert.ToInt32(kv[1].Trim()) == filmID;
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
            catch
            {
                return false;
            }
        }
        public bool FilmGuncelle(Film film, int salonID)
        {
            string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
            string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
            string dosyaYolu = Path.Combine(kayitDizini, "filmler.txt");
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
                        for (int i = 0; i < satirParcalari.Length; i++)
                        {
                            int satirFilmID = Convert.ToInt32(satirParcalari[0].Split('=', 2)[1]);
                            if (satirFilmID == film.FilmID)
                            {
                                Console.WriteLine(satirParcalari[i]);
                            }
                        }
                    }
                }
            }
            return true;
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
        public bool SalonSec(Film film,int salonID)
        {
            try
            {
                film.SalonID = salonID;
                return true;
            }
            catch {
                return false;
            }
        }

        public bool BiletSat(string bilet)
        {
            try
            {
                string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
                string dosyaYolu = Path.Combine(kayitDizini, "biletler.txt");
                Directory.CreateDirectory(kayitDizini);
                using (var streamYazici = new StreamWriter(dosyaYolu, append: true, Encoding.UTF8))
                {
                    streamYazici.WriteLine(bilet);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public List<string> BiletOku()
        {
            var biletler = new List<string>();
            try
            {
                string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
                string dosyaYolu = Path.Combine(kayitDizini, "biletler.txt");
                if (File.Exists(dosyaYolu) == true)
                {
                    using (var streamOkuyucu = new StreamReader(dosyaYolu, Encoding.UTF8))
                    {
                        string satir = string.Empty;
                        while ((satir = streamOkuyucu.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(satir))
                                continue;
                            biletler.Add(satir);
                        }
                    }
                }
            }
            catch { }
            return biletler;
        }

        public bool BiletSil(string bilet)
        {
            try
            {
                string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                string kayitDizini = Path.Combine(calismaDizini, "Kayitlar");
                string dosyaYolu = Path.Combine(kayitDizini, "biletler.txt");
                if (File.Exists(dosyaYolu) == true)
                {
                    var satirlar = File.ReadAllLines(dosyaYolu).ToList();
                    satirlar = satirlar
                        .Where(s => !string.Equals(s, bilet, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    File.WriteAllLines(dosyaYolu, satirlar);
                    BiletOku();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
