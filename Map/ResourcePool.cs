using Godot;
using System;
using System.Collections.Generic;

public class ResourcePool : EcoNode
{  
	public Godot.Collections.Array<ResourceAgr> members = new Godot.Collections.Array<ResourceAgr>();

	// A resrouce pool can only have 1 trade route.
	public TradeRoute tradeRoute;

	public float shipWeight;

	// Convenience function. Makes children at start of scene into members.
	public override void _Ready(){
		base._Ready();
		foreach (Resource child in GetChildren()){
			Add(child);
			GD.Print($"Adding resource to pool. {GetPath()}");
		}
	}

	public void Add(Resource _resource){
		//Check if aggrigator already exists.
		GetType(_resource.Type)._add.Add(_resource);
	}
	public ResourceAgr GetType(int code){
		ResourceAgr newResource = null;
		foreach (ResourceAgr r in members){
			if (r.Type == code){
				newResource = r;
				break;
			}
		} 
		if (newResource == null) {
			newResource = new ResourceAgr();
            newResource.Type = code;
			members.Add(newResource);
			AddChild(newResource);
		}
		return newResource;
	}
	// Get resources with code between range.
	public IEnumerable<ResourceAgr> GetRange(int min, int max){
		foreach (ResourceAgr r in members){
			if (min <= r.Type &&  r.Type <= max){
					yield return r;
			} 
		}
	}
	
	public IEnumerable<ResourceAgr> GetStandard(){
		return GetRange(1, 100);
	}

	public override void EFrameCollect(){
		shipWeight=GetShipWeight();
	}

	public float GetShipWeight(){
		float shipWeightImport = 0;
		float shipWeightExport = 0;
		foreach (Resource child in GetStandard()){
			if (child.Sum > 0){
				shipWeightExport += child.Sum * Resources.ShipWeight(child.Type);
			} else {
				shipWeightImport += child.Sum * Resources.ShipWeight(child.Type);
			}
		}
		return Math.Max(shipWeightExport, shipWeightImport);
	}
}
