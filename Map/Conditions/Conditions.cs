using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
namespace Game;

public partial class Conditions
{
    // Utility function
    public static object TryGetDefault(Dictionary<string, object> dict, string key, object defaultValue = null)
    {
        dict.TryGetValue(key, out defaultValue);
        return defaultValue;
    }
    // Condition is some logic affecting a feature, evaluated every EFrame
    public interface IConditionable : Entities.IEntityable
    {
        public FeatureBase Feature { get; } // parent reference.

        public virtual void OnAdd() { } //Called when added to feature.
        public virtual void OnRemove() { } //Called when removed
        public virtual void OnEFrame() { } //Called every eframe.
        public virtual void OnRequestSet() { } // Called when requests are calculated.

    }

}
