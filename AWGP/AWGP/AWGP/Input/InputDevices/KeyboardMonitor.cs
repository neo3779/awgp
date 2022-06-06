using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace AWGP
{
    public class KeyboardMonitor : iKeyboardMonitor
    {

        #region Variables

        //Keyboard current and previous states
        private KeyboardState preKeyboardState, curKeyboardState;
        //Keys that have been pressed and released
        private List<Keys> keysPressed, keysReleased,keysToWatch;
        //Events for new keys and old keys that are pressed
        public event EventHandler NewKeyPressed, OldKeyReleased;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keysToWatch">Buttons to be watched by monitor</param>
        public KeyboardMonitor(List<Keys> keysToWatch)
        {
            //Create keys to watch list
            this.keysToWatch = keysToWatch;
            //Initailize keys pressed list
            this.keysPressed = new List<Keys>();
            //Initailize keys released list
            this.keysReleased = new List<Keys>();
            //Initailize the states
            Update();
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        ~KeyboardMonitor()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Update the states
        /// </summary>
        public void Update()
        {
            //Get current keyboard states
            this.curKeyboardState = Keyboard.GetState();
            //Check to see if state has changed
            if (!this.curKeyboardState.Equals(preKeyboardState))
            {
                //Loop thorugh all the new pressed keys
                foreach (Keys w in keysToWatch)
                {
                    //Determine if new
                    if (this.NewKeyPressed != null && IsNewPress(w))
                    {
                        //Add to pressed key list
                        this.keysPressed.Add(w);
                        //Create event for new key pressed
                        this.NewKeyPressed(this, new EventArgs());
                        this.keysPressed.Remove(w);
                    }
                    //Determine if old
                    if (this.OldKeyReleased != null && IsOldPress(w))
                    {
                        //Remove from pressed keys
                        this.keysPressed.Remove(w);
                        //Add to released keys
                        this.keysReleased.Add(w);
                        //Create event for old keys relesaed
                        this.OldKeyReleased(this, new EventArgs());
                        //Remove from released buttons
                        this.keysReleased.Remove(w);
                    }
                }
                //Update previous keyboard states
                this.preKeyboardState = curKeyboardState;
            }
            
        }

        /// <summary>
        /// Determine if keys is newly preseed
        /// </summary>
        /// <param name="key">Key to be checked</param>
        /// <returns></returns>
        private bool IsNewPress(Keys key)
        {
            //Check is new and return state
            return (this.preKeyboardState.IsKeyUp(key) && this.curKeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Determine if keys is old pressed
        /// </summary>
        /// <param name="key">Key to be checked</param>
        /// <returns></returns>
        private bool IsOldPress(Keys key)
        {
            //Check is old and return state
            return (this.preKeyboardState.IsKeyDown(key) && this.curKeyboardState.IsKeyUp(key));
        }

        /// <summary>
        /// Keys that are being watch by monitor 
        /// </summary>
        public List<Keys> KeysToWatch
        {
            //Array of keys
            get { return this.keysToWatch; }
        }

        /// <summary>
        /// To string to return all the current information about the keyboard
        /// </summary>
        /// <returns>String with details regarding keyboard</returns>
        public new string ToString()
        {
            //Create string
            string s = "";
            //Update sting with keyboard information
            s += "Keyboard" + Environment.NewLine;
            s += "Key Pesssed: ";
            //Loop thorugh all the keys that have been pressed and output them in the string
            foreach (Keys k in Keyboard.GetState().GetPressedKeys())
            {
                s += k.ToString() + " ";
            }
            //Insert new line
            s += Environment.NewLine;
            s += Environment.NewLine;
            //Return string
            return s;
        }

        /// <summary>
        /// Keys that are currently pressed and being watched
        /// </summary>
        /// <returns>Key pressed</returns>
        public Keys[] KeysPressed()
        {
            //Array of keys
            return this.keysPressed.ToArray();
        }

        /// <summary>
        /// Keys that have been released and being watched
        /// </summary>
        /// <returns></returns>
        public Keys[] KeysReleased()
        {
            //Array of keys
            return  this.keysReleased.ToArray();
        }

        #endregion 
    }
}


