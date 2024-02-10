using Godot;
using System;

public partial class UIFeatureSmall : PanelContainer, Lists.IListable<Feature>
{

	public bool Destroy { get; set; }

	public Feature feature;
    public Feature GameElement { get { return feature; } }


    public void Init(Feature _feature)
    {
        feature = _feature;
	}

	public void Update(){

	}

	public void Remove()
    {
        QueueFree();
    }
}
