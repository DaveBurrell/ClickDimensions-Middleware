#region Using Directives
using System.Net;
#endregion Using Directives
namespace FormCapture
{
    public static class Extensions
    {
        public static string ToEncodedString(this string s)
        {
            return WebUtility.UrlEncode(s);
        }
    }
}