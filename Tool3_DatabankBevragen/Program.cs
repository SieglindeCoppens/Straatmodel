using System;
using System.Collections.Generic;

namespace Tool3_DatabankBevragen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            DataBevrager db = new DataBevrager("Data Source=DESKTOP-HT91N8R\\SQLEXPRESS;Initial Catalog=db_Wegennet;Integrated Security=True");
            

            Console.WriteLine("Wat wenst u te doen (Typ het nummer)?");
            Console.WriteLine("1. Lijst van straatID's van een gemeente opvragen");
            Console.WriteLine("2. Straatinfo opvragen via een straatID");
            Console.WriteLine("3. Straatinfo opvragen via straatnaam en gemeentenaam");
            Console.WriteLine("4. Alle straatnamen van een gemeente opvragen (alfabetische volgorde)");

            int keuze = int.Parse(Console.ReadLine().Trim('.', ' '));

            switch (keuze)
            {
                case 1:
                    Console.WriteLine("Van welke gemeente wenst u de straatID's op te vragen?");
                    string gemeente = Console.ReadLine();
                    IEnumerable<int> straatIDs = db.GeefStraatIDs(gemeente);
                    foreach(int straatID in straatIDs)
                    {
                        Console.WriteLine(straatID);
                    }


                    break;
                case 2:
                    Console.WriteLine("Van welke straatID wenst u de straatinfo? ");
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }
    }
}
