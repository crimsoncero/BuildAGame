using System;
using UnityEngine;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers 
{
    /// <summary>
    /// Destroy all child objects of this transform (Unintentionally evil sounding).
    /// Use it like so:
    /// <code>
    /// transform.DestroyChildren();
    /// </code>
    /// </summary>
    public static void DestroyChildren(this Transform t) {
        foreach (Transform child in t) UnityEngine.Object.Destroy(child.gameObject);
    }


    /// <summary>
    /// Formats the time given in seconds to an MM:SS string.
    /// </summary>
    /// <param name="time"> Time in seconds</param>
    /// <returns>A string in an MM:SS format</returns>
    public static string SecondsToMMSS(int time)
    {
        var span = TimeSpan.FromSeconds(time);
        return string.Format("{0:00}:{1:00}", (int)span.TotalMinutes, span.Seconds);
        
    }
}
