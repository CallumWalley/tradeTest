using Godot;
using System;

public partial class UIIncriment : VBoxContainer
{
    // TODO: modify keys
    public Action<float> callback;
    public override void _Ready()
    {
        GetNode("MoveUp").Connect("pressed", new Callable(this, "Incriment"));
        GetNode("MoveDown").Connect("pressed", new Callable(this, "Decriment"));
    }
    void Incriment(){
        if (callback != null){
            callback(1f);
        }
    }
    void Decriment(){
        if (callback != null){
            callback(-1f);
        }
    }

}
