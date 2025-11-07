using SinemaYonetimSistemi.Siniflar;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace SinemaYonetimSistemi
{
    public partial class FormAna : Form
    {
        SinemaYonetici _sinema;
        List<string> _seanslar;
        List<Salon> _salonlar;
        OpenFileDialog _filmFotografDialog;
        Kontrol _kontrol;
        string _filmFotografDosya = string.Empty;
        string _filmFotografHedefDosya = string.Empty;

        public FormAna()
        {
            InitializeComponent();
            _sinema = new SinemaYonetici();
            _seanslar = new List<string>();
            _salonlar = new List<Salon>();
            _kontrol = new Kontrol();
            _filmFotografDialog = new OpenFileDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxKategori.SelectedIndex = 0;
            salonlariYukle();
        }

        private void salonlariYukle()
        {
            _salonlar = _sinema.SalonlariOku();
            listBoxSalonlar.Items.Clear();
            foreach (Salon salon in _salonlar)
            {
                listBoxSalonlar.Items.Add(salon.SalonAdi);
            }
        }

        private void buttonSeansEkle_Click(object sender, EventArgs e)
        {
            int seansSirasi = listBoxSeanslar.Items.Count + 1;
            string seansSaati = dateTimePickerSeansSaat.Text;
            listBoxSeanslar.Items.Add(seansSirasi + ". > [" + seansSaati + "]");
            _seanslar.Add(seansSaati);
        }

        private void buttonSeansSil_Click(object sender, EventArgs e)
        {
            int itemIndex = listBoxSeanslar.SelectedIndex;
            if (listBoxSeanslar.SelectedIndex >= 0)
            {
                listBoxSeanslar.Items.RemoveAt(itemIndex);
                _seanslar.RemoveAt(itemIndex);
                for (int i = 0; i < listBoxSeanslar.Items.Count; i++)
                {
                    string? item;
                    int? itemSira;
                    string? itemDeger;
                    item = listBoxSeanslar.Items[i].ToString();
                    if (item != null)
                    {
                        itemSira = Convert.ToInt32(item.Split(".")[0]);
                        itemDeger = "." + item.ToString().Split(".")[1] + "." + item.ToString().Split(".")[2];
                        if (itemSira >= itemIndex + 2)
                        {
                            listBoxSeanslar.Items[i] = (i + 1).ToString() + itemDeger;
                        }
                    }
                }
            }
        }

        private void buttonFilmEkle_Click(object sender, EventArgs e)
        {
            if (!_kontrol.KontrolString(textBoxFilmAdi.Text, "Film Adý"))
            {
                return;
            }
            if (!filmFotografYukle())
            {
                return;
            }
            Film yeniFilm = new Film();
            yeniFilm.FilmAdi = textBoxFilmAdi.Text;
            yeniFilm.FilmFiyat = Convert.ToDouble(numericUpDownFiyat.Value);
            yeniFilm.FilmKategori = comboBoxKategori.Text;
            yeniFilm.FilmSure = Convert.ToInt32(numericUpDownSure.Value);
            yeniFilm.FilmImdb = Convert.ToInt32(numericUpDownImdb.Value);
            yeniFilm.FilmFotograf = _filmFotografHedefDosya;
            yeniFilm.FilmSeanslar = _seanslar;
            _sinema.FilmEkle(yeniFilm);
            MessageBox.Show("Yeni Film [" + yeniFilm.FilmAdi + "] kaydedildi.");
        }

        public void filmFotografEkle(object sender, EventArgs e)
        {
            _filmFotografDialog.Title = "Bir fotoðraf seçiniz";
            _filmFotografDialog.Filter = "Resim Dosyalarý|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
            _filmFotografDialog.Multiselect = false;
            if (_filmFotografDialog.ShowDialog() == DialogResult.OK)
            {
                _filmFotografDosya = _filmFotografDialog.FileName;
                if (pictureBoxFilmFotograf.Image != null)
                {
                    pictureBoxFilmFotograf.Image.Dispose();
                    pictureBoxFilmFotograf.Image = null;
                }
                pictureBoxFilmFotograf.Image = new Bitmap(_filmFotografDosya);
                pictureBoxFilmFotograf.SizeMode = PictureBoxSizeMode.StretchImage;
                labelFotograf.Visible = false;
            }
            else
            {
                return;
            }
        }

        public bool filmFotografYukle()
        {
            if (_filmFotografDosya == string.Empty)
            {
                MessageBox.Show("Film için bir fotoðraf seçiniz!");
                return false;
            }
            else
            {
                try
                {
                    string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
                    string kayitlarDizini = Path.Combine(calismaDizini, "Kayitlar");
                    string kayitDizini = Path.Combine(kayitlarDizini, "Fotograflar");
                    Directory.CreateDirectory(kayitDizini);
                    string fotografUzanti = Path.GetExtension(_filmFotografDosya);
                    string fotografAdi = Path.GetFileNameWithoutExtension(_filmFotografDosya);
                    string fotografTarih = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string fotografYeniAdi = fotografAdi + "_" + fotografTarih + fotografUzanti;
                    _filmFotografHedefDosya = Path.Combine(kayitDizini, fotografYeniAdi);
                    File.Copy(_filmFotografDosya, _filmFotografHedefDosya, overwrite: false);
                    return true;
                }
                catch
                {
                    MessageBox.Show("Fotoðraf yükleme hatasý!");
                    return false;
                }
            }
        }

        private void numericUpDownFiyat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void numericUpDownSalonFiyatHesapla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true;
            }
            SalonKapasiteHesapla();
        }

        private void buttonSalonSil_Click(object sender, EventArgs e)
        {
            int itemIndex = listBoxSalonlar.SelectedIndex;
            if (listBoxSalonlar.SelectedIndex >= 0)
            {
                _sinema.SalonSil(_salonlar[itemIndex].SalonID);
            }
            salonlariYukle();
        }

        private void buttonYeniSalonEkle_Click(object sender, EventArgs e)
        {
            if (!_kontrol.KontrolString(textBoxSalonAdi.Text, "Salon Adý"))
            {
                return;
            }
            Salon yeniSalon = new Salon();
            yeniSalon.SalonAdi = textBoxSalonAdi.Text.Trim();
            yeniSalon.SalonSiraSayisi = Convert.ToInt32(numericUpDownSalonSiraSayisi.Value);
            yeniSalon.SalonSiraKoltukSayisi = Convert.ToInt32(numericUpDownSalonSiraKoltukSayisi.Value);
            _sinema.SalonEkle(yeniSalon);
            salonlariYukle();
            MessageBox.Show("Yeni Salon [" + yeniSalon.SalonAdi + "] kaydedildi.");
        }

        private void numericUpDownSalonSiraSayisi_ValueChanged(object sender, EventArgs e)
        {
            SalonKapasiteHesapla();
        }

        private void SalonKapasiteHesapla()
        {
            int siraSayisi = Convert.ToInt32(numericUpDownSalonSiraSayisi.Value);
            int siraKoltukSayisi = Convert.ToInt32(numericUpDownSalonSiraKoltukSayisi.Value);
            labelYeniSalonKapasite.Text = "Kapasite : (" + siraSayisi.ToString() + "X" + siraKoltukSayisi.ToString() + ") = " + (siraSayisi * siraKoltukSayisi);
        }

        private void numericUpDownSalonSiraSayisi_Leave(object sender, EventArgs e)
        {
            SalonKapasiteHesapla();
        }
    }
}
