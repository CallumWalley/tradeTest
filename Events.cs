using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// An event. Has a time, and at least 1 thing. 
/// </summary>
public partial class Events
{
    public partial class Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }

        public List<Entity> entities;

        public string level; // unimplimented. Idea is to have a DEBUG/INFO/WARN system.
        public string scope; // unimplimented. Idea is to allow moving camera to location.

        // Events could be scoped e.g.
        //
        //         |   feature    |    domain    |    system    |   global     |
        //         |--------------|--------------|--------------|--------------|
        // DEBUG   |    show      |    hide      |    hide      |    hide      |
        // INFO    |    show      |    show      |    hide      |    hide      |
        // WARN    |    show      |    show      |    show      |    hide      |
        // ERROR   |    show      |    show      |    show      |    show      |
        //
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