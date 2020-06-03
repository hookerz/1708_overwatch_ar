using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hook.OWL
{
    public class HeroGridElementController : MonoBehaviour
    {
        #region Properties

        [SerializeField] private TextMeshProUGUI HeroName;
        [SerializeField] private Image HeroImage;
        [SerializeField] private Image Progress;
        
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