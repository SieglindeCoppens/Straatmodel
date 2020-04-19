using System;
using System.Collections.Generic;
using System.Text;

namespace Tool1_BestandSchrijven
{
    class Graaf
    {
        public int GraafID { get; set; }
        public Dictionary<Knoop, List<Segment>> Map { get; set; }

        public Graaf(List<Segment> segmentLijst)
        {
            Map = new Dictionary<Knoop, List<Segment>>();
            GraafID = IDGenerator.CreateGraafID();

            //Map van de graaf vullen met de knopen van de segmenten van de straat die je hebt!
          
            foreach(Segment segment in segmentLijst)
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
            Console.WriteLine("\n**************************************************************************************************");
            Console.WriteLine($"Graaf met ID {this.GraafID} heeft de knopen:");
            foreach(KeyValuePair<Knoop, List<Segment>> knoopMetSegmenten in this.Map)
            {
                Console.WriteLine($"Knoop: {knoopMetSegmenten.Key} met segmenten: ");

                foreach(Segment segment in knoopMetSegmenten.Value)
                {
                    Console.Write($"-   {segment} \n");
                }
     
            }
           
        }

        public List<Knoop> GetKnopen()
        {
            //Wat hier te returnen?
            List<Knoop> knopen = new List<Knoop>();
            foreach(KeyValuePair<Knoop, List<Segment>> knoopMetSegmenten in this.Map)
            {
                knopen.Add(knoopMetSegmenten.Key);
            }
            return knopen;
        }
    }
}
