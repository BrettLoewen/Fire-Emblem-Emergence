using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;

/// <summary>
/// Used to manage the tactics scene, turn cycle, and battle state
/// </summary>
public class TacticsGameManager: Singleton<TacticsGameManager>
{
    #region Variables

    [SerializeField] private TextMeshProUGUI turnDisplayText;

    [SerializeField] private Transform[] spawnPointsPlayer;
    [SerializeField] private Transform[] spawnPointsEnemy;

    [SerializeField] private UnitData[] enemyUnitDatas;

    [SerializeField] private TeamTactics teamTacticsPrefab;
    [SerializeField] private PlayerTactics playerTacticsPrefab;

    private Queue<TeamTactics> teams;

    public bool BattleOver { get; private set; }

    #endregion //end Variables

    #region Unity Control Methods

    // Awake is called before Start before the first frame update
    protected override async void Awake()
    {
        // Setup the base class singleton
        base.Awake();

        // In case the persistent scene is not loaded, load it
#pragma warning disable CS4014
        LevelManager.LoadPersistentScene(Scenes.Tactics);

        // Make sure the save system loads the current save file
        await SaveSystem.LoadCurrentSaveFile();
    }//end Awake

    // Start is called before the first frame update
    async void Start()
    {
        // Setup the player and enemy units and team tactics
        await SetupTeamTactics();

        await Task.Delay(100);

        // Close the loading screen
        await LevelManager.Instance.SetLoadingFinished();

        await Task.Delay(100);

        // Start the turn cycle
        NextTurn();
    }//end Start

    // Update is called once per frame
    void Update()
    {
        
    }//end Update

    #endregion //end Unity Control Methods

    #region

    /// <summary>
    /// Setup the player and enemy tactics objects
    /// </summary>
    /// <returns></returns>
    private async Task SetupTeamTactics()
    {
        // Wait until the save file has been loaded before using it to initialize the tactics scene
        while (SaveSystem.currentSaveData == null)
        {
            await Task.Yield();
        }

        // Create and setup the player tactics
        TeamTactics _playerTactics = Instantiate(playerTacticsPrefab, transform);
        _playerTactics.Setup(DataManager.GetUnits(), spawnPointsPlayer);

        // Create a list of the enemy units
        List<Unit> _enemyUnits = new List<Unit>();
        for (int i = 0; i < enemyUnitDatas.Length; i++)
        {
            _enemyUnits.Add(new Unit(enemyUnitDatas[i]));
        }

        // Create and setup the enemy tactics
        TeamTactics _enemyTactics = Instantiate(teamTacticsPrefab, transform);
        _enemyTactics.Setup(_enemyUnits, spawnPointsEnemy);

        // Initialize the queue of TeamTactics and add both newly created TeamTactics objects to the queue
        teams = new Queue<TeamTactics>();
        teams.Enqueue(_playerTactics);
        teams.Enqueue(_enemyTactics);

        // Wait some time to guarantee everything is setup
        await Task.Delay(100);
    }//end SetupTeamTactics

    /// <summary>
    /// Used to get the next TeamTactics from the team turn order queue and start its turn
    /// </summary>
    public void NextTurn()
    {
        // Get the TeamTactics at the front of the queue
        TeamTactics _activeTeam = teams.Dequeue();

        // Tell that TeamTactics object that it is their turn
        _activeTeam.StartTurn();

        // If it is the player's turn, display that
        if(_activeTeam.IsPlayer())
        {
            turnDisplayText.text = "Player's Turn";
        }
        // If it is the enemy's turn, display that
        else
        {
            turnDisplayText.text = "Enemy's Turn";
        }

        // Requeue the TeamTactics
        teams.Enqueue(_activeTeam);
    }//end NextTurn


    public void EndGame(bool playerWon)
    {
        // Mark the battle as over
        BattleOver = true;

        if(playerWon)
        {
            Debug.Log("Player won");
        }
        else
        {
            Debug.Log("Player lost");
        }
    }

    #endregion
}