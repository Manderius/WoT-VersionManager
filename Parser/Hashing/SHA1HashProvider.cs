using System;
using System.IO;
using System.Security.Cryptography;

namespace VersionManager.Hashing
{
    public class SHA1HashProvider : HashProvider
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
