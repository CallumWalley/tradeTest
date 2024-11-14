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
    public float SemiMajorAxis { get; set; }
    [Export]
    public float Anomaly { get; set; }
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


            return viewport / Mathf.Max(1, Mathf.Max(maxX - minX, maxY - minY));
        }
    }



    new public Vector2 CameraPosition
    {
        get
        {
            return GlobalPosition;
            // size = this.Max<Entities.IPosition>(x => { return ((Node2D)x).Position.DistanceTo(Position); });
            // return this.Average<Entities.IPosition>(x => { return ((Node2D)x).Position; }); ;
        }
    }

    public override IEnumerable<Entities.IFeature> Features
    {
        get
        {
            foreach (Entities.IPosition p in this)
            {
                foreach (Entities.IFeature f in p)
                {
                    yield return f;
                }
            }
        }
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
        if (SemiMajorAxis == 0) { return; }
        float AphelionMod = (((float)PlayerConfig.config.GetValue("interface", "linearLogBase")) < 2) ? SemiMajorAxis :
(float)Math.Log(SemiMajorAxis, (float)PlayerConfig.config.GetValue("interface", "linearLogBase"));
        // would be better if this was just eccentricity.
        Position = CalculatePosition(AphelionMod, AphelionMod, Anomaly);
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
    public static Vector2 CalculatePosition(float semiMajorAxis, float semiMinorAxis, float anomaly)
    {
        float x = semiMajorAxis * Mathf.Cos(anomaly);
        float y = semiMinorAxis * Mathf.Sin(anomaly);

        return new Vector2(x, y);
    }
}
