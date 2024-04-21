using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Kriptoloji_Proje
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
           
        }

        private void btn_mesajgonder_Click(object sender, EventArgs e)
        {
            string mesaj = txt_gonderilenmesaj.Text;
            if (string.IsNullOrEmpty(mesaj))
            {
                MessageBox.Show("Lütfen bir mesaj giriniz!", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DiffieHellman diffieortak = new DiffieHellman();
            Random key = new Random();
            diffieortak.pUret(key);
            diffieortak.gUret(key);

            DiffieHellman gonderici = new DiffieHellman();
            DiffieHellman alici = new DiffieHellman();
            gonderici.setg(diffieortak.getg());
            gonderici.setp(diffieortak.getp());
            alici.setp(diffieortak.getp());
            alici.setg(diffieortak.getg());

            gonderici.privateKeyUret(key);
            alici.privateKeyUret(key);

            gonderici.publicKeyUret();
            alici.publicKeyUret();

            alici.paylasilmisKeyTuret(gonderici.getPublicKey());
            gonderici.paylasilmisKeyTuret(alici.getPublicKey());


            DES desgonderen = new DES();
            desgonderen.setIleti(mesaj);
            string anahtargonderici = gonderici.CalculateMD5Hash(gonderici.getPaylasiliTuretilmisKey().ToString()).Substring(0, 8);
            desgonderen.mesaj64bitcevir(desgonderen.getIleti());
            desgonderen.setAnahtar(desgonderen.anahtarBinary(anahtargonderici));
            desgonderen.anahtarUretimi(desgonderen.getAnahtar());
            string sifrelimetin = desgonderen.sifreleme();
            txt_sifrelimesaj.Text = desgonderen.binarydenASCIIye(sifrelimetin);

            Hash hashing = new Hash();
            hashing.setKaynak(txt_gonderilenmesaj.Text.ToString());
            string sifremetinhash = hashing.hashle();

            RSA rsa = new RSA();
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            byte[] sifreliozet;
            byte[] sayisalimza;

            sifreliozet = ByteConverter.GetBytes(sifremetinhash);
            sayisalimza = rsa.Encryption(sifreliozet, RSA.ExportParameters(false), false);
            txt_sayisalimza.Text = ByteConverter.GetString(sayisalimza);


            DES desalici = new DES();
            string anahtaralici = alici.CalculateMD5Hash(alici.getPaylasiliTuretilmisKey().ToString()).Substring(0, 8);
            desalici.setAnahtar(desalici.anahtarBinary(anahtaralici));
            desalici.anahtarUretimi(desalici.getAnahtar());

            desalici.setIleti(sifrelimetin);
            txt_aliciekrani.Text = desalici.binarydenASCIIye(desalici.desifreleme(desalici.getIleti()));


            hashing.setKaynak(txt_aliciekrani.Text.ToString());
            string desifremetinhash = hashing.hashle();
            txt_mesajhash.Text = desifremetinhash;
            

            byte[] aliciozet = rsa.Decryption(sayisalimza, RSA.ExportParameters(true), false);
            string desifreliimza = ByteConverter.GetString(aliciozet);
            txt_imzahash.Text = desifreliimza;

            if(desifremetinhash == desifreliimza)
            {
                txt_kimlik.Text = "Gönderen kimliği doğrulandı.";
                txt_dogruluk.Text = "Mesaj değiştirilmemiş.";
            }
            else
            {
                txt_kimlik.Text = "Gönderen kişi doğrulanamadı.";
                txt_dogruluk.Text = "Mesaj manipule edilmiş!";
            }
        }
    }
}
