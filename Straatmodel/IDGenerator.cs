using System;
using System.Collections.Generic;
using System.Text;

namespace Straatmodel
{
    class IDGenerator
    {
        private static int straatID = 1;
        private static int graafID = 1;
        public static int CreateStraatID()
        {
            return straatID ++;
        }
        public static int CreateGraafID()
        {
            return graafID++;
        }
    }
}
