using Godot;
using System;

public partial class IndustryInputType
{
    public partial class Base
    {
        public Resource.RStatic Request;
        public Resource.RStatic Response;
        protected Industry Industry;
        public int State { get; set; }
        public int Type { get { return Request.Type(); } }

        public Base(Industry _Industry, Resource.RStatic _request)
        {
            Industry = _Industry;
            Request = _request;
            Response = new Resource.RStatic(Request.Type(), 0);
        }
        // No inputs if request fulfilled.
        public virtual void Respond()
        {
            Response.Set(Request.Sum());
            State = 0;
        }
        // No fulfilled value returned if not fulfilled.
        public virtual void Respond(double value)
        {
            Response.Set(value);
            // GD.Print($"Partial request {value}/{Request.Sum()}");
            State = 1;
        }
        public override string ToString()
        {
            return $"{Response.Sum()}/{Request.Sum()}";
        }
    }
    public partial class Linear : Base
    {
        Resource.RStatic multiplier;

        public Linear(Industry _Industry, Resource.RStatic _request) : base(_Industry, _request)
        {
            multiplier = new Resource.RStatic(Type, 0);
            ((Resource.RGroup)Industry.Production[Type]).Multiply(multiplier);
            Industry.AddSituation(new Situations.OutputModifier(Response, multiplier, Industry));
        }
        public override void Respond()
        {
            base.Respond();
            multiplier.Set(1f);
        }
        public override void Respond(double value)
        {
            base.Respond(value);
            multiplier.Set((value - Request.Sum()) / Request.Sum());
        }
    }
    // // Small class to handle 'request, vs receive'
    // public class Requester
    // {

    //     public Resource.IResource.RStatic request;
    //     // 0 fulfilled.
    //     // 1 partially fulfilled.
    //     // 2 unfulfilled.
    //     public int Type { get { return request .Type(); } }
    //     public double Sum { get { return request.Sum(); } }
    //     public Resource.IResource.RStatic Response { get; set; }
    //     public Requester(Resource.RStatic _request)
    //     {
    //         request = _request;
    //     }
    //     public void Respond(Resource.RStatic _response, int _status)
    //     {
    //         Response = _response;
    //     }
    // }
}
