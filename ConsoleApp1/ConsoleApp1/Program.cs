// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;

namespace Project2
{
    public static class ExtensionMethods
    {
        public static bool isNotPrime(this BigInteger i)
        {
            if (i < 0)
            {
                return true;
            }

            if (i % 2 == 0)
            {
                return true;
            }
            
            return false;
        }
        
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

                var x = BigInteger.ModPow(a, d, i);
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
        public BigInteger GenerateBigInt(int bytecount)
        {
            BigInteger newBigInt;
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            while (true)
            {
                byte[] bytearr = new byte[bytecount];
                rng.GetBytes(bytearr);
                newBigInt = new BigInteger(bytearr);
                if (!newBigInt.isNotPrime())
                {
                    if (newBigInt.IsProbablyPrime())
                    {
                        break;
                    }
                }
            }
            return newBigInt;
        }

        public static void Main(string[] args)
        {
            project2 p = new project2();
            int count = 0;
            int bitCount = 0;
            if (args.Length == 1)
            {
                bitCount = Convert.ToInt32(args[0]);
                count = 1;
            }
            else if (args.Length == 2)
            {
                bitCount = Convert.ToInt32(args[0]);
                count = Convert.ToInt32(args[1]);
            }
            else
            {
                Console.WriteLine("Error");
                Environment.Exit(0);
            }

            if (bitCount < 32 || bitCount % 8 != 0)
            {
                Console.WriteLine("error");
                Environment.Exit(0);
            }

            var byteCount = bitCount / 8;
            int printCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 1; i <= count; i++)
            {
                BigInteger temp = p.GenerateBigInt(byteCount);
                Console.WriteLine(i + ": " + temp);
            }
            sw.Stop();
            Console.WriteLine("Time to generate: " + sw.Elapsed);
        }
    }
}