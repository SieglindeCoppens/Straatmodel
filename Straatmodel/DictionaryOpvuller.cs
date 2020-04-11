using System;
using System.Collections.Generic;
using System.Text;

namespace Straatmodel
{
    class DictionaryOpvuller
    {
        //<provincienaam, <gemeentenaam, List<Straat>>>
        //DOORGEVEN GEGEVENS VIA MAIN VIA PARAMETERS??
        public static Dictionary<string, Dictionary<string, List<Straat>>> geefStratenDictionary()
        {
            Dictionary<string, Dictionary<string, List<Straat>>> provincies = new Dictionary<string, Dictionary<string, List<Straat>>>();

            List<Straat> straten = Stratenmaker.MaakStraten();

            //Provinciesinlezen (gemeenteID, provincienaam)
            Dictionary<string, string> gemeenteIDProvincie = GegevensLezer_adressen.LeesProvincies();

            //Gemeentes inlezen (ID als key, naam als value)
            Dictionary<string, string> gemeentes = GegevensLezer_adressen.LeesGemeentes();

            //key straatID en value gemeenteID
            Dictionary<string, string> stratenIDgemeentesID = GegevensLezer_adressen.LeesLink();

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
