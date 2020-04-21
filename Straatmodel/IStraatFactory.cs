using System;
using System.Collections.Generic;
using System.Text;

namespace Tool1_BestandSchrijven
{
    interface IStraatFactory
    {
        List<Straat> MaakStraten(Dictionary<string, Dictionary<string, Dictionary<string, List<Segment>>>> provincies);
    }
}
