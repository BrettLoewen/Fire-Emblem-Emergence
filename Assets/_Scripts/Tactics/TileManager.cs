using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the generation and management of the tilemap.
/// Note: The gameobject holding this component should be positioned where the 0,0 (bottom left) tile should go
/// </summary>
public class TileManager: Singleton<TileManager>
{
    #region Variables

    // Tilemap generation variables
    [SerializeField] private Tile tilePrefab;
    [Tooltip("The number of tiles in the x and z directions")]
    [SerializeField] private Vector2Int tilemapDimensions;
    [Tooltip("The length of a tile (if the tile's scale is 1.5, this value should be 1.5)")]
    [SerializeField] private float sizeOfTile;

    private Tile[] tiles;

    // Tile display materials
    [SerializeField] private Material tileMaterialWall;
    [SerializeField] private Material tileMaterialWalkable;
    [SerializeField] private Material tileMaterialAttackable;
    [SerializeField] private Material tileMaterialNothing;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override void Awake()
    {
        base.Awake();

        // Get a reference to all tiles
        GetAllTiles();

        // Calculate the links between the tiles
        CalculateTileLinks();

        // Ensure the pathfinding variables are correct
        ResetPathfinding();
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        
    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end Update

    #endregion //end Unity Control Methods

    #region Tilemap Control

    /// <summary>
    /// Uses the tilemap dimensions to spawn every tile needed
    /// </summary>
    [ContextMenu("Tilemap/Generate Tilemap")]
    private void GenerateTilemap()
    {
        // Ensure the old tilemap gets destroyed before the new one is created
        DestroyTilemap();

        // Create tiles according to the set tilemap dimensions
        for (int x = 0; x < tilemapDimensions.x; x++)
        {
            for (int z = 0; z < tilemapDimensions.y; z++)
            {
                // Create the new tile
                Tile _tile = Instantiate(tilePrefab, transform);

                // Calculate the correct position of the tile
                float _xPos = (x * sizeOfTile) + transform.position.x;
                float _zPos = (z * sizeOfTile) + transform.position.z;

                // Move the tile to its correct position
                _tile.transform.position = new Vector3(_xPos, transform.position.y, _zPos);

                // Set the tile's name according to its coordinates
                _tile.name = $"{x} {z}";
            }
        }
    }//end GenerateTilemap

    /// <summary>
    /// Destroys every tile in the tilemap
    /// </summary>
    [ContextMenu("Tilemap/Destroy Tilemap")]
    private void DestroyTilemap()
    {
        // Destroy every child object of the tile manager
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }//end DestroyTilemap

    /// <summary>
    /// Make every tile display their tile types (wall, walkable, etc.) visually. 
    /// Note: This can only be called while in Play mode
    /// </summary>
    [ContextMenu("Tilemap/Display Types")]
    private void DisplayTileTypes()
    {
        // For every tile in the tilemap...
        foreach (Transform child in transform)
        {
            // Get the tile
            Tile _tile = child.GetComponent<Tile>();

            // Make the tile display its status
            _tile.DisplayTileStatus();
        }
    }//end DisplayTileTypes

    #endregion

    #region Tilemap Boundary Checking

    /// <summary>
    /// Returns whether or not the passed vector3 position is within the bounds of the tilemap
    /// </summary>
    /// <param name="_position"></param>
    /// <returns>Returns true if the position is in bounds and false otherwise</returns>
    public bool PositionInTilemapBounds(Vector3 _position)
    {
        // Calcalate the bounds of the tilemap
        float _halfSizeOfTile = (sizeOfTile / 2f);
        float _minX = transform.position.x - _halfSizeOfTile;
        float _minZ = transform.position.z - _halfSizeOfTile;
        float _maxX = transform.position.x + _halfSizeOfTile + (sizeOfTile * (tilemapDimensions.x - 1));
        float _maxZ = transform.position.z + _halfSizeOfTile + (sizeOfTile * (tilemapDimensions.y - 1));

        // If the passed position is outside of the bounds (less than a minimum or greater than a maximum), return false
        if(_position.x < _minX || _position.x > _maxX || _position.z < _minZ || _position.z > _maxZ)
        {
            return false;
        }
        // If the passed position in inside the bounds, return true
        else
        {
            return true;
        }
    }//end PositionInTilemapBounds

    #endregion

    #region Tilemap Pathfinding

    /// <summary>
    /// Get and store all the tiles in the tilemap
    /// </summary>
    private void GetAllTiles()
    {
        // Get and store all the tiles in the scene
        tiles = FindObjectsOfType<Tile>();
    }//end GetAllTiles

    /// <summary>
    /// Tell every tile in the tilemap to generate its links to nearby tiles
    /// </summary>
    private void CalculateTileLinks()
    {
        // Calculate the tile linkes for each tile in the tilemap
        foreach (Tile _tile in tiles)
        {
            _tile.CalculateTileLinks();
        }
    }//end CalculateTileLinks

    /// <summary>
    /// Tell every tile in the tilemap to reset their pathfinding variables
    /// </summary>
    public void ResetPathfinding()
    {
        // Reset the pathfinding variables on each tile in the tilemap
        foreach (Tile _tile in tiles)
        {
            _tile.ResetPathfinding();
        }
    }//end ResetPathfinding

    /// <summary>
    /// Calculate which tiles a unit could walk to from the starting tile that is within the given movement amount
    /// </summary>
    /// <param name="_startingTile">The tile to start searching from (the tile the unit is standing on)</param>
    /// <param name="_movementAmount">The distance to search for (the distance the unit can move)</param>
    /// <returns>Returns a list of tiles containing every tile that can be walked to given the passed information</returns>
    public List<Tile> CalculateWalkableTiles(Tile _startingTile, int _movementAmount)
    {
        // Create a list to hold the valid walkable tiles so they can be returned
        List<Tile> walkableTiles = new List<Tile>();

        // Ensure the pathfinding variables are fresh so the results are accurate
        ResetPathfinding();

        // Create a queue which new valid tiles will be added to in before being fully checked
        Queue<Tile> _tilesToCheck = new Queue<Tile>();

        // Add the starting tile to the walkable tiles list
        _tilesToCheck.Enqueue(_startingTile);

        // Mark the starting tile as visited by the algorithm
        _startingTile.Visited = true;

        // While there are still tiles that need to be checked
        while (_tilesToCheck.Count > 0)
        {
            // Get the tile that has been in the queue the longest
            Tile _tileToCheck = _tilesToCheck.Dequeue();

            // If the tile's distance is within the walking distance of the unit AND either the tile is not obstructed OR the tile is the starting tile
            // If the tile can be walked to and can be stood on
            if (_tileToCheck.Distance <= _movementAmount && (_tileToCheck.CheckWalkable() || _tileToCheck.Equals(_startingTile)))
            {
                // The tile being checked is valid, so add it to the list of walkable tiles
                walkableTiles.Add(_tileToCheck);

                // Get the valid tile's linked tiles to check them
                foreach (Tile _linkedTile in _tileToCheck.LinkedTiles)
                {
                    // Get the walking distance to this linked tile
                    float _distance = _tileToCheck.Distance + 1;

                    // If the linked tile has not already been visited by the algorithm OR the new path for this tile is faster than the old one
                    if (_linkedTile.Visited == false || _linkedTile.Distance > _distance)
                    {
                        // Give the linked tile the path that leads to it
                        _linkedTile.Parent = _tileToCheck;

                        // Mark the linked tile as visited by the algorithm
                        _linkedTile.Visited = true;

                        // Record the walking distance to the linked tile 
                        _linkedTile.Distance = _distance;

                        // Add the linked tile to the queue for further validation later
                        _tilesToCheck.Enqueue(_linkedTile);
                    }
                }
            }
        }

        // Return a list containing all of the valid tiles that could be walked to
        return walkableTiles;
    }//end CalculateWalkableTiles

    #endregion

    #region Attackable Tile Calculation

    /// <summary>
    /// Calculate the tiles a unit could attack given their walkable tiles and the range of their attack
    /// </summary>
    /// <param name="_walkableTiles"></param>
    /// <param name="_range"></param>
    /// <returns>Returns a list of tiles a unit could attack</returns>
    public List<Tile> CalculateAttackableTiles(List<Tile> _walkableTiles, int _range)
    {
        // Create a list to hold the list of attackable tiles
        List<Tile> _attackableTiles = new List<Tile>();

        // Loop through every walkable and add it to the list of attackable tiles
        // It is guaranteed that every tile that can be walked to could be attacked by the previous tile on the path
        foreach (Tile _tile in _walkableTiles)
        {
            // Verify that no tiles are added twice
            if(_attackableTiles.Contains(_tile) == false)
            {
                _attackableTiles.Add(_tile);
            }
        }

        // Extend away from the walkable tiles to get the nearby tiles that fall within attack range
        foreach (Tile _tile in _walkableTiles)
        {
            // Get the tile's neighbouring tiles (adjacent and further out based on range)
            List<Tile> _nearbyTiles = CalculateNearbyTiles(_tile, 1, _range);

            // Loop through each of the tile's neighbours
            foreach (Tile _nearbyTile in _nearbyTiles)
            {
                // Verify that no tiles are added twice
                if (_attackableTiles.Contains(_nearbyTile) == false)
                {
                    // Tell the neighbouring tile which walkable tile can attack it
                    _nearbyTile.Parent = _tile;

                    // Add the tile
                    _attackableTiles.Add(_nearbyTile);
                }
            }
        }

        // Return the list of attackable tiles
        return _attackableTiles;
    }

    /// <summary>
    /// A recursive search function used to find the tiles that are within a search range.
    /// Given a starting tile and depth, it will keep searching through linked tiles and return
    /// a list of all the tiles it found
    /// </summary>
    /// <param name="_startingTile"></param>
    /// <param name="_depth"></param>
    /// <param name="_maxDepth"></param>
    /// <returns></returns>
    private List<Tile> CalculateNearbyTiles(Tile _startingTile, int _depth, int _maxDepth)
    {
        // Create a list to hold the list of attackable tiles
        List<Tile> _attackableTiles = new List<Tile>();

        // Loop through each tile that is adjacent to the starting tile
        foreach (Tile _tile in _startingTile.LinkedTiles)
        {
            // Verify that no tiles are added/processed twice
            if (_attackableTiles.Contains(_tile) == false)
            {
                _attackableTiles.Add(_tile);

                // If the depth of the recursive search (range of the attack) has not been exceeded...
                if (_depth < _maxDepth)
                {
                    // Recursively get this tile's neighbouring tiles (adjacent and further out based on range)
                    List<Tile> _nearbyTiles = CalculateNearbyTiles(_tile, _depth + 1, _maxDepth);

                    // Loop through each of the neighbouring tiles and add them
                    foreach (Tile _nearbyTile in _nearbyTiles)
                    {
                        // Verify that no tiles are added twice
                        if (_attackableTiles.Contains(_nearbyTile) == false)
                        {
                            _attackableTiles.Add(_nearbyTile);
                        }
                    }
                }
            }
        }

        // Return the list of attackable tiles
        return _attackableTiles;
    }//end CalculateNearbyTiles

    #endregion

    #region Tile Materials

    /// <summary>
    /// Return the defined material that a tile should display if its type is Wall
    /// </summary>
    /// <returns></returns>
    public static Material GetTileMaterialWall()
    {
        return Instance.tileMaterialWall;
    }//end GetTileMaterialWall

    /// <summary>
    /// Return the defined material that a tile should display if its not displaying anything
    /// </summary>
    /// <returns></returns>
    public static Material GetTileMaterialNothing()
    {
        return Instance.tileMaterialNothing;
    }//end GetTileMaterialNothing

    /// <summary>
    /// Return the defined material that a tile should display if its Walkable
    /// </summary>
    /// <returns></returns>
    public static Material GetTileMaterialWalkable()
    {
        return Instance.tileMaterialWalkable;
    }//end GetTileMaterialWalkable

    /// <summary>
    /// Return the defined material that a tile should display if its Attackable
    /// </summary>
    /// <returns></returns>
    public static Material GetTileMaterialAttackable()
    {
        return Instance.tileMaterialAttackable;
    }//end GetTileMaterialAttackable

    #endregion
}