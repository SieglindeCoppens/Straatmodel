using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;

namespace Straatmodel
{
    class Unzipper
    {
        public static void UnzipStraten()
        {
            if(!(File.Exists(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRstraatnamen.csv")))
            {
                string zipPath = @"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRstraatnamen.zip";
                string extractPath = @"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo";

                ZipFile.ExtractToDirectory(zipPath, extractPath);
            }
        }

        //We werken nu om onze code te testen maar met een bestandje van de 10 eerste segmenten 
        public static void UnzipData()
        {
            if (!(File.Exists(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRdata.csv")))
            {
                string zipPath = @"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRdata.zip";
                string extractPath = @"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo";

                ZipFile.ExtractToDirectory(zipPath, extractPath);

            }
        }

    }
}

//Ik zou deze twee ook in 1 methode kunnen steekn met als parameter het zipPath, ze hebben toch beide hetzelfde extractpath!
//Misschien eerder zorgen dat de geextracte files gedelete worden in plaats van te checken of ze al unzipped zijn!! 
