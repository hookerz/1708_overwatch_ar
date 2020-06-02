using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hook.PlayerInput
{
    public class SwipeEventArgs : EventArgs
    {
        #region Properties
    
        public SwipeDirection Direction { get; private set; }
        public float SwipeDistance { get; private set; }
        public Vector3 SwipeDistances { get; private set; }
        public Vector3 SwipeStart { get; private set; }
        public Vector3 SwipeEnd { get; private set; }
        
        #endregion
    
        #region Constructor
    
        public SwipeEventArgs(SwipeDirection direction, float swipeDistance)
        {
            Direction = direction;
            SwipeDistance = swipeDistance;
        }

        public SwipeEventArgs(SwipeDirection direction, Vector3 swipeDistances)
        {
            Direction = direction;
            SwipeDistances = swipeDistances;
        }
        
        #endregion
    }
}
