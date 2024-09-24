using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Linq;

public partial class ConditionBase : Node, Conditions.IConditionable
{
    public FeatureBase Feature { get { return (FeatureBase)GetParent(); } } // parent reference.
    new public string Name { get { return base.Name; } set { base.Name = value; } }
    public string Description { get; set; }
    public virtual void OnAdd() { } //Called when added to feature.
    public virtual void OnRemove()
    {
        QueueFree();
    } //Called when removed

    public virtual void OnEFrame() { } //Called every eframe.
    public virtual void OnRequestSet() { }

}