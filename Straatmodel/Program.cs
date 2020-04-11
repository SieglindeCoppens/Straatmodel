using System;
using System.Collections.Generic;

namespace Straatmodel
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Lees de bestanden in van de straten 
            Dictionary<int, string> straatnaamIDStraatnaam = GegevensLezer_segmenten.LeesStraten();
            Dictionary<int, List<Segment>> straatnaamIDSegmentlijst = GegevensLezer_segmenten.LeesSegmenten();

            //We maken met deze gegevens een lijst van Straat objecten aan 
            List<Straat> straten = Stratenmaker.MaakStraten(straatnaamIDStraatnaam, straatnaamIDSegmentlijst);

            //Lees de bestanden in van gemeentes/provincies
            Dictionary<string, string> gemeenteIDProvincie = GegevensLezer_adressen.LeesProvincies();
            Dictionary<string, string> gemeentes = GegevensLezer_adressen.LeesGemeentes();
            Dictionary<string, string> stratenIDgemeentesID = GegevensLezer_adressen.LeesLink();

            //Vul een dictionary op met gecombineerde gegevens provincies, gemeentes,straten 
            var provincies = DictionaryOpvuller.geefStratenDictionary(straten, gemeenteIDProvincie, gemeentes, stratenIDgemeentesID);

            provincies["Oost-Vlaanderen"]["Gent"][0].Graaf.Map;
            provincies["Oost-Vlaanderen"]["Gent"][1].Graaf.ShowGraaf();
            provincies["Oost-Vlaanderen"]["Gent"][2].Graaf.ShowGraaf();



        }
    }
}


//Algemene vragen: geef je je dictionaries en andere gegevens zoals straatnaamIDStraatnaam het beste door via de main? 
//Kan je met het unzippen best je path als parameter doorgeven voor de herbruikbaarheid, of beter niet? 
//Beste om alle lijsten door te geven via de main? 