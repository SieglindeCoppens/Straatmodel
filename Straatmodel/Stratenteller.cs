//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Tool1_BestandSchrijven
//{
//    class Stratenteller
//    {
//        public static int TelStraten(Dictionary<string, Dictionary<string, List<Straat>>> provincies)
//        {
//            int teller = 0;
//            foreach (KeyValuePair<string, Dictionary<string, List<Straat>>> provincie in provincies)
//            {
//                teller += TelStraten(provincie.Value);

//            }
//            return teller;
//        }
//        public static int TelStraten(Dictionary<string, List<Straat>> provincie)
//        {
//            int teller = 0;
//            foreach (KeyValuePair<string, List<Straat>> gemeente in provincie)
//            {
//                teller+= gemeente.Value.Count;
//            }
//            return teller;
//        }
//    }
//}