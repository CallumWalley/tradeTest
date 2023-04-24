using Godot;
using System;
using System.Collections.Generic;
public class Resources
{
	public struct Resource
	{
		public string name;
		public Texture icon;
		public float shipWeight;

		// If this resrource is something that can be store, or only instant;
		public bool storable;

		public Resource(string _name, Texture _icon, float _shipWeight, bool _storable)
		{
			name = _name;
			icon = _icon;
			shipWeight = _shipWeight;
			storable = _storable;
		}
	}

	public static Dictionary<int, Resource> _index = new Dictionary<int, Resource>(){
			{0, new Resource("Unset", GD.Load<Texture>("res://assets/icons/resources/unity_grey.dds"), 1f, false)},
			{1, new Resource("Minerals", GD.Load<Texture>("res://assets/icons/resources/minerals.dds"), 1f, true)},
			{2, new Resource("Fuel", GD.Load<Texture>("res://assets/icons/resources/energy.dds"), 0.5f, true)},
			{3, new Resource("Food", GD.Load<Texture>("res://assets/icons/resources/food.dds"), 0.5f, true)},
			{4, new Resource("H2O", GD.Load<Texture>("res://assets/icons/resources/h2o.png"), 1f, true)},
			{901, new Resource("Freighter", GD.Load<Texture>("res://assets/icons/freighter.png"), -1f, false)}
		};
	public static Resource Index(int resourceCode)
	{
		return _index[resourceCode];
	}
	public static Texture Icon(int resourceCode)
	{
		return _index[resourceCode].icon;
	}
	public static float ShipWeight(int resourceCode)
	{
		return _index[resourceCode].shipWeight;
	}


}

//	}
//	public Resource(string _name, Texture _icon, float _shipWeight) {
//	  name = _name;
//	  shipWeight = _shipWeight;
//	  icon = _icon;
//	}


//	public static Texture Icon(int resourceCode){
//		try{
//			return _Index[resourceCode].icon;
//		}
//		catch{
//			return GD.Load<Texture>("res://assets/icons/resources/unity_grey.dds");
//		}
//	}


