using Godot;
using System;
using System.Collections.Generic;

public partial class Feature : Node
{
    [Export]
    public string slug;
    public Resource.RList<Resource.IRequestable> Production { get; set; }
    public Resource.RList<Resource.IRequestable> Consumption { get; set; }
    public List<Situations.Base> Situations { get; protected set; }

    // 0-100 

    //////////////////////////
    // Internal state
    /////////////////////////
    int maxCapacity = 1; // How many 'buildings' this industry contains.
    int operationalCapacity = 1; // How many 'buildings' are currently enabled.
    byte capacityUtilisation = 255; // What percentage of maximum inputs for currently operational [0-255]
    double efficiency = 1; // Modifies all output value 

    // output = baseProduction * operationalCapacity *  capacityUtilisation * efficiency

    byte breakDown; // Decays without maintainance. [0-255]


    // Output 

    public IndustryRegister.IndustryType ttype;
    public string TypeName { get { return ttype.Name; } }
    public string TypeSlug { get { return ttype.Slug; } }
    public string TypeClass { get { return ttype.Superclass; } }
    public string TypeSubclass { get { return ttype.Subclass; } }
    public string TypeImage { get { return ttype.Image; } }


    //public string TypeRequirements{get{ttype.Requiremnts}
    public string[] Tags { get; set; }
    public string Description { get; set; }
    public int Prioroty { get; set; }

    // this could be made less confusing.

    public override void _Ready()
    {
        // If instantiated in editor

        ttype ??= GetNode<IndustryRegister>("/root/Global/IndustryRegister").GetFromSlug(slug);
        Name = ttype.Name;
        Tags = ttype.Tags;
        Description = ttype.Description;
        Prioroty = ttype.defaultPrioroty;
        Situations = new List<Situations.Base>();

        Consumption = new(GetInputClassFromTemplate(ttype.Consumption));
        Production = new(GetGroupFromTemplate(ttype.Production));
    }


    // public virtual IEnumerable<Resource.BaseRequest> Consumed()
    // {
    //     foreach (Resource.BaseRequest tip in Consumption)
    //     {
    //         yield return tip;
    //     }
    // }

    // IEnumerable<Requester> GetFromTemplate(Dictionary<int, double> template)
    // {    
    //     if (template == null) { yield break; }
    //     foreach (KeyValuePair<int, double> kvp in template)
    //     {
    //         yield return new Requester(new Resource.IResource.RStatic(kvp.Key, kvp.Value, Name));
    //     }
    // }
    IEnumerable<Resource.IRequestable> GetGroupFromTemplate(Dictionary<int, double> template)
    {
        if (template == null) { yield break; }
        foreach (KeyValuePair<int, double> kvp in template)
        {
            yield return new Resource.RGroupRequests<Resource.IRequestable>(new Resource.RRequest(kvp.Key, kvp.Value, "Base Yield", "Base Yield", true), Name, Description);
        }
    }

    IEnumerable<Resource.RStatic> GetStaticFromTemplate(Dictionary<int, double> template)
    {
        if (template == null) { yield break; }
        foreach (KeyValuePair<int, double> kvp in template)
        {
            yield return new Resource.RStatic(kvp.Key, kvp.Value, Name);
        }
    }

    IEnumerable<Resource.IRequestable> GetInputClassFromTemplate(Dictionary<int, double> template)
    {
        if (template == null) { yield break; }
        foreach (KeyValuePair<int, double> kvp in template)
        {
            yield return new Resource.RRequest(kvp.Key, kvp.Value, $"{TypeName} Upkeep", "Base Yield");
        }
    }
    public void AddSituation(Situations.Base s)
    {
        Situations.Add(s);
    }
}
