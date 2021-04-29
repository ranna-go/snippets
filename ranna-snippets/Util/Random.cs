using System;
using System.Linq;
using System.Text;

namespace ranna_snippets.Util
{
    public static class RandomUtil
    {
        private static Random rng = new Random();

        public static string GetString(int len, string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789") =>
            new string(Enumerable.Repeat(charset, len).Select(s => s[rng.Next(s.Length)]).ToArray());
    }
}
