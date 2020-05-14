using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Hook.OWL
{
    public class TeamGridElementController : MonoBehaviour
    {
        #region Properties

        [SerializeField] private Image TeamLogo;
        [SerializeField] private Text TeamName;
        [SerializeField] private Image Background;

        private TeamData _data;
        
        #endregion
        
        #region MonoBehaviour
        
        void Start()
        {
            /*
            var team = new TeamData();
            team.TeamId = 1;
            team.TeamName = "Atlanta Reign";
            team.TeamLogo = "Teams/Atlanta-Reign.png";
            
            Initialize(team);
            */
        }

        void Update()
        {
        }
        
        #endregion
        
        #region Class Methods

        public void Initialize(TeamData data)
        {
            // saving team data
            _data = data;
            
            // setting team name
            TeamName.text = _data.TeamName;
            
            // setting team color
            Color backgroundColor;
            ColorUtility.TryParseHtmlString(_data.GetTeamColor(), out backgroundColor);
            Background.color = backgroundColor;
            
            // loading team logo
            LoadTeamLogo();
        }

        private void LoadTeamLogo()
        {
            // getting path to image
            var path = string.Format("{0}/{1}", Application.streamingAssetsPath, _data.TeamLogo);
            
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
            Debug.LogFormat("Texture size: [ width: {0}, height: {1} ]", texture.width, texture.height);
            Debug.LogFormat("[ rect.height: {0}, newWidth: {1}, ratio: {2} ]", imageTransform.rect.height, newWidth, currentAspectRatio);
            imageTransform.sizeDelta = new Vector2(newWidth, imageTransform.rect.height);
            Debug.LogFormat("Image rect: [ width: {0}, height: {1} ]", imageTransform.sizeDelta.x, imageTransform.sizeDelta.y);
        }
        
        #endregion
        
        #region Event Handlers

        public void OnTeamSelected()
        {
            Debug.LogFormat("[OnTeamSelected]");
        }
        
        #endregion
    }
}