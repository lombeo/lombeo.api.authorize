using Lombeo.Api.Authorize.Infra.Constants;
using System.Text.RegularExpressions;

namespace Lombeo.Api.Authorize.Infra.Helps
{
    public static class Extensions
    {
        public static string ResolveUserName(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            if (value.Contains("@"))
            {
                var index = value.IndexOf('@') / 2;
                string pattern = @"(?<=[\w-._\+%]{" + index + @"})[\w\-._\+%]*(?=[\w]{0}@)";
                return Regex.Replace(value, pattern, m => new string('*', m.Length));
            }

            return value;
        }
    }
}
