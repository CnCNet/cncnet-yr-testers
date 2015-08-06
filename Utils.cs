using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CnCNetTesters
{
    static public class Utils
    {
        /// <summary>
        /// Read CSV Files
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string[] ReadCSV(string line)
        {
            while (line.StartsWith(" ")) line = line.Substring(1, line.Length - 1);
            var result = new List<string>();
            bool ccontinue = false;
            string value = "";
            string[] tempValues = line.Split(',');

            foreach (string tempValue in tempValues)
            {
                if (ccontinue)
                {
                    // End of field
                    if (tempValue.EndsWith("\""))
                    {
                        value += "," + tempValue.Substring(0, tempValue.Length - 1);
                        result.Add(value);
                        value = "";
                        ccontinue = false;
                        continue;
                    }
                    else
                    {
                        // Field still not ended
                        value += "," + tempValue;
                        continue;
                    }
                }

                // Fully encapsulated with no comma within
                if (tempValue.StartsWith("\"") && tempValue.EndsWith("\""))
                {
                    if ((tempValue.EndsWith("\"\"") && !tempValue.EndsWith("\"\"\"")) && tempValue != "\"\"")
                    {
                        ccontinue = true;
                        value = tempValue;
                        continue;
                    }

                    result.Add(tempValue.Substring(1, tempValue.Length - 2));
                    continue;
                }

                // Start of encapsulation but comma has split it into at least next field
                if (tempValue.StartsWith("\"") && !tempValue.EndsWith("\""))
                {
                    ccontinue = true;
                    value += tempValue.Substring(1);
                    continue;
                }

                // Non encapsulated complete field
                result.Add(tempValue);

            }
            return result.ToArray();
        }

        /// <summary>
        /// Read Remote File
        /// </summary>
        /// <param name="uRL"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] ReadWebsite(string uRL, char[] separator)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(uRL))
                    using (var streamReader = new StreamReader(stream))
                    {
                        var lines = new List<String>();
                        var streamReaderLines = streamReader.ReadToEnd().Split(separator);
                        foreach (string line in streamReaderLines)
                        {
                            if (line.Length > 0) lines.Add(line);
                        }
                        return lines.ToArray();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get SHA1 checksum of file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetSHA1Checksum(string file)
        {
            string checksum = "";
            if (!File.Exists(file)) return "FileDoesNotExist";
            try
            {
                using (var sHA1 = new SHA1CryptoServiceProvider())
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    checksum = BitConverter.ToString(sHA1.ComputeHash(fileStream)).Replace("-", "").ToLower();
                return checksum;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex);
                return checksum;
            }
        }
    }
}
