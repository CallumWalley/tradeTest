using Godot;
using System;
namespace Game;

public partial class UIFeatureSmall : UIButton, Lists.IListable<FeatureBase>
{

    public bool Destroy { get; set; }

    public FeatureBase feature;
    public FeatureBase GameElement { get { return feature; } }


    public void Init(FeatureBase _feature)
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
