using System;
using System.Collections.Generic;
using System.Text;

namespace Tool1_BestandSchrijven
{
    class DictionaryOpvuller
    {
        public static Dictionary<string, Dictionary<string, Dictionary<int, List<Segment>>>> geefStratenDictionary(Dictionary<int,List<Segment>> straatnaamIdSegmentlijst, Dictionary<string, string> gemeenteIDProvincie, Dictionary<string, string> gemeentes, Dictionary<string, string> stratenIDgemeentesID)
        {
            Dictionary<string, Dictionary<string, Dictionary<int, List<Segment>>>> provincies = new Dictionary<string, Dictionary<string, Dictionary<int, List<Segment>>>>();


            //Dictionary opvullen met gemeentenamen als key en een dictionary van straatnaamIdsegmentlijst als value
            //Dictionary<string, Dictionary<int, List<Segment>>> gemeentesMetStraatDict = new Dictionary<string, Dictionary<int, List<Segment>>>();

            foreach(KeyValuePair<int, List<Segment>> straat in straatnaamIdSegmentlijst)
            {
                string straatId = straat.Key.ToString();
                if (stratenIDgemeentesID.ContainsKey(straatId))
                {
                    string gemeenteID = stratenIDgemeentesID[straatId];
                    if (gemeenteIDProvincie.ContainsKey(gemeenteID))
                    {
                        string gemeenteNaam = gemeentes[gemeenteID];
                        string provincie = gemeenteIDProvincie[gemeenteID];

                        if (!provincies.ContainsKey(provincie))
                        {
                            Dictionary<string, Dictionary<int, List<Segment>>> gemeente = new Dictionary<string, Dictionary<int, List<Segment>>>();
                            provincies.Add(provincie, gemeente);
                        }
                        if (!provincies[provincie].ContainsKey(gemeenteNaam))
                        {
                            Dictionary<int, List<Segment>> straatnaamIdSegmenten = new Dictionary<int, List<Segment>>();
                            provincies[provincie].Add(gemeenteNaam, straatnaamIdSegmenten);
                        }
                        provincies[provincie][gemeenteNaam].Add(straat.Key,straat.Value);
                    }
                }   
            }
            return provincies;
        }
    }
}
