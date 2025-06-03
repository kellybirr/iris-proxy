using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace IrisProxy.Models
{
    public static class BasicAuth
    {
        public static bool Decode(HttpRequestMessage req, out NetworkCredential credential)
        {
            credential = null;

            string authStr = req.Headers.Authorization?.ToString();
            if (string.IsNullOrEmpty(authStr)) return false;

            string[] parts = authStr.Split(' ');
            if (parts.Length != 2) return false;
            if (!parts[0].Equals("Basic", StringComparison.OrdinalIgnoreCase)) return false;
            
            string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]));

            string[] userPass = decoded.Split(':');
            if (userPass.Length != 2) return false;

            credential = new NetworkCredential(userPass[0].Trim(), userPass[1].Trim());
            return true;
        }
    }
}