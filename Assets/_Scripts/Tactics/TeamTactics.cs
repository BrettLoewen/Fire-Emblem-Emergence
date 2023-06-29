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

    #endregion
}