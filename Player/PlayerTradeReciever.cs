using Godot;
using System;

public class PlayerTradeReciever : Node
{
	[Export]
	public Godot.Collections.Array<ResourcePool> _index;

	public PlayerTradeReciever()
	{
		_index = new Godot.Collections.Array<ResourcePool>();
	}

	public void RegisterResourcePool(ResourcePool tr){
		_index.Add(tr);
	}
	public ResourcePool GetTradeDestination(int i){
		return _index[i];
	}
	public Godot.Collections.Array<ResourcePool> List(){
		return _index;
	}
}
