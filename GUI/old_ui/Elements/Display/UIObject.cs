using Godot;
using System;

public interface IUIObject<TObject>
{
    public TObject GameElement { get; set; }
    public void Init(TObject @object);
    public void Update();
    public void Destroy();

}
// A ui object that mirrors a game object.
