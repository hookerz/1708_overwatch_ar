﻿using System;
using System.Collections;
using DG.Tweening;
using Hook.HXF;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hook.OWL
{
    public class HeroGridElementController : MonoBehaviour
    {
        #region Properties

        [CanBeNull] public Action<HeroData, bool> OnHeroGridElementSelected;
        
        [SerializeField] private TextMeshProUGUI HeroName;
        [SerializeField] private Image HeroImage;
        [SerializeField] private Image Progress;
        [SerializeField] private TextMeshProUGUI HeroMetricValue;

        private HeroData _heroData;
        private int _heroMetricValue;
        private RectTransform _transform;
        private bool _hasDisplayed;
        private bool _isSelected;
        
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

        public void Initialize(HeroData heroData, int heroMetricValue)
        {
            _heroData = heroData;
            _heroMetricValue = heroMetricValue;
            
            // setting hero name
            HeroName.text = heroData.Hero;
            
            // setting hero metric value
            HeroMetricValue.gameObject.SetActive(true);
            HeroMetricValue.text = _heroMetricValue.ToString();
            
            // loading hero image
            if (HeroImage.sprite == null)
            {
                AssetLoader.LoadImage(heroData.HeroImageUrl, HeroImage);
            }

            _hasDisplayed = false;
            
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
        
        #region Event Handlers

        public void OnHeroSelected()
        {
            _isSelected = !_isSelected;
            
            OnHeroGridElementSelected?.Invoke(_heroData, _isSelected);
        }
        
        #endregion
    }
}