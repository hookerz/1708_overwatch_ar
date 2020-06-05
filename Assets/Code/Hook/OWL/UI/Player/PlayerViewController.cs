using System.Collections;
using System.Collections.Generic;
using System.IO;
using Hook.HXF;
using TMPro;
using UnityEngine;
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
            // TODO populate career stats view
            
            // getting heroes used for player
            var heroesUsed = _data.GetAllHeroes(player.OverwatchName);
            
            // TODO populate heroes used view
            PopulateHeroes(heroesUsed);
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