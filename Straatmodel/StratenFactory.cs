using System;
using System.Collections.Generic;
using System.Text;

namespace Tool1_BestandSchrijven
{
    class StratenFactory
    {
        public List<Straat> MaakStraten(Dictionary<string, Dictionary<string, Dictionary<int, List<Segment>>>> provincies, Dictionary<int, string> straatnaamIDStraatnaam)
        {
            List<Straat> straten = new List<Straat>();
            foreach(KeyValuePair<string, Dictionary<string, Dictionary<int, List<Segment>>>> provincie in provincies)
            {
                foreach(KeyValuePair<string, Dictionary<int, List<Segment>>> gemeente in provincie.Value)
                {
                    foreach(KeyValuePair<int, List<Segment>> straatIdSeg in gemeente.Value)
                    {
                        string straatnaam = straatnaamIDStraatnaam[straatIdSeg.Key];
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
