using Godot;
using System;
using System.Collections.Generic;
public partial class Condition
{
    // Condition is some logic affecting a feature, evaluated every EFrame


    public partial class BaseCondition
    {
        // Base class for conditions.
        public new string Name { get; set; }
        public new string Description { get; set; }
        Features.FeatureBase Feature;
        public BaseCondition(Features.FeatureBase _Feature, string _name = "Unknown", string _description = "This doesn't concern you.")
        {
            Feature = _Feature;
            Name = _name;
            Description = _description;
        }
    }

    public partial class OutputModifier : BaseCondition
    {
        // For features representing a transformation of resources.
        public new string Name { get; set; }
        public new string Description { get; set; }
        public readonly Resource.IResource cause;
        public readonly Resource.IResource effect;
        Features.FeatureBase Feature;
        public OutputModifier(Resource.IResource _cause, Resource.IResource _effect, Features.FeatureBase _Feature, string _name = "Resource Shortfall", string _description = "Output is being affected by resource shortfall") : base(_Feature, _name, _description)
        {
            cause = _cause;
            effect = _effect;
        }
    }

    // public class Resource.IResourceShortage : BaseFeature{
    //     Feature.Requester requester;
    //     Resource.RList outputs;
    //     public Resource.IResourceShortage(Feature _Feature, Feature.Requester _requester, List<Resource> _outputs) : base(_Feature){
    //         outputs = _outputs;
    //         requester = _requester;
    //     }
    // }
}
