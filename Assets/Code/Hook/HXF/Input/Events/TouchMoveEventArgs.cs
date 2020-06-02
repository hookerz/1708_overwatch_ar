using UnityEngine;

namespace Hook.PlayerInput
{
    public class TouchMoveEventArgs
    {
        #region Properties
        
        public Vector3 MovementDelta { get; private set; }
        
        #endregion
        
        #region Constructor

        public TouchMoveEventArgs(Vector3 movementDelta)
        {
            MovementDelta = movementDelta;
        }
        
        #endregion
    }
}