using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerTradeReciever : Node
{
	public List<Installation> list;

	public PlayerTradeReciever()
	{
		list = new List<Installation>();
	}

	public void RegisterInstallation(Installation tr){
		list.Add(tr);
	}
	public Installation GetTradeDestination(int i){
		return list[i];
	}
	// public List List(){
	// 	return _index;
	// }
}
