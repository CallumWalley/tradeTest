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
    public Godot.Collections.Dictionary Factors;

    [Export(PropertyHint.Range, "0,1,0.01")]
    public double StartingCapability = 0.1;
    //public Resource.RDict<Resource.RStatic> factors = new Resource.RDict<Resource.RStatic>();
    Resource.RStatic capabilityMain;
    Dictionary<Resource.RGroup<Resource.RStatic>, Resource.RStatic> inputFullfillments = new();
    protected JsonSerializer serializer = new();

    // Sets 'output' in proportion to fulfillment.

    // public SimpleIndustry(object obj) : this() { }
    // public SimpleIndustry() : this(InitialScale, Factors) { }
    // public SimpleIndustry(double _initialScale, string _factorString) : this(InitialScale, JsonConvert.DeserializeObject<Dictionary<int, double>>(_factorString)) { }
    // public SimpleIndustry(double _initialScale, Dictionary<int, double> _factors) : base()
    // {
    //     InitialScale = _initialScale;
    //     factorsDict = _factors;
    //     // foreach (KeyValuePair<int, double> kvp in _inputs)
    //     // {
    //     //     if (kvp.Key < 800)
    //     //     {
    //     //         if (kvp.Value < 0)
    //     //         {

    //     //         }
    //     //         else
    //     //         {

    //     //         }
    //     //     }
    //     //     else if (kvp.Key < 900)
    //     //     {

    //     //     }
    //     //     else if (kvp.Key < 1000)
    //     //     {

    //     //     }
    //     //     else
    //     //     {
    //     //         throw new Exception("Invalid key");
    //     //     }
    //     //     Resource.RGroup<Resource.RStatic> newr = new(new Resource.RStatic(kvp.Key, 0, kvp.Value, "Base", "Base input"));
    //     //     // newr.Mux(Feature.FactorsSingle[800]);
    //     //     Resource.RStatic newf = new Resource.RStatic(801, 1, 0, $"{Resource.Name(kvp.Key)} Fullfilment.", $"How much of the requested resource was delivered");
    //     //     inputFullfillments[newr] = newf;
    //     // }
    // }
    // //     if (_outputs == null)
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
        Feature.FactorsLocal[801].Name = "Input Fulfillment";
        Feature.FactorsLocal[801].Add(new Resource.RStatic(801, 1, 0, "Base", "Expected Fulfillment"));
        Feature.FactorsLocal[802].Name = "Capability";
        //Feature.FactorsLocal[802].Add(new Resource.RStatic(802, 1, 1, "Cabability", "Cabability"));
        capabilityMain = new Resource.RStatic(802, StartingCapability, 0, "Consistancy", "Slow Start Size");
        Feature.FactorsLocal[802].Mux(capabilityMain);


        foreach (KeyValuePair<Variant, Variant> r in Factors)
        {
            switch ((int)r.Key)
            {
                // Accruable
                case < 800:
                    // If output
                    if ((double)r.Value > 0)
                    {
                        Feature.FactorsGlobalOutput[(int)r.Key].Add(new Resource.RStatic((int)r.Key, (double)r.Value, (double)r.Value, "Base", "Expected Yield"));
                        Feature.FactorsGlobalOutput[(int)r.Key].Mux(Feature.FactorsLocal[801]); // Input fulfillment
                        Feature.FactorsGlobalOutput[(int)r.Key].Mux(Feature.FactorsSingle[901]); // Scale
                        Feature.FactorsGlobalOutput[(int)r.Key].Mux(Feature.FactorsLocal[802]); // Efficacy
                    }
                    // If input
                    else
                    {
                        Resource.RGroup<Resource.RStatic> input = new(new Resource.RStatic((int)r.Key, 0, (double)r.Value, "Base", "Base input"));
                        inputFullfillments[input] = new Resource.RStatic(801, 0, 1, $"{Resource.Name((int)r.Key)} fullfillment.", $"{Resource.Name((int)r.Key)} fullfillment.");
                        Feature.FactorsLocal[801].Mux(inputFullfillments[input]);

                        Feature.FactorsGlobalInput[(int)r.Key].Add(input);
                        Feature.FactorsGlobalInput[(int)r.Key].Mux(Feature.FactorsSingle[901]); // Scale
                        Feature.FactorsGlobalInput[(int)r.Key].Mux(Feature.FactorsLocal[802]); // Cabability
                    }
                    break;
                // Non Accuable
                case < 900:
                    Feature.FactorsGlobalOutput[(int)r.Key].Add(new Resource.RStatic((int)r.Key, (double)r.Value, (double)r.Value, "Base", "Expected Yield"));
                    Feature.FactorsGlobalOutput[(int)r.Key].Mux(Feature.FactorsLocal[801]); // Input fulfillment
                    Feature.FactorsGlobalOutput[(int)r.Key].Mux(Feature.FactorsSingle[901]); // Scale
                    Feature.FactorsGlobalOutput[(int)r.Key].Mux(Feature.FactorsLocal[802]); // Efficacy
                    break;
                case < 999:
                    break;
                default:
                    throw new ArgumentException($"Invalid factor key {r.Key}");
            }


            // Cabability
        }

        foreach (KeyValuePair<Resource.RGroup<Resource.RStatic>, Resource.RStatic> kvp in inputFullfillments)
        {
            /// fulfilment is equal to this

        }
    }


    public override void OnEFrame()
    {
        base.OnEFrame();
        foreach (KeyValuePair<Resource.RGroup<Resource.RStatic>, Resource.RStatic> kvp in inputFullfillments)
        {
            //double rolling = ((kvp.Key.Fraction() + kvp.Value.Sum) / 2);
            kvp.Value.Sum = kvp.Key.Fraction();
        }
        double dif =
        capabilityMain.Sum += ((Feature.FactorsLocal[801].Sum - capabilityMain.Sum + ((GD.Randf() - 0.6) / (Feature.FactorsSingle[901].Sum * 5))) / 10);
    }
}
