using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Linq;

public partial class ConditionBase : Entity, Conditions.IConditionable
{
    public FeatureBase Feature { get { return (FeatureBase)GetParent(); } } // parent reference.

    public virtual void OnAdd() { } //Called when added to feature.
    public virtual void OnRemove()
    {
        QueueFree();
    } //Called when removed

    public virtual void OnEFrame() { } //Called every eframe.
    public virtual void OnRequestSet() { }

}