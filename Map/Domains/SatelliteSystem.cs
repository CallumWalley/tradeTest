using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Game;

public partial class SatelliteSystem : Domain, Entities.IOrbital, IEnumerable<Entities.IPosition>
{
    // [ExportGroup("General")]
    // [Export]
    // new public string Name { get { return base.Name; } set { base.Name = value; } }

    [ExportGroup("Orbital")]
    [Export]
    public float Aphelion { get; set; }
    [Export]
    public float Perihelion { get; set; }
    [Export]
    public float SemiMajorAxis { get; set; }
    [Export]
    public float Eccentricity { get; set; }
    [Export]
    public float Period { get; set; }
    public Entities.IPosition Eldest { get { return GetChildOrNull<Entities.IPosition>(0); } }

    [ExportGroup("Economic")]
    [Export]
    public bool HasSpaceport { get; set; }
    new public float CameraZoom
    {
        get
        {
            float viewport = GetViewportRect().Size[0];
            float size;
            // If has less than two children raw width is size of thing itself.
            if (GetChildCount() < 2)
            {
                return this.First<Entities.IPosition>().CameraZoom;
            }
            else
            {
                size = this.Max<Entities.IPosition>(x => { return x.Aphelion; });

                float zl = (((float)PlayerConfig.config.GetValue("interface", "linearLogBase")) < 2) ? size : (float)Math.Log(size, (float)PlayerConfig.config.GetValue("interface", "linearLogBase"));
                return (viewport) / (zl * 4 * 0.1f);// Make moons fit on screen. //(float)PlayerConfig.config.GetValue("interface", "linearScale");
            }

        }
    }



    new public Vector2 CameraPosition
    {
        get
        {
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
    public IEnumerator<Entities.IPosition> GetEnumerator()
    {
        foreach (Node c in GetChildren())
        {
            if (typeof(Entities.IPosition).IsAssignableFrom(c.GetType()))
            {
                yield return (Entities.IPosition)c;
            }
        }
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        float place = (((float)PlayerConfig.config.GetValue("interface", "linearLogBase")) < 2) ? Aphelion : (float)Math.Log(Aphelion, (float)PlayerConfig.config.GetValue("interface", "linearLogBase"));
        place *= 1; //(float)PlayerConfig.config.GetValue("interface", "linearScale");
        Position = new Vector2(place, 0);
    }

    UIMapOverlayElement overlayElement;
    public UIMapOverlayElement OverlayElement
    {
        get
        {
            if (overlayElement == null)
            {
                overlayElement = new UIMapOverlayElement();
                overlayElement.element = Eldest;
            }
            return overlayElement;
        }
    }
    // public override void _Draw(){
    //     Eldest._Draw();
    // }
}
