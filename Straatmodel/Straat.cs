using System;
using System.Collections.Generic;
using System.Text;

namespace Straatmodel
{
    public class Straat
    {
        public Graaf Graaf { get; set; }
        public int StraatID { get; set; }
        public string Straatnaam { get; set; }

        public Straat(int straatID, string straatnaam, Graaf graaf)
        {
            StraatID = straatID;
            Straatnaam = straatnaam;
            Graaf = graaf;

        }

        public double BerekenLengte()
        {
            //Dit doen we door de afstanden tussen de vertices van elk van de segmenten te berekenen
            double lengte = 0;

            Graaf graaf = this.Graaf;
            var map = graaf.Map;

            //In een graaf komen meerdere keren hetzelfde segment voor, dus hou ik een lijst bij met de segmentID's van reeds berekende segmenten
            List<int> berekendeSegmenten = new List<int>();

            foreach(KeyValuePair<Knoop, List<Segment>> knoop in map)
            {
                foreach(Segment segment in knoop.Value)
                {

                }
            }





            return lengte;
        }

        public List<Knoop> getKnopen() 
        {
            return Graaf.GetKnopen();
        }
        public void showStraat()
        {

        }


    }
}
