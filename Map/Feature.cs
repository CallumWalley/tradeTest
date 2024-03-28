using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class Feature : Node
{

    public Resource.RList<Resource.IRequestable> Factors { get; set; }
    public List<Situations.Base> Situations { get; protected set; }

    public Texture2D iconMedium;

    // 0-100 

    //////////////////////////
    // Internal state
    /////////////////////////
    // int maxCapacity = 1; // How many 'buildings' this industry contains.
    // int operationalCapacity = 1; // How many 'buildings' are currently enabled.
    // byte capacityUtilisation = 255; // What percentage of maximum inputs for currently operational [0-255]
    // double efficiency = 1; // Modifies all output value 

    // output = baseProduction * operationalCapacity *  capacityUtilisation * efficiency

    // byte breakDown; // Decays without maintainance. [0-255]


    // Output 

    public FeatureRegister.FeatureType ttype;
    public string TypeName { get { return ttype.Name; } }

    [Export(PropertyHint.Enum, "unset,f_dockyard,orbit_storage_fuel,orbit_storage_h20,mine_surf_old,mine_h20_surf_old,reclaim,cfuel_water")]
    public string TypeSlug { get; set; } = "unset";
    public string TypeClass { get { return ttype.Superclass; } }
    public string TypeSubclass { get { return ttype.Subclass; } }
    public string TypeImage { get { return ttype.Image; } }


    //public string TypeRequirements{get{ttype.Requiremnts}
    public string[] Tags { get; set; }
    public string Description { get; set; }
    public int Prioroty { get; set; }

    public void InitFirstStep(){
        // If instantiated in editor

        ttype ??= GetNode<FeatureRegister>("/root/Global/FeatureRegister").GetFromSlug(TypeSlug);
        Name ??= ttype.Name;
        Tags ??= ttype.Tags;
        if (string.IsNullOrWhiteSpace(Description)){
            Description = ttype.Description;
        }
        // Prioroty = ttype.defaultPrioroty;
        // Situations = new List<Situations.Base>();

        if (iconMedium == null)
        {
            if (ResourceLoader.Exists(ttype.Image))
            {
                iconMedium = (Texture2D)GD.Load<Texture2D>(ttype.Image);
            }
            else
            {
                GD.Print($"couldn't load icon {ttype.Image}");
                iconMedium = (Texture2D)GD.Load<Texture2D>("res://assets/icons/58x58/ship_part_computer_default.dds");
            }
        }
        Factors = new(GetFactorsFromTemplate(ttype.Factors));
    }

    // this could be made less confusing.
    public override void _Ready()
    {
        InitFirstStep();
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
    IEnumerable<Resource.IRequestable> GetFactorsFromTemplate(Dictionary<int, double> template)
    {
        if (template == null) { yield break; }
        foreach (KeyValuePair<int, double> kvp in template)
        {
            yield return new Resource.RGroupRequests<Resource.IRequestable>(new Resource.RRequest(kvp.Key, kvp.Value, "Base Yield", "Base Yield", true), Name, Description);
        }
    }

    void SetFromTemplateIfUnset(){

    }

    public void AddSituation(Situations.Base s)
    {
        Situations.Add(s);
    }
}
