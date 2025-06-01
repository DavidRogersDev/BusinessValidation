using System.Security.Cryptography;

namespace BusinessValidation.Tests.Generators
{
    internal class StringGenerator
    {
        const string allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        internal static string GetString(int maxLength)
        {
            int length = RandomNumberGenerator.GetInt32(maxLength + 1);

            return GetStringInner(length);
        }
        
        internal static string GetString2(int minLength)
        {
            int length = RandomNumberGenerator.GetInt32(minLength, 500);

            return GetStringInner(length);
        }

        private static string GetStringInner(int strlen)
        {            
            char[] randomChars = new char[strlen];

            for (int i = 0; i < strlen; i++)
            {
                randomChars[i] = allowed[RandomNumberGenerator.GetInt32(0, allowed.Length)];
            }

            string result = new string(randomChars);

            return result;
        }
    }
}
