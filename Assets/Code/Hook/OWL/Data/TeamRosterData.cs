using System;
using System.Collections.Generic;

namespace Hook.OWL
{
    [Serializable]
    public class TeamRosterData
    {
        #region Properties
        
        public List<PlayerProfileData> Players { get; private set; }
        
        #endregion

        #region Constructor

        public TeamRosterData(List<PlayerProfileData> players)
        {
            Players = players;
        }

        #endregion
    }
}