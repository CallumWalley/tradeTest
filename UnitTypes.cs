using Godot;
using System;

public static class UnitTypes
{
    static readonly string[] prefixSI = { "k", "M", "G", "T", "P", "E" };

    public static string Distance(string[] prefixes, string unit, double d)
    {
        // Include Max here somewhere
        d *= 400;
        int degree = (int)Math.Floor(Math.Log10(Math.Abs(d)) / 3);
        double scaled = d * Mathf.Pow(1000, -degree);
        string prefix = prefixes[degree];
        return string.Format("{0:N1} {1}{2}", scaled, prefix, unit);
    }

    public static string DistanceSI(double d)
    {
        return Distance(prefixSI, "m", d);
    }


    public static string TimeSol(double t)
    {
        t *= 10;
        return string.Format("{0:N1} months", t);
    }
}