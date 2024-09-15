using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Linq;

public partial class SimpleIndustry : ConditionScale
{

    [Export]

    public static string Factors { get; set; }
    public Dictionary<int, double> factorsDict;
    public Resource.RDict<Resource.RStatic> factors = new Resource.RDict<Resource.RStatic>();

    Dictionary<Resource.RGroup<Resource.RStatic>, Resource.RStatic> inputFullfillments = new();
    protected JsonSerializer serializer = new();

    // Sets 'output' in proportion to fulfillment.

    // public SimpleIndustry(object obj) : this() { }
    public SimpleIndustry() : this(InitialScale, Factors) { }
    public SimpleIndustry(double _initialScale, string _factorString) : this(InitialScale, JsonConvert.DeserializeObject<Dictionary<int, double>>(_factorString)) { }
    public SimpleIndustry(double _initialScale, Dictionary<int, double> _factors) : base()
    {
        InitialScale = _initialScale;
        factorsDict = _factors;
        // foreach (KeyValuePair<int, double> kvp in _inputs)
        // {
        //     if (kvp.Key < 800)
        //     {
        //         if (kvp.Value < 0)
        //         {

        //         }
        //         else
        //         {

        //         }
        //     }
        //     else if (kvp.Key < 900)
        //     {

        //     }
        //     else if (kvp.Key < 1000)
        //     {

        //     }
        //     else
        //     {
        //         throw new Exception("Invalid key");
        //     }
        //     Resource.RGroup<Resource.RStatic> newr = new(new Resource.RStatic(kvp.Key, 0, kvp.Value, "Base", "Base input"));
        //     // newr.Mux(Feature.FactorsSingle[800]);
        //     Resource.RStatic newf = new Resource.RStatic(801, 1, 0, $"{Resource.Name(kvp.Key)} Fullfilment.", $"How much of the requested resource was delivered");
        //     inputFullfillments[newr] = newf;
        // }
    }
    //     if (_outputs == null)
    //     {
    //         foreach (KeyValuePair<int, double> kvp in _outputs)
    //         {
    //             outputs.Add(new Resource.RStatic(kvp.Key, kvp.Value, kvp.Value, "Base", "Expected Yield"));
    //         }
    //     }
    // }

    public override void OnAdd()
    {
        base.OnAdd();

        foreach (Resource.RStatic r in factors)
        {
            if (r.Sum < 0)
            {

            }

            Feature.FactorsGlobalOutput[r.Type].Add(r);
            Feature.FactorsGlobalOutput[r.Type].Mux(Feature.FactorsLocal[801]); // Input fulfillment
            Feature.FactorsGlobalOutput[r.Type].Mux(Feature.FactorsSingle[901]); // Scale
            Feature.FactorsGlobalOutput[r.Type].Mux(Feature.FactorsLocal[802]); // Cabability
        }
        Feature.FactorsLocal[801].Name = "Input Fulfillment";
        Feature.FactorsLocal[801].Add(new Resource.RStatic(801, 1, 0, "Base", "Expected Fulfillment"));
        Feature.FactorsLocal[802].Name = "Capability";
        Feature.FactorsLocal[802].Add(new Resource.RStatic(802, 1, 0, "Base", "Cabability"));
        foreach (KeyValuePair<Resource.RGroup<Resource.RStatic>, Resource.RStatic> kvp in inputFullfillments)
        {
            /// fulfilment is equal to this
            Feature.FactorsLocal[801].Mux(kvp.Value);

            Feature.FactorsGlobalInput[kvp.Key.Type].Add(kvp.Key);
            Feature.FactorsGlobalInput[kvp.Key.Type].Mux(Feature.FactorsSingle[901]); // Scale
            Feature.FactorsGlobalInput[kvp.Key.Type].Mux(Feature.FactorsLocal[802]); // Cabability
        }
    }


    public override void OnEFrame()
    {
        base.OnEFrame();
        foreach (KeyValuePair<Resource.RGroup<Resource.RStatic>, Resource.RStatic> kvp in inputFullfillments)
        {
            kvp.Value.Set(kvp.Key.Fraction());
        }
    }
}
