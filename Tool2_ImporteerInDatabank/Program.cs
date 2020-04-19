using System;
using System.Collections.Generic;

namespace Tool2_ImporteerInDatabank
{
    class Program
    {
        static void Main(string[] args)
        {
            //De bestanden inlezen, alle info is gewoon gebundeld zodat de overeenkomstige waarden op dezelfde index staan
            List<List<string>> stratenInfo = BestandenLezer.LeesStraten();
            List<List<string>> segmentInfo = BestandenLezer.LeesSegmenten();
            List<List<string>> knoopInfo = BestandenLezer.LeesKnopen();

            DataBeheer db = new DataBeheer("Data Source=DESKTOP-HT91N8R\\SQLEXPRESS;Initial Catalog=db_Wegennet;Integrated Security=True");

            //IN EEN TRANSACTIE STEKEN?? ALLES LUKT OF NIETS LUKT!!
            db.voegStratenToe(stratenInfo);
            //db.VoegSegmentenToe(segmentInfo);
            //db.VoegKnopenToe(knoopInfo);

        }
    }
}
