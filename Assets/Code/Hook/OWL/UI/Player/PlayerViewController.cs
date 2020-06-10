using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
        [SerializeField] private TextMeshProUGUI CareerStatsDescription;
        [SerializeField] private GameObject StatGridElementPrefab;
        [SerializeField] private Transform StatGridContainer;
        [SerializeField] private GameObject HeroGridElementPrefab;
        [SerializeField] private Transform HeroGridContainer;
        [SerializeField] private TMP_Dropdown HeroMetricsDropdown;
        
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
            
            // populate career stats view
            CareerStatsDescription.text = _heroTotals.Hero;
            PopulateCareerStats(_heroTotals);
            
            // populate heroes used view
            PopulateHeroes(_heroes);
        }

        private void PopulateCareerStats(HeroData heroData)
        {
            // getting layout group dimensions
            var layoutGroup = StatGridContainer.GetComponent<GridLayoutGroup>();
            var height = layoutGroup.cellSize.y;
            
            // getting fields to create career stats for
            var fieldsToSkip = new String[] {"Hero", "UsageCount", "HeroImageUrl", "Percent"};
            var fields = heroData.GetType().GetFields();
            
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
                    gridController.Initialize(f.GetValue(heroData).ToString(), ParseCamelCase(fieldName), "");
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
                var grid = Instantiate(HeroGridElementPrefab, HeroGridContainer);
                var gridController = grid.GetComponent<HeroGridElementController>();
                gridController.Initialize(hero, hero.UsageCount);
                gridController.OnHeroGridElementSelected += OnHeroGridElementSelected;
                height += (spacing + grid.GetComponent<RectTransform>().rect.height);
            });
            
            // updating hero container height
            var rectTransform = HeroGridContainer.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, height);
        }

        private void UpdateCareerStats(HeroData heroData)
        {
            // getting all StatGridElementController instances
            var statGridElements = StatGridContainer.GetComponentsInChildren<StatGridElementController>();
            
            // getting fields to create career stats for
            var fieldsToSkip = new String[] {"Hero", "UsageCount", "HeroImageUrl", "Percent"};
            var fields = heroData.GetType().GetFields();
            var index = 0;
            
            // creating stat grid elements
            foreach (var f in fields)
            {
                var fieldName = f.Name;
                if (!fieldsToSkip.Contains(fieldName))
                {
                    var gridController = statGridElements[index];
                    gridController.Initialize(f.GetValue(heroData).ToString(), ParseCamelCase(fieldName), "");

                    index++;
                }
            }
        }

        private string ParseCamelCase(string str)
        {
            return Regex.Replace( Regex.Replace( str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2" ), @"(\p{Ll})(\P{Ll})", "$1 $2" );
        }
        
        #endregion
        
        #region Event Handlers

        private void OnHeroGridElementSelected(HeroData obj, bool isSelected)
        {
            var heroData = isSelected ? obj : _heroTotals;
            
            CareerStatsDescription.text = heroData.Hero;
            
            UpdateCareerStats(heroData);
        }

        public void OnHeroOptionSelected(int index)
        {
            var heroGridElements = HeroGridContainer.GetComponentsInChildren<HeroGridElementController>();
            var selectedMetric = HeroMetricsDropdown.options[HeroMetricsDropdown.value].text.ToLower();
            if (string.Equals(selectedMetric, "usage count"))
            {
                // calculating percent used for each hero
                double total = _heroes.Sum(hero => hero.UsageCount);
                _heroes.ForEach(hero => hero.Percent = hero.UsageCount / total);
                int current = 0;
                foreach (var heroGrid in heroGridElements)
                {
                    var heroData = _heroes[current];
                    heroGrid.Initialize(heroData, heroData.UsageCount);
                    current++;
                }
            }
            else if (string.Equals(selectedMetric, "eliminations"))
            {
                // calculating percent used for each hero
                double total = _heroes.Sum(hero => hero.Eliminations);
                _heroes.ForEach(hero => hero.Percent = hero.Eliminations / total);
                int current = 0;
                foreach (var heroGrid in heroGridElements)
                {
                    var heroData = _heroes[current];
                    heroGrid.Initialize(heroData, heroData.Eliminations);
                    current++;
                }
            }
            else if (string.Equals(selectedMetric, "final blows"))
            {
                // calculating percent used for each hero
                double total = _heroes.Sum(hero => hero.FinalBlows);
                _heroes.ForEach(hero => hero.Percent = hero.FinalBlows / total);
                int current = 0;
                foreach (var heroGrid in heroGridElements)
                {
                    var heroData = _heroes[current];
                    heroGrid.Initialize(heroData, heroData.FinalBlows);
                    current++;
                }
            }
            else if (string.Equals(selectedMetric, "deaths"))
            {
                // calculating percent used for each hero
                double total = _heroes.Sum(hero => hero.Deaths);
                _heroes.ForEach(hero => hero.Percent = hero.Deaths / total);
                int current = 0;
                foreach (var heroGrid in heroGridElements)
                {
                    var heroData = _heroes[current];
                    heroGrid.Initialize(heroData, heroData.Deaths);
                    current++;
                }
            }
            else if (string.Equals(selectedMetric, "assists"))
            {
                // calculating percent used for each hero
                double total = _heroes.Sum(hero => hero.Assists);
                _heroes.ForEach(hero => hero.Percent = hero.Assists / total);
                int current = 0;
                foreach (var heroGrid in heroGridElements)
                {
                    var heroData = _heroes[current];
                    heroGrid.Initialize(heroData, heroData.Assists);
                    current++;
                }
            }
            else if (string.Equals(selectedMetric, "time played"))
            {
                // TODO calculate total time played per hero
            }
        }
        
        public void OnOverviewSelected()
        {
            
        }

        public void OnStatisticsSelected()
        {
            
        }
        
        #endregion
    }
}