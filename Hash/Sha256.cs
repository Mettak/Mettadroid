using Java.Security;
using System.Text;

namespace Mettarin.Android.Hash
{
    public class Sha256
    {
        public static string HashBytes(byte[] data)
        {
            MessageDigest md = MessageDigest.GetInstance("SHA-256");
            md.Update(data);
            byte[] digest = md.Digest();
            StringBuilder hashString = new StringBuilder();

            foreach (var @byte in digest)
            {
                hashString.Append(@byte.ToString("x2"));
            }

            return hashString.ToString();
        }

        public static string HashString(string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            MessageDigest md = MessageDigest.GetInstance("SHA-256");
            md.Update(encoding.GetBytes(text));
            byte[] digest = md.Digest();
            StringBuilder hashString = new StringBuilder();

            foreach (var @byte in digest)
            {
                hashString.Append(@byte.ToString("x2"));
            }

            return hashString.ToString();
        }

        public static string HashString(string text)
        {
            return HashString(text, Encoding.UTF8);
        }
    }
}
