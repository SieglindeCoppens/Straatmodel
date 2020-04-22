using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tool2_ImporteerInDatabank
{
    class BestandenLezer
    {
        public static List<List<string>> LeesStraten(string path)
        {
            List<List<string>> straatInfo = new List<List<string>>();
            List<string> straatIds = new List<string>();
            List<string> straatnamen = new List<string>();
            List<string> gemeentes = new List<string>();
            List<string> provincies = new List<string>();
            List<string> graafIds = new List<string>();
            List<string> lengtes = new List<string>();

            using (StreamReader sr = File.OpenText(path))
            {
                string input = null;
                sr.ReadLine();
                while ((input = sr.ReadLine()) != null)
                {
                    string[] inputs = input.Split(';');
                    straatIds.Add(inputs[0]);
                    straatnamen.Add(inputs[1]);
                    gemeentes.Add(inputs[2]);
                    provincies.Add(inputs[3]);
                    graafIds.Add(inputs[4]);
                    lengtes.Add(inputs[5]);
                }
            }
            straatInfo.Add(straatIds);
            straatInfo.Add(straatnamen);
            straatInfo.Add(gemeentes);
            straatInfo.Add(provincies);
            straatInfo.Add(graafIds);
            straatInfo.Add(lengtes);

            return straatInfo;
        }

        public static List<List<string>> LeesSegmenten(string path)
        {
            List<List<string>> segmentInfo = new List<List<string>>();
            List<string> segmentIds = new List<string>();
            List<string> beginknoopIds = new List<string>();
            List<string> eindknoopIds = new List<string>();
            List<string> straatIds = new List<string>();

            using (StreamReader sr = File.OpenText(path))
            {
                string input = null;
                sr.ReadLine();
                while ((input = sr.ReadLine()) != null)
                {
                    string[] inputs = input.Split(';');
                    segmentIds.Add(inputs[0]);
                    beginknoopIds.Add(inputs[1]);
                    eindknoopIds.Add(inputs[2]);
                    straatIds.Add(inputs[3]);
                }
            }
            segmentInfo.Add(segmentIds);
            segmentInfo.Add(beginknoopIds);
            segmentInfo.Add(eindknoopIds);
            segmentInfo.Add(straatIds);

            return segmentInfo;
        }


        public static List<List<string>> LeesKnopen(string path)
        {
            List<List<string>> knoopInfo = new List<List<string>>();
            List<string> knoopIds = new List<string>();
            List<string> xs = new List<string>();
            List<string> ys = new List<string>();

            using (StreamReader sr = File.OpenText(path))
            {
                string input = null;
                sr.ReadLine();
                while ((input = sr.ReadLine()) != null)
                {
                    string[] inputs = input.Split(';');
                    knoopIds.Add(inputs[0]);
                    xs.Add(inputs[1]);
                    ys.Add(inputs[2]);
                }
            }
            knoopInfo.Add(knoopIds);
            knoopInfo.Add(xs);
            knoopInfo.Add(ys);

            return knoopInfo;
        }

        public static List<List<string>> LeesPunten(string path)
        {
            List<List<string>> puntInfo = new List<List<string>>();
            List<string> xs = new List<string>();
            List<string> ys = new List<string>();
            List<string> segmentIds = new List<string>();
            List<string> posities = new List<string>();

            using (StreamReader sr = File.OpenText(path))
            {
                string input = null;
                sr.ReadLine();
                while ((input = sr.ReadLine()) != null)
                {
                    string[] inputs = input.Split(';');
                    xs.Add(inputs[0]);
                    ys.Add(inputs[1]);
                    segmentIds.Add(inputs[2]);
                    posities.Add(inputs[3]);
                }
            }
            puntInfo.Add(xs);
            puntInfo.Add(ys);
            puntInfo.Add(segmentIds);
            puntInfo.Add(posities);

            return puntInfo;

        }
    }
}
