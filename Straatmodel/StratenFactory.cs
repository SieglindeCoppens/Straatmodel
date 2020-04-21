using System;
using System.Collections.Generic;
using System.Text;

namespace Tool1_BestandSchrijven
{
    class StratenFactory : IStraatFactory
    {
        public List<Straat> MaakStraten(Dictionary<string, Dictionary<string, Dictionary<string, List<Segment>>>> provincies)
        {
            List<Straat> straten = new List<Straat>();
            foreach(KeyValuePair<string, Dictionary<string, Dictionary<string, List<Segment>>>> provincie in provincies)
            {
                foreach(KeyValuePair<string, Dictionary<string, List<Segment>>> gemeente in provincie.Value)
                {
                    foreach(KeyValuePair<string, List<Segment>> straatIdSeg in gemeente.Value)
                    {
                        string straatnaam = straatIdSeg.Key;
                        Graaf graaf = new Graaf(straatIdSeg.Value);

                        Straat straat = new Straat(straatnaam, graaf, provincie.Key, gemeente.Key);

                        straten.Add(straat); 
                    }
                }
            }
            return straten;
        }
    }
}
