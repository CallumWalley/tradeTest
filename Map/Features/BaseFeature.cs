using Godot;
using System;
using System.Collections.Generic;

[Tool]
public partial class BaseFeature : Node
{
    
    public Resource.RList<Resource.IRequestable> FactorsLocal { get; set; }
    public Resource.RList<Resource.IRequestable> FactorsGlobal { get; set; }
    public List<Situations.Base> Situations { get; protected set; }
    public Texture2D iconMedium;

    public string[] Tags { get; set; }
    public string Description { get; set; }

    public void AddSituation(Situations.Base s)
    {
        Situations.Add(s);
    }
}
