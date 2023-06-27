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

    private Transform[][] tiles;

    // Tile display materials
    [SerializeField] private Material tileMaterialWall;
    [SerializeField] private Material tileMaterialWalkable;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override void Awake()
    {
        base.Awake();
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

    #region

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

    /// <summary>
    /// Return the defined material that a tile should display if its type is Wall
    /// </summary>
    /// <returns></returns>
    public static Material GetTileMaterialWall()
    {
        return Instance.tileMaterialWall;
    }//end GetTileMaterialWall

    /// <summary>
    /// Return the defined material that a tile should display if its type is Walkable
    /// </summary>
    /// <returns></returns>
    public static Material GetTileMaterialWalkable()
    {
        return Instance.tileMaterialWalkable;
    }//end GetTileMaterialWalkable
}