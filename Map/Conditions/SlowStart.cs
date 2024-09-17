using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Linq;

public partial class SlowStart : ConditionBase
{

    // Starts efficiency out at zero and slowly increases.

    public Resource.RStatic slowStart = new Resource.RStatic(802, 0.0, 1, "Slow Start", "Slow Start Size");

    public override void OnAdd()
    {
        base.OnAdd();
        Feature.FactorsLocal[802].Mux(slowStart);
    }
    public override void OnRemove()
    {
        Feature.FactorsLocal[802].UnMux(slowStart);
        base.OnRemove();
    }
    public override void OnEFrame()
    {
        base.OnEFrame();
        slowStart.Set((Mathf.Max(((1f - slowStart.Sum) * (GD.Randf() - 0.1f)) * Feature.FactorsLocal[801].Sum, 0)) + slowStart.Sum);
        if (slowStart.Sum > 0.95)
        {
            OnRemove();
        }
    }
}

