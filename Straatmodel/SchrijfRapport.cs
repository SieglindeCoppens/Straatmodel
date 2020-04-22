
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Tool1_BestandSchrijven
{
    class SchrijfRapport
    {
        public static void PrintRapport(List<Straat> straten)
        {
            if (File.Exists(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Rapport.txt"))
            {
                File.Delete(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Rapport.txt");
            }

            using StreamWriter writer = File.CreateText(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Rapport.txt");
            writer.WriteLine($"Totaal aantal straten: {straten.Count()}\n");
            writer.WriteLine("Aantal straten per provincie:");

            //Straten ordenen op alfabetische volgorde van de provincies, dan op 
            var geordendestraten = straten.OrderBy(s => s.Provincie).ThenBy(s => s.Gemeente);
            
            //Aantal straten per provincie tellen
            var provincies = geordendestraten.Select(s => s.Provincie).Distinct();
            foreach(var provincie in provincies)
            {
                writer.WriteLine($"   -  {provincie} : {geordendestraten.Count(s=>s.Provincie==provincie)}");
            }
            //ER KUNNEN VERSCHILLENDE STEDEN ZIJN MET DEZELFDE NAAM, MAAR IN EEN ANDERE PROVINCIE!!
            foreach(var provincie in provincies)
            {
                writer.WriteLine($"\n Straatinfo {provincie}");
                var gemeenten = geordendestraten.Where(s => s.Provincie == provincie).Select(s => s.Gemeente).Distinct();
                foreach(var gemeente in gemeenten)
                {
                    var stratenVanGemeente = geordendestraten.Where(s => (s.Gemeente == gemeente) && (s.Provincie == provincie));

                    int aantalStraten = stratenVanGemeente.Count();
                    double totaleLengte = stratenVanGemeente.Sum(s => s.Lengte);
                    writer.WriteLine($"   -  {gemeente} : {aantalStraten}, {totaleLengte}");

                    var gesorteerdestraten = stratenVanGemeente.OrderBy(s => s.Lengte);
                    Straat kortste = gesorteerdestraten.First();
                    Straat langste = gesorteerdestraten.Last();
                    writer.WriteLine($"         o  {kortste.StraatID}, {kortste.Straatnaam}, {kortste.Lengte}");
                    writer.WriteLine($"         o  {langste.StraatID}, {langste.Straatnaam}, {langste.Lengte}");
                }

            }
        }
    }
}