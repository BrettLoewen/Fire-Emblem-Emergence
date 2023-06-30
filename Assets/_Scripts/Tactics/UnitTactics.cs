using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to represent, control, and reference units
/// </summary>
public class UnitTactics: MonoBehaviour
{
    #region Variables

    // Movement
    [SerializeField] private int movement = 5;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float turnTime = 0.33f;
    private bool isMoving;
    private Stack<Tile> path;
    private Vector3 nextTilePosition;
    private Vector3 endTilePosition;
    private float stoppingDistance = 0.2f;

    private Unit unit;
    private TeamTactics teamTactics;

    // Stats
    private int currentHealth;
    private int maxHealth;
    private int strength;
    private int defense;

    public bool HasActed { get; set; } = true;

    [SerializeField] private Slider healthBar;

    [SerializeField] private UnitAnimator animator;
    [SerializeField] private UnitCustomizer customizer;

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

    #region Unit/Turn Management

    /// <summary>
    /// Store references to the unit this UnitTactics object represents and the TeamTactics it belongs to.
    /// Setup the UnitCustomizer to match the appearance of the unit
    /// </summary>
    /// <param name="_unit"></param>
    /// <param name="_team"></param>
    public void Setup(Unit _unit, TeamTactics _team)
    {
        // Store the references
        unit = _unit;
        teamTactics = _team;

        // Setup the stats
        movement = unit.UnitData.Movement;
        strength = unit.UnitData.Strength;
        defense = unit.UnitData.Defense;
        maxHealth = unit.UnitData.Health;
        currentHealth = maxHealth;

        // Update the health bar to reflect the new health stats
        UpdateHealthbar();

        // Setup the customizer to use the unit's appearance
        customizer.SetCustomization(unit.UnitData.Customization);
    }//end Setup

    /// <summary>
    /// Returns true if this unit belongs to the player's TeamTactics and false otherwise
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerUnit()
    {
        // Returns true if this unit belongs to the player's TeamTactics and false otherwise
        return teamTactics.IsPlayer();
    }//end IsPlayerUnit

    /// <summary>
    /// Mark that this unit has finished acting and tell the TeamTactics that a unit has finished acting
    /// </summary>
    private void FinishActing()
    {
        HasActed = true;
        teamTactics.UnitFinishedActing();
    }//end FinishActing

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

            // End the unit's turn
            FinishActing();
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

    /// <summary>
    /// Used to update the unit's health bar which displays the unit's current health
    /// </summary>
    private void UpdateHealthbar()
    {
        // Update the relevant values of the health bar slider with the unit's health variables
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }//end UpdateHealthbar
}