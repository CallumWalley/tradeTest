using Game;
using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using YAT.Commands;

public partial class ActionBase : Node, Entities.IAction
{
    public virtual bool Visible { get { return Active; } }
    public virtual string Description { get; set; }
    public virtual new StringName Name { get; set; }
    public virtual bool Active
    {
        get
        {
            return true;
        }
    }
    public virtual Godot.Vector2 CameraPosition { get; }
    public virtual float CameraZoom { get; }
    public virtual void OnAction() { }

}

