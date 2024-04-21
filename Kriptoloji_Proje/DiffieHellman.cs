using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kriptoloji_Proje
{
    class DiffieHellman
    {
        private long privateKey;
        private long publicKey;
        private long paylasiliTuretilmisKey;
        private long p;
        private long g;

        public void setPaylasiliTuretilmisKey(long p)
        {
            this.paylasiliTuretilmisKey = p;
        }
        public void setp(long p)
        {
            this.p = p;
        }
        public void setg(long g)
        {
            this.g = g;
        }
        public void setPublicKey(long publicKey)
        {
            this.publicKey = publicKey;
        }
        public void setPrivateKey(long privateKey)
        {
            this.privateKey = privateKey;
        }
        public long getp()
        {
            return p;
        }
        public long getg()
        {
            return g;
        }
        public long getPrivateKey()
        {
            return privateKey;
        }
        public long getPublicKey()
        {
            return publicKey;
        }
        public long getPaylasiliTuretilmisKey()
        {
            return paylasiliTuretilmisKey;
        }

        public long power(long ha, long hb, long hp)
        {
            if (hb == 0)
                return 1;

            long sonuc = 1;
            ha = ha % hp;

            while (hb > 0)
            {
                if (hb % 2 == 1)
                    sonuc = (sonuc * ha) % hp;

                ha = (ha * ha) % hp;

                hb /= 2;
            }
            return sonuc;
        }
        public string CalculateMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public void privateKeyUret(Random random)
        {
            long a = random.Next(2, 15); 
            setPrivateKey(a);
            Console.WriteLine($"Gönderen private key:a: {a}");
        }
        public void pUret(Random random)
        {
            long p = random.Next(2, 50);
            
            int ip = 2;
            int kontrolp = 0;
            while (ip < p)
            {
                if (p % ip == 0)
                {
                    kontrolp++;
                    break;
                }
                ip++;
            }
            if (kontrolp != 0)
                pUret(random); 
            else
                setp(p);
        }
        public void gUret(Random random)
        {
            long g = random.Next(2, 50);
            
            int ig = 2;
            int kontrolg = 0;
            while (ig < g)
            {
                if (g % ig == 0)
                {
                    kontrolg++;
                    break;
                }
                ig++;
            }
            if (kontrolg != 0)
                gUret(random);
            else
                setg(g);
        }
        public void publicKeyUret()
        {
            long A;
            A = power(getg(), getPrivateKey(), getp());
            setPublicKey(A);
            Console.WriteLine($"A publicGonderen :{A}");
        }
        public void paylasilmisKeyTuret(long karsiTarafPublic)
        {
            long KA;
            KA = power(karsiTarafPublic, getPrivateKey(), getp());
            setPaylasiliTuretilmisKey(KA);
        }
    }
}
