using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hook.OWL
{
    public class TeamGridElementController : MonoBehaviour
    {
        #region Events

        public static EventHandler<TeamGridElementEventArgs> OnGridElementSelected;
        
        #endregion
        
        #region Properties

        [SerializeField] private GameObject TeamView;
        [SerializeField] private GameObject PlayerView;
        [SerializeField] private Image TeamLogo;
        [SerializeField] private Image Background;
        [Header("Team view properties")]
        [SerializeField] private Text TeamName;
        [Header("Player view properties")]
        [SerializeField] private TextMeshProUGUI PlayerName;
        [SerializeField] private TextMeshProUGUI OverwatchName;

        private TeamData _teamData;
        private PlayerProfileData _playerData;
        
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

        public void Initialize(TeamData data)
        {
            // saving team data
            _teamData = data;
            
            // setting team name
            TeamName.text = _teamData.TeamName;
            
            // setting team color
            Color backgroundColor;
            ColorUtility.TryParseHtmlString(_teamData.GetTeamColor(), out backgroundColor);
            Background.color = backgroundColor;
            
            // loading team logo
            LoadImage(_teamData.TeamLogo);
        }

        public void Initialize(PlayerProfileData data)
        {
            _playerData = data;

            TeamName.text = _playerData.PlayerName;
            
            // loading player image
            if (!string.IsNullOrEmpty(_playerData.ProfileImageUrl))
            {
                LoadImage(_playerData.ProfileImageUrl);
            }
        }
        
        private void LoadImage(string assetPath)
        {
            // getting path to image
            var path = string.Format("{0}/{1}", Application.streamingAssetsPath, assetPath);
            
            // loading image data
            var imageData = File.ReadAllBytes(path);
            
            // creating texture from image data
            var texture = new Texture2D(2,2);
            texture.LoadImage(imageData);
            
            // creating/applying sprite to Image component
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            TeamLogo.sprite = sprite;

            // updating aspect ratio based on height
            var imageTransform = TeamLogo.GetComponent<RectTransform>();
            var currentAspectRatio = (float)texture.width / texture.height;
            var newWidth = imageTransform.rect.height * currentAspectRatio;
            imageTransform.sizeDelta = new Vector2(newWidth, imageTransform.rect.height);
        }
        
        #endregion
        
        #region Event Handlers

        public void OnTeamSelected()
        {
            if (OnGridElementSelected != null)
            {
                OnGridElementSelected(this, new TeamGridElementEventArgs(_teamData));
            }
        }
        
        #endregion
    }
}