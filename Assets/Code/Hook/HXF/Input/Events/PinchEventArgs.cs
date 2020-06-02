using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hook.PlayerInput
{
    public class PinchEventArgs : EventArgs
    {
        #region Properties
    
        public float Delta { get; private set; }
        
        #endregion
    
        #region Constructor
    
        public PinchEventArgs(float delta)
        {
            Delta = delta;
        }
        
        #endregion
    }
}
