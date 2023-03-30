using Godot;
using System;
using System.Collections.Generic;

public class TransformerList : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public List<TransformerType> list;

    public struct TransformerType{

        Dictionary<object, object> Type{get;set;}
        
        public TransformerType(Dictionary<object, object> _type){
            Type=_type;
        }
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Directory directory = new Directory();
        directory.Open("Transformers");
        directory.ListDirBegin();

        while (true){
            string filename = directory.GetNext();  
            if (filename==""){break;}    
            if (filename.Length < 5 || filename.Substr(filename.Length-4, filename.Length) != ".json"){continue;}
            Godot.File file = new Godot.File();
            file.Open(filename, Godot.File.ModeFlags.Read);
            foreach (Dictionary<object, object> t in (Godot.Collections.Array)JSON.Parse(file.GetAsText()).Result){
                list.Add(new TransformerType(t));
            }
            file.Close();
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
