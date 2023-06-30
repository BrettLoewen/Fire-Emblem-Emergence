using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class used to create the units for a team in the tactics scene and manage them
/// </summary>
public class TeamTactics: MonoBehaviour
{
    #region Variables

    [SerializeField] private UnitTactics unitTacticsPrefab;

    protected List<UnitTactics> units;

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
    /// Spawn the passed units at the given spawn points. Store these new unit tactics objects for later
    /// </summary>
    /// <param name="_units"></param>
    /// <param name="_spawnPoints"></param>
    public void Setup(List<Unit> _units, Transform[] _spawnPoints)
    {
        // Initialize the list of unit tactics
        units = new List<UnitTactics>();

        // Loop through every unit (stop early if the number of spawn points is exceeded)
        for (int i = 0; i < _units.Count && i < _spawnPoints.Length; i++)
        {
            // Spawn the unit tactics object
            UnitTactics _unitTactics = Instantiate(unitTacticsPrefab, transform);

            // Move/rotate the unit tactics object to match its spawn point
            _unitTactics.transform.SetPositionAndRotation(_spawnPoints[i].position, _spawnPoints[i].rotation);

            // Setup the unit tactics object with its unit data
            _unitTactics.Setup(_units[i], this);

            // Store the unit tactics object for later
            units.Add(_unitTactics);
        }
    }//end Setup

    /// <summary>
    /// Returns true if this TeamTactics is the player's and false otherwise.
    /// Only a PlayerTactics can be the player's TeamTactics, so this just returns false
    /// </summary>
    /// <returns></returns>
    public virtual bool IsPlayer()
    {
        // Only a PlayerTactics can be the player's TeamTactics, so this just returns false
        return false;
    }//end IsPlayer

    /// <summary>
    /// Start this TeamTactics' turn. Refresh the units and make them act. For the base implementation, 
    /// the turn is skipped by invoking the EndTurn method early.
    /// </summary>
    public virtual void StartTurn()
    {
        // Tell each unit that they can perform an action
        RefreshUnits();

        // End the turn in 1 second
        Invoke("EndTurn", 1f);
    }//end StartTurn

    /// <summary>
    /// Tell each unit that they can perform an action. Exists so different implementations of StartTurn can call it
    /// since StartTurn is meant to be completely overridden (no base.StartTurn() allowed)
    /// </summary>
    protected void RefreshUnits()
    {
        // Tell each unit that they can perform an action
        foreach (UnitTactics _unit in units)
        {
            _unit.HasActed = false;
        }
    }//end RefreshUnits

    /// <summary>
    /// Called by UnitTactics when a unit finishs acting. When this happens, check if all units have finished acting.
    /// If they have all finished, end the turn
    /// </summary>
    public void UnitFinishedActing()
    {
        // Check if all units have finished acting
        foreach (UnitTactics _unit in units)
        {
            // If a unit has not acted yet, return
            if(_unit.HasActed == false)
            {
                return;
            }
        }

        // If this line was reached, every unit has acted, so this TeamTactics' turn is done, so end the turn
        EndTurn();
    }//end UnitFinishedActing


    public void UnitDied(UnitTactics _unit)
    {
        // Remove this unit from the list of units
        units.Remove(_unit);

        // If this team has no more units, this team has lost, so tell the game manager this
        if(units.Count <= 0)
        {
            // Tell the game manager that the battle is over
            // If this was the player's team, pass false since the player did not win
            // If this was not the player's team, pass true since the player did win
            TacticsGameManager.Instance.EndGame(!IsPlayer());
        }
    }

    /// <summary>
    /// End this TeamTactics' turn and tell the game manager to move to the next TeamTactics' turn
    /// </summary>
    private void EndTurn()
    {
        // Tell the game manager to move to the next TeamTactics' turn
        TacticsGameManager.Instance.NextTurn();
    }//end EndTurn

    #endregion
}