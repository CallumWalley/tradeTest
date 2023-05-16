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

    public partial class BaseIndustry : Base
    {
        public new string Name { get; set; }
        public new string Description { get; set; }
        Industry Industry;
        public BaseIndustry(Industry _Industry, string _name = "Unknown", string _description = "This doesn't concern you.")
        {
            Industry = _Industry;
            Name = _name;
            Description = _description;
        }
    }

    public partial class OutputModifier : BaseIndustry
    {
        public new string Name { get; set; }
        public new string Description { get; set; }
        public readonly Resource.IResource cause;
        public readonly Resource.IResource effect;
        Industry Industry;
        public OutputModifier(Resource.IResource _cause, Resource.IResource _effect, Industry _Industry, string _name = "Resource Shortfall", string _description = "Output is being affected by resource shortfall") : base(_Industry, _name, _description)
        {
            cause = _cause;
            effect = _effect;
        }
    }

    // public class Resource.IResourceShortage : BaseIndustry{
    //     Industry.Requester requester;
    //     Resource.RList outputs;
    //     public Resource.IResourceShortage(Industry _Industry, Industry.Requester _requester, List<Resource> _outputs) : base(_Industry){
    //         outputs = _outputs;
    //         requester = _requester;
    //     }
    // }
}
