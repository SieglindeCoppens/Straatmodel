
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tool1_BestandSchrijven
{
    class SchrijfBestand
    {
        /*We overlopen per straat de graaf, en voegen gaandeweg de knopen en segmenten die we tegenkomen op. 
            * MAAR:
            * - We zullen segmenten verschillende keren tegenkomen in 1 straat, aangezien ze in de graaf aan meerdere knopen gekoppeld zijn
            * - We zullen over de straten heen verschillende keren dezelfde knopen tegenkomen omdat deze knopen de verschillende straten verbinden
            * 
            * Dus ik hou van beide bij welke er al uitgeprint zijn! 
            * */
        public static void PrintDocumenten(Dictionary<string, Dictionary<string, List<Straat>>> provincies)
        {
            if (File.Exists(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Straten.txt") && File.Exists(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Segmenten.txt") && File.Exists(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Knopen.txt") && File.Exists(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Punten.txt"))
            {
                File.Delete(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Straten.txt");
                File.Delete(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Segmenten.txt");
                File.Delete(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Knopen.txt");
                File.Delete(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Punten.txt");
            }
           
            List<int> uitgeprinteSegmentenID = new List<int>();
            List<int> uitgeprinteKnopenID = new List<int>();

            using StreamWriter writer = File.CreateText(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Straten.txt");
            using StreamWriter writer2 = File.CreateText(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Segmenten.txt");
            using StreamWriter writer3 = File.CreateText(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Knopen.txt");
            using StreamWriter writer4 = File.CreateText(@"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\Punten.txt");
            writer.WriteLine("StraatID;Straatnaam;Gemeente;Provincie;GraafID;Lengte");
            writer2.WriteLine("SegmentID;BeginknoopID;EindknoopID;straatID");
            writer3.WriteLine("KnoopID;X;Y");
            writer4.WriteLine("X,Y,SegmentID, positie");
            foreach (KeyValuePair<string, Dictionary<string, List<Straat>>> provincie in provincies)
            {
                foreach (KeyValuePair<string, List<Straat>> gemeente in provincie.Value)
                {
                    foreach(Straat straat in gemeente.Value)
                    {
                        writer.WriteLine($"{straat.StraatID};{straat.Straatnaam.Trim()};{gemeente.Key};{provincie.Key};{straat.Graaf.GraafID};{straat.Lengte}");

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
                                    List<Punt> puntenlijst = segment.Vertices;

                                    //De eerste en laatste waarde van de vertices knip ik er hier af, die zitten al in mijn tekstbestand van begin en eindknoop!!
                                    for (int i = 1; i < puntenlijst.Count-1; i++)
                                    {
                                        writer4.WriteLine($"{puntenlijst[i].X};{puntenlijst[i].Y};{segment.SegmentID};{i}");
                                    }
                                    writer2.WriteLine($"{segment.SegmentID};{segment.BeginKnoop.KnoopID};{segment.EindKnoop.KnoopID};{straat.StraatID}");
                                    uitgeprinteSegmentenID.Add(segment.SegmentID);
                                }

                            }
                        }
                    }
                }
            }

        }
    }
}
