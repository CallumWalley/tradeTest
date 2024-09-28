using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SatelliteSystem : Node2D, Entities.IEntityable, IEnumerable<Domain>
{
    new public string Name { get { return base.Name; } set { base.Name = value; } }
    public string Description { get; set; }

    public Domain Eldest {get{ return GetChildOrNull<Domain>(0);}}

    public float ZoomLevel {get{
        float viewport = GetViewportRect().Size[1];
        float size;
        // If has less than two children raw width is size of thing itself.
        if (GetChildCount() < 2){size = this.First<Domain>().ZoomLevel;}else
        {
            size = this.Max<Domain>(x=> { return x.Position.Length();} ) ;
        }
        GD.Print($"Viewport: {viewport}, size: {size}, Zoom Level: {viewport / size}"); 
        return viewport / size;
    }}

    public override void _Ready()
    {
        base._Ready();

    }
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
	public IEnumerator<Domain> GetEnumerator()
	{
		foreach (Node c in GetChildren())
		{
            GD.Print(c);
			if (typeof(Domain).IsAssignableFrom(c.GetType()))
			{
				yield return (Domain)c;
			}
		}
	}
    public override void _Draw(){
        base._Draw();
        Eldest._Draw();
    }
}
