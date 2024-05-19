using Godot;
using System;
using System.Collections;

public partial class FeatureConstructor : Features.BasicFactory
{

    public override void _Ready()
    {
        GetNode<Global>("/root/Global").Connect("Setup", new Callable(this, "Setup"));

        // Only intended for use in editor.
        // Replaces feature constructor with feature.
        // GetParent().CallDeferred("add_child", );
        // QueueFree();
    }
    void Setup()
    {
        GetParent().AddChild(Instantiate());
        GetParent().RemoveChild(this);
        QueueFree();
    }
}