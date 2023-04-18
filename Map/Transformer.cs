using Godot;
using System;
using System.Collections.Generic;

public class Transformer : EcoNode
{
    [Export]
    public string slug;
    public Resource output;
    public ResourceList Production { get; protected set; }
    public List<TransformerInputType.Base> Consumption { get; protected set; }
    public List<Resource> Storage { get; protected set; }
    public List<Situations.Base> Situations { get; protected set; }
    // 0-100 
    // Decays without maintainance.
    float breakDown;

    //How many 'buildings' this industry contains.
    int weight = 1;

    //How many 'buildings' are currently offline.
    int weightOffline = 1;

    public TransformerRegister.TransformerType ttype;
    public string TypeName { get { return ttype.Name; } }
    public string TypeSlug { get { return ttype.Slug; } }
    public string TypeClass { get { return ttype.Superclass; } }
    public string TypeSubclass { get { return ttype.Subclass; } }
    public string TypeImage { get { return ttype.Image; } }

    //public string TypeRequirements{get{ttype.Requiremnts}}

    public string[] Tags { get; set; }
    public string Description { get; set; }
    public int Prioroty { get; set; }

    public override void _Ready()
    {
        base._Ready();

        // If instantiated in editor
        if (ttype is null)
        {
            ttype = GetNode<TransformerRegister>("/root/Global/TransformerRegister").GetFromSlug(slug);
        }

        Tags = ttype.Tags;
        Description = ttype.Description;
        Prioroty = ttype.defaultPrioroty;
        Situations = new List<Situations.Base>();
        Consumption = new List<TransformerInputType.Base>(GetInputClassFromTemplate(ttype.Consumption));
        Production = new ResourceList(GetStaticFromTemplate(ttype.Production));
        Storage = new List<Resource>(GetStaticFromTemplate(ttype.Storage));

    }


    public virtual IEnumerable<TransformerInputType.Base> Consumed()
    {
        foreach (TransformerInputType.Base tip in Consumption)
        {
            yield return tip;
        }
    }
    // IEnumerable<Requester> GetFromTemplate(Dictionary<int, float> template)
    // {
    //     if (template == null) { yield break; }
    //     foreach (KeyValuePair<int, float> kvp in template)
    //     {
    //         yield return new Requester(new ResourceStatic(kvp.Key, kvp.Value, Name));
    //     }
    // }
    IEnumerable<ResourceStatic> GetStaticFromTemplate(Dictionary<int, float> template)
    {
        if (template == null) { yield break; }
        foreach (KeyValuePair<int, float> kvp in template)
        {
            yield return new ResourceStatic(kvp.Key, kvp.Value, Name);
        }
    }

    IEnumerable<TransformerInputType.Base> GetInputClassFromTemplate(Dictionary<int, float> template)
    {
        if (template == null) { yield break; }
        foreach (KeyValuePair<int, float> kvp in template)
        {
            yield return new TransformerInputType.Base(this, new ResourceStatic(kvp.Key, kvp.Value));
        }
    }
    public void AddSituation(Situations.Base s)
    {
        Situations.Add(s);
    }
}
