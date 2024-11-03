using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
namespace Game;

public partial class ConditionBase : Node, Entities.ICondition
{
    public FeatureBase Feature { get { return (FeatureBase)GetParent(); } } // parent reference.
    public Godot.Vector2 CameraPosition { get { throw new NotImplementedException(); } }
    public float CameraZoom { get { throw new NotImplementedException(); } }
    new public string Name { get { return base.Name; } set { base.Name = value; } }
    [Export]
    public string Description { get; set; } = "Nothing is known about this.";
    public virtual void OnAdd() { } // Called when added to feature.
    public virtual void OnRemove()
    {
        QueueFree();
    } //Called when removed

    public virtual void OnEFrame()
    {
        if (!IsInsideTree()) { return; }
    } //Called every eframe.
    public virtual void OnRequestSet() { }

}