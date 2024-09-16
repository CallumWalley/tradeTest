using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;
using System.Linq;

/// <summary>
/// Regestry for templates.
/// </summary>
public partial class PlayerFeatureTemplates : Node, IEnumerable<PlayerFeatureTemplate>
{
	public List<Domain> Zones { get; set; }

	public IEnumerator<PlayerFeatureTemplate> GetEnumerator()
	{
		foreach (PlayerFeatureTemplate f in GetChildren().Cast<PlayerFeatureTemplate>())
		{
			yield return f;
		}
	}
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
