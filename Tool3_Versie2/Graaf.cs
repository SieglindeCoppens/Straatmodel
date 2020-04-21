using System;
using System.Collections.Generic;
using System.Text;

namespace Tool3_Versie2
{
    class Graaf
    {
        public int GraafID { get; set; }
        public Dictionary<Knoop, List<Segment>> Map { get; set; }

        public Graaf(List<Segment> segmentLijst, int graafId)
        {
            Map = new Dictionary<Knoop, List<Segment>>();
            GraafID = graafId;

            //Map van de graaf vullen met de knopen van de segmenten van de straat die je hebt!

            foreach (Segment segment in segmentLijst)
            {
                if (!(Map.ContainsKey(segment.BeginKnoop)))             //Knoop toevoegen aan graaf als de knoop er nog niet inzit
                {
                    Map.Add(segment.BeginKnoop, new List<Segment>());
                }
                Map[segment.BeginKnoop].Add(segment);                   //altijd segment toevoegen aan de knoop 

                if (!(Map.ContainsKey(segment.EindKnoop)))
                {
                    Map.Add(segment.EindKnoop, new List<Segment>());
                }
                Map[segment.EindKnoop].Add(segment);
            }
        }

        public void ShowGraaf()
        {
             foreach(KeyValuePair<Knoop, List<Segment>> knoopmap in Map)
            {
                Console.WriteLine($"Knoop[{knoopmap.Key.KnoopID},[{knoopmap.Key.Punt.X},{knoopmap.Key.Punt.Y}]]");
                foreach(Segment segment in knoopmap.Value)
                {
                    Console.WriteLine($"     [segment:{segment.SegmentID},begin{segment.BeginKnoop.KnoopID},eind{segment.EindKnoop.KnoopID}]");
                    Console.WriteLine($"            ({segment.BeginKnoop.Punt.X},{segment.BeginKnoop.Punt.Y})");
                    foreach(Punt punt in segment.Vertices)
                    {
                        Console.WriteLine($"            ({punt.X},{punt.Y})");
                    }
                    Console.WriteLine($"            ({segment.EindKnoop.Punt.X},{segment.EindKnoop.Punt.Y})");
                }
            }
        }

        public List<Knoop> GetKnopen()
        {
            //Wat hier te returnen?
            List<Knoop> knopen = new List<Knoop>();
            foreach (KeyValuePair<Knoop, List<Segment>> knoopMetSegmenten in this.Map)
            {
                knopen.Add(knoopMetSegmenten.Key);
            }
            return knopen;
        }
    }
}
