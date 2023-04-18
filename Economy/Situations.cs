using Godot;
using System;
using System.Collections.Generic;
public class Situations
{

    public class Base
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    };

    public class BaseTransformer : Base
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

    public class OutputModifier : BaseTransformer
    {
        public new string Name { get; set; }
        public new string Description { get; set; }
        public readonly Resource cause;
        public readonly Resource effect;
        Transformer transformer;
        public OutputModifier(Resource _cause, Resource _effect, Transformer _transformer, string _name = "Resource Shortfall", string _description = "Output is being affected by resource shortfall") : base(_transformer, _name, _description)
        {
            cause = _cause;
            effect = _effect;
        }
    }

    // public class ResourceShortage : BaseTransformer{
    //     Transformer.Requester requester;
    //     ResourceList outputs;
    //     public ResourceShortage(Transformer _transformer, Transformer.Requester _requester, List<Resource> _outputs) : base(_transformer){
    //         outputs = _outputs;
    //         requester = _requester;
    //     }
    // }
}
