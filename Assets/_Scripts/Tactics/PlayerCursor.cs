using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the player to move around the tactics scene and interact with units
/// </summary>
public class PlayerCursor: MonoBehaviour
{
    #region Variables

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float timeToRotate = 0.5f;
    [SerializeField] private Camera Camera;
    private bool isRotating;

    [SerializeField] private float tileDetectRadius = 0.5f;
    [SerializeField] private LayerMask tileDetectMask;

    private Tile selectedTile;
    private Tile prevSelectedTile;

    private UnitTactics selectedUnit;
    private Tile selectedUnitTile;

    private List<Tile> walkableTiles;
    private List<Tile> attackableTiles;

    [SerializeField] private PathRenderer pathRenderer;

    private PlayerInputHandler inputHandler;

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }//end Awake

    // Start is called before the first frame update
    void Start()
    {
        
    }//end Start

    // Update is called once per frame
    void Update()
    {
        // Move and rotate the cursor according to player input
        MoveCursor();
        RotateCursor();

        // Calculate which tile the cursor is currently selecting
        DetermineSelectedTile();

        // Display the selected tile
        DisplaySelectedTile();

        // Display the path renderer (when appropriate)
        HandlePathRenderer();

        // Select and command units
        HandleUnitCommanding();
    }//end Update

    #endregion //end Unity Control Methods

    #region Cursor Control

    /// <summary>
    /// Move the cursor according to player input relatively to the camera
    /// </summary>
    private void MoveCursor()
    {
        // Get the direction of movement
        Vector2 _moveInput = inputHandler.MoveInput;
        Vector3 _direction = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;

        // Only move if there is movement input
        if (_direction.magnitude >= 0.1f)
        {
            // Get and smooth the angle for the movement direction relative to the camera
            float _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + Camera.transform.eulerAngles.y;

            // Turn the move angle into a new movement direction
            Vector3 _moveDir = Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward;

            // Calculate the position that the cursor is trying to move to
            Vector3 _newPosition = transform.position + (_moveDir.normalized * moveSpeed * Time.deltaTime);

            // Determine whether or not the cursor's new position is within the tilemap boundaries
            bool _newPositionInBounds = TileManager.Instance.PositionInTilemapBounds(_newPosition);

            // Only allow the cursor to move if it will remain within the tilemap boundaries
            if(_newPositionInBounds)
            {
                // Move the cursor
                transform.LeanMove(_newPosition, Time.deltaTime);
            }
        }
    }//end MoveCursor

    /// <summary>
    /// Rotate the cursor according to player input
    /// </summary>
    private void RotateCursor()
    {
        // If the player inputted to rotate the camera right...
        if (inputHandler.LookRightInput)
        {
            // Cancel the input
            inputHandler.LookRightInput = false;

            // Only start a rotation if the cursor is not already rotating
            if (isRotating == false)
            {
                // Trigger the rotation, record that a rotation is occuring, and trigger a delayed rotation end
                transform.LeanRotateY(transform.eulerAngles.y - 45, timeToRotate);
                isRotating = true;
                Invoke("EndRotate", timeToRotate);
            }
        }

        // If the player inputted to rotate the camera left...
        if (inputHandler.LookLeftInput)
        {
            // Cancel the input
            inputHandler.LookLeftInput = false;

            // Only start a rotation if the cursor is not already rotating
            if (isRotating == false)
            {
                // Trigger the rotation, record that a rotation is occuring, and trigger a delayed rotation end
                transform.LeanRotateY(transform.eulerAngles.y + 45, timeToRotate);
                isRotating = true;
                Invoke("EndRotate", timeToRotate);
            }
        }
    }//end RotateCursor

    /// <summary>
    /// Used to end cursor rotation after a delay
    /// </summary>
    private void EndRotate()
    {
        isRotating = false;
    }//end EndRotate

    #endregion

    #region Selected Tile

    /// <summary>
    /// Calculate which tile the cursor is currently over and store that tile as the currently selected tile
    /// </summary>
    private void DetermineSelectedTile()
    {
        // Get all the tile colliders that the cursor is near
        Collider[] _colliders = Physics.OverlapSphere(transform.position, tileDetectRadius, tileDetectMask);

        // If there is at least one tile that could be selected...
        if(_colliders.Length > 0)
        {
            // Search for the nearest tile so it can be selected
            // Start the search with the first collider found
            Collider _nearest = _colliders[0];
            float _shortestDistance = Helpers.FlatDistance(transform.position, _nearest.transform.position);

            // Check all of the nearby tiles to see which one is closest to the cursor
            for (int i = 1; i < _colliders.Length; i++)
            {
                // Get the distance from the cursor to the tile
                float _distance = Helpers.FlatDistance(transform.position, _colliders[i].transform.position);

                // If the tile is closer to the cursor than the previous best option, store it
                if (_distance < _shortestDistance)
                {
                    _nearest = _colliders[i];
                    _shortestDistance = _distance;
                }
            }

            // Get the tile component from the nearest collider
            Tile _tile = _nearest.GetComponent<Tile>();

            // Ensure the tile component exists before continuing
            if(_tile != null)
            {
                // If there was no previously selected tile or the currently selected tile changed...
                if(prevSelectedTile == null || selectedTile != _tile)
                {
                    // Update the previously selected tile
                    prevSelectedTile = selectedTile;
                }

                // Set the nearest tile to the cursor as the currently selected tile
                selectedTile = _tile;
            }
        }
    }//end DetermineSelectedTile

    /// <summary>
    /// Update the tile displays to properly display which tile is currently selected
    /// </summary>
    private void DisplaySelectedTile()
    {
        // Ensure a tile is selected
        if(selectedTile != null)
        {
            // Tell the selected tile that it has been selected
            selectedTile.SetIsSelected(true);
        }

        // Ensure a tile has been selected previously
        if(prevSelectedTile != null)
        {
            // Tell the previously selected tile it is no longer selected
            prevSelectedTile.SetIsSelected(false);
        }
    }//end DisplaySelectedTile

    #endregion

    #region Unit Selection

    /// <summary>
    /// If the player pressed Cancel, deselect the selected unit if one was selected.
    /// If the player pressed Select, either try to select a unit if none is selected 
    /// or try to move the selected unit to a new tile
    /// </summary>
    private void HandleUnitCommanding()
    {
        // If the player pressed cancel
        if(inputHandler.CancelInput)
        {
            // Cancel the input
            inputHandler.CancelInput = false;

            // If the player had selected a unit...
            if(selectedUnit != null)
            {
                // Deselect the unit
                DeselectUnit();
            }
        }

        // If the player pressed select
        if(inputHandler.InteractInput)
        {
            // Cancel the input
            inputHandler.InteractInput = false;

            // If the player has not selected a unit yet, try to select a unit
            if(selectedUnit == null)
            {
                SelectUnit();
            }
            // If the player has already selected a unit...
            else
            {
                // If the current tile is one the selected unit can walk to...
                if (walkableTiles.Contains(selectedTile))
                {
                    // Calculate the path to the selected tile that the unit will need to take
                    Stack<Tile> _path = CalculatePathToTile(selectedTile);

                    // Tell the unit to move according to the calculated path
                    selectedUnit.StartMove(_path, selectedTile);

                    // Deselect the unit
                    DeselectUnit();
                }
            }
        }
    }//end HandleUnitCommanding

    /// <summary>
    /// Try to get a unit from the currently selected tile and, if successful, select that unit
    /// </summary>
    private void SelectUnit()
    {
        // Get the unit that is standing on the tile the cursor is currently hovering over
        UnitTactics _unit = selectedTile.GetUnitOnTile();

        // If a unit was found, that unit belongs to the player, and that unit has not acted yet...
        if (_unit != null && _unit.IsPlayerUnit() && _unit.HasActed == false)
        {
            // Select that unit
            selectedUnit = _unit;

            // Start the selection visual
            selectedUnitTile = selectedTile;
            selectedUnitTile.SetUnitSelected(true);

            // Calculate the walkable tiles for the selected unit
            walkableTiles = selectedUnit.GetWalkableTiles(selectedUnitTile);

            // Display which tiles are walkable and which ones are not
            foreach (Tile _tile in walkableTiles)
            {
                _tile.SetIsWalkable();
            }

            // Calculate the attackable tiles for the selected unit
            attackableTiles = selectedUnit.GetAttackableTiles(walkableTiles);

            // Display which tiles are attackable and which ones are not
            foreach (Tile _tile in attackableTiles)
            {
                _tile.SetIsAttackable();
            }
        }
    }//end SelectUnit

    /// <summary>
    /// Null the selected unit and reset and selection indicators
    /// </summary>
    private void DeselectUnit()
    {
        // Deselect the unit
        selectedUnit = null;

        // Stop the selection visual
        selectedUnitTile.SetUnitSelected(false);
        selectedUnitTile = null;

        // Clear any pathfinding when the unit is deselected
        TileManager.Instance.ResetPathfinding();
    }//end DeselectUnit

    /// <summary>
    /// Trace the tiles parent heritage from the target tile backwards until the starting tile is found and return the path
    /// </summary>
    /// <param name="_endingTile">The target tile that the path should end at</param>
    /// <returns>Returns a stack of tiles representing the path of tiles from the unit's tile to the target tile</returns>
    public Stack<Tile> CalculatePathToTile(Tile _endingTile)
    {
        // Create a stack to hold the path
        Stack<Tile> _path = new Stack<Tile>();

        // Add the target tile to the stack
        _path.Push(_endingTile);

        // Start at the target tile
        Tile _current = _endingTile;

        // While the current tile has a parent, continue the back-trace
        // When the parent is null, the starting tile (unit's tile) has been reached
        while (_current.Parent != null)
        {
            // Add the current tile's parent tile to the stack
            _path.Push(_current.Parent);

            // Update the current tile to be that parent
            _current = _current.Parent;
        }

        // Return the calculated stack of tiles representing the path
        return _path;
    }//end CalculatePathToTile

    #endregion

    /// <summary>
    /// Display the walkable path from the selected unit to the selected tile when appropriate
    /// </summary>
    private void HandlePathRenderer()
    {
        // Ensure a unit is selected
        if(selectedUnit != null)
        {
            // Only update the displayed path if the selected unit and walk to the currently selected tile
            if(walkableTiles.Contains(selectedTile))
            {
                // Tell the path renderer to display the path from the selected unit to the selected tile
                pathRenderer.DisplayPath(CalculatePathToTile(selectedTile));
            }
        }
        // If a unit is not currently selected, do not display the path
        else
        {
            pathRenderer.Disable();
        }
    }//end HandlePathRenderer
}