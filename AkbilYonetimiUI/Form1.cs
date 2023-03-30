using AkbilYonetimiIsKatmani;
using AkbilYonetimiVeriKatmani.Models;

namespace AkbilYonetimiUI
{
    public partial class Form1 : Form
    {
        public string Email { get; set; }//kay�t ol formunda kay�t olan kullan�c�n�n emaili buraya gelsin
        AkbiluygulamadbContext context = new AkbiluygulamadbContext();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (Email != null)
            {
                txtEmail.Text = Email;
            }
            txtEmail.TabIndex = 1;
            txtSifre.TabIndex = 2;
            checkBoxHatirla.TabIndex = 3;
            btnGirisYap.TabIndex = 4;
            btnKayitOl.TabIndex = 5;
            txtSifre.PasswordChar = '*';

            //beni hatirlayi Properties.Settings ile yapana kadar buras� b�yle kolayl�k saglasin
            txtEmail.Text = "cerensemiz@gmail.com";
            txtSifre.Text = "123";

        }
        private void btnKayitOl_Click(object sender, EventArgs e)
        {
            //Bu formu gizleyece�iz.
            //Kay�t ol formunu a�aca��z.
            this.Hide();
            FrmKayitOl frm = new FrmKayitOl();
            frm.Show();

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            GirisYap();
        }

        private void GirisYap()
        {
            try
            {
                //1)Email ve sifre textboxlar� dolu mu?
                if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtSifre.Text))
                {
                    MessageBox.Show("Bilgilerinizi eksiksiz giriniz", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                //2)Girdi�i email ve �ifre veritaban�nda mevcut mu?
                var kullanici = context.Kullanicilars.FirstOrDefault(x => x.Email == txtEmail.Text && x.Parola == GenelIslemler.MD5Encryption(txtSifre.Text));

                if (kullanici==null)
                {
                    MessageBox.Show("Email ya da �ifrenizi yanl�� girdiniz ! ");
                    return;
                }
                else
                {
                    MessageBox.Show($"Ho�geldiniz...{kullanici.Ad} {kullanici.Soyad}");
                    GenelIslemler.GirisYapanKullaniciID = kullanici.Id;
                    GenelIslemler.GirisYapanKullaniciEmail = kullanici.Email;
                    GenelIslemler.GirisYapanKullaniciAdSoyad = $"{kullanici.Ad} {kullanici.Soyad}";
                    //Beni Hatirlayi settings ile yazalim.

                    //temizlik
                    txtEmail.Clear(); txtSifre.Clear();
                    FrmAnasayfa frmAnasayfa = new FrmAnasayfa();
                    this.Hide();
                    frmAnasayfa.Show();

                }
            }
            catch (Exception hata)
            {
                //Dipnot: Exceptionlar asla kullan�c�ya g�sterilemez.Exceptionlar loglan�r, yaz�l�mc�ya iletilir.Biz ��renmek i�in mbox �n i�ine yazd�k.

                MessageBox.Show("Beklenmedik bir sorun olu�tu!" + hata.Message);
            }
        }

        private void checkBoxHatirla_CheckedChanged(object sender, EventArgs e)
        {
            BeniHatirla();
        }

        private void BeniHatirla()
        {

        }

        private void txtSifre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))//basilan tus enter ise giris yapilacak
            {
                GirisYap();
            }
        }
    }
}