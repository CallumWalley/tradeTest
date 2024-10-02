using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
                zl = this.First<Entities.IOrbital>().CameraZoom;
            }
            else
            {
                size = this.Max<Entities.IOrbital>(x => { return x.Aphelion; });
                if ((float)PlayerConfig.config.GetValue("interface", "logBase") <= 1)
                {
                    zl = (viewport / size * 1000000) / 10;

                }
                else
                {
                    zl = (viewport / ((float)Math.Log(size * 1000000, (float)PlayerConfig.config.GetValue("interface", "logBase")))) / 10;

                }

                GD.Print("Composite");

            }
            return zl;
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


