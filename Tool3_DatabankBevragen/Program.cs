using System;
using System.Collections.Generic;

namespace Tool3_DatabankBevragen
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBevrager db = new DataBevrager("Data Source=DESKTOP-HT91N8R\\SQLEXPRESS;Initial Catalog=db_Wegennet;Integrated Security=True");

            db.GeefStraatinfo("gent", "goudenregenstraat");

            Console.WriteLine("Wat wenst u te doen (Typ het nummer)?");
            Console.WriteLine("1. Lijst van straatID's van een gemeente opvragen");
            Console.WriteLine("2. Straatinfo opvragen via een straatID");
            Console.WriteLine("3. Straatinfo opvragen via straatnaam en gemeentenaam");
            Console.WriteLine("4. Alle straatnamen van een gemeente opvragen (alfabetische volgorde)");
            Console.WriteLine("5. Een overzichtsrapport voor een provincie");
            Console.WriteLine("6. Alle straatnamen grenzend aan een straat opvragen");

            int keuze = int.Parse(Console.ReadLine().Trim('.', ' '));

            switch (keuze)
            {
                case 1:
                    Console.WriteLine("Van welke gemeente wenst u de straatID's op te vragen?");
                    string gemeente = Console.ReadLine().ToLower();
                    IEnumerable<int> straatIDs = db.GeefStraatIDs(gemeente);
                    foreach (int straatID in straatIDs)
                    {
                        Console.WriteLine(straatID);
                    }
                    break;

                case 2:
                    Console.WriteLine("Van welke straatID wenst u de straatinfo? (geef straatID) ");
                    int straatId = int.Parse(Console.ReadLine());
                    db.GeefStraatinfo(straatId);

                    break;
                case 3:
                    Console.WriteLine("Geef de straatnaam:");
                    string straatnaam = Console.ReadLine().ToLower();
                    Console.WriteLine("Geef de gemeentenaam:");
                    string gemeentenaam = Console.ReadLine().ToLower();
                    db.GeefStraatinfo(gemeentenaam, straatnaam);
                    break;
                case 4:
                    Console.WriteLine("Geef de gemeentenaam:");
                    string gemeentenaam2 = Console.ReadLine();
                    List<string> straatnamen = db.GeefStraatnamenVanGemeente(gemeentenaam2);
                    foreach (string straatnaam2 in straatnamen)
                    {
                        Console.WriteLine(straatnaam2);
                    }
                    break;
                case 5:
                    Console.WriteLine("Geef de provincie:");
                    string provincie = Console.ReadLine();
                    db.GeefProvincieoverzicht(provincie);
                    break;
                case 6:
                    Console.WriteLine("Geef een straatID: ");
                    int strId = int.Parse(Console.ReadLine());
                    List<string> aangrenzendeStraten = db.GeefAangrenzendeStraten(strId);
                    foreach (string straat in aangrenzendeStraten)
                    {
                        Console.WriteLine(straat);
                    }
                    break;
            }
        }
    }
}