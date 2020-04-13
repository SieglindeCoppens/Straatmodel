
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
                    foreach (Straat straat in straten)
                    {

                    }


                    writer.WriteLine($"{gemeente.Key} : {gemeente.Value.Count}, {}");
                    //var gesorteerdestraten = from Straat straat in gemeente.Value
                    //                         orderby straat.Lengte
                    //                         select straat;

                    
                    straten.Sort();

                    Straat langste = straten[0];
                    Straat kortste = straten[straten.Count - 1];

                }

            }

            


        }
    }
}
