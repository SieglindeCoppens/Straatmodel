using System;
using System.Collections.Generic;

namespace Straatmodel
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var provincies = DictionaryOpvuller.geefStratenDictionary();

            provincies["Oost-Vlaanderen"]["Gent"][0].Graaf.ShowGraaf();
            provincies["Oost-Vlaanderen"]["Gent"][1].Graaf.ShowGraaf();
            provincies["Oost-Vlaanderen"]["Gent"][2].Graaf.ShowGraaf();



        }
    }
}


//Algemene vragen: geef je je dictionaries en andere gegevens zoals straatnaamIDStraatnaam het beste door via de main? 
//Kan je met het unzippen best je path als parameter doorgeven voor de herbruikbaarheid, of beter niet? 
//Beste om alle lijsten door te geven via de main? 