using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;

/// <summary>
/// Tags to identify feature types.
/// </summary>
public static class Tags 
{    

    
    static Tag defaultTag = new Tag("orbital", "Orbital", "Must be built in orbit");
    public class Tag
    {
        public string Slug;
        public string Name;
        public string Description;

        public Tag(string _slug, string _name, string _description)
        {
            Slug = _slug;
            Name = _name;
            Description = _description;
        }
    }
    
    public static string Name(string slug){
        return _tags[slug].Name;
    }
    public static string Description(string slug){
        return _tags[slug].Description;
    }
    /// <summary>
    /// Dictionary of feature types.
    /// </summary>
    public static Dictionary<string, Tag> _tags = new Dictionary<string, Tag>(){
        {"orbital", new Tag("orbital", "Orbital", "Must be built in orbit")},
        {"planetary", new Tag("planetary", "Planetary", "Must be built on the surface of a planet")}
    };
}