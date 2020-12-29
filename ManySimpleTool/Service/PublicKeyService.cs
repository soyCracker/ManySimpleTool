using ManySimpleTool.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ManySimpleTool.Service
{
    public class PublicKeyService : IPublicKeyService
    {
        private readonly ILogger logger;
        private int min = 2;
        private int max = 10;
        private int primeNumP = 0;
        private int primeNumQ = 0;
        private int numN = 0;
        private int eulerR = 0;
        private int randomE = 0;
        private int multiInverseD = 0;
        private Random rnd;
        private string utf8 = "utf-8";

        public PublicKeyService(ILogger logger)
        {
            this.logger = logger;
            this.rnd = new Random();
        }

        private bool IsPrimeNumber(int num)
        {
            if(num==0)
            {
                return false;
            }
            else
            {
                for (int i = 2; i < num; i++)
                {
                    if (num % i == 0)
                    {
                        return false;
                    }
                }
                return true;
            }     
        }

        private void SetPrimeNumP()
        {
            while(!IsPrimeNumber(primeNumP))
            {
                primeNumP = rnd.Next(min, max);
            }           
        }

        private void SetPrimeNumQ()
        {
            while(!IsPrimeNumber(primeNumQ) || GetGreatestCommonDivisor(primeNumP, primeNumQ)!=1)
            {
                primeNumQ = rnd.Next(min, max);
            }
        }

        private int GetGreatestCommonDivisor(int p, int q)
        {
            if(p!=q && p!=0 && q!=0)
            {
                int tempB = p > q ? p : q;
                int tempS = p > q ? q : p;
                for(int i=tempS;i>=1;i--)
                {
                    if(tempB%i==0 && tempS%i==0)
                    {
                        return i;
                    }
                }
            }        
            return 0;
        }

        private void SetNumN()
        {
            numN = primeNumP * primeNumQ;
        }

        private void SetEulerR()
        {
            eulerR = (primeNumP - 1) * (primeNumQ - 1);
        }

        private void SetRandomE()
        {
            while (GetGreatestCommonDivisor(randomE, eulerR) != 1)
            {
                randomE = rnd.Next(2, eulerR);
            }            
        }

        private void SetMultiInverseD()
        {
            int d = 1;
            while(true)
            {
                if(d!=randomE && (randomE * d - 1)%eulerR==0)
                {
                    multiInverseD = d;
                    break;
                }
                d++;
            }
        }

        private byte[] GetBase64Byte(string msg)
        {
            return Encoding.GetEncoding(utf8).GetBytes(msg);
        }

        private string GetOriginFromBase64Str(List<byte> base64)
        {
            byte[] b = base64.ToArray();
            return Encoding.GetEncoding(utf8).GetString(b);
        }

        public void CreateKey()
        {
            SetPrimeNumP();
            logger.LogDebug("P: " + primeNumP);
            SetPrimeNumQ();
            logger.LogDebug("Q: " + primeNumQ);
            SetNumN();
            logger.LogDebug("N: " + numN);
            SetEulerR();
            logger.LogDebug("R: " + eulerR);
            SetRandomE();
            logger.LogDebug("E: " + randomE);
            SetMultiInverseD();
            logger.LogDebug("D: " + multiInverseD);
        }

        private string OneCharEncryption(byte b)
        {
            return (Math.Pow(b, randomE) % numN).ToString();
        }

        private byte OneCharDecryption(string str)
        {
            return Convert.ToByte(Math.Pow(Convert.ToInt32(str), multiInverseD) % numN);
        }

        public List<string> Encrypt(string msg)
        {
            byte[] base64Byte = GetBase64Byte(msg);
            foreach (byte b in base64Byte)
            {
                logger.LogDebug("base64Byte: " + b);
            }
            List<string> res = new List<string>();
            foreach(byte b in base64Byte)
            {
                res.Add(OneCharEncryption(b));
            }
            return res;
        }

        public string Decrypt(List<string> encryptMsg)
        {
            List<byte> deByte = new List<byte>();
            foreach (string str in encryptMsg)
            {
                deByte.Add(OneCharDecryption(str));
            }
            return GetOriginFromBase64Str(deByte);
        }

        public void Test(string msg)
        {
            logger.LogDebug("msg: " + msg);
            CreateKey();
            List<string> encryptMsg = Encrypt(msg);
            foreach(string str in encryptMsg)
            {
                logger.LogDebug("EncryptMsg: " + str);
            }
            
            string decryptMsg = Decrypt(encryptMsg);
            logger.LogDebug("DecryptMsg: " + decryptMsg);
        }
    }
}
    