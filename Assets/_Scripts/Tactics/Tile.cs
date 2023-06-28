using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a place on a grid (the tilemap) that a unit can stand on and how those places (tiles) link together
/// </summary>
public class Tile: MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform[] tileDetectionPoints;

    // Tile status detection and display variables
    [SerializeField] private GameObject tileSelectedDisplay;
    [SerializeField] private Renderer tileTypeDisplay;
    [SerializeField] private Transform tileTypeDetectionPoint;
    [SerializeField] private LayerMask tileTypeDetectionMask;
    [SerializeField] private float tileTypeDetectionRadius;

    // Tile status variables
    private bool displaySelected;
    private bool isWall;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        
    }//end Start

    // Update is called once per frame
    void Update()
    {
        DisplayTileStatus();
    }//end Update

    #endregion //end Unity Control Methods

    #region Tile Display

    /// <summary>
    /// Displays the tile's status and type information
    /// </summary>
    public void DisplayTileStatus()
    {
        // Ensure the tile knows what type of tile it is
        DetectTileType();

        // Display the selected display if the tile is selected
        tileSelectedDisplay.SetActive(displaySelected);

        // Display the correct color based on the tile's type
        if(isWall)
        {
            tileTypeDisplay.material = TileManager.GetTileMaterialWall();
        }
        else
        {
            //tileTypeDisplay.SetMaterials(TileManager.GetTileMaterialWalkable());
            tileTypeDisplay.material = TileManager.GetTileMaterialWalkable();
        }
    }//end DisplayTileStatus

    /// <summary>
    /// Detects whether or not this tile is a Wall
    /// </summary>
    private void DetectTileType()
    {
        // Get any tile type colliders that affect this tile
        Collider[] _colliders = Physics.OverlapSphere(tileTypeDetectionPoint.position, tileTypeDetectionRadius, tileTypeDetectionMask);

        // If there was at least one tile type definer that affects this tile, then this tile is a wall
        if(_colliders.Length > 0)
        {
            isWall = true;
        }
    }//end DetectTileType

    /// <summary>
    /// Update whether or not this tile should display the selected visual with the passed value
    /// </summary>
    /// <param name="_isSelected"></param>
    public void SetIsSelected(bool _isSelected)
    {
        displaySelected = _isSelected;
    }//end SetIsSelected

    #endregion
}