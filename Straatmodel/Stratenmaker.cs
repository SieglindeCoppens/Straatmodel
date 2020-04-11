using System;
using System.Collections.Generic;
using System.Text;

namespace Tool1_BestandSchrijven
{
    public static class Stratenmaker
    {
        public static List<Straat> MaakStraten(Dictionary<int, string> straatnaamIDStraatnaam, Dictionary<int, List<Segment>> straatnaamIDSegmentlijst)
        {
            List<Straat> straten = new List<Straat>();

            foreach (KeyValuePair<int, List<Segment>> straatnaamIDSegment in straatnaamIDSegmentlijst)
            {
                int straatID = straatnaamIDSegment.Key;
                string straatnaam = straatnaamIDStraatnaam[straatnaamIDSegment.Key];
                Graaf graaf = new Graaf(straatnaamIDSegment.Value);

                Straat straat = new Straat(straatID, straatnaam, graaf);
                straten.Add(straat);
            }
            return straten;
        }
    }
}
