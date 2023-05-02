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

    public partial class BaseTransformer : Base
    {
        public new string Name { get; set; }
        public new string Description { get; set; }
        Transformer transformer;
        public BaseTransformer(Transformer _transformer, string _name = "Unknown", string _description = "This doesn't concern you.")
        {
            transformer = _transformer;
            Name = _name;
            Description = _description;
        }
    }

    public partial class OutputModifier : BaseTransformer
    {
        public new string Name { get; set; }
        public new string Description { get; set; }
        public readonly Resource.IResource cause;
        public readonly Resource.IResource effect;
        Transformer transformer;
        public OutputModifier(Resource.IResource _cause, Resource.IResource _effect, Transformer _transformer, string _name = "Resource Shortfall", string _description = "Output is being affected by resource shortfall") : base(_transformer, _name, _description)
        {
            cause = _cause;
            effect = _effect;
        }
    }

    // public class Resource.IResourceShortage : BaseTransformer{
    //     Transformer.Requester requester;
    //     Resource.RList outputs;
    //     public Resource.IResourceShortage(Transformer _transformer, Transformer.Requester _requester, List<Resource> _outputs) : base(_transformer){
    //         outputs = _outputs;
    //         requester = _requester;
    //     }
    // }
}
