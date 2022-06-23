using System;

namespace AulpagCollecte.Tools
{
    class ToolString
    {
        public static string CapitalizeFirstLetter(string s)
        {         
            if (String.IsNullOrEmpty(s)) return s;
            if (s.Length == 1) return s.ToUpper();
            string t = s.Remove(1).ToUpper() + s.Substring(1);
            return t;
        }
    }
}
