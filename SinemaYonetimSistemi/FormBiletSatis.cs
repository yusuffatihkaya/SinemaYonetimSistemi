using SinemaYonetimSistemi.Siniflar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SinemaYonetimSistemi
{
    public partial class FormBiletSatis : Form
    {
        string _salonAdi, _filmAdi, _seansAdi;
        int _salonSiraSayisi, _salonSiraKoltukSayisi;
        double _fiyat;
        string _koltuk = string.Empty;
        Kontrol _kontrol;

        public FormBiletSatis(string salonAdi, string filmAdi, string seansAdi, int salonSiraSayisi, int salonSiraKoltukSayisi, double filmFiyat)
        {
            InitializeComponent();
            _kontrol = new Kontrol();
            _salonAdi = salonAdi;
            _filmAdi = filmAdi;
            _seansAdi = seansAdi;
            _salonSiraSayisi = salonSiraSayisi;
            _salonSiraKoltukSayisi = salonSiraKoltukSayisi;
            _fiyat = filmFiyat;
            labelSecilen.Text = _salonAdi + ">" + _filmAdi + ">" + _seansAdi;
            labelFiyat.Text = _fiyat.ToString() + " ₺";
            listBoxKoltuklar.Items.Clear();
            for (int i = 0; i < _salonSiraSayisi; i++)
            {
                char harf = (char)('A' + i);
                for (int j = 0; j < _salonSiraKoltukSayisi; j++)
                {
                    string yeniKoltuk = harf + ":" + (j + 1);
                    listBoxKoltuklar.Items.Add(yeniKoltuk);
                }
            }
        }

        private void buttonBiletSatisKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonBiletSat_Click(object sender, EventArgs e)
        {
            if (!_kontrol.KontrolString(textBoxMusteriAdi.Text, "Müşteri Adı"))
            {
                return;
            }
            if (!_kontrol.KontrolString(textBoxMusteriSoyadi.Text, "Müşteri Soyadı"))
            {
                return;
            }
            string odemeSekli = string.Empty;
            if (radioButtonNakit.Checked)
                odemeSekli = "Nakit";
            if (radioButtonKrediKarti.Checked)
                odemeSekli = "KrediKarti";
            string bilet = _salonAdi + ">" + _filmAdi + ">" + _seansAdi + ">" + _koltuk + ">" 
                + textBoxMusteriAdi.Text.Trim()
                + ">" + textBoxMusteriSoyadi.Text.Trim() + ">" + _fiyat + ">" + odemeSekli;
            SinemaYonetici _sinema = new SinemaYonetici();
            _sinema.BiletSat(bilet);
            MessageBox.Show("Bilet satış işlemi tamamlandı.");
            this.Close();
        }

        private void listBoxKoltuklar_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxKoltuklar.SelectedIndex;
            if (index >= 0)
            {
                labelKoltuk.Text = listBoxKoltuklar.Items[index].ToString();
                _koltuk = labelKoltuk.Text;
            }
        }
    }
}
