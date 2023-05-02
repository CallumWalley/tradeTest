using Godot;
using System;

public partial class UIOrderedItem : Control
{

    public override void _Ready()
    {   
        TextureButton moveUpButton = GetNode<TextureButton>("Summary/AlignRight/Incriment/MoveUp");
		TextureButton moveDownButton = GetNode<TextureButton>("Summary/AlignRight/Incriment/MoveDown");
		moveUpButton.Connect("pressed", new Callable(GetParent(), "ReorderUp"));
		moveDownButton.Connect("pressed", new Callable(GetParent(), "ReorderDown"));
    }
}
