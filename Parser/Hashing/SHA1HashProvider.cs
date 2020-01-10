using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VersionSwitcher_Server.Hashing
{
    class SHA1HashProvider : HashProvider
    {
        public override string FromStream(Stream stream)
        {
            byte[] arrbytHashValue;

            SHA1CryptoServiceProvider oSHA1Hasher = new SHA1CryptoServiceProvider();

            arrbytHashValue = oSHA1Hasher.ComputeHash(stream);

            string strHashData = BitConverter.ToString(arrbytHashValue);
            strHashData = strHashData.Replace("-", "");

            return strHashData;
        }
    }
}
