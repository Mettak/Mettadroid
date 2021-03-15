using Java.Security;
using System;
using System.Text;

namespace Mettarin.Android.Hash
{
    public class Sha256
    {
        public const int HashStringSize = 64;

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

        public static StringBuilder HashString(StringBuilder input, StringBuilder output, Encoding encoding)
        {
            if (output.Capacity != 64)
            {
                throw new ArgumentOutOfRangeException();
            }

            char[] chars = new char[input.Length];
            input.CopyTo(0, chars, 0, input.Length);

            MessageDigest md = MessageDigest.GetInstance("SHA-256");
            md.Update(encoding.GetBytes(chars));
            Array.Clear(chars, 0, chars.Length);
            byte[] digest = md.Digest();
            output.Clear();

            foreach (var @byte in digest)
            {
                output.Append(@byte.ToString("x2"));
            }

            return output;
        }

        public static StringBuilder HashString(StringBuilder input, StringBuilder output)
        {
            return HashString(input, output, Encoding.UTF8);
        }
    }
}
