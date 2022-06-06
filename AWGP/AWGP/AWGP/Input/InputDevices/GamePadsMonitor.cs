using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public class GamePadMonitor : iGamePadsMonitor
    {
        #region Variables

        private GamePadState preGamePadState, curGamePadState;
        private readonly PlayerIndex index;
        private readonly List<Buttons> buttonsPressed, buttonsReleased, buttonsToWatch;
        public event EventHandler GamePadDisconnected, GamePadConnected, NewLeftStickPosition,
                                  NewLeftTriggerPosition, NewRightStickPosition, NewRightTriggerPosition,
                                  NewButtonPressed, OldButtonReleased,
                                  NewDPadPress, OldDPadPress, OldLeftStickPosition, OldRightStickPosition;
        private readonly List<InputDirection> watchDPad, rightStickWatch, leftStickWatch;
        private readonly bool rightTriggerWatch = false, leftTriggerWatch = false;
        private InputDirection DpadState, rightStickState, leftStickState, preDpadState, preRightStickState, preLeftStickState;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for gamepad monitor
        /// </summary>
        /// <param name="index">Player Index</param>
        /// <param name="buttonsToWatch">Button to watch</param>
        /// <param name="watchDPad">DPad directions to watch for</param>
        /// <param name="rightStickWatch">Right stick directions to watch for</param>
        /// <param name="leftStickWatch">Left stick directions to watch for</param>
        /// <param name="rightTriggerWatch">Watch right triggect</param>
        /// <param name="leftTriggerWatch">Watch left trigger</param>
        public GamePadMonitor(PlayerIndex index, List<Buttons> buttonsToWatch, List<InputDirection> watchDPad, List<InputDirection> rightStickWatch, List<InputDirection> leftStickWatch,
                              bool rightTriggerWatch, bool leftTriggerWatch)
        {
            this.index = index;
            this.buttonsToWatch = buttonsToWatch;
            this.watchDPad = watchDPad;
            this.rightStickWatch = rightStickWatch;
            this.leftStickWatch = leftStickWatch;
            this.rightTriggerWatch = rightTriggerWatch;
            this.leftTriggerWatch = leftTriggerWatch;
            this.buttonsPressed = new List<Buttons>();
            this.buttonsReleased = new List<Buttons>();
            Update();
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        ~GamePadMonitor()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// To string for diagnotics
        /// </summary>
        /// <returns>String with status</returns>
        public new string ToString()
        {
            string s = "";

            s += "Game Pad " + index.ToString();
            s += GamePad.GetState(index).ToString();
            s += Environment.NewLine;
            s += GamePad.GetState(index).Buttons.ToString();
            s += Environment.NewLine;
            s += GamePad.GetState(index).DPad.ToString();
            s += Environment.NewLine;
            s += GamePad.GetState(index).Triggers.ToString();
            s += Environment.NewLine;
            s += GamePad.GetState(index).ThumbSticks.ToString();
            s += Environment.NewLine;
            s += Environment.NewLine;

            return s;
        }

        /// <summary>
        /// Get the index of the controller
        /// </summary>
        public PlayerIndex Index
        {
            get { return this.index; }
        }

        /// <summary>
        /// Check to see if button is newly pressed
        /// </summary>
        /// <param name="button">Button to be checked</param>
        /// <returns>Bool of if button was pressed</returns>
        private bool IsNewPress(Buttons button)
        {
            return (this.preGamePadState.IsButtonUp(button) && this.curGamePadState.IsButtonDown(button));
        }

        /// <summary>
        /// Get the array of the button currently being pressed
        /// </summary>
        /// <returns>Button Array</returns>
        public Buttons[] ButtonsPressed()
        {
            return this.buttonsPressed.ToArray();
        }

        /// <summary>
        /// Get all the buttons that have been rleased in from the last fram
        /// </summary>
        /// <returns>Button Array</returns>
        public Buttons[] ButtonsReleased()
        {
            return this.buttonsReleased.ToArray();
        }

        /// <summary>
        /// Check to see button has been realsed i.e. let go
        /// </summary>
        /// <param name="button">Button to be checked</param>
        /// <returns>Bool state</returns>
        private bool IsOldPress(Buttons button)
        {
            return (this.preGamePadState.IsButtonDown(button) && this.curGamePadState.IsButtonUp(button));
        }

        /// <summary>
        /// Get the state of the left trigger velocity
        /// </summary>
        public float LeftTriggerVelocity
        {
            get { return this.curGamePadState.Triggers.Left - this.preGamePadState.Triggers.Left; }
        }

        /// <summary>
        /// Get the left sticks position
        /// </summary>
        public Vector2 LeftStickPosition
        {
            get { return new Vector2(this.curGamePadState.ThumbSticks.Left.X, this.curGamePadState.ThumbSticks.Left.Y); }
        }

        /// <summary>
        /// Get the left sticks velocity
        /// </summary>
        public Vector2 LeftStickVelocity
        {
            get { return this.curGamePadState.ThumbSticks.Left - this.preGamePadState.ThumbSticks.Left; }
        }

        /// <summary>
        /// Get the postition of the right stick
        /// </summary>
        public Vector2 RightStickPosition
        {
            get { return new Vector2(this.curGamePadState.ThumbSticks.Right.X, this.curGamePadState.ThumbSticks.Right.Y); }
        }

        /// <summary>
        /// Get the velocity of the right stick
        /// </summary>
        public Vector2 RightStickVelocity
        {
            get { return this.curGamePadState.ThumbSticks.Right - this.preGamePadState.ThumbSticks.Right; }
        }

        /// <summary>
        /// Get left stick postion                              
        /// </summary>
        public float LeftTriggerPosition
        {
            get { return this.curGamePadState.Triggers.Left; }
        }

        /// <summary>
        /// Get right trigger postion
        /// </summary>
        public float RightTriggerPosition
        {
            get { return this.curGamePadState.Triggers.Right; }
        }

        /// <summary>
        /// Get right trigger velocity
        /// </summary>
        public float RightTriggerVelocity
        {
            get { return this.curGamePadState.Triggers.Right - this.preGamePadState.Triggers.Right; }
        }

        /// <summary>
        /// Get the state of the Dpad
        /// </summary>
        public InputDirection DPadState
        {
            get { return this.DpadState; }
        }

        /// <summary>
        /// Get the state of the left stick
        /// </summary>
        public InputDirection LeftStickState
        {
            get { return this.leftStickState; }
        }

        /// <summary>
        /// Get the state of the right stick
        /// </summary>
        public InputDirection RightStickState
        {
            get { return this.rightStickState; }
        }

        public Enum PreDPadState { get { return this.preDpadState; } }

        public Enum PreLeftStickState { get { return this.preLeftStickState; } }

        public Enum PreRightStickState { get { return this.preRightStickState; } }

        private void checkConnectionState()
        {
            //flag than controller had entered the game
            if (this.curGamePadState.IsConnected == true &&
                this.preGamePadState.IsConnected == false)
            {
                if (this.GamePadConnected != null)
                {
                    this.GamePadConnected(this, new EventArgs());
                }
            }

            //flag that the controller has been disconneted
            if (this.curGamePadState.IsConnected == false &&
                this.preGamePadState.IsConnected == true)
            {
                if (this.GamePadDisconnected != null)
                {
                    this.GamePadDisconnected(this, new EventArgs());
                }

            }
        }

        private void updateDPadState()
        {
            if (this.watchDPad.Count != 0)
            {
                if (this.curGamePadState.DPad.Up == ButtonState.Pressed)
                {
                    if (this.curGamePadState.DPad.Up == ButtonState.Pressed &&
                        this.curGamePadState.DPad.Left == ButtonState.Pressed)
                    {
                        if (watchDPad.Contains(InputDirection.UP_LEFT))
                        {
                            this.DpadState = InputDirection.UP_LEFT;
                        }
                    }
                    else if (this.curGamePadState.DPad.Up == ButtonState.Pressed &&
                        this.curGamePadState.DPad.Right == ButtonState.Pressed)
                    {
                        if (watchDPad.Contains(InputDirection.UP_RIGHT))
                        {
                            this.DpadState = InputDirection.UP_RIGHT;
                        }
                    }
                    else
                    {
                        if (watchDPad.Contains(InputDirection.UP))
                        {
                            this.DpadState = InputDirection.UP;
                        }
                    }
                }
                else if (this.curGamePadState.DPad.Down == ButtonState.Pressed)
                {
                    if (this.curGamePadState.DPad.Down == ButtonState.Pressed &&
                        this.curGamePadState.DPad.Left == ButtonState.Pressed)
                    {
                        if (watchDPad.Contains(InputDirection.DOWN_LEFT))
                        {
                            this.DpadState = InputDirection.DOWN_LEFT;
                        }
                    }
                    else if (this.curGamePadState.DPad.Down == ButtonState.Pressed &&
                        this.curGamePadState.DPad.Right == ButtonState.Pressed)
                    {
                        if (watchDPad.Contains(InputDirection.DOWN_RIGHT))
                        {
                            this.DpadState = InputDirection.DOWN_RIGHT;
                        }
                    }
                    else
                    {
                        if (watchDPad.Contains(InputDirection.DOWN))
                        {
                            this.DpadState = InputDirection.DOWN;
                        }
                    }
                }
                else if (this.curGamePadState.DPad.Right == ButtonState.Pressed)
                {
                    if (watchDPad.Contains(InputDirection.RIGHT))
                    {
                        this.DpadState = InputDirection.RIGHT;
                    }
                }
                else if (this.curGamePadState.DPad.Left == ButtonState.Pressed)
                {
                    if (watchDPad.Contains(InputDirection.LEFT))
                    {
                        this.DpadState = InputDirection.LEFT;
                    }
                }
                else
                {
                    this.DpadState = InputDirection.NONE;
                }

                
            }
        }

        private void updateStickState()
        {
            #region Leftstick

            if (this.leftStickWatch.Count != 0)
            {
                if (!(this.curGamePadState.ThumbSticks.Left.X == 0.0f &&
                     this.curGamePadState.ThumbSticks.Left.Y == 0.0f))
                {

                    if (this.curGamePadState.ThumbSticks.Left.X != this.preGamePadState.ThumbSticks.Left.X &&
                        this.curGamePadState.ThumbSticks.Left.Y != this.preGamePadState.ThumbSticks.Left.Y)
                    {
                        if (this.curGamePadState.ThumbSticks.Left.X > 0 &&
                            this.curGamePadState.ThumbSticks.Left.Y > 0)
                        {
                            if (leftStickWatch.Contains(InputDirection.UP_RIGHT))
                            {
                                this.leftStickState = InputDirection.UP_RIGHT;
                            }
                        }
                        else if (this.curGamePadState.ThumbSticks.Left.X < 0 &&
                                this.curGamePadState.ThumbSticks.Left.Y < 0)
                        {
                            if (this.leftStickWatch.Contains(InputDirection.DOWN_RIGHT))
                            {
                                this.leftStickState = InputDirection.DOWN_RIGHT;
                            }
                        }
                        else if (this.curGamePadState.ThumbSticks.Left.X < 0 &&
                                this.curGamePadState.ThumbSticks.Left.Y > 0)
                        {
                            if (this.leftStickWatch.Contains(InputDirection.UP_LEFT))
                            {
                                this.leftStickState = InputDirection.UP_LEFT;
                            }
                        }
                        else if (this.curGamePadState.ThumbSticks.Left.X > 0 &&
                                this.curGamePadState.ThumbSticks.Left.Y < 0)
                        {
                            if (this.leftStickWatch.Contains(InputDirection.DOWN_LEFT))
                            {
                                this.leftStickState = InputDirection.DOWN_LEFT;
                            }
                        }

                    }
                    else if (this.curGamePadState.ThumbSticks.Left.X != this.preGamePadState.ThumbSticks.Left.X)
                    {
                        if (this.curGamePadState.ThumbSticks.Left.X > 0)
                        {
                            if (this.leftStickWatch.Contains(InputDirection.RIGHT))
                            {
                                this.leftStickState = InputDirection.RIGHT;
                            }
                        }
                        else
                        {
                            if (this.leftStickWatch.Contains(InputDirection.RIGHT))
                            {
                                this.leftStickState = InputDirection.RIGHT;
                            }
                        }
                    }
                    else if (this.curGamePadState.ThumbSticks.Left.Y != this.preGamePadState.ThumbSticks.Left.Y)
                    {
                        if (this.curGamePadState.ThumbSticks.Left.Y > 0)
                        {
                            if (this.leftStickWatch.Contains(InputDirection.UP))
                            {
                                this.leftStickState = InputDirection.UP;
                            }
                        }
                        else
                        {
                            if (this.leftStickWatch.Contains(InputDirection.DOWN))
                            {
                                this.leftStickState = InputDirection.DOWN;
                            }
                        }
                    }
                }
                else
                {
                    this.leftStickState = InputDirection.NONE;
                }

            }

            #endregion

            #region RightStick

            if (rightStickWatch.Count != 0)
            {
                if (!(this.curGamePadState.ThumbSticks.Left.X == 0 &&
                      this.curGamePadState.ThumbSticks.Left.Y == 0))
                {
                    if (this.curGamePadState.ThumbSticks.Right.X != this.preGamePadState.ThumbSticks.Right.X &&
                        this.curGamePadState.ThumbSticks.Right.Y != this.preGamePadState.ThumbSticks.Right.Y)
                    {
                        if (this.curGamePadState.ThumbSticks.Right.X > 0 &&
                            this.curGamePadState.ThumbSticks.Right.Y > 0)
                        {
                            if (this.rightStickWatch.Contains(InputDirection.UP_RIGHT))
                            {
                                this.rightStickState = InputDirection.UP_RIGHT;
                            }
                        }
                        else if (this.curGamePadState.ThumbSticks.Right.X < 0 &&
                                this.curGamePadState.ThumbSticks.Right.Y < 0)
                        {
                            if (this.rightStickWatch.Contains(InputDirection.DOWN_RIGHT))
                            {
                                this.rightStickState = InputDirection.DOWN_RIGHT;
                            }
                        }
                        else if (this.curGamePadState.ThumbSticks.Right.X < 0 &&
                                this.curGamePadState.ThumbSticks.Right.Y > 0)
                        {
                            if (this.rightStickWatch.Contains(InputDirection.UP_LEFT))
                            {
                                this.rightStickState = InputDirection.UP_LEFT;
                            }
                        }
                        else if (this.curGamePadState.ThumbSticks.Right.X > 0 &&
                                this.curGamePadState.ThumbSticks.Right.Y < 0)
                        {
                            if (this.rightStickWatch.Contains(InputDirection.DOWN_LEFT))
                            {
                                this.rightStickState = InputDirection.DOWN_LEFT;
                            }
                        }

                    }
                    else if (this.curGamePadState.ThumbSticks.Right.X != this.preGamePadState.ThumbSticks.Right.X)
                    {
                        if (this.curGamePadState.ThumbSticks.Left.X > 0)
                        {
                            if (this.rightStickWatch.Contains(InputDirection.RIGHT))
                            {
                                this.rightStickState = InputDirection.RIGHT;
                            }
                        }
                        else
                        {
                            if (this.rightStickWatch.Contains(InputDirection.LEFT))
                            {
                                this.rightStickState = InputDirection.LEFT;
                            }
                        }
                    }
                    else if (this.curGamePadState.ThumbSticks.Right.Y != this.preGamePadState.ThumbSticks.Right.Y)
                    {
                        if (this.curGamePadState.ThumbSticks.Right.Y > 0)
                        {
                            if (this.rightStickWatch.Contains(InputDirection.UP))
                            {
                                this.rightStickState = InputDirection.UP;
                            }
                        }
                        else
                        {
                            if (this.rightStickWatch.Contains(InputDirection.DOWN))
                            {
                                this.rightStickState = InputDirection.DOWN;
                            }
                        }
                    }
                }
                else
                {
                    this.rightStickState = InputDirection.NONE;
                }

            }

            #endregion
        }

        private void updateTriggers()
        {
            if (this.leftTriggerWatch)
            {
                if (this.curGamePadState.Triggers.Left != this.preGamePadState.Triggers.Left)
                {
                    if (this.NewLeftTriggerPosition != null)
                    {
                        this.NewLeftTriggerPosition(this, new EventArgs());
                    }
                }
            }

            if (this.rightTriggerWatch)
            {
                if (this.curGamePadState.Triggers.Right != this.preGamePadState.Triggers.Right)
                {
                    if (this.NewRightTriggerPosition != null)
                    {
                        this.NewRightTriggerPosition(this, new EventArgs());
                    }
                }
            }

        }

        private void updateButtons()
        {
            if (buttonsToWatch.Count != 0)
            {
                foreach (Buttons w in this.buttonsToWatch)
                {
                    if (IsNewPress(w))
                    {
                        if (this.NewButtonPressed != null)
                        {
                            this.buttonsPressed.Add(w);
                            this.NewButtonPressed(this, new EventArgs());
                            this.buttonsPressed.Remove(w);
                        }
                    }
                    if (IsOldPress(w))
                    {
                        if (this.OldButtonReleased != null)
                        {
                            this.buttonsReleased.Add(w);
                            this.OldButtonReleased(this, new EventArgs());
                            this.buttonsReleased.Remove(w);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update the state
        /// </summary>
        public void Update()
        {
            //Update current states of the game pad
            this.curGamePadState = GamePad.GetState(this.index,GamePadDeadZone.IndependentAxes);

            //Check for change
            if (!this.curGamePadState.Equals(this.preGamePadState))
            {
                checkConnectionState();
                
                if (this.curGamePadState.IsConnected == true)
                {

                   updateDPadState();

                    updateStickState();

                    updateTriggers();

                    updateButtons();
                    
                }

                this.preGamePadState = this.curGamePadState;

            }
            else
            {
                //Rest states
                this.DpadState = InputDirection.NONE;
                this.leftStickState = InputDirection.NONE;
                this.rightStickState = InputDirection.NONE;
            }

            if (this.DpadState != this.preDpadState)
            {
                if (this.OldDPadPress != null)
                {
                    this.OldDPadPress(this, new EventArgs());
                }
                if (this.NewDPadPress != null)
                {
                    this.NewDPadPress(this, new EventArgs());
                }
                this.preDpadState = this.DpadState;
            }

            if (this.leftStickState != this.preLeftStickState)
            {
                if (this.OldLeftStickPosition != null)
                {
                    this.OldLeftStickPosition(this, new EventArgs());
                }
                if (this.NewLeftStickPosition != null)
                {
                    this.NewLeftStickPosition(this, new EventArgs());
                }
                this.preLeftStickState = this.leftStickState;
            }
            if (this.rightStickState != this.preRightStickState)
            {
                if (this.OldRightStickPosition != null)
                {
                    this.OldRightStickPosition(this, new EventArgs());
                }
                if (this.NewRightStickPosition != null)
                {
                    this.NewRightStickPosition(this, new EventArgs());
                }
                this.preRightStickState = this.rightStickState;
            }
        }

        #endregion
    }
}

