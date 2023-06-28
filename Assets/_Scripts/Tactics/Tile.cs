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
    [SerializeField] private float detectionRadius = 0.5f;
    [SerializeField] private LayerMask tileDetectionMask;

    // Pathfinding variables
    public List<Tile> LinkedTiles { get; private set; }
    public float Distance;
    public bool Visited;
    public Tile Parent;

    // Tile status detection and display variables
    [SerializeField] private GameObject tileSelectedDisplay;
    [SerializeField] private Renderer tileTypeDisplay;
    [SerializeField] private Transform detectionPoint;
    [SerializeField] private LayerMask tileTypeDetectionMask;
    [SerializeField] private LayerMask unitDetectionMask;

    // Tile status variables
    private bool displaySelected;
    private bool displayUnitSelected;
    private bool isWall;
    private bool isWalkable;

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
        tileSelectedDisplay.SetActive(displaySelected || displayUnitSelected);

        // Display the correct color based on the tile's type
        if(isWalkable)
        {
            tileTypeDisplay.material = TileManager.GetTileMaterialWalkable();
        }
        else
        {
            tileTypeDisplay.material = TileManager.GetTileMaterialNothing();
        }
    }//end DisplayTileStatus

    /// <summary>
    /// Detects whether or not this tile is a Wall
    /// </summary>
    private void DetectTileType()
    {
        // Get any tile type colliders that affect this tile
        Collider[] _colliders = Physics.OverlapSphere(detectionPoint.position, detectionRadius, tileTypeDetectionMask);

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

    /// <summary>
    /// Update whether or not this tile should display the selected (unit) visual with the passed value
    /// </summary>
    /// <param name="_isSelected"></param>
    public void SetUnitSelected(bool _isSelected)
    {
        displayUnitSelected = _isSelected;
    }//end SetUnitSelected

    #endregion

    #region Pathfinding

    /// <summary>
    /// Find and store the nearby tiles this tile should be linked to
    /// </summary>
    public void CalculateTileLinks()
    {
        // Reset (if necessary) the list of linked tiles
        LinkedTiles = new List<Tile>();

        // Loop through each possible place a nearby tile could be
        foreach (Transform _detectionPoint in tileDetectionPoints)
        {
            // Get the tile collider at this point (should be 0 or 1)
            Collider[] _colliders = Physics.OverlapSphere(_detectionPoint.position, detectionRadius, tileDetectionMask);

            // If at least one collider was found...
            if(_colliders.Length > 0)
            {
                // Get the tile component from the collider
                Tile _tile = _colliders[0].GetComponent<Tile>();

                // Ensure the tile was found
                if(_tile != null)
                {
                    // Add the tile to list of linked tiles
                    LinkedTiles.Add(_tile);
                }
            }
        }
    }//end CalculateTileLinks

    /// <summary>
    /// Reset the pathfinding variables so they do not cause any inaccurate/odd results/behaviour
    /// </summary>
    public void ResetPathfinding()
    {
        // Reset the pathfinding variables
        Distance = 0f;
        Visited = false;
        Parent = null;

        // Reset the pathfinding visual variables
        isWalkable = false;
    }//end ResetPathfinding

    /// <summary>
    /// Returns true if this tile could be walked on by a unit and false otherwise
    /// </summary>
    /// <returns></returns>
    public bool CheckWalkable()
    {
        // If the tile is a wall or a unit is standing on this tile, it is not walkable, so return false
        if(isWall || GetUnitOnTile() != null)
        {
            return false;
        }

        // If this line was reached, the tile is walkable, so return true
        return true;
    }//end CheckWalkable

    /// <summary>
    /// Tell the tile to display that it is walkable
    /// </summary>
    public void SetIsWalkable()
    {
        isWalkable = true;
    }//end SetIsWalkable

    #endregion

    /// <summary>
    /// Check for a unit (tactics) that might be standing on this tile and return it
    /// </summary>
    /// <returns>Returns the UnitTactics if a unit was found and null otherwise</returns>
    public UnitTactics GetUnitOnTile()
    {
        // Get any unit tactics gameobjects that are standing on this tile (should be only 1)
        Collider[] _colliders = Physics.OverlapSphere(detectionPoint.position, detectionRadius, unitDetectionMask);

        // Ensure there was at least one unit tactics found
        if (_colliders.Length > 0)
        {
            // Get the unit tactics component
            UnitTactics _unit = _colliders[0].GetComponent<UnitTactics>();

            // Return the unit tactics component (could be null)
            return _unit;
        }

        // There were no colliders found, so there was no unit, so return null
        return null;
    }//end GetUnitOnTile
}