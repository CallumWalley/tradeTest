using Godot;
using System;
using System.Collections.Generic;

public class ResourcePool : EcoNode
{  

	public void Add(Resource _resource){
		//Check if aggrigator already exists.

		GetType(_resource.Type)._add.Add(_resource);
	}
	public ResourceAgr GetType(int code){
		ResourceAgr parent = null;
		foreach (ResourceAgr r in GetChildren()){
			if (r.Type == code){
				parent = r;
				break;
			}
		} 
		if (parent == null) {
			parent = new ResourceAgr();
			parent.Type = code;
			AddChild(parent);
		}
		return parent;
	}
	// Get resources with code between range.
	public IEnumerable<ResourceAgr> GetRange(int min, int max){
		foreach (ResourceAgr r in GetChildren()){
			if (min <= r.Type &&  r.Type <= max){
					yield return r;
			} 
		}
	}
	
	public IEnumerable<ResourceAgr> GetStandard(){
		return GetRange(1, 100);
	}
}
