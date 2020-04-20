using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tool1_BestandSchrijven
{
    class GegevensLezer_adressen
    {
        /*Bevat alle lezers voor bestanden die informatie over gemeenten en provincies bevatten:
         * WRGemeentenaam.csv, WRGemeenteID.csv, ProvincieInfo.csv, ProvincieIDsVlaanderen.csv*/ 


        public static Dictionary<string, string> LeesGemeentes()
        {
            Dictionary<string, string> gemeentes = new Dictionary<string, string>();

            using (StreamReader sr = File.OpenText(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRGemeentenaam.csv"))
            {
                string input = null;
                sr.ReadLine();
                while ((input = sr.ReadLine()) != null)
                {
                    string[] inputs = input.Split(';');
                    if (inputs[2] == "nl")
                    {
                        gemeentes.Add(inputs[1], inputs[3]);
                    }
                }
            }
            return gemeentes;
        }

        public static Dictionary<string, string> LeesLink()
        {
            //Geeft een dictionary met key straatID en value gemeenteID
            Dictionary<string, string> stratenGemeentes = new Dictionary<string, string>();

            using (StreamReader sr = File.OpenText(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\WRGemeenteID.csv"))
            {
                string input = null;
                sr.ReadLine();
                while ((input = sr.ReadLine()) != null)
                {
                    string[] inputs = input.Split(';');
                    stratenGemeentes.Add(inputs[0], inputs[1]);
                }
            }
            return stratenGemeentes;
        }

        //Geeft een dictionary terug van de 
        public static Dictionary<string, string> LeesProvincies()
        {
            List<string> vlaamseProv = LeesVlaamseProvincieIDs();
            Dictionary<string, string> provincies = new Dictionary<string, string>();
            using (StreamReader sr = File.OpenText(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\ProvincieInfo.csv"))
            {
                string input = null;
                sr.ReadLine();
                while ((input = sr.ReadLine()) != null)
                {
                    string[] inputs = input.Split(';');
                    if (inputs[2] == "nl" && vlaamseProv.Contains(inputs[1]))
                    {
                        provincies.Add(inputs[0], inputs[3]);
                    }
                }
            }
            return provincies;
        }

        //Deze methode heb ik op private gezet, omdat ze enkel wordt aangeroepen in LeesProvincies. 
        private static List<string> LeesVlaamseProvincieIDs()
        {
            List<string> vlaamseProvincies = new List<string>();

            using (StreamReader sr = File.OpenText(@"C:\Users\Sieglinde\OneDrive\Documenten\Programmeren\semester2\programmeren 3\Labo\ProvincieIDsVlaanderen.csv"))
            {
                string input = null;
                while ((input = sr.ReadLine()) != null)
                {
                    string[] inputs = input.Split(',');
                    foreach (string i in inputs)
                    {
                        vlaamseProvincies.Add(i);
                    }
                }
            }
            return vlaamseProvincies;
        }
    }
}
