using System;
using System.Collections.Generic;
using System.Text;

namespace Tool3_Versie2
{
    class Segment
    {
        public Knoop BeginKnoop { get; set; }
        public Knoop EindKnoop { get; set; }
        public int SegmentID { get; set; }
        public List<Punt> Vertices { get; set; }

        public Segment(int segmentID, Knoop beginKnoop, Knoop eindKnoop, List<Punt> vertices)
        {
            SegmentID = segmentID;
            BeginKnoop = beginKnoop;
            EindKnoop = eindKnoop;
            Vertices = vertices;
        }



        public override bool Equals(object obj)
        {
            if (obj is Segment)
                return ((this.SegmentID == ((Segment)obj).SegmentID));
            else return false;
        }
        public override int GetHashCode()
        {
            return SegmentID.GetHashCode();
        }
        public override string ToString()
        {
            return $"{SegmentID}";
        }
    }
}
