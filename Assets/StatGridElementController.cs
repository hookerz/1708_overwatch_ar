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
    }
}