using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTactics: MonoBehaviour
{
    #region Variables

    [SerializeField] private int movement = 5;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float turnTime = 0.33f;

    private bool isMoving;
    private Stack<Tile> path;
    private Vector3 nextTilePosition;
    private Vector3 endTilePosition;

    private float stoppingDistance = 0.2f;

    [SerializeField] private UnitAnimator animator;

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
        // If the unit should be moving, move
        if(isMoving)
        {
            Move();
        }
        // If the unit should not be moving, stop animating for movement
        else
        {
            // Tell the animator that the unit is not moving
            // (Done here and not in `Move` to allow for smoothly updating the float)
            animator.SetSpeedPercent(0f);
        }
    }//end Update

    #endregion //end Unity Control Methods

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

    #region Movement

    /// <summary>
    /// Given a path of tiles and an end tile, start moving the unit along the path
    /// </summary>
    /// <param name="_path">The path of tiles from the unit's current tile to the end tile</param>
    /// <param name="_targetTile">The tile at the end of the path</param>
    public void StartMove(Stack<Tile> _path, Tile _targetTile)
    {
        // Store the passed path of tiles
        path = _path;

        // Flag that movement has started
        isMoving = true;

        // Calculate the necessary starting values for moving
        nextTilePosition = path.Pop().transform.position;
        endTilePosition = _targetTile.transform.position;
    }//end StartMove

    /// <summary>
    /// Move the unit along the path until the path ends
    /// </summary>
    private void Move()
    {
        // If the unit has not reached the next tile, keep moving towards the next tile
        if (Vector3.Distance(nextTilePosition, transform.position) > stoppingDistance)
        {
            MoveUnit();
        }
        // If the unit has reached the next tile but it is not the last tile, update which tile is next
        else if (Vector3.Distance(endTilePosition, transform.position) > stoppingDistance)
        {
            // Update which tile is next
            nextTilePosition = path.Pop().transform.position;

            // Keep the unit moving to avoid juttery movement where the unit stops for 1 frame
            MoveUnit();

            // Get the movement direction and trigger the unit to rotate in that direction
            Vector3 moveDirection = (nextTilePosition - transform.position).normalized;
            float _targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.LeanRotateY(_targetAngle, turnTime);
        }
        // If the unit has reached the last tile, stop moving
        else
        {
            // Flag that movement is done
            isMoving = false;
        }
    }//end Move

    /// <summary>
    /// Used to move the unit to the next tile position
    /// </summary>
    private void MoveUnit()
    {
        // Get the direction the unit needs to move in
        Vector3 moveDirection = (nextTilePosition - transform.position).normalized;

        // Move the unit in the desired direction
        transform.position += moveDirection * Time.deltaTime * moveSpeed;

        // Tell the animator that the unit is moving
        animator.SetSpeedPercent(1f);
    }//end MoveUnit

    #endregion
}