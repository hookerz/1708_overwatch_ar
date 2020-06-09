using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Hook.HXF;
using TMPro;
using UnityEditor.VersionControl;
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

        private HeroData _heroData;
        private RectTransform _transform;
        private bool _hasDisplayed;
        
        #endregion
        
        #region MonoBehaviour

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
        }

        void Start()
        {
        }

        void Update()
        {
        }
        
        #endregion
        
        #region Class Methods

        public void Initialize(HeroData heroData)
        {
            _heroData = heroData;
            
            // setting hero name
            HeroName.text = heroData.Hero;
            
            // loading hero image
            AssetLoader.LoadImage(heroData.HeroImageUrl, HeroImage);

            StartCoroutine(CheckVisibility());
        }

        private IEnumerator CheckVisibility()
        {
            do
            {
                var isVisible = RectTransformExtension.IsFullyVisibleFrom(_transform);
                if (isVisible && !_hasDisplayed)
                {
                    Progress.transform.DOScaleX((float)_heroData.Percent, 0.5f).SetEase(Ease.OutCubic);
                    _hasDisplayed = true;
                }
                else if (!isVisible && _hasDisplayed)
                {
                    Progress.transform.DOScaleX(0, 0);
                    _hasDisplayed = false;
                }
                yield return new WaitForSeconds(.1f);
            } while (true);
        }
        
        #endregion
    }
}