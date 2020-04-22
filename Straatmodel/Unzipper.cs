using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;

namespace Tool1_BestandSchrijven
{
    class Unzipper
    {
        public static void Unzip(string extractpath, string zippath, string path)
        {
            if (!(File.Exists(path)))
            {
                ZipFile.ExtractToDirectory(zippath, extractpath);
            }
        }
    }
}
