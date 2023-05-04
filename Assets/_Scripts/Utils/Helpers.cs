using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    /// <summary>
    /// Calculates and returns the flat distance (distance along the XZ plane) between 2 Vector3s
    /// </summary>
    /// <param name="a">The first Vector3</param>
    /// <param name="b">The second Vector3</param>
    /// <returns>Returns the flat distance between the 2 passed Vector3s</returns>
    public static float FlatDistance(Vector3 a, Vector3 b)
    {
        Vector2 flatA = new Vector2(a.x, a.z);
        Vector2 flatB = new Vector2(b.x, b.z);

        return Vector2.Distance(flatA, flatB);
    }
}