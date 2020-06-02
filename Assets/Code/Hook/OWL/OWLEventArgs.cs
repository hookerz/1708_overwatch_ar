using System;

namespace Hook.OWL
{
    public class OWLEventArgs : EventArgs
    {
        #region Properties
        
        public PlayerProfileData SelectedPlayer { get; private set; }
        public TeamData SelectedTeam { get; private set; }
        
        #endregion
        
        #region Constructor

        public OWLEventArgs(PlayerProfileData selectedPlayer)
        {
            SelectedPlayer = selectedPlayer;
        }

        public OWLEventArgs(TeamData selectedTeam)
        {
            SelectedTeam = selectedTeam;
        }
        
        #endregion
    }
}