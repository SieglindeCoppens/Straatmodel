using System;
using System.Collections.Generic;
using System.Text;

namespace Tool1_BestandSchrijven
{
    class Straat
    {
        public Graaf Graaf { get; set; }
        public int StraatID { get; set; }
        public string Straatnaam { get; set; }

        public double Lengte { get; set; }

        public string Provincie { get; set; }

        public string Gemeente { get; set; }

        public Straat(string straatnaam, Graaf graaf, string provincie, string gemeente)
        {
            StraatID = IDGenerator.CreateStraatID();
            Straatnaam = straatnaam;
            Graaf = graaf;
            Gemeente = gemeente;
            Provincie = provincie;
            Lengte = this.BerekenLengte();

        }

        private double BerekenLengte()
        {
            //Dit doen we door de afstanden tussen de vertices van elk van de segmenten te berekenen
            double totaleLengte = 0;

            Graaf graaf = this.Graaf;
            var map = graaf.Map;

            //In een graaf komen meerdere keren hetzelfde segment voor, dus hou ik een lijst bij met de segmentID's van reeds berekende segmenten
            List<int> berekendeSegmentIDs = new List<int>();

            foreach(KeyValuePair<Knoop, List<Segment>> knoop in map)
            {
                foreach(Segment segment in knoop.Value)
                {
                    if (!berekendeSegmentIDs.Contains(segment.SegmentID))
                    {
                        List<Punt> vertices = segment.Vertices;
                        for (int i = 1; i < vertices.Count; i++)
                        {
                            Punt eerstePunt = vertices[i - 1];
                            Punt tweedePunt = vertices[i];


                            double lengte = Math.Sqrt(Math.Pow(Math.Abs(eerstePunt.X - tweedePunt.X), 2) + Math.Pow(Math.Abs(eerstePunt.Y - tweedePunt.Y), 2));

                            totaleLengte += lengte;
                        }
                        berekendeSegmentIDs.Add(segment.SegmentID);
                    }
                }
            }
            return Math.Round(totaleLengte,4);
        }

        public List<Knoop> getKnopen() 
        {
            return Graaf.GetKnopen();
        }


        //NOG TE IMPLEMENTEREN!
        public void showStraat()
        {
            Console.WriteLine("\n**************************************************************************************************");
            Console.WriteLine($"{StraatID} : {Straatnaam} met lengte {Lengte}m");
        }
    }
}
