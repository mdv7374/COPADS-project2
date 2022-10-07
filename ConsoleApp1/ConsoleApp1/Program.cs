// See https://aka.ms/new-console-template for more information

using System.Numerics;
using System.Security.Cryptography;

namespace Project2
{
    public static class ExtensionMethods
    {
        public static bool IsProbablyPrime(this BigInteger i, int k = 10)
        {
            var findPow = true;
            var temp = i-1;
            var r = 0;
            var rng = RandomNumberGenerator.Create();
            //first find the highest power that fits into i
            while (findPow)
            {
                if (temp % 2 == 0)
                {
                    temp /= 2;
                    r++;
                }
                else
                {
                    findPow = false;
                }
            }
            //then get d
            var d = i/ Convert.ToInt32(Math.Pow(2,r));
            BigInteger a;
            for (int count = 0; count < k; count++)
            {
                //loop used to get random BigInt a
                for (;;)
                {
                    var atemp = new byte[i.GetByteCount()];
                    rng.GetBytes(atemp);
                    a = new BigInteger(atemp);
                    if (a > 2 && a < i-1)
                    {
                        break;
                    }
                }
                var x = BigInteger.Pow(a, (int)d);
                if (x == 1 || x == i - 1)
                {
                    continue;
                }

                var isNotPrime = true;
                for (int j = 0; j < r -1; j++)
                {
                   x = BigInteger.Pow(x,2)%i;
                   if (x == i - 1)
                   {
                       isNotPrime = false;
                       break;
                   }
                }
                if (isNotPrime)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class project2
    {
        
        public static void Main(string[] args)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            var t = new byte[4];
            rng.GetBytes(t);
            foreach (var v in t)
            {
                Console.WriteLine(v);
            }
            var bigi = new BigInteger(t);
            Console.WriteLine(bigi.GetByteCount());
            Console.WriteLine("/////");
            var bigi2 = new BigInteger(97);
            Console.WriteLine(bigi2.IsProbablyPrime());
            var bigi3 = new BigInteger(99);
            Console.WriteLine(bigi3.IsProbablyPrime());
        }
    }
}