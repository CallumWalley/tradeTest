using Godot;
using System;

public class PlayerTradeReciever : Node
{
	public Godot.Collections.Array<ResourcePool> _index;

	public PlayerTradeReciever()
	{
		_index = new Godot.Collections.Array<ResourcePool>();
	}

	public void RegisterResourcePool(ResourcePool tr){
		_index.Add(tr);
	}
	public ResourcePool GetTradeDestination(int i){
		GD.Print(_index[i].GetParent<Body>().Name);
		return _index[i];
	}
	public Godot.Collections.Array<ResourcePool> List(){
		return _index;
	}
}
