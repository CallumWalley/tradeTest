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
        [Export]
        string Description { get; set; }

        /// <summary>
        /// An  element that can have the camera centered on it.
        /// (conisder mergining this with IEntity)
        /// </summary>
        Godot.Vector2 CameraPosition { get; }
        float CameraZoom { get; }
    }
    public interface IOrbital : IEntityable
    {
        float Aphelion { get; set; }
        float Perihelion { get; set; }
        float SemiMajorAxis { get; set; }
        float Eccentricity { get; set; }
        float Period { get; set; }
    }

}