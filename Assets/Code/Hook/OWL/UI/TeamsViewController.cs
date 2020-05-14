using System.Collections;
using System.Collections.Generic;
using Hook.OWL;
using UnityEngine;

public class TeamsViewController : MonoBehaviour
{
    #region Properties

    [SerializeField] private GameObject TeamGridElementPrefab;
    [SerializeField] private Transform TeamGridElementContainer;

    private List<TeamData> _data;
    
    #endregion
    
    #region MonoBehaviour
    
    void Start()
    {
    }

    void Update()
    {
    }
    
    #endregion
    
    #region Class Methods

    public void Intialize(List<TeamData> teamsData)
    {
        _data = teamsData;
        _data.ForEach((team) => { CreateTeamGridElement(team); });
        //CreateTeamGridElement(_data[0]);
    }

    private void CreateTeamGridElement(TeamData team)
    {
        // creating prefab instance
        var gridElement = Instantiate(TeamGridElementPrefab, TeamGridElementContainer);
        gridElement.name = team.TeamName;
        var gridController = gridElement.GetComponent<TeamGridElementController>();
        gridController.Initialize(team);
    }
    
    #endregion
}