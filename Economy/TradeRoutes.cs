using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public static partial class TradeRoutes
{

    public static double GetFrieghterWeight()
    {
        // double shipWeightImport = 0;
        // double shipWeightExport = 0;
        // foreach (Resource.IResource child in Balance)
        // {
        //     if (child.Sum > 0)
        //     {
        //         shipWeightExport += child.Sum;
        //     }
        //     else
        //     {
        //         shipWeightImport += child.Sum;
        //     }
        // }
        return 1; //-Math.Max(shipWeightExport, shipWeightImport);
    }

}