using System;
using System.Collections.Generic;
using System.Text;

namespace Tool3_Versie2
{
    class Straat
    {
        public Graaf Graaf { get; set; }
        public int StraatID { get; set; }
        public string Straatnaam { get; set; }

        public double Lengte { get; set; }

        public string Provincie { get; set;}
        public string Gemeente { get; set; }

        public Straat(int straatID, string straatnaam, Graaf graaf, string provincie, string gemeente, double lengte)
        {
            StraatID = straatID;
            Straatnaam = straatnaam;
            Graaf = graaf;
            Lengte = lengte;
            Provincie = provincie;
            Gemeente = gemeente;

        }

        public List<Knoop> getKnopen()
        {
            return Graaf.GetKnopen();
        }

        public void showStraat()
        {
            Console.WriteLine($"{StraatID},{Straatnaam},{Gemeente},{Provincie}");
            Console.WriteLine($"{Graaf} : ");

        }
    }
}
