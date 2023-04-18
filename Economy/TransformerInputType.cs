using Godot;
using System;

public class TransformerInputType
{
    public class Base
    {
        public ResourceStatic Request;
        public ResourceStatic Response;
        protected Transformer transformer;

        public int Type { get; protected set; }

        public Base(Transformer _transformer, ResourceStatic _request)
        {
            transformer = _transformer;
            Request = _request;
            Response = new ResourceStatic(Request.Type, 0);
        }
        public virtual void Respond(ResourceStatic _response, int _status)
        {
        }
    }
    public class Linear : Base
    {
        ResourceStatic multiplier;

        public Linear(Transformer _transformer, ResourceStatic _request) : base(_transformer, _request)
        {
            transformer = _transformer;
            Request = _request;
            Type = Request.Type;
            multiplier = new ResourceStatic(Type, 0);
            transformer.Production.Multiply(multiplier);
            transformer.AddSituation(new Situations.OutputModifier(Response, multiplier, transformer));
        }
        public override void Respond(ResourceStatic _response, int _status)
        {
            Response = _response;
            multiplier.Sum = ((Response.Sum - Request.Sum) / Request.Sum);
        }
    }
    // // Small class to handle 'request, vs receive'
    // public class Requester
    // {

    //     public ResourceStatic request;
    //     // 0 fulfilled.
    //     // 1 partially fulfilled.
    //     // 2 unfulfilled.
    //     public int Type { get { return request.Type; } }
    //     public float Sum { get { return request.Sum; } }
    //     public ResourceStatic Response { get; set; }
    //     public Requester(ResourceStatic _request)
    //     {
    //         request = _request;
    //     }
    //     public void Respond(ResourceStatic _response, int _status)
    //     {
    //         Response = _response;
    //     }
    // }
}
