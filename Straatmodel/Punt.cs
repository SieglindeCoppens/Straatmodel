using System;
using System.Collections.Generic;
using System.Text;

namespace Tool1_BestandSchrijven
{
    class Punt
    {
        public double X { get;set;}
        public double Y{ get;set;}
        public Punt(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Punt)
                return ((this.Y == ((Punt)obj).Y) && (this.X == ((Punt)obj).X));
            else return false;
        }
        public override int GetHashCode()
        {
            return X.GetHashCode()^Y.GetHashCode();
        }
        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}
