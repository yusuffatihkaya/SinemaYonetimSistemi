using Microsoft.VisualBasic.ApplicationServices;
using SinemaYonetimSistemi.Siniflar;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SinemaYonetimSistemi
{
    public partial class FormAna : Form
    {
        SinemaYonetici _sinema;
        List<string> _seanslar;
        List<Salon> _salonlar;
        List<Film> _filmler;
        List<string> _biletler;
        OpenFileDialog _filmFotografDialog;
        Kontrol _kontrol;
        ImageList _filmFotoList;
        string _filmFotografDosya = string.Empty;
        string _filmFotografHedefDosya = string.Empty;
        int filmIndex;
        int salonIndex;
        int seansIndex;
        int _filmID;
        int indexListView;
        string _biletSalonAdi = string.Empty;
        string _biletFilmAdi = string.Empty;
        string _biletSeansAdi = string.Empty;
        int _biletSalonSiraSayisi = 0;
        int _biletSalonSiraKoltukSayisi = 0;
        double _biletFiyat = 0;
        int biletIndex;

        public FormAna()
        {
            InitializeComponent();
            _sinema = new SinemaYonetici();
            _seanslar = new List<string>();
            _salonlar = new List<Salon>();
            _filmler = new List<Film>();
            _biletler = new List<string>();
            _kontrol = new Kontrol();
            _filmFotografDialog = new OpenFileDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxKategori.SelectedIndex = 0;
            tabControlSinemaYonetim.SelectedIndex = 4;
            salonlariYukle();
            filmleriYukle();
            biletleriYukle();

        }

        private void biletleriYukle()
        {
            _biletler = _sinema.BiletOku();
            listBoxBiletler.Items.Clear();
            foreach (string bilet in _biletler)
            {
                listBoxBiletler.Items.Add(bilet);
            }
        }
        private void salonlariYukle()
        {
            _salonlar = _sinema.SalonlariOku();
            listBoxSalonlar.Items.Clear();
            listViewSalonlar.Items.Clear();
            listViewSalonlar.View = View.SmallIcon;
            listViewSalonlar.MultiSelect = false;
            listViewSalonlar.UseCompatibleStateImageBehavior = false;
            string calismaDizini = AppDomain.CurrentDomain.BaseDirectory;
            string dosyaYolu = Path.Combine(calismaDizini, "Kaynaklar", "sinema2.png");
            Image _salonFoto = Image.FromFile(dosyaYolu);
            ImageList _salonFotoImageList = new ImageList();
            _salonFotoImageList.ImageSize = new Size(32,32);
            _salonFotoImageList.ColorDepth = ColorDepth.Depth32Bit;
            foreach (Salon salon in _salonlar)
            {
                listBoxSalonlar.Items.Add(salon.SalonAdi);
                var item = new ListViewItem(salon.SalonAdi, 0);
                _salonFotoImageList.Images.Add(_salonFoto);
                listViewSalonlar.Items.Add(item);
            }
            listViewSalonlar.SmallImageList = _salonFotoImageList;
        }
        private void filmleriYukle()
        {
            _filmler = _sinema.FilmleriOku();
            listBoxFilmler.Items.Clear();
            listViewFilmler.Items.Clear();
            listViewFilmler.View = View.LargeIcon;
            listViewFilmler.MultiSelect = false;
            listViewFilmler.UseCompatibleStateImageBehavior = false;
            _filmFotoList = new ImageList();
            _filmFotoList.ImageSize = new Size(64, 128);
            _filmFotoList.ColorDepth = ColorDepth.Depth32Bit;
            foreach (Film film in _filmler)
            {
                listBoxFilmler.Items.Add(film.FilmAdi);
                Image filmFoto = Image.FromFile(film.FilmFotograf);
                int indexFoto = _filmFotoList.Images.Count;
                var item = new ListViewItem(film.FilmAdi, indexFoto);
                listViewFilmler.Items.Add(item);
                _filmFotoList.Images.Add(filmFoto);
            }
            listViewFilmler.LargeImageList = _filmFotoList;
        }

        private void seanslariYukle(List<string> seanslar)
        {
            checkedListBoxSeanslar.Items.Clear();
            if (seanslar == null || seanslar.Count == 0)
            {
                return;
            }
            foreach (string seans in seanslar)
            {
                checkedListBoxSeanslar.Items.Add(seans);
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
            if (!_kontrol.KontrolString(textBoxFilmAdi.Text, "Film Adı"))
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
            filmleriYukle();
            MessageBox.Show("Yeni Film [" + yeniFilm.FilmAdi + "] kaydedildi.");
            textBoxFilmAdi.Text = "";
            numericUpDownFiyat.Value = 0;
            comboBoxKategori.SelectedIndex = 0;
            numericUpDownSure.Value = 0;
            numericUpDownImdb.Value = 0;
            pictureBoxFilmFotograf.Image.Dispose();
            pictureBoxFilmFotograf.Image = null;
            _filmFotografHedefDosya = "";
            dateTimePickerSeansSaat.Value = DateTime.Now;

        }

        public void filmFotografEkle(object sender, EventArgs e)
        {
            _filmFotografDialog.Title = "Bir fotoğraf seçiniz";
            _filmFotografDialog.Filter = "Resim Dosyaları|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
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
                MessageBox.Show("Film için bir fotoğraf seçiniz!");
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
                    MessageBox.Show("Fotoğraf yükleme hatası!");
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
            if (!_kontrol.KontrolString(textBoxSalonAdi.Text, "Salon Adı"))
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

        private void buttonFilmSil_Click(object sender, EventArgs e)
        {
            if (filmIndex >= 0)
            {
                _sinema.FilmSil(_filmID);
            }
            filmleriYukle();
            checkedListBoxSeanslar.Items.Clear();
            pictureBoxFilmFotografFilmYonet.Dispose();
            labelFilmFiyat.Text = "";
        }

        private void checkedListBoxSeanslar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBoxFilmler_SelectedIndexChanged(object sender, EventArgs e)
        {
            filmIndex = listBoxFilmler.SelectedIndex;
            if (filmIndex >= 0)
            {
                _filmID = _filmler[filmIndex].FilmID;
                seanslariYukle(_filmler[filmIndex].FilmSeanslar);
                pictureBoxFilmFotografFilmYonet.Image = new Bitmap(_filmler[filmIndex].FilmFotograf.ToString());
                pictureBoxFilmFotografFilmYonet.SizeMode = PictureBoxSizeMode.StretchImage;
                labelFilmFiyat.Text = _filmler[filmIndex].FilmFiyat.ToString() + " ₺";
            }
        }

        private void listViewFilmler_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                indexListView = listViewFilmler.SelectedItems[0].Index;
                if (indexListView >= 0 || indexListView != null)
                {
                    listViewSeanslar.Items.Clear();
                    foreach (var item in _filmler[indexListView].FilmSeanslar)
                    {
                        listViewSeanslar.Items.Add(item.ToString());
                    }
                }
                _biletFilmAdi = _filmler[indexListView].FilmAdi;
                _biletFiyat = _filmler[indexListView].FilmFiyat;
            }
            catch { }
        }

        private void listViewSalonlar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                salonIndex = listViewSalonlar.SelectedItems[0].Index;
                _biletSalonAdi = _salonlar[salonIndex].SalonAdi;
                _biletSalonSiraSayisi = _salonlar[salonIndex].SalonSiraSayisi;
                _biletSalonSiraKoltukSayisi = _salonlar[salonIndex].SalonSiraKoltukSayisi;
            }
            catch { }
        }

        private void listViewSeanslar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                seansIndex = listViewSeanslar.SelectedItems[0].Index;
                _biletSeansAdi = _filmler[indexListView].FilmSeanslar[seansIndex].ToString();
                if (_biletSalonAdi == string.Empty)
                {

                    listViewSeanslar.SelectedItems.Clear();
                    MessageBox.Show("Salon seçiniz.");
                    return;
                }
                if (_biletFilmAdi == string.Empty)
                {
                    listViewSeanslar.SelectedItems.Clear();
                    MessageBox.Show("Film seçiniz.");
                    return;
                }
                if (_biletSeansAdi == string.Empty)
                {
                    listViewSeanslar.SelectedItems.Clear();
                    MessageBox.Show("Seans seçiniz.");
                    return;
                }

                FormBiletSatis _formBiletSatis = new FormBiletSatis(
                    _biletSalonAdi,
                    _biletFilmAdi,
                    _biletSeansAdi,
                    _biletSalonSiraSayisi,
                    _biletSalonSiraKoltukSayisi,
                    _biletFiyat
                );
                _formBiletSatis.ShowDialog();
                biletleriYukle();
                //MessageBox.Show(_biletSalonAdi + _biletFilmAdi + _biletSeansAdi);
            }
            catch { }
        }

        private void listViewSeanslar_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //
        }

        private void listViewSeanslar_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //
        }

        private void buttonBiletİptalEt_Click(object sender, EventArgs e)
        {
            _sinema.BiletSil(listBoxBiletler.Items[biletIndex].ToString());
            biletleriYukle();
        }

        private void listBoxBiletler_SelectedIndexChanged(object sender, EventArgs e)
        {
            biletIndex = listBoxBiletler.SelectedIndex;
        }
    }
}
