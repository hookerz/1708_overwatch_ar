using System;
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

        [CanBeNull] public Action<HeroGridElementController, HeroData, bool> OnHeroGridElementSelected;
        
        [SerializeField] private TextMeshProUGUI HeroName;
        [SerializeField] private Image HeroImage;
        [SerializeField] private Image Progress;
        [SerializeField] private TextMeshProUGUI HeroMetricValue;
        [SerializeField] private GameObject SelectedView;
        
        private HeroData _heroData;
        private int _heroMetricValue;
        private RectTransform _transform;
        private bool _hasDisplayed;
        private bool _isSelected;
        private Color _borderColor;
        private Vector3 _startPosition;
        private float _height;
        
        #endregion
        
        #region MonoBehaviour

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _borderColor = SelectedView.GetComponent<Image>().color;
            _startPosition = _transform.position;
            _height = _transform.rect.height;
        }

        void Start()
        {
        }

        void Update()
        {
            if (_heroData != null && string.Equals(_heroData.Hero, "Doomfist"))
            {
                Debug.LogFormat("[{0}] Position: {1}", _heroData.Hero, _transform.position);
            }
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

        public void UpdateState(bool newSelectedState)
        {
            _isSelected = newSelectedState;
            
            var border = SelectedView.GetComponent<Image>();
            var end = _borderColor;
            if (_isSelected)
            {
                end.a = 1;
            }
            border.DOColor(end, .4f);
        }
        
        #endregion
        
        #region Event Handlers

        public void OnHeroSelected()
        {
            UpdateState(!_isSelected);
            
            OnHeroGridElementSelected?.Invoke(this, _heroData, _isSelected);
        }
        
        #endregion
    }
}