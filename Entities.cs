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
        float SemiMajorAxis { get; set; }
        float Anomaly { get; set; }
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
        public void AddFeature(Entities.IFeature feature);
        public void RemoveFeature(Entities.IFeature feature);

        public IFeature this[int index] { get; }
        public List<string> Tags { get; set; }

    }
    public interface IFeature : IEntityable, IEnumerable<ICondition>
    {
        public IPosition Site { get; }
        /// <summary>
        ///  These are tranches
        /// </summary>
        public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsOutput { get; set; }
        public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsInput { get; set; }
        public Resource.RDict<Resource.RGroup<Resource.IResource>> FactorsLocal { get; set; }
        public Resource.RDict<Resource.RStatic> FactorsSingle { get; set; }
        public PlayerFeatureTemplate Template { get; set; }
        public Texture2D IconMedium { get; }
        public string SplashScreenPath { get; }
        public string TypeSlug { get; set; }
        public Godot.Collections.Array<string> Tags { get; set; }
        public void AddCondition(ConditionBase s);
        public void RemoveCondition(ConditionBase s);

        public IEnumerable<Entities.IAction> Actions { get; }
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
    /// <summary>
    /// An action is a descision that changes the game state.
    /// </summary>
    public interface IAction : IEntityable
    {
        public bool Visible { get; } // If action is visible.
        public bool Active { get; } // If action is valid.
        public virtual void OnAction() { } // When action confirmed
    }

    /// <summary>
    /// An input element to an action
    /// </summary>
    // public interface ActionInput<T>
    // {
    //     public T Value { get; set; }
    // }
    // public class ActionInputSlider : IActionInput<double>
    // {
    //     public ActionInputSlider(double _value, double _step, double _min, double _max) => (Value, Step, Min, Max) = (_value, _step, _min, _max);
    //     public double Value { get; set; } = 1;
    //     public double Step { get; set; } = 1;
    //     public double Min { get; set; } = 0;
    //     public double Max { get; set; } = 10;
    // }


}