using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptoloji_Proje
{
    public class DES
    {
        private string anahtar;
        private string ileti;
        private string[] ileti64bitblok = { };

        private string[] donguanahtar = new string[17];
        public string getAnahtar() { return anahtar; }
        public string getIleti() { return ileti; }
        public void setAnahtar(string anahtar) { this.anahtar = anahtar; }
        public void setIleti(string ileti) { this.ileti = ileti; }
        public void mesaj64bitcevir(string mesaj)
        {
            byte[] bitmesaj = System.Text.Encoding.UTF8.GetBytes(mesaj);
            string bit64 = null;
            foreach (byte bit in bitmesaj)
            {
                bit64 += Convert.ToString(bit, 2).PadLeft(8, '0');
            }
            if (bit64.Length % 64 != 0)
            {
                int eklebas = 64 - bit64.Length % 64;
                while (eklebas > 0)
                {
                    bit64 += "0";
                    eklebas--;
                }
            }
            int bloksayi = bit64.Length / 64;
            this.ileti64bitblok = new string[bloksayi];
            int i = 0; int dongu = 0;
            while (i < bit64.Length && dongu < bloksayi)
            {
                this.ileti64bitblok[dongu] = (bit64.Substring(i, 64));
                i += 64;
                dongu++;
            }
        }
        public string anahtarBinary(string key)
        {
            byte[] bitkey = System.Text.Encoding.UTF8.GetBytes(key);
            string bit64 = null;
            foreach (byte bit in bitkey)
            {
                bit64 += Convert.ToString(bit, 2).PadLeft(8, '0');
            }
            return bit64;
        }
        public string binarydenASCIIye(string bin)
        {
            string[] binstring = new string[bin.Length / 8];
            byte[] bytedizi = new byte[bin.Length / 8];
            int s = 0;
            string sonuc;
            for (int i = 0; i < binstring.Length; i++, s += 8)
            {
                binstring[i] = bin.Substring(s, 8);
                bytedizi[i] = Convert.ToByte(binstring[i], 2);
            }
            sonuc = Encoding.UTF8.GetString(bytedizi);
            return sonuc;
        }
        public void anahtarUretimi(string anahtar)
        {
            anahtar = permutatedChoice1(anahtar);

            string solanahtar = anahtar.Substring(0, 28);
            string saganahtar = anahtar.Substring(28, 28);
            this.donguanahtar[0] = solanahtar + saganahtar;

            for (int i = 1; i < 17; i++)
            {
                solanahtar = solabitKaydirma(solanahtar, i);
                saganahtar = solabitKaydirma(saganahtar, i);
                this.donguanahtar[i] = permutatedChoice2(solanahtar + saganahtar);
            }
        }
        public string permutatedChoice1(string anahtar)
        {
            int[] pc1tablo = { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10,
                                2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47,
                                39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45,
                                37, 29, 21, 13, 5, 28, 20, 12, 4 };

            string tempanahtar = null;

            for (int i = 0; i < pc1tablo.Length; i++)
            {
                tempanahtar += anahtar.Substring(pc1tablo[i] - 1, 1);
            }
            return tempanahtar;
        }
        public string solabitKaydirma(string anahtar, int dongusayisi)
        {
            int[] kaydirilacakbit = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            string kaydirilmisan = null;
            int kaydir = kaydirilacakbit[dongusayisi - 1];

            if (kaydir == 1)
            {
                for (int i = 1; i < anahtar.Length; i++)
                {
                    kaydirilmisan += anahtar[i];
                }
                kaydirilmisan += anahtar[0];
            }
            else if (kaydir == 2)
            {
                for (int i = 2; i < anahtar.Length; i++)
                {
                    kaydirilmisan += anahtar[i];
                }
                kaydirilmisan += anahtar[0];
                kaydirilmisan += anahtar[1];
            }
            return kaydirilmisan;
        }
        public string permutatedChoice2(string anahtar56bit)
        {
            int[] pc2tablo = { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4,
                               26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40,
                               51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };

            string tempanahtar = null;

            for (int i = 0; i < pc2tablo.Length; i++)
            {
                tempanahtar += anahtar56bit.Substring(pc2tablo[i] - 1, 1);
            }
            return tempanahtar;
        }
        public string sifreleme()
        {
            int boyut = this.ileti64bitblok.Length;
            string[] basper = baslangicPermutasyonu(this.ileti64bitblok);

            string[,] solmesaj = new string[17, boyut];
            string[,] sagmesaj = new string[17, boyut];

            string[,] sagtempmesaj = new string[17, boyut];
            for (int i = 0; i < boyut; i++)
            {
                solmesaj[0, i] = basper[i].Substring(0, 32);
                sagmesaj[0, i] = basper[i].Substring(32);
            } 

            string[,] parca6bit = new string[9, boyut];

            for (int i = 1; i < 17; i++)
            {
                for (int j = 0; j < boyut; j++)
                {
                    solmesaj[i, j] = sagmesaj[i - 1, j];
                    sagmesaj[i, j] = genisletmePermutasyonu(sagmesaj[i - 1, j]);
                    sagtempmesaj[i, j] = XOR(sagmesaj[i, j], donguanahtar[i]);
                    parca6bit[1, j] = sagtempmesaj[i, j].Substring(0, 6);
                    parca6bit[2, j] = sagtempmesaj[i, j].Substring(6, 6);
                    parca6bit[3, j] = sagtempmesaj[i, j].Substring(12, 6);
                    parca6bit[4, j] = sagtempmesaj[i, j].Substring(18, 6);
                    parca6bit[5, j] = sagtempmesaj[i, j].Substring(24, 6);
                    parca6bit[6, j] = sagtempmesaj[i, j].Substring(30, 6);
                    parca6bit[7, j] = sagtempmesaj[i, j].Substring(36, 6);
                    parca6bit[8, j] = sagtempmesaj[i, j].Substring(42);

                    string[,] sboxsonuc = new string[9, boyut];
                    for (int z = 1; z < 9; z++)
                    {
                        sboxsonuc[z, j] = SBOX(z, parca6bit[z, j]);
                    }
                    string birlesik = null;
                    for (int z = 1; z < 9; z++)
                    {
                        birlesik += sboxsonuc[z, j];
                    }
                    birlesik = permutasyon(birlesik);
                    sagmesaj[i, j] = XOR(solmesaj[(i - 1), j], birlesik);
                }
            }
            string sifrelimesaj = null;
            for (int i = 0; i < boyut; i++)
            {
                sifrelimesaj += tersBaslangicPer(sagmesaj[16, i] + solmesaj[16, i]);
            }
            return sifrelimesaj;
        }
        public string desifreleme(string sifremetin)
        {
            string[] metin64bit = new string[sifremetin.Length / 64];
            this.ileti64bitblok = new string[metin64bit.Length];
            for (int i = 0, s = 0; s < metin64bit.Length; i += 64, s++)
            {
                metin64bit[s] = sifremetin.Substring(i, 64);
                this.ileti64bitblok[s] = metin64bit[s];
            }
            int boyut = this.ileti64bitblok.Length;
            string[] basper = baslangicPermutasyonu(this.ileti64bitblok);

            string[,] solmesaj = new string[17, boyut];
            string[,] sagmesaj = new string[17, boyut];

            string[,] sagtempmesaj = new string[17, boyut];
            for (int i = 0; i < boyut; i++)
            {
                solmesaj[0, i] = basper[i].Substring(0, 32);
                sagmesaj[0, i] = basper[i].Substring(32);
            }

            string[,] parca6bit = new string[9, boyut];
            int anahtarindis = 16;
            for (int i = 1; i < 17; i++, anahtarindis--)
            {
                for (int j = 0; j < boyut; j++)
                {
                    solmesaj[i, j] = sagmesaj[i - 1, j];
                    sagmesaj[i, j] = genisletmePermutasyonu(sagmesaj[i - 1, j]);
                    sagtempmesaj[i, j] = XOR(sagmesaj[i, j], donguanahtar[anahtarindis]);
                    parca6bit[1, j] = sagtempmesaj[i, j].Substring(0, 6);
                    parca6bit[2, j] = sagtempmesaj[i, j].Substring(6, 6);
                    parca6bit[3, j] = sagtempmesaj[i, j].Substring(12, 6);
                    parca6bit[4, j] = sagtempmesaj[i, j].Substring(18, 6);
                    parca6bit[5, j] = sagtempmesaj[i, j].Substring(24, 6);
                    parca6bit[6, j] = sagtempmesaj[i, j].Substring(30, 6);
                    parca6bit[7, j] = sagtempmesaj[i, j].Substring(36, 6);
                    parca6bit[8, j] = sagtempmesaj[i, j].Substring(42);

                    string[,] sboxsonuc = new string[9, boyut];
                    for (int z = 1; z < 9; z++)
                    {
                        sboxsonuc[z, j] = SBOX(z, parca6bit[z, j]);
                    }

                    string birlesik = null;
                    for (int z = 1; z < 9; z++)
                    {
                        birlesik += sboxsonuc[z, j];
                    }
                    birlesik = permutasyon(birlesik);
                    sagmesaj[i, j] = XOR(solmesaj[(i - 1), j], birlesik);
                }
            }
            string desifremesaj = null;
            for (int i = 0; i < boyut; i++)
            {
                desifremesaj += tersBaslangicPer(sagmesaj[16, i] + solmesaj[16, i]);
            }
            return desifremesaj;
        }
        public string tersBaslangicPer(string sonelde)
        {
            int[] ptablo = {40,8,48,16,56,24,64,32,
                            39,7,47,15,55,23,63,31,
                            38,6,46,14,54,22,62,30,
                            37,5,45,13,53,21,61,29,
                            36,4,44,12,52,20,60,28,
                            35,3,43,11,51,19,59,27,
                            34,2,42,10,50,18,58,26,
                            33,1,41,9,49,17,57,25};

            string temp = null;
            for (int i = 0; i < ptablo.Length; i++)
            {
                temp += sonelde.Substring(ptablo[i] - 1, 1);
            }
            return temp;
        }
        public string permutasyon(string s)
        {
            int[] ptablo = { 16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31,
                             10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25 };

            string temp = null;
            for (int i = 0; i < ptablo.Length; i++)
            {
                temp += s.Substring(ptablo[i] - 1, 1);
            }
            return temp;
        }
        public string SBOX(int sira, string blok)
        {
            int[] sbox1 = { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7,
                            0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8, 4, 1, 14,
                            8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0, 15, 12, 8, 2, 4, 9,
                            1, 7, 5, 11, 3, 14, 10, 0, 6, 13 };
            int[] sbox2 = { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10,
                            3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5, 0, 14, 7,
                            11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15, 13, 8, 10, 1, 3,
                            15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 };
            int[] sbox3 = { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8,
                            13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1, 13, 6, 4,
                            9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7, 1, 10, 13, 0, 6, 9,
                            8, 7, 4, 15, 14, 3, 11, 5, 2, 12 };
            int[] sbox4 = { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15,
                            13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9, 10, 6, 9,
                            0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4, 3, 15, 0, 6, 10, 1,
                            13, 8, 9, 4, 5, 11, 12, 7, 2, 14 };
            int[] sbox5 = { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9,
                            14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6, 4, 2, 1,
                            11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14, 11, 8, 12, 7, 1,
                            14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 };
            int[] sbox6 = { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11,
                            10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8, 9, 14,
                            15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6, 4, 3, 2, 12, 9,
                            5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 };
            int[] sbox7 = { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1,
                            13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6, 1, 4, 11,
                            13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2, 6, 11, 13, 8, 1, 4,
                            10, 7, 9, 5, 0, 15, 14, 2, 3, 12 };
            int[] sbox8 = { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7,
                            1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2, 7, 11, 4,
                            1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8, 2, 1, 14, 7, 4, 10,
                            8, 13, 15, 12, 9, 0, 3, 5, 6, 11 };

            string uclar = blok.Substring(0, 1) + blok.Substring(5);
            int satir = Convert.ToInt32(uclar, 2);
            int sutun = Convert.ToInt32(blok.Substring(1, 4), 2);

            satir = satir * 16;

            switch (sira)
            {
                case 1:
                    return Convert.ToString(sbox1[(satir + sutun)], 2).PadLeft(4, '0');
                case 2:
                    return Convert.ToString(sbox2[(satir + sutun)], 2).PadLeft(4, '0');
                case 3:
                    return Convert.ToString(sbox3[(satir + sutun)], 2).PadLeft(4, '0');
                case 4:
                    return Convert.ToString(sbox4[(satir + sutun)], 2).PadLeft(4, '0');
                case 5:
                    return Convert.ToString(sbox5[(satir + sutun)], 2).PadLeft(4, '0');
                case 6:
                    return Convert.ToString(sbox6[(satir + sutun)], 2).PadLeft(4, '0');
                case 7:
                    return Convert.ToString(sbox7[(satir + sutun)], 2).PadLeft(4, '0');
                case 8:
                    return Convert.ToString(sbox8[(satir + sutun)], 2).PadLeft(4, '0');
            }
            return null;
        }
        public string XOR(string sagyari, string anahtar)
        {
            string sonuc = null;
            int uzunluk = sagyari.Length;

            for (int i = 0; i < uzunluk; i++)
            {
                if (sagyari[i] != anahtar[i])
                    sonuc += "1";
                else
                    sonuc += "0";
            }
            return sonuc;
        }
        public string genisletmePermutasyonu(string sagmesaj)
        {
            int[] genistablo = { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13,
                                 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23,
                                 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };

            string tempsag = null;

            for (int i = 0; i < genistablo.Length; i++)
            {
                tempsag += sagmesaj.Substring(genistablo[i] - 1, 1);
            }
            return tempsag;
        }
        public string[] baslangicPermutasyonu(string[] mesajblok)
        {
            int[] bptablo = { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12,
                               4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16,
                               8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
                               61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };
            string tempblok;
            string[] tempblokdizi = new string[mesajblok.Length];
            for (int i = 0; i < mesajblok.Length; i++)
            {
                tempblok = null;
                for (int j = 0; j < bptablo.Length; j++)
                {
                    tempblok += mesajblok[i].Substring(bptablo[j] - 1, 1);
                }
                tempblokdizi[i] = tempblok;
            }
            return tempblokdizi;
        }
    }
}
