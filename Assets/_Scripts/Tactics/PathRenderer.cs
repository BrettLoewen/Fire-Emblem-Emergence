using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to draw a line along a path of tiles for tilemap navigation
/// </summary>
public class PathRenderer: MonoBehaviour
{
    #region Variables

    [SerializeField] private LineRenderer lineRenderer;  // The LineRenderer component used to create the visual effect
    [SerializeField] private Vector3 offsetFromTile = new Vector3(0f, 0.1f, 0f);

    #endregion //end Variables

    #region Enable/Disable

    /// <summary>
    /// Turn off the PathRenderer
    /// </summary>
    public void Disable()
    {
        // Disable the LineRenderer
        lineRenderer.enabled = false;
    }//end Disable

    /// <summary>
    /// Turn on the PathRenderer
    /// </summary>
    public void Enable()
    {
        // Enable the LineRenderer
        lineRenderer.enabled = true;
    }//end Enable

    #endregion

    #region Configure Display

    /// <summary>
    /// Display the passed path of Tiles
    /// </summary>
    /// <param name="path">The Stack of Tiles which defines the path to display</param>
    public void DisplayPath(Stack<Tile> path)
    {
        // Turn on the PathRenderer
        Enable();

        // Convert the passed Stack of Tiles to an Array of Tiles
        Tile[] tiles = path.ToArray();

        // Set the LineRenderer's position count to have the necessary number of points
        lineRenderer.positionCount = tiles.Length;

        // Loop through the path of Tiles
        for (int i = 0; i < tiles.Length; i++)
        {
            // Add the Tile's position to the LineRenderer
            lineRenderer.SetPosition(i, tiles[i].transform.position + offsetFromTile);
        }
    }//end DisplayPath

    #endregion
}