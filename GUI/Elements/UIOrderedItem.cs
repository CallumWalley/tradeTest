using Godot;
using System;

public class UIOrderedItem : Control
{

    public override void _Ready()
    {   
        TextureButton moveUpButton = GetNode<TextureButton>("Summary/AlignRight/Incriment/MoveUp");
		TextureButton moveDownButton = GetNode<TextureButton>("Summary/AlignRight/Incriment/MoveDown");
		moveUpButton.Connect("pressed", GetParent(), "ReorderUp");
		moveDownButton.Connect("pressed", GetParent(), "ReorderDown");
    }
}
