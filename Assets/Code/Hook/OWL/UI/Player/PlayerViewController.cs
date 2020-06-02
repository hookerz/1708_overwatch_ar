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

        public void Initialize(PlayerProfileData player)
        {
            // loading player image
            AssetLoader.LoadImage(player.ProfileImageUrl, PlayerImage);
            
            // setting Overwatch, player name
            OverwatchName.text = player.OverwatchName;
            PlayerName.text = player.PlayerName;
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