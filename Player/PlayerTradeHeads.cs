using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerTradeHeads : Node, IEnumerable<Installation>
{
	List<Installation> list;

	public PlayerTradeHeads()
	{
		list = new List<Installation>();
	}
	public IEnumerator<Installation> GetEnumerator()
	{
		return list.GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
	public void RegisterInstallation(Installation tr)
	{
		list.Add(tr);
	}
	public Installation GetTradeDestination(int i)
	{
		return list[i];
	}
	// public List List(){
	// 	return _index;
	// }
}
