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
    public SlowStart(Dictionary<string, object> kvp) : this(Convert.ToDouble(Conditions.TryGetDefault(kvp, "initialModifier", "0"))) { }

    //public SlowStart(Dictionary<string, object> kvp) : this(Convert.ToDouble(TryGetDefault(kvp, "initialModifier", "0"))) { }
    public SlowStart(double initialModifier) { }
    public SlowStart() { }
    public Resource.RStatic slowStart = new Resource.RStatic(802, 0.0, 0, "Slow Start", "Slow Start Size");

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
        slowStart.Set((0.1 * Feature.FactorsLocal[801].Sum) + slowStart.Sum);
        if (slowStart.Sum > 0.9)
        {
            OnRemove();
        }
    }
}

