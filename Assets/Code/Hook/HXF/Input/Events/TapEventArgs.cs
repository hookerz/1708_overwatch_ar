using System;
using UnityEngine;

namespace Hook.PlayerInput
{
    public class TapEventArgs : EventArgs
    {
        #region Proeprties
        
        public float TimeDown { get; private set; }
        public Vector3 NormalizedTapLocation { get; private set; }
        
        #endregion
        
        #region Constructor

        public TapEventArgs(float timeDown, Vector3 normalizedTapLocation)
        {
            TimeDown = timeDown;
            NormalizedTapLocation = normalizedTapLocation;
        }
        
        #endregion
    }
}