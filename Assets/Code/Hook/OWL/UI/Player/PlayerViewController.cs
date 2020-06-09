using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Hook.HXF;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace Hook.OWL
{
    public class PlayerViewController : MonoBehaviour
    {
        #region Properties

        [SerializeField] private Image PlayerImage;
        [SerializeField] private TextMeshProUGUI OverwatchName;
        [SerializeField] private TextMeshProUGUI PlayerName;
        [SerializeField] private GameObject StatGridElementPrefab;
        [SerializeField] private Transform StatGridContainer;
        [SerializeField] private GameObject HeroGridElementPrefab;
        [SerializeField] private Transform HeroGridContainer;
        
        private OWLData _data;
        private List<HeroData> _heroes;
        private HeroData _heroTotals;
        
        #endregion
        
        #region MonoBehevaiour
        
        void Start()
        {
        }

        void Update()
        {
        }
        
        #endregion
        
        #region Class Methods

        public void Initialize(OWLData data, PlayerProfileData player)
        {
            // saving DB reference
            _data = data;
            
            // loading player image
            AssetLoader.LoadImage(player.ProfileImageUrl, PlayerImage);
            
            // setting Overwatch, player name
            OverwatchName.text = player.OverwatchName;
            PlayerName.text = player.PlayerName;
            
            // TODO getting battle stats for player
            
            // getting heroes used for player
            var heroesUsed = _data.GetAllHeroes(player.OverwatchName);
            _heroTotals = heroesUsed[heroesUsed.Count - 1];
            heroesUsed.Remove(_heroTotals);
            _heroes = heroesUsed;
            
            // TODO populate career stats view
            PopulateCareerStats();
            
            // TODO populate heroes used view
            PopulateHeroes(_heroes);
        }

        private void PopulateCareerStats()
        {
            // getting layout group dimensions
            var layoutGroup = StatGridContainer.GetComponent<GridLayoutGroup>();
            var height = layoutGroup.cellSize.y;
            
            // getting fields to create career stats for
            var fieldsToSkip = new String[] {"Hero", "UsageCount", "HeroImageUrl", "Percent"};
            var fields = _heroTotals.GetType().GetFields();
            
            // getting total height of stat grid
            var totalFields = fields.Length - fieldsToSkip.Length;
            var totalRows = Mathf.CeilToInt((float) totalFields / layoutGroup.constraintCount);
            var totalSpacing = (totalRows - 1) * layoutGroup.spacing.y;
            var totalHeight = height * totalRows + totalSpacing + layoutGroup.padding.top + layoutGroup.padding.bottom;
            
            // creating stat grid elements
            foreach (var f in fields)
            {
                var fieldName = f.Name;
                if (!fieldsToSkip.Contains(fieldName))
                {
                    var grid = Instantiate(StatGridElementPrefab, StatGridContainer);
                    var gridController = grid.GetComponent<StatGridElementController>();
                    gridController.Initialize(f.GetValue(_heroTotals).ToString(), fieldName, "");
                }
            }
            
            // updating stat container height
            var rectTransform = StatGridContainer.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, totalHeight);
        }
        
        private void PopulateHeroes(List<HeroData> heroes)
        {
            var layoutGroup = HeroGridContainer.GetComponent<VerticalLayoutGroup>();
            float height = layoutGroup.padding.top + layoutGroup.padding.bottom;
            float spacing = layoutGroup.spacing;
            
            // creating hero grid elements
            heroes.ForEach(hero =>
            {
                var grid = Instantiate(HeroGridElementPrefab);
                var gridController = grid.GetComponent<HeroGridElementController>();
                gridController.Initialize(hero);
                grid.transform.SetParent(HeroGridContainer);
                height += (spacing + grid.GetComponent<RectTransform>().rect.height);
            });
            
            // updating hero container height
            var rectTransform = HeroGridContainer.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, height);
        }

        private void CreateStatGridElement(string statValue, string statLabel, string detailStat)
        {
            
        }
        
        #endregion
        
        #region Event Handlers

        public void OnOverviewSelected()
        {
            
        }

        public void OnStatisticsSelected()
        {
            
        }
        
        #endregion
    }
}