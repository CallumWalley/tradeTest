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

            if (this.Count() < 2)
            {
                return this.First().CameraZoom;
            }
            float maxX = -Mathf.Inf;
            float maxY = -Mathf.Inf;
            float minX = Mathf.Inf;
            float minY = Mathf.Inf;
            // If has less than two children raw width is size of thing itself.

            foreach (Node2D position in this)
            {
                maxX = Mathf.Max(maxX, position.GlobalPosition.X);
                maxY = Mathf.Max(maxY, position.GlobalPosition.Y);
                minX = Mathf.Min(minX, position.GlobalPosition.X);
                minY = Mathf.Min(minY, position.GlobalPosition.Y);
            }


            return viewport / Mathf.Max(1, 2 * Mathf.Max(maxX - minX, maxY - minY));
        }
    }


    public Vector2 CameraPosition
    {
        get
        {
            if (this.Count() < 2)
            {
                return this.First().CameraPosition;
            }
            float maxX = -Mathf.Inf;
            float maxY = -Mathf.Inf;
            float minX = Mathf.Inf;
            float minY = Mathf.Inf;
            // If has less than two children raw width is size of thing itself.

            foreach (Node2D position in this)
            {
                maxX = Mathf.Max(maxX, position.Position.X);
                maxY = Mathf.Max(maxY, position.Position.Y);
                minX = Mathf.Min(minX, position.Position.X);
                minY = Mathf.Min(minY, position.Position.Y);
            }

            return new Vector2(maxX + minX / 2, maxY + minY / 2);
        }
    }





    public override void _Draw()
    {
        base._Draw();
    }
}


