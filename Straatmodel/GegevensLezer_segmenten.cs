using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Tool1_BestandSchrijven
{
    /*Bevat alle lezers voor bestanden die informatie over straten bevat:
       * WRstraatnamen.csv, WRdata.csv*/
    public class GegevensLezer_segmenten
    {
        public static Dictionary<int, string> LeesStraten()
        {
            Unzipper.UnzipStraten();
            Dictionary<int, string> straatnaamIDStraatnaam = new Dictionary<int, string>();
            using (StreamReader sr = File.OpenText(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRstraatnamen.csv"))
            {
                string input = null;
                sr.ReadLine();
                sr.ReadLine();
                while ((input = sr.ReadLine()) != null)
                {
                    string[] inputs = input.Split(';');
                    straatnaamIDStraatnaam.Add(int.Parse(inputs[0]), inputs[1]);
                }
            }
            return straatnaamIDStraatnaam;
        }
        
        public static Dictionary<int, List<Segment>> LeesSegmenten()
        {
            Unzipper.UnzipData();
            Dictionary<int, List<Segment>> straatIDSegmentlijst = new Dictionary<int, List<Segment>>();
            using (StreamReader sr = File.OpenText(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRdata.csv"))
            {
                string input = null;
                sr.ReadLine();
                while ((input = sr.ReadLine()) != null)
                {
                    string[] inputs = input.Split(';');

                    if (!(int.Parse(inputs[6]) == -9 && int.Parse(inputs[7])==-9))
                    {
                        int segmentID = int.Parse(inputs[0]);
                        List<Punt> vertices = new List<Punt>();
                        string stringMetPunten = inputs[1].Remove(0, 12);       //Punten zoeken, deze moeten in de List<punt> vetrices van het segment komen. 
                        string stringMetPuntenZonderHaken = stringMetPunten.Trim('(', ')', ' ');
                        string[] xys = stringMetPuntenZonderHaken.Split(',');
                        foreach(string xy in xys)
                        {
                            string xyTrimmed = xy.Trim(' ');
                            string[] xeny = xyTrimmed.Split(' ');

                            Punt punt = new Punt(double.Parse(xeny[0].Trim('(')), double.Parse(xeny[1]));
                            vertices.Add(punt);
                        }

                        Knoop beginknoop = new Knoop(int.Parse(inputs[4]), vertices[0]);                        //Eerste punt van de vertices is de beginknoop, laatste punt is de eindknoop! 
                        Knoop eindknoop = new Knoop(int.Parse(inputs[5]), vertices[vertices.Count-1]);

                        Segment segment = new Segment(segmentID, beginknoop, eindknoop, vertices);


                        // segmenten toevoegen aan een dictionary die de straatID en segmenten bij die straat bijhoudt

                        int straatID = int.Parse(inputs[6]);
                        if(!(straatID == -9))
                        {
                            if (straatIDSegmentlijst.ContainsKey(straatID))                                     //als er al een record in de dictionary zit voor de straat
                            {
                                straatIDSegmentlijst[straatID].Add(segment);
                            }
                            else                                                                                //anders nieuwe record toevoegen en segment aan die lijst toevoegen 
                            {
                                straatIDSegmentlijst.Add(straatID, new List<Segment>());
                                straatIDSegmentlijst[straatID].Add(segment);
                            }
                        }

                        int straatID2 = int.Parse(inputs[7]);
                        if (!(straatID2 == -9))
                        {
                            if (straatIDSegmentlijst.ContainsKey(straatID2))                                     //als er al een record in de dictionary zit voor de straat
                            {
                                straatIDSegmentlijst[straatID2].Add(segment);
                            }
                            else
                            {
                                straatIDSegmentlijst.Add(straatID2, new List<Segment>());                   //Nieuwe record in dictionary toevoegen 
                                straatIDSegmentlijst[straatID2].Add(segment);
                            }
                        }                      
                    }
                }
            }
            return straatIDSegmentlijst;
        }

    }
}