using Godot;
using System;

public partial class UIFeatureSmall : UIButton, Lists.IListable<Features.FeatureBase>
{

    public bool Destroy { get; set; }

    public Features.FeatureBase feature;
    public Features.FeatureBase GameElement { get { return feature; } }


    public void Init(Features.FeatureBase _feature)
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
