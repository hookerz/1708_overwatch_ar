using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hook.PlayerInput
{
    public enum SwipeDirection
    {
        Left,
        Right,
        Up,
        Down,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft
    }
    
    public class InputManager : MonoBehaviour
    {
        #region Properties
    
        public static InputManager Instance
        {
            get
            {
                return _instance;
            }
        }
    
        public float HorizontalThreshold = 0.25f;
        public float VerticalThreshold = 0.5f;
    
        private static InputManager _instance;
    
        private Vector3 _touchStartPosition;
        private Vector3 _touchPreviousPosition;
        private bool _isTouchStarted;
        private bool _isPinchStarted;
        private float _tapDownTime;
        private float _pinchDeltaStart;
    
        #endregion
    
        #region MonoBehaviour
    
        protected void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
    
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                // TODO add error check for additional instances being created
            }
        }
    
        protected void Update()
        {
            if (Application.platform == RuntimePlatform.Android &&
                Input.GetKeyDown(KeyCode.Escape) &&
                InputEvents.OnAndroidBackButtonDetected != null)
            {
                InputEvents.OnAndroidBackButtonDetected(this, new EventArgs());
            }
            
            if (Input.touchCount == 0)
            {
                _isPinchStarted = false;
            }
            
            if (Input.touchCount == 2 && !_isPinchStarted)
            {
                _pinchDeltaStart = (Input.touches[0].position - Input.touches[1].position).magnitude;
                _isPinchStarted = true;

                return;
            }

            if (Input.touchCount == 2 && _isPinchStarted)
            {
                float newPinchDeltaStart = (Input.touches[0].position - Input.touches[1].position).magnitude;
                float pinchChange = _pinchDeltaStart - newPinchDeltaStart;
                _pinchDeltaStart = newPinchDeltaStart;

                if (InputEvents.OnPinchDetected != null)
                {
                    InputEvents.OnPinchDetected(this, new PinchEventArgs(pinchChange));
                }
                
                return;
            }
            
            if (Input.GetMouseButtonDown(0) && !_isTouchStarted)
            {
                _touchStartPosition = Input.mousePosition;
                _touchPreviousPosition = Input.mousePosition;
                _tapDownTime = Time.time;
                _isTouchStarted = true;

            }

            if (Input.GetMouseButton(0) && _isTouchStarted)
            {
                var delta = Input.mousePosition - _touchPreviousPosition;
                if (InputEvents.OnTouchMoveDetected != null)
                {
                    InputEvents.OnTouchMoveDetected(this, new TouchMoveEventArgs(delta));
                }

                _touchPreviousPosition = Input.mousePosition;
            }
            
            if (Input.GetMouseButtonUp(0) && _isTouchStarted)
            {
                // getting delta between touch start and touch end positions
                Vector3 delta = Input.mousePosition - _touchStartPosition;

                // getting elapsed time that tap has been down
                float elapsedTime = Time.time - _tapDownTime;
                
                // checking for tap (no horizontal or vertical swipe threshold met)
                if (Mathf.Abs(delta.x) < HorizontalThreshold && Mathf.Abs(delta.y) < VerticalThreshold)
                {
                    // normalize touch point
                    Vector3 normPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                    
                    // dispatching tap
                    if (InputEvents.OnTapDetected != null)
                    {
                        InputEvents.OnTapDetected(this, new TapEventArgs(elapsedTime, normPosition));
                    }
                }
                else
                {
                    bool isHorizontalThresholdReached = false;
                    bool isVerticalThresholdReached = false;
                    
                    // checking if delta has reached horizontal threshold
                    if (Mathf.Abs(delta.x) >= HorizontalThreshold)
                    {
                        isHorizontalThresholdReached = true;
                    }
    
                    // checkin if delta has reached vertical threshold
                    if (Mathf.Abs(delta.y) >= VerticalThreshold)
                    {
                        isVerticalThresholdReached = true;
                    }
                    
                    if (isHorizontalThresholdReached || isVerticalThresholdReached)
                    {
                        // getting directions
                        SwipeDirection horizontalDirection = delta.x < 0 ? SwipeDirection.Left : SwipeDirection.Right;
                        SwipeDirection verticalDirection = delta.y < 0 ? SwipeDirection.Down : SwipeDirection.Up;
                        SwipeDirection finalDirection = SwipeDirection.Down;
                        float magnitude = 0f;
                        
                        if (isHorizontalThresholdReached && isVerticalThresholdReached)
                        {
                            if (horizontalDirection == SwipeDirection.Left && verticalDirection == SwipeDirection.Down)
                            {
                                finalDirection = SwipeDirection.DownLeft;
                            }
                            else if (horizontalDirection == SwipeDirection.Left && verticalDirection == SwipeDirection.Up)
                            {
                                finalDirection = SwipeDirection.UpLeft;
                            }
                            else if (horizontalDirection == SwipeDirection.Right && verticalDirection == SwipeDirection.Down)
                            {
                                finalDirection = SwipeDirection.DownRight;
                            }
                            else
                            {
                                finalDirection = SwipeDirection.UpRight;
                            }
                        }
                        else if (isHorizontalThresholdReached)
                        {
                            finalDirection = horizontalDirection;
                        }
                        else
                        {
                            finalDirection = verticalDirection;
                        }
                        
                        // dispatching swipe up/down event
                        if (InputEvents.OnSwipeDetected != null)
                        {
                            InputEvents.OnSwipeDetected(this, new SwipeEventArgs(finalDirection, delta));
                        }
                    }
                }
    
                // reset touch start flag
                _isTouchStarted = false;
            }
        }
    
        #endregion
    }
}
