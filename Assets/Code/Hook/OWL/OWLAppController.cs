using System.Collections.Generic;
using UnityEngine;

namespace Hook.OWL
{
    public class OWLAppController : MonoBehaviour
    {
        #region Constants
        
        private const string kDatabaseName = "overwatch_stats_db.db";
        
        #endregion
        
        #region Properties

        [SerializeField] private TeamsViewController TeamsController;
        
        private OWLData _data;
    
        #endregion
        
        #region MonoBehaviour
        
        void Start()
        {
            _data = new OWLData(kDatabaseName);
            
            var teams = _data.GetTeams();
            //teams.ForEach(team => Debug.LogFormat("[{0}] Roster: {1}", team.TeamName, team.Roster.Players.Count));
            TeamsController.Intialize(teams);
        }

        void Update()
        {
        }
        
        #endregion
        
        #region Class Methods

        private void LogResults(List<BattleStatsData> data)
        {
            data.ForEach(dataPoint =>
            {
                Debug.Log(dataPoint);
            });
        }
        
        #endregion
    }
}