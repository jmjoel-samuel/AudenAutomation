using System;
using System.Linq;

namespace AudenAutomationTests.Helper
{
    public static class CommonHelper
    {
        public static string GetRandomAlphaNumeric()
        {
            Random random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(8).ToArray());
        }

        public static string GetRandomString(int stringLenght)
        {
            Random random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(stringLenght).ToArray());
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string GetRandomPhoneNumber(int stringLenght)
        {
            Random random = new Random();
            var chars = "0123456789";
            return new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(stringLenght).ToArray());
        }

        public static string GetEndString(string name, string seperator)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (seperator is null)
            {
                throw new ArgumentNullException(nameof(seperator));
            }

            int posA = name.LastIndexOf(seperator);
            if (posA == -1)
            {
                return string.Empty;
            }

            int adjustedPosA = posA + seperator.Length;
            if (adjustedPosA >= name.Length)
            {
                return string.Empty;
            }

            string trimedValue = name.Substring(adjustedPosA).Remove(0, 3);
            return trimedValue;
        }
    }
}