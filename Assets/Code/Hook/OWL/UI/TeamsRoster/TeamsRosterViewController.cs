using DG.Tweening;
using Hook.HXF;
using System.Collections.Generic;
using UnityEngine;

namespace Hook.OWL
{
    public class TeamsRosterViewController : MonoBehaviour
    {
        #region Constants

        private const int kObjectPoolSize = 25;
        private const float kDelayBetweenAnimations = 0.05f;
        
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
            
            // getting TeamGridElementController
            var gridController = gridElement.GetComponent<TeamGridElementController>();
            gridController.Initialize(player);
        }

        private Sequence Transition(bool shouldEnter, float delay)
        {
            var startAlpha = shouldEnter ? 0 : 1f;
            var endAlpha = shouldEnter ? 1 : 0f;
            var newScale = Vector3.one * (shouldEnter ? 1f : 1.2f);
            var elements = TeamGridElementContainer.GetComponentsInChildren<TeamGridElementController>();
            var newSequence = DOTween.Sequence();
            var index = 0;
            foreach (var element in elements)
            {
                var canvasGroup = element.GetComponent<CanvasGroup>();
                var alphaTween = DOVirtual.Float(startAlpha, endAlpha, .25f, alphaLevel => { canvasGroup.alpha = alphaLevel; });
                var scaleTween = element.transform.DOScale(newScale, .15f);
                newSequence.Insert(index * delay, alphaTween);
                newSequence.Insert(index * delay, scaleTween);
                index++;
            }

            newSequence.OnStart(() => { Debug.LogFormat("{0} starting...", shouldEnter ? "ENTER" : "EXIT"); });

            return newSequence;
        }
        
        #endregion
        
        #region Event Handlers

        private void OnBackSelected()
        {
            
        }

        private void OnPlayerSelected(object sender, TeamGridElementEventArgs e)
        {
            if (OWLEvents.OnPlayerViewSelected != null)
            {
                OWLEvents.OnPlayerViewSelected(this, new OWLEventArgs(e.SelectedPlayer));
            }
        }
        
        private void OnTeamSelected(object sender, TeamGridElementEventArgs e)
        {
            var newSequence = Transition(false, kDelayBetweenAnimations);
            newSequence.OnComplete(() =>
            {
                _pool.ReturnAllObjects();
            
                e.SelectedTeam.Roster.Players.ForEach((player) =>
                {
                    CreatePlayerGridElement(player);
                });
                
                Transition(true, .075f);
            });
        }
        
        #endregion
    }
}