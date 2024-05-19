using Godot;
using System;

public partial class UIFeatureSmall : UIButton, Lists.IListable<Features.Basic>
{

    public bool Destroy { get; set; }

    public Features.Basic feature;
    public Features.Basic GameElement { get { return feature; } }


    public void Init(Features.Basic _feature)
    {
        feature = _feature;
    }

    public void Update()
    {

    }

    public void Remove()
    {
        QueueFree();
    }
}
