using System;
using System.Collections;
using System.Collections.Generic;

namespace Hook.PlayerInput
{
    public class InputEvents
    {
        #region Events

        public static EventHandler OnAndroidBackButtonDetected;
        public static EventHandler<SwipeEventArgs> OnSwipeDetected;
        public static EventHandler<TapEventArgs> OnTapDetected;
        public static EventHandler<PinchEventArgs> OnPinchDetected;
        public static EventHandler<TouchMoveEventArgs> OnTouchMoveDetected;

        #endregion
    }
}
