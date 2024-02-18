using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bashirov_16.Helpers
{
    public class Hashing
    {
        public static string GetHashedPassword(string password)
        {
            using (var hash = SHA1.Create())
            {
                byte[] hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                return string.Concat(hashBytes.Select(x => x.ToString()));
            }
        }
    }
}
