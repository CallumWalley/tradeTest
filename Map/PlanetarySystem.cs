using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Game;

public partial class PlanetarySystem : Node2D, Entities.IEntityable, IEnumerable<Entities.IOrbital>
{
    new public string Name { get { return base.Name; } set { base.Name = value; } }
    public string Description { get; set; }


    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public IEnumerator<Entities.IOrbital> GetEnumerator()
    {
        foreach (Node c in GetChildren())
        {
            if (typeof(Entities.IOrbital).IsAssignableFrom(c.GetType()))
            {
                yield return (Entities.IOrbital)c;
            }
        }
    }

    public Entities.IOrbital Eldest { get { return GetChildOrNull<Entities.IOrbital>(0); } }
    public float CameraZoom
    {
        get
        {
            float viewport = GetViewportRect().Size[0];
            float size;
            float zl;
            // If has less than two children raw width is size of thing itself.
            if (GetChildCount() < 2)
            {
                return this.First<Entities.IOrbital>().CameraZoom;
            }
            else
            {
                size = this.Max<Entities.IOrbital>(x => { return x.Aphelion; });

                zl = (((float)PlayerConfig.config.GetValue("interface", "linearLogBase")) < 2) ? size : (float)Math.Log(size, (float)PlayerConfig.config.GetValue("interface", "linearLogBase"));
                return (viewport) / (zl * 0.1f * 4);
            }

        }
    }


    public Vector2 CameraPosition
    {
        get
        {
            return GlobalPosition;
        }
    }





    public override void _Draw()
    {
        base._Draw();
    }
}


