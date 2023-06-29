using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTactics: TeamTactics
{
    #region Variables



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
    /// Returns true if this TeamTactics is the player's and false otherwise.
    /// Only a PlayerTactics can be the player's TeamTactics, so this just returns true
    /// </summary>
    /// <returns></returns>
    public override bool IsPlayer()
    {
        // Only a PlayerTactics can be the player's TeamTactics, so this just returns true
        return true;
    }//end IsPlayer

    #endregion
}