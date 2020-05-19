using System;

namespace Hook.OWL
{
    public class TeamGridElementEventArgs : EventArgs
    {
        #region Properties
        
        public TeamData SelectedTeam { get; private set; }
        public PlayerProfileData SelectedPlayer { get; private set; }
        
        #endregion
        
        #region Constructor

        public TeamGridElementEventArgs(TeamData selectedTeam)
        {
            SelectedTeam = selectedTeam;
        }

        public TeamGridElementEventArgs(PlayerProfileData selectedPlayer)
        {
            SelectedPlayer = selectedPlayer;
        }
        
        #endregion
    }
}