using Godot;
using System;
using System.Collections.Generic;
public class Resources{
	public struct Resource{
		public string name;
		public  Texture icon;
		public float shipWeight;
		
		public Resource(string _name, Texture _icon, float _shipWeight){
			name=_name;
			icon = _icon;
			shipWeight=_shipWeight;
		}
	}

	public static Dictionary<int, Resource> _index = new Dictionary<int, Resource>(){
			{0, new Resource("Uset", GD.Load<Texture>("res://assets/icons/resources/unity_grey.dds"), 1f)},
			{1, new Resource("Minerals", GD.Load<Texture>("res://assets/icons/resources/minerals.dds"), 1f)},
			{2, new Resource("Fuel", GD.Load<Texture>("res://assets/icons/resources/energy.dds"), 0.5f)},
			{3, new Resource("Food", GD.Load<Texture>("res://assets/icons/resources/food.dds"), 0.5f)}
		};
	public static Resource index(int resourceCode){
		return _index[resourceCode];
	}
	public static Texture Icon(int resourceCode){
		return _index[resourceCode].icon;
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


