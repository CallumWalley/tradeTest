using Godot;
using System;
using System.Collections.Generic;
public partial class Situations
{

    public partial class Base
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    };

    public partial class BaseFeature : Base
    {
        public new string Name { get; set; }
        public new string Description { get; set; }
        Feature Feature;
        public BaseFeature(Feature _Feature, string _name = "Unknown", string _description = "This doesn't concern you.")
        {
            Feature = _Feature;
            Name = _name;
            Description = _description;
        }
    }

    public partial class OutputModifier : BaseFeature
    {
        public new string Name { get; set; }
        public new string Description { get; set; }
        public readonly Resource.IResource cause;
        public readonly Resource.IResource effect;
        Feature Feature;
        public OutputModifier(Resource.IResource _cause, Resource.IResource _effect, Feature _Feature, string _name = "Resource Shortfall", string _description = "Output is being affected by resource shortfall") : base(_Feature, _name, _description)
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
