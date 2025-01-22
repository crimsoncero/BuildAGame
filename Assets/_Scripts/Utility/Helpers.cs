using System;
using UnityEngine;
using UnityEngine.Pool;

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

    /// <summary>
    /// Creates an initial amount of entities in the pool to be utilized during the scene. Use at start of scene.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pool"></param>
    /// <param name="amount"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void PreWarm<T>(this ObjectPool<T> pool, int amount) where T : class
    {
        if (pool is null)
        {
            throw new ArgumentNullException(nameof(pool));
        }

        T[] entities = new T[amount];
        for(int i = 0; i < amount; i++)
        {
            entities[i] = pool.Get();
        }
        foreach (T entity in entities)
        {
            pool.Release(entity);
        }
    }
}
