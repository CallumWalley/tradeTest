using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Game;

/// <summary>
/// A thing. Subtype of node. Should be used instead of node for game objects.
/// </summary>
public static class Entities
{
    public interface IEntityable
    {
        StringName Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// An  element that can have the camera centered on it.
        /// (conisder mergining this with IEntity)
        /// </summary>
        Godot.Vector2 CameraPosition { get; }
        float CameraZoom { get; }
    }
    /// <summary>
    /// Entity that can be placed in orbit.
    /// </summary>
    public interface IOrbital : IEntityable
    {
        float Aphelion { get; set; }
        float Perihelion { get; set; }
        float SemiMajorAxis { get; set; }
        float Eccentricity { get; set; }
        float Period { get; set; }
    }
    public interface IDomain : IEntityable
    {
        public Godot.Collections.Dictionary<int, double> StartingResources { get; set; }
        public IEnumerable<IFeature> Features { get; }
        public int Order { get; }
        public Domain Network { get; set; }
        public string NetworkName { get; set; }
        public TradeRoute UplineTraderoute { get; }
        public List<TradeRoute> DownlineTraderoutes { get; }
        public void RegisterUpline(TradeRoute i);
        public void DeregisterUpline(TradeRoute i, bool upline = false);
        public void RegisterDownline(TradeRoute i);
        public void DeregisterDownline(TradeRoute i);
    }
    public interface IPosition : IEntityable, IOrbital, IEnumerable<IFeature>
    {
        public IDomain Domain { get; }
        public bool HasEconomy { get; set; }
        public IFeature AddFeature(PlayerFeatureTemplate template, StringName name);
        public IFeature this[int index] { get; }
        public List<string> Tags { get; set; }

    }
    public interface IFeature : IEntityable, IEnumerable<ICondition>
    {
        public IPosition Position { get; }
        public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsGlobalOutput { get; set; }
        public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsGlobalInput { get; set; }
        public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsLocal { get; set; }
        public Resource.RDict<Resource.RStatic> FactorsSingle { get; set; }
        public PlayerFeatureTemplate Template { get; set; }
        public Texture2D IconMedium { get; }
        public string SplashScreenPath { get; }
        public string TypeSlug { get; set; }
        public Godot.Collections.Array<string> Tags { get; set; }
        public void AddCondition(ConditionBase s);
        public void RemoveCondition(ConditionBase s);
        public void OnEFrame();
    }

    public interface ICondition : IEntityable
    {
        public FeatureBase Feature { get; } // parent reference.
        public bool Visible { get; set; }
        public virtual void OnAdd() { } //Called when added to feature.
        public virtual void OnRemove() { } //Called when removed
        public virtual void OnEFrame() { } //Called every eframe.
        public virtual void OnRequestSet() { } // Called when requests are calculated.

    }
}