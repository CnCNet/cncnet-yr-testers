using System;
using System.IO;
using System.Security.Cryptography;

namespace CnCNetTesters
{
    public class CheckForUpdate
    {
        public static string CheckSum(string file)
        {
            String sourceFileName = file;
            Byte[] shaHash;

            //Use Sha1Managed if you really want sha1
            using (var shaForStream = new SHA256Managed())
            using (Stream sourceFileStream = File.Open(sourceFileName, FileMode.Open))
            using (Stream sourceStream = new CryptoStream(sourceFileStream, shaForStream, CryptoStreamMode.Read))
            {
                //Do something with the sourceStream 
                //NOTE You need to read all the bytes, otherwise you'll get an exception ({"Hash must be finalized before the hash value is retrieved."}) 
                while (sourceStream.ReadByte() != -1) ;
                shaHash = shaForStream.Hash;
            }
            Console.WriteLine(Convert.ToBase64String(shaHash));
            return Convert.ToBase64String(shaHash);
        }
    }
}
