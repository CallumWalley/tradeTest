using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SatelliteSystem : Node2D, Entities.IEntityable, Entities.IOrbital, IEnumerable<Entities.IOrbital>
{   
    [ExportGroup("General")]
    [Export]
    new public string Name { get { return base.Name; } set { base.Name = value; } }
    public string Description { get; set; }
	[ExportGroup("Orbital")]
	[Export]
	public float Aphelion {get;set;}
	[Export]
	public float Perihelion {get;set;}
	[Export]
	public float SemiMajorAxis {get;set;}
	[Export]
	public float Eccentricity {get;set;}
	[Export]
	public float Period {get;set;}
    public Entities.IOrbital Eldest {get{ return GetChildOrNull<Entities.IOrbital>(0);}}

    public float CameraZoom {get{
        float viewport = GetViewportRect().Size[0];
        float size;
        float zl;
        // If has less than two children raw width is size of thing itself.
        if (GetChildCount() < 2){
            zl = this.First<Entities.IOrbital>().CameraZoom;
        }
        else
        {
            size = this.Max<Entities.IOrbital>(x=> { return x.Aphelion;} );
			GD.Print("Composite");
            zl =( viewport / ((float)Math.Log(size * 1000000, (int)PlayerConfig.config.GetValue("interface", "logBase"))))/10;
        }
        return zl;
    }}

    public Vector2 CameraPosition {
        get{
            return GlobalPosition;
        }
    }

    public override void _Ready()
    {
        base._Ready();

    }
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
    public override void _Process(double delta)
    {
        base._Process(delta);
		if (Aphelion > 0){
			Position = new Vector2((float)Math.Log(Aphelion * 1000000, (int)PlayerConfig.config.GetValue("interface", "logBase")), 0);
		}
    }
    
    UIMapOverlayElement overlayElement;
    public UIMapOverlayElement OverlayElement {
        get{
        if (overlayElement == null){
            overlayElement = new UIMapOverlayElement();
            overlayElement.element = Eldest;
        }
        return overlayElement;
        }}
    // public override void _Draw(){
    //     Eldest._Draw();
    // }
}
