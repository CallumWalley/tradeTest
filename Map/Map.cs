using Godot;
using System;
namespace Game;

public partial class Map : Node
{
    [Export]
    public Game.PlanetarySystem startView;

    public override void _Ready()
    {
        base._Ready();
        GetNode<Game.Screen>("../Screen").DrawSystem(startView);
        GetNode<YAT.YAT>("/root/YAT").TerminalManager.CurrentTerminal.SelectedNode.ChangeSelectedNode(startView.GetPath());
    }
}
