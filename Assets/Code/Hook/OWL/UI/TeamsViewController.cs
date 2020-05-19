using System;
using System.Collections;
using System.Collections.Generic;
using Hook.HXF;
using Hook.OWL;
using UnityEngine;

public class TeamsViewController : MonoBehaviour
{
    #region Constants

    private const int kObjectPoolSize = 25;
    
    #endregion
    
    #region Properties

    [SerializeField] private GameObject TeamGridElementPrefab;
    [SerializeField] private Transform TeamGridElementContainer;

    private OWLData _dataSource;
    private List<TeamData> _data;
    private List<TeamGridElementController> _gridElements;
    private ObjectPool _pool;
    
    #endregion
    
    #region MonoBehaviour

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnDestroy()
    {
        TeamGridElementController.OnTeamSelectedEvent -= OnTeamSelected;
        TeamGridElementController.OnPlayerSelectedEvent -= OnPlayerSelected;
    }

    #endregion
    
    #region Class Methods

    public void Intialize(OWLData dataSource, List<TeamData> teamsData)
    {
        // saving reference to OWLData
        _dataSource = dataSource;
        
        // creating object pool
        _pool = new ObjectPool(TeamGridElementPrefab, kObjectPoolSize);
        
        // saving TeamData, creating grid elements
        _data = teamsData;
        _data.ForEach((team) => { CreateTeamGridElement(team); });
        
        // adding listeners
        TeamGridElementController.OnTeamSelectedEvent += OnTeamSelected;
        TeamGridElementController.OnPlayerSelectedEvent += OnPlayerSelected;
    }

    private void CreateTeamGridElement(TeamData team)
    {
        // getting prefab instance from object pool
        var gridElement = _pool.GetObject();
        gridElement.transform.SetParent(TeamGridElementContainer);
        gridElement.name = team.TeamName;
        var gridController = gridElement.GetComponent<TeamGridElementController>();
        gridController.Initialize(team);
    }

    private void CreatePlayerGridElement(PlayerProfileData player)
    {
        // getting prefab instance from object pool
        var gridElement = _pool.GetObject();
        gridElement.transform.SetParent(TeamGridElementContainer);
        gridElement.name = player.PlayerName;
        var gridController = gridElement.GetComponent<TeamGridElementController>();
        gridController.Initialize(player);
    }
    
    #endregion
    
    #region Event Handlers

    private void OnBackSelected()
    {
        
    }

    private void OnPlayerSelected(object sender, TeamGridElementEventArgs e)
    {
        // TODO initialize stats controller, display stats view
    }
    
    private void OnTeamSelected(object sender, TeamGridElementEventArgs e)
    {
        _pool.ReturnAllObjects();
        
        e.SelectedTeam.Roster.Players.ForEach((player) =>
        {
            CreatePlayerGridElement(player);
        });
    }
    
    #endregion
}