
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Tool1_BestandSchrijven
{
    class SchrijfRapport
    {
        //Telstraten kan ik ook in main aanroepen en dan doorgeven? Is dit nuttig? 
        //kan ik best alles berekenen apart en dan in een dictionary zetten en die doorgeven aan PrintRapport?
        public static void PrintRapport(Dictionary<string, Dictionary<string, List<Straat>>> provincies)
        {
            if (File.Exists(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Rapport.txt"))
            {
                File.Delete(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Rapport.txt");
            }

            using StreamWriter writer = File.CreateText(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Rapport.txt");
            writer.WriteLine($"Totaal aantal straten: {Stratenteller.TelStraten(provincies)}\n");

            writer.WriteLine("Aantal straten per provincie:");
            foreach(KeyValuePair<string, Dictionary<string, List<Straat>>>  provincie in provincies)
            {
                writer.WriteLine($"   -  {provincie.Key} : {Stratenteller.TelStraten(provincie.Value)}");
            }

            foreach(KeyValuePair<string, Dictionary<string, List<Straat>>> provincie in provincies)
            {
                writer.WriteLine($"\n Straatinfo {provincie.Key}");

                foreach(KeyValuePair<string, List<Straat>> gemeente in provincie.Value)
                {
                    List<Straat> straten = gemeente.Value;
                    double totaleLengte = 0;
                    foreach (Straat straat in straten)
                    {
                        totaleLengte += straat.Lengte;
                    }

                    writer.WriteLine($"   -  {gemeente.Key} : {gemeente.Value.Count}, {totaleLengte}");
                    //var gesorteerdestraten = from Straat straat in gemeente.Value
                    //                         orderby straat.Lengte
                    //                         select straat;

                    
                    straten.Sort();

                    Straat kortste = straten[0];
                    Straat langste = straten[straten.Count - 1];

                    writer.WriteLine($"         o  {kortste.StraatID}, {kortste.Straatnaam}, {kortste.Lengte}");
                    writer.WriteLine($"         o  {langste.StraatID}, {langste.Straatnaam}, {langste.Lengte}");

                }

            }

            


        }
    }
}
