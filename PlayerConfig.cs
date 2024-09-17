using Godot;
using System;
using System.Collections.Generic;
using System.IO;
public partial class PlayerConfig : Node
{
    [Export]
    string confFilePath = "user://config.cfg";

    public static ConfigFile config = new ConfigFile();

    public override void _Ready()
    {
        base._Ready();
        LoadConfigFile();
    }

    public void LoadConfigFile()
    {
        Error e = config.Load(confFilePath);
        if (e != Error.Ok)
        {
            GD.Print($"Writing default config file to {confFilePath}");
            config.SetValue("interface", "stepNumericalInterpolation", true);
            config.Save(confFilePath);
        }
    }
}