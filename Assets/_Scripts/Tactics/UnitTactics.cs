using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTactics: MonoBehaviour
{
    #region Variables

    [SerializeField] private int movement = 5;

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
        
    }//end Update

    #endregion //end Unity Control Methods

    #region

    /// <summary>
    /// Get and return a list of tiles this unit can walk to
    /// </summary>
    /// <param name="_startingTile">The tile to start the search for walkable tiles from</param>
    /// <returns></returns>
    public List<Tile> GetWalkableTiles(Tile _startingTile)
    {
        // Ask the tile manager to calculate the walkable tiles and return the result
        return TileManager.Instance.CalculateWalkableTiles(_startingTile, movement);
    }//end GetWalkableTiles

    #endregion
}