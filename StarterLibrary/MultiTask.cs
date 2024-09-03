using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarterLibrary
{
    public static class MultiTask
    {
        /// <summary>
        /// this method checks if number is power of two 
        /// </summary>
        /// <param name="num">integer to test</param>
        /// <returns>true if num is power of 2</returns>
        public static bool IsNumberPowerOfTwo(int num)
        {
            if (num <= 0)
            {
                return false;
            }
            return (num & (num - 1)) == 0;
        }

        /// <summary>
        /// this method takes a string and reverse it 
        /// </summary>
        /// <param name="input">string param to test</param>
        /// <returns>returns reversed string</returns>
        public static string ReverseString(string input)
        {
            char[] reversedString = new char[input.Length];
            int j = 0;

            for (int i = input.Length - 1; i >= 0; i--)
            {
                reversedString[j++] = input[i];
            }

            return new string(reversedString);
        }
        /// <summary>
        /// this returns replicated strings based on number of times 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="times"></param>
        /// <returns>the method returns replicated string</returns>
        public static string ReplicateString(string input, int times)
        {
            if (times <= 0)
                return string.Empty;

            var replicatedStringBuilder = new StringBuilder(input.Length * times);

            for (int i = 0; i < times; i++)
            {
                replicatedStringBuilder.Append(input);
            }
            return replicatedStringBuilder.ToString();
        }
        /// <summary>
        /// print odd numbers from 1 to 100
        /// </summary>
        public static void PrintOddNumbers()
        {
            for (int i = 1; i < 100; i += 2)
            {
                Console.WriteLine(i);
            }
        }
    }
}
