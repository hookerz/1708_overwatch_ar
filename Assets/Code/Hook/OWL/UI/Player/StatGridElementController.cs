using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hook.OWL
{
    public class StatGridElementController : MonoBehaviour
    {
        #region Properties

        [SerializeField] private Image StatIcon;
        [SerializeField] private TextMeshProUGUI StatValueText;
        [SerializeField] private TextMeshProUGUI StatLabelText;
        [SerializeField] private TextMeshProUGUI DetailStatText;
        
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

        public void Initialize(string statValue, string statLabel, string detailStat)
        {
            if (string.Equals(statLabel, StatLabelText.text))
            {
                var newValue = float.Parse(statValue);
                var oldValue = float.Parse(StatValueText.text);
                var delta = oldValue - newValue;
                float duration = .5f;
                DOVirtual.Float(oldValue, newValue, duration, (currentValue) =>
                {
                    StatValueText.text = Mathf.FloorToInt(currentValue).ToString();
                });
            }
            else
            {
                StatValueText.text = statValue;
                StatLabelText.text = statLabel;
                DetailStatText.text = detailStat;
            }
        }
        
        #endregion
    }
}