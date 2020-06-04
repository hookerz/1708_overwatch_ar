using System;
using Hook.HXF;
using System.Collections.Generic;
using Hook.PlayerInput;
using UnityEngine;

namespace Hook.OWL
{
    public class OWLAppController : MonoBehaviour
    {
        #region Constants
        
        private const string kDatabaseName = "overwatch_stats_db.db";
        
        #endregion
        
        #region Properties

        [SerializeField] private TeamsRosterViewController teamsRosterController;
        [SerializeField] private PlayerViewController playerController;
        
        private OWLData _data;
    
        #endregion
        
        #region MonoBehaviour

        private void Awake()
        {
            OWLEvents.OnPlayerViewSelected += OnPlayerViewSelected;
            InputEvents.OnAndroidBackButtonDetected += OnAndroidBackButtonSelected;
        }

        void Start()
        {
            _data = new OWLData(kDatabaseName);
            
            var teams = _data.GetTeams();
            teamsRosterController.Intialize(_data, teams);
        }

        void Update()
        {
        }

        private void OnDestroy()
        {
            OWLEvents.OnPlayerViewSelected -= OnPlayerViewSelected;
            InputEvents.OnAndroidBackButtonDetected -= OnAndroidBackButtonSelected;
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
        
        #region Event Handlers

        private void OnAndroidBackButtonSelected(object sender, EventArgs e)
        {
        }

        private void OnPlayerViewSelected(object sender, OWLEventArgs e)
        {
            // TODO perform transition to player view here
            
            // hiding team/roster view
            teamsRosterController.gameObject.SetActive(false);
            
            // displaying player view
            playerController.gameObject.SetActive(true);
            playerController.Initialize(_data, e.SelectedPlayer);
        }
        
        #endregion
    }
}