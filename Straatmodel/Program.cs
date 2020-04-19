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

            //We maken met deze gegevens een lijst van Straat objecten aan, hierin zitten ook nog de straten in niet-Vlaanderen
            List<Straat> straten = Stratenmaker.MaakStraten(straatnaamIDStraatnaam, straatnaamIDSegmentlijst);


            //Lees de bestanden in van gemeentes/provincies
            Dictionary<string, string> gemeenteIDProvincie = GegevensLezer_adressen.LeesProvincies();
            Dictionary<string, string> gemeentes = GegevensLezer_adressen.LeesGemeentes();
            Dictionary<string, string> stratenIDgemeentesID = GegevensLezer_adressen.LeesLink();

            //Vul een dictionary op met gecombineerde gegevens provincies, gemeentes,straten 
            Dictionary<string, Dictionary<string, List<Straat>>> provincies = DictionaryOpvuller.geefStratenDictionary(straten, gemeenteIDProvincie, gemeentes, stratenIDgemeentesID);

            //Rapport uitprinten
            SchrijfRapport.PrintRapport(provincies);

            //Bestanden uitprinten 
            SchrijfBestand.PrintDocumenten(provincies);
        }
    }
}
