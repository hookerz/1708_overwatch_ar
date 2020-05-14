using System;

namespace Hook.OWL
{
    [Serializable]
    public class PlayerProfileData
    {
        #region Properties
        
        public string PlayerName;
        public string OverwatchName;
        public string ProfileImageUrl;
        
        #endregion
        
        #region Class Methods

        public override string ToString()
        {
            return string.Format("[ Name: {0}, Tag: {1} ]", PlayerName, OverwatchName);
        }

        #endregion
    }
}