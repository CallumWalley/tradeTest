using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A thing. Subtype of node. Should be used instead of node for game objects.
/// </summary>
public partial class Events
{
    public partial class Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }

        public List<Entity> entities;

        public string level;
        public string center;

        Event(List<Entity> _entities, int _time, string _name = "Unknown", string _description = "Nothing else is known at this time.")
        {
            Name = _name;
            Description = _description;
            Time = _time;
        }
    }
    public void EmitEvent(Event _event)
    {

    }
}