using Godot;
using System;
using System.Collections.Generic;
public class Situations{

    public class Base{

    };

    public class BaseTransformer : Base{
        public string Name{get; set;}
        public string Description{get; set;}
        Transformer transformer;
        public BaseTransformer(Transformer _transformer){
            transformer = _transformer;
        } 
    }

    public class ResourceShortage : BaseTransformer{
        Transformer.Requester requester;
        List<Resource> outputs;
        public ResourceShortage(Transformer _transformer, Transformer.Requester _requester, List<Resource> _outputs) : base(_transformer){
            outputs = _outputs;
            requester = _requester;
        }
    }
}
