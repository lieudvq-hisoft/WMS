using System;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Utils
{
    public static class MyFunction
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            var idClaim = user.Claims.FirstOrDefault(i => i.Type.Equals("UserId"));
            if (idClaim != null)
            {
                return idClaim.Value;
            }
            return "";
        }
        public static string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }

        public static TimeSpan TimespanCalculate(DateTime dateTime)
        {
            return dateTime - DateTime.Now;
        }
    }
}

