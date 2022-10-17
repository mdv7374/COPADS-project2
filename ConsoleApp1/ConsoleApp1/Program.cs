// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;

namespace Project2
{
    public static class ExtensionMethods
    {
        public static bool IsNotPrime(this BigInteger i)
        {
            //list of all primes from 2 - 1000
            int[] primes = new int[]{3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83,
                89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193,
                197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313,
                317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443,
                449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587,
                593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719,
                727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859,
                863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997};
            //if it is negative, it is not a prime
            if (i < 0)
            {
                return true;
            }
            //if it is even, it is not a prime number
            if (i.IsEven)
            {
                return true;
            }

            //divide the value by all primes less than 1000, if mod equals 0, it is a prime number
            foreach (var prime in primes)
            {
                if (i % prime == 0)
                {
                    return true;
                }
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
            BigInteger newBigInt = new BigInteger(0);
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            Parallel.For(0, Int32.MaxValue, (i,state) =>
            {
                byte[] bytearr = new byte[bytecount];
                rng.GetBytes(bytearr);
                BigInteger temp = new BigInteger(bytearr);
                if (!temp.IsNotPrime())
                {
                    if (temp.IsProbablyPrime())
                    {
                        newBigInt = temp;
                        state.Stop();
                    }
                }
            });
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