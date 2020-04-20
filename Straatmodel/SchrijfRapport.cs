
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Tool1_BestandSchrijven
{
    class SchrijfRapport
    {
        public static void PrintRapport(Dictionary<string, Dictionary<string, List<Straat>>> provincies)
        {
            if (File.Exists(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Rapport.txt"))
            {
                File.Delete(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Rapport.txt");
            }

            using StreamWriter writer = File.CreateText(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Rapport.txt");
            writer.WriteLine($"Totaal aantal straten: {Stratenteller.TelStraten(provincies)}\n");
            writer.WriteLine("Aantal straten per provincie:");

            //De provincies ordenen op alfabetische volgorde
            var alfabetischeProvincies = provincies.OrderBy(p => p.Key);
            foreach(var provincie in alfabetischeProvincies)
            {
                writer.WriteLine($"   -  {provincie.Key} : {Stratenteller.TelStraten(provincie.Value)}");
            }

            foreach (var provincie in alfabetischeProvincies)
            {
                writer.WriteLine($"\n Straatinfo {provincie.Key}");

                //Gemeentes ordenen op alfabetische volgorde
                var gesorteerdegemeentes = provincie.Value.OrderBy(g => g.Key);
                foreach (var gemeente in gesorteerdegemeentes)
                {
                    List<Straat> straten = gemeente.Value;

                    //Totale lengte berekenen met linq
                    double totaleLengte = straten.Sum(s => s.Lengte);
                    writer.WriteLine($"   -  {gemeente.Key} : {gemeente.Value.Count}, {totaleLengte}");

                    var gesorteerdestraten = straten.OrderBy(s => s.Lengte);
                    Straat kortste = gesorteerdestraten.First();
                    Straat langste = gesorteerdestraten.Last();
                    writer.WriteLine($"         o  {kortste.StraatID}, {kortste.Straatnaam}, {kortste.Lengte}");
                    writer.WriteLine($"         o  {langste.StraatID}, {langste.Straatnaam}, {langste.Lengte}");
                }
            }
        }
    }
}