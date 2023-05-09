using Godot;
using System;

public partial class PlayerTech : Node
{
    public double freighterA = 0.0000015f; // 0.00000015
                                           // multiply by approx 7000000 to get m/s^2

    public double GetFreighterTons(double weight, double distance)
    {
        // distance in MM
        // weight in freightersKiloTonnes / month
        return (Mathf.Sqrt(distance / freighterA) * 0.0002f * weight) / 10f;
        // return freightersTonnes
    }

}
