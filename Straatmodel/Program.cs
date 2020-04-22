using System;
using System.Collections.Generic;

namespace Tool1_BestandSchrijven
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Unzipper.Unzip(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo", @"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRstraatnamen.zip", @"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRstraatnamen.csv");
            Unzipper.Unzip(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo", @"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRdata.zip", @"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRdata.csv");

            //Lees de bestanden in van de straten 
            Dictionary<int, string> straatnaamIDStraatnaam = GegevensLezer_segmenten.LeesStraten(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRstraatnamen.csv");
            Dictionary<int, List<Segment>> straatnaamIDSegmentlijst = GegevensLezer_segmenten.LeesSegmenten(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRdata.csv");

            //Lees de bestanden in van gemeentes/provincies
            Dictionary<string, string> gemeenteIDProvincie = GegevensLezer_adressen.LeesProvincies(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\ProvincieInfo.csv", @"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\ProvincieIDsVlaanderen.csv");
            Dictionary<string, string> gemeentes = GegevensLezer_adressen.LeesGemeentes(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRGemeentenaam.csv");
            Dictionary<string, string> stratenIDgemeentesID = GegevensLezer_adressen.LeesLink(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRGemeenteID.csv");

            Dictionary<string, Dictionary<string, Dictionary<string, List<Segment>>>> provincies = DictionaryOpvuller.geefStratenDictionary(straatnaamIDSegmentlijst, gemeenteIDProvincie, gemeentes, stratenIDgemeentesID, straatnaamIDStraatnaam);

            StratenFactory sf = new StratenFactory();
            List<Straat> straten = sf.MaakStraten(provincies);

            //Rapport uitprinten
            SchrijfRapport.PrintRapport(straten);

            ////Bestanden uitprinten 
            SchrijfBestand.PrintDocumenten(straten);
        }
    }
}
