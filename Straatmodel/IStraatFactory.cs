﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tool1_BestandSchrijven
{
    interface IStraatFactory
    {
        List<Straat> MaakStraten(Dictionary<int, string> straatnaamIDStraatnaam, Dictionary<int, List<Segment>> straatnaamIDSegmentlijst);
    }
}
