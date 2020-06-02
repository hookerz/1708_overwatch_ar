using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Hook.HXF;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hook.OWL
{
    public enum TeamGridContentType
    {
        Teams,
        Players
    }
    
    public class TeamGridElementController : MonoBehaviour
    {
        #region Events

        public static EventHandler<TeamGridElementEventArgs> OnTeamSelectedEvent;
        public static EventHandler<TeamGridElementEventArgs> OnPlayerSelectedEvent;
        
        #endregion
        
        #region Properties

        [SerializeField] private GameObject TeamView;
        [SerializeField] private GameObject PlayerView;
        [SerializeField] private Image Background;
        [Header("Team view properties")]
        [SerializeField] private TextMeshProUGUI TeamName;
        [SerializeField] private Image TeamLogo;
        [Header("Player view properties")]
        [SerializeField] private TextMeshProUGUI PlayerName;
        [SerializeField] private TextMeshProUGUI OverwatchName;
        [SerializeField] private Image PlayerLogo;

        private TeamGridContentType _contentType;
        private TeamData _teamData;
        private PlayerProfileData _playerData;
        private Color _initialBackgroundColor;
        
        #endregion
        
        #region MonoBehaviour

        private void Awake()
        {
            _initialBackgroundColor = Background.color;
        }
        
        #endregion
        
        #region Class Methods

        public void Initialize(TeamData data)
        {
            // setting content type
            _contentType = TeamGridContentType.Teams;
            
            // saving team data
            _teamData = data;
            
            // setting team name
            TeamName.text = _teamData.TeamName;
            
            // setting team color
            Color backgroundColor;
            ColorUtility.TryParseHtmlString(_teamData.GetTeamColor(), out backgroundColor);
            Background.color = backgroundColor;
            
            // loading team logo
            AssetLoader.LoadImage(_teamData.TeamLogo, TeamLogo);
            //LoadImage(_teamData.TeamLogo, TeamLogo);
            
            // displaying team view
            TeamView.SetActive(true);
            PlayerView.SetActive(false);
        }

        public void Initialize(PlayerProfileData data)
        {
            // setting content type
            _contentType = TeamGridContentType.Players;
            
            // saving player data
            _playerData = data;

            // setting player name
            PlayerName.text = _playerData.PlayerName;
            
            // setting Overwatch name
            OverwatchName.text = _playerData.OverwatchName;
            
            // setting background color
            Background.color = _initialBackgroundColor;
            
            // loading player image
            if (!string.IsNullOrEmpty(_playerData.ProfileImageUrl))
            {
                AssetLoader.LoadImage(_playerData.ProfileImageUrl, PlayerLogo);
                //LoadImage(_playerData.ProfileImageUrl, PlayerLogo);
            }
            
            // displaying player view
            TeamView.SetActive(false);
            PlayerView.SetActive(true);
        }
        
        private void LoadImage(string assetPath, Image logo)
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
            logo.sprite = sprite;

            // updating aspect ratio based on height
            var imageTransform = logo.GetComponent<RectTransform>();
            var currentAspectRatio = (float)texture.width / texture.height;
            var newWidth = imageTransform.rect.height * currentAspectRatio;
            imageTransform.sizeDelta = new Vector2(newWidth, imageTransform.rect.height);
        }
        
        #endregion
        
        #region Event Handlers

        public void OnGridElementSelected()
        {
            if (_contentType == TeamGridContentType.Teams)
            {
                if (OnTeamSelectedEvent != null)
                {
                    OnTeamSelectedEvent(this, new TeamGridElementEventArgs(_teamData));
                }
            }
            else if (_contentType == TeamGridContentType.Players)
            {
                if (OnPlayerSelectedEvent != null)
                {
                    OnPlayerSelectedEvent(this, new TeamGridElementEventArgs(_playerData));
                }
            }
        }

        #endregion
    }
}