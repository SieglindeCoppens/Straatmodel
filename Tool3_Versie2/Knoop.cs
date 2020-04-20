using System;
using System.Collections.Generic;
using System.Text;

namespace Tool3_Versie2
{
    class Knoop
    {
        public int KnoopID { get; set; }
        public Punt Punt { get; set; }
        public Knoop(int knoopID, Punt punt)
        {
            Punt = punt;
            KnoopID = knoopID;
        }

        public override bool Equals(object obj)
        {
            if (obj is Knoop)
                return ((this.KnoopID == ((Knoop)obj).KnoopID));
            else return false;
        }
        public override int GetHashCode()
        {
            return KnoopID.GetHashCode();
        }
        public override string ToString()
        {
            return $"{KnoopID}";
        }
    }
}
