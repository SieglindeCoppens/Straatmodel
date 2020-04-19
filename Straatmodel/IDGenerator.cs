using System;
using System.Collections.Generic;
using System.Text;

namespace Tool1_BestandSchrijven
{
    class IDGenerator
    {
        private static int StraatID = 1;
        private static int GraafID = 1;
        public static int CreateStraatID()
        {
            return StraatID++;
        }
        public static int CreateGraafID()
        {
            return GraafID++;
        }
    }
}
