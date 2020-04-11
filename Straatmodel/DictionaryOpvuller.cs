using System;
using System.Collections.Generic;
using System.Text;

namespace Straatmodel
{
    /*Overloopt alle straten en vult zo een Dictionary met vorm <provincienaam, <gemeentenaam, List<Straat>>> op*/
    class DictionaryOpvuller
    {
        public static Dictionary<string, Dictionary<string, List<Straat>>> geefStratenDictionary(List<Straat> straten, Dictionary<string, string> gemeenteIDProvincie, Dictionary<string, string> gemeentes, Dictionary<string, string> stratenIDgemeentesID)
        {
            Dictionary<string, Dictionary<string, List<Straat>>> provincies = new Dictionary<string, Dictionary<string, List<Straat>>>();


            //Dictionary opvullen met gemeentenamen als key en een lijst van straten als value
            Dictionary<string, List<Straat>> gemeentesMetStraten = new Dictionary<string, List<Straat>>();

            foreach(Straat straat in straten)
            {
                //In ons datamodel is de ID een int, maar in de dictionary een string! 
                string straatID = $"{straat.StraatID}";

                //StraatID 114812 werd niet gevonden?? Dus hier een if ingesteld als het ID niet in de lijst zit. 
                if (stratenIDgemeentesID.ContainsKey(straatID))
                {
                    string gemeenteID = stratenIDgemeentesID[straatID];
                   
                    if (gemeenteIDProvincie.ContainsKey(gemeenteID)){
                        string gemeenteNaam = gemeentes[gemeenteID];
                        string provincie = gemeenteIDProvincie[gemeenteID];

                        if (!provincies.ContainsKey(provincie))
                        {
                            Dictionary<string, List<Straat>> gemeente = new Dictionary<string, List<Straat>>();
                            provincies.Add(provincie, gemeente);
                        }
                        if (!provincies[provincie].ContainsKey(gemeenteNaam))
                        {
                            List<Straat> stratenInGemeente = new List<Straat>();
                            provincies[provincie].Add(gemeenteNaam, stratenInGemeente);
                        }
                        provincies[provincie][gemeenteNaam].Add(straat);
                    }     
                }
            }
            return provincies;

        }







    }
}
