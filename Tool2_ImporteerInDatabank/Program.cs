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
            //List<List<string>> knoopInfo = BestandenLezer.LeesKnopen();
            //List<List<string>> puntInfo = BestandenLezer.LeesPunten();

            DataBeheer db = new DataBeheer("Data Source=DESKTOP-HT91N8R\\SQLEXPRESS;Initial Catalog=db_Wegennet;Integrated Security=True");

            db.voegStratenToe(stratenInfo);
            db.VoegSegmentenToe(segmentInfo);
            //db.VoegKnopenToe(knoopInfo);
            //db.voegPuntenToe(puntInfo);

        }
    }
}