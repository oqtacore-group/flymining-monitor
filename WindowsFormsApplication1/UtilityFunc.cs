using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BitcoinInfoMiner
{
    static class UtilityFunc
    {
        private static readonly Regex sWhitespace = new Regex(@"\s+"); 
        public static string ReplaceWhitespace(string input, string replacement)
        {
            return sWhitespace.Replace(input, replacement);
        }
        private static readonly Regex rxNonDigits = new Regex(@"[^\d]+");// Для отбрасывания все не цифр
        public static string CleanStringOfNonDigits(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            string cleaned = rxNonDigits.Replace(s, "");
            return cleaned;
        }
    }
}
