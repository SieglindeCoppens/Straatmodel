using System;
using System.Collections.Generic;

namespace Tool1_BestandSchrijven
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Lees de bestanden in van de straten 
            Dictionary<int, string> straatnaamIDStraatnaam = GegevensLezer_segmenten.LeesStraten();
            Dictionary<int, List<Segment>> straatnaamIDSegmentlijst = GegevensLezer_segmenten.LeesSegmenten();

            //Lees de bestanden in van gemeentes/provincies
            Dictionary<string, string> gemeenteIDProvincie = GegevensLezer_adressen.LeesProvincies();
            Dictionary<string, string> gemeentes = GegevensLezer_adressen.LeesGemeentes();
            Dictionary<string, string> stratenIDgemeentesID = GegevensLezer_adressen.LeesLink();

            Dictionary<string, Dictionary<string, Dictionary<int, List<Segment>>>> provincies = DictionaryOpvuller.geefStratenDictionary(straatnaamIDSegmentlijst, gemeenteIDProvincie, gemeentes, stratenIDgemeentesID, straatnaamIDStraatnaam);

            StratenFactory sf = new StratenFactory();
            List<Straat> straten = sf.MaakStraten(provincies);

            //Rapport uitprinten
            SchrijfRapport.PrintRapport(straten);

            ////Bestanden uitprinten 
            SchrijfBestand.PrintDocumenten(straten);
        }
    }
}
