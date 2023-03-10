using Godot;
using System;

public class GlobalTech : Node
{
    public float freighterA = 0.0000015f; // 0.00000015
	// multiply by approx 7000000 to get m/s^2

    public float GetFreighterTons(float weight, float distance){
		// distance in MM
		// weight in freightersKiloTonnes / month
		return (float)((int)(Math.Sqrt( distance / freighterA ) * 0.0002f * weight) / 10f);
		// return freightersTonnes
	}

}
