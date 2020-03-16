using System;
using System.Collections.Generic;
using System.Text;

namespace Straatmodel
{
    public class Straat
    {
        public Graaf Graaf { get; set; }
        public int StraatID { get; set; }
        public string Straatnaam { get; set; }

        public Straat(int straatID, string straatnaam, Graaf graaf)
        {
            StraatID = straatID;
            Straatnaam = straatnaam;
            Graaf = graaf;

        }

        public List<Knoop> getKnopen() 
        {
            return Graaf.GetKnopen();
        }
        public void showStraat()
        {

        }


    }
}
