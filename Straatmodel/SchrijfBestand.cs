
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tool1_BestandSchrijven
{
    class SchrijfBestand
    {
        public static void PrintDocumenten(Dictionary<string, Dictionary<string, List<Straat>>> provincies)
        {
            if (File.Exists(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Straten.txt") && File.Exists(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Segmenten.txt") && File.Exists(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Knopen.txt"))
            {

            }


            /*We overlopen per straat de graaf, en voegen gaandeweg de knopen en segmenten die we tegenkomen op. 
             * MAAR:
             * - We zullen segmenten verschillende keren tegenkomen in 1 straat, aangezien ze in de graaf aan meerdere knopen gekoppeld zijn
             * - We zullen over de straten heen verschillende keren dezelfde knopen tegenkomen omdat deze knopen de verschillende straten verbinden
             * 
             * Dus ik hou van beide bij welke er al uitgeprint zijn! 
             * */
            List<int> uitgeprinteSegmentenID = new List<int>();
            List<int> uitgeprinteKnopenID = new List<int>();


            //Beter apart of samen? 
            using StreamWriter writer = File.CreateText(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Straten.txt");
            using StreamWriter writer2 = File.CreateText(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Segmenten.txt");
            using StreamWriter writer3 = File.CreateText(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Knopen.txt");
            writer.WriteLine("StraatID;Straatnaam;Gemeente;Provincie;GraafID");
            writer2.WriteLine("SegmentID;BeginknoopID;EindknoopID;straatID;puntenlijst");
            writer3.WriteLine("KnoopID;X;Y");
            foreach (KeyValuePair<string, Dictionary<string, List<Straat>>> provincie in provincies)
            {
                foreach (KeyValuePair<string, List<Straat>> gemeente in provincie.Value)
                {
                    foreach(Straat straat in gemeente.Value)
                    {
                        writer.WriteLine($"{straat.StraatID};{straat.Straatnaam.Trim()};{gemeente.Key};{provincie.Key};{straat.Graaf.GraafID}");

                        Dictionary<Knoop, List<Segment>> map = straat.Graaf.Map;

                        foreach(KeyValuePair<Knoop, List<Segment>> knoopMetSegmenten in map)
                        {
                            Knoop knoop = knoopMetSegmenten.Key;
                            if (!uitgeprinteKnopenID.Contains(knoop.KnoopID))
                            {
                                writer3.WriteLine($"{knoop.KnoopID};{knoop.Punt.X};{knoop.Punt.Y}");
                                uitgeprinteKnopenID.Add(knoop.KnoopID);
                            }

                            foreach(Segment segment in knoopMetSegmenten.Value)
                            {
                                if (!uitgeprinteSegmentenID.Contains(segment.SegmentID))
                                {
                                    string punten = "";
                                    List<Punt> puntenlijst = segment.Vertices;

                                    for (int i = 0; i < puntenlijst.Count; i++)
                                    {
                                        //Deze if zodat er enkel tussen de 
                                        if (!(i == segment.Vertices.Count - 1))
                                            punten += $"{puntenlijst[i].X} {puntenlijst[i].Y},";
                                        else
                                            punten += $"{puntenlijst[i].X} {puntenlijst[i].Y}";

                                    }
                                    writer2.WriteLine($"{segment.SegmentID};{segment.BeginKnoop.KnoopID};{segment.EindKnoop.KnoopID};{straat.StraatID};{punten}");
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
