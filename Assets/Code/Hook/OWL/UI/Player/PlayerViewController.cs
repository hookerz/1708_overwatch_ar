using System.Collections;
using System.Collections.Generic;
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