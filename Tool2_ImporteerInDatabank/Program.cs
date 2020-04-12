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


        }
    }
}
