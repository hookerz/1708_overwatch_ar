using System;

namespace Hook.OWL
{
    [Serializable]
    public class TeamData
    {
        #region Properties
        
        public int TeamId;
        public string TeamName;
        public string TeamLogo;
        public string TeamColor;
        
        #endregion
        
        #region Class Methods

        public string GetTeamColor()
        {
            return string.Format("#{0}", TeamColor);
        }
        
        public override string ToString()
        {
            return string.Format("[ Id: {0}, Name: {0}, Logo: {1}, Color: {2} ]", TeamId, TeamName, TeamLogo, TeamColor);
        }
        
        #endregion
    }
}