using Godot;
using System;
using System.Collections.Generic;

public partial class Industry : EcoNode, Resource.IResourceConsumer
{
    [Export]
    public string slug;
    public Resource.IResource output;
    // public Resource.RList<Resource.RGroup> Production { get; protected set; }
    public Resource.RList<Resource.RGroup> production;
    public Resource.RList<Resource.IRequestable> consumption;
    public Resource.RList<Resource.RStatic> stored;

    // public List<Resource.BaseRequest> Consumption { get; protected set; }

    public Resource.RList<Resource.RStatic> Storage { get; protected set; }
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

    public System.Object Driver()
    {
        return this;
    }
    // this could be made less confusing.
    public Resource.RList<Resource.IRequestable> Consumed()
    {
        return consumption;
    }
    public Resource.RList<Resource.RGroup> Produced()
    {
        return production;
    }


    public override void _Ready()
    {
        base._Ready();

        // If instantiated in editor
        ttype ??= GetNode<IndustryRegister>("/root/Global/IndustryRegister").GetFromSlug(slug);
        Name = ttype.Name;
        Tags = ttype.Tags;
        Description = ttype.Description;
        Prioroty = ttype.defaultPrioroty;
        Situations = new List<Situations.Base>();

        consumption = new Resource.RList<Resource.IRequestable>(GetInputClassFromTemplate(ttype.Consumption));
        production = new Resource.RList<Resource.RGroup>(GetGroupFromTemplate(ttype.Production));
        Storage = new Resource.RList<Resource.RStatic>(GetStaticFromTemplate(ttype.Storage));
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
    IEnumerable<Resource.RGroup> GetGroupFromTemplate(Dictionary<int, double> template)
    {
        if (template == null) { yield break; }
        foreach (KeyValuePair<int, double> kvp in template)
        {
            yield return new Resource.RGroup(kvp.Key, new Resource.RStatic(kvp.Key, kvp.Value, Name));
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

    IEnumerable<Resource.BaseRequest> GetInputClassFromTemplate(Dictionary<int, double> template)
    {
        if (template == null) { yield break; }
        foreach (KeyValuePair<int, double> kvp in template)
        {
            yield return new Resource.BaseRequest(new Resource.RStatic(kvp.Key, kvp.Value, Name));
        }
    }
    public void AddSituation(Situations.Base s)
    {
        Situations.Add(s);
    }
}
