using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Xml;
using AWGP.Components;

namespace AWGP
{
    public class InputManager
    {

        #region Variables

        //Number of contorls thatn can be controlled
        private static int maxGamePads = 4, maxKeyboard = 2, maxMouse = 1, maxPlayers = maxGamePads + maxKeyboard + maxMouse;
        //List of players
        private List<Player> players;
        //List of keyboard monitors
        private List<KeyboardMonitor> keyboards;
        //List of gamepads monitors
        private List<GamePadMonitor> gamepads;
        //Mouse monitor
        private MouseMonitor mouse;
        //Player index for game pads
        private PlayerIndex index = PlayerIndex.One;
        //Player event object
        private PlayerEvent playerEvent;
        //Game pad for event handler
        private GamePadMonitor pad;
        //Mouse for event handler 
        private MouseMonitor mice;
        //Keyboard for event handler
        private KeyboardMonitor keyboard;
        //Event handler 
        public event EventHandler InputEvent, PlayerLeftGame, PlayerEnteredGame;
        //Number of players
        private int numberOfPlayers = 0;

        #endregion

        #region Constuctor

        /// <summary>
        /// Construcutor for input manager
        /// </summary>
        /// <param name="fileName">Filr name</param>
        public InputManager(string fileName)
        {
            InitializeInput(ReadXML(fileName));
            InputComponent.NotifyCreated += HandleComponentCreated;
        }

        /// <summary>
        /// Deconstrcutor
        /// </summary>
        ~InputManager()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Number of player read in
        /// </summary>
        public int NumberOfPlayers
        {
            get { return this.numberOfPlayers; }
        }

        /// <summary>
        /// Update the state of monitors
        /// </summary>
        public void Update()
        {
            //Update keyboard if it initialized
            if (keyboards != null)
            {
                //Loop thorugh keyboard players
                foreach (KeyboardMonitor k in keyboards)
                {
                    //Update keyboard
                    k.Update();
                }
            }
            //Update mouse if it initialized
            if (mouse != null)
            {
                //Update mouse
                mouse.Update();
            }
            //Update game pads if the it initialized
            if (gamepads != null)
            {
                //Loop through game pads players
                foreach (GamePadMonitor g in gamepads)
                {
                    g.Update();
                }
            }
        }

        /// <summary>
        /// To string for error checking
        /// </summary>
        /// <returns>To string</returns>
        public new string ToString()
        {

            //Create string
            string s = "";
            //If keboard used
            if (keyboards != null)
            {
                //For each keyboard user
                foreach (KeyboardMonitor k in keyboards)
                {
                    //Get to string
                    s += k.ToString();
                }
            }

            //If mouse used
            if (mouse != null)
            {
                //Get to string for mouse
                s += mouse.ToString();
                //Instert new line
                s += Environment.NewLine;
            }

            //If gamepads used
            if (gamepads != null)
            {
                //For each game pad user
                foreach (GamePadMonitor g in gamepads)
                {
                    //Get to string
                    s += g.ToString();
                }
            }
            //Retrun string
            return s;
        }

        /// <summary>
        /// Readh XML file in to setup input manager
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <returns>List of players</returns>
        private List<Player> ReadXML(string filename)
        {
            XmlDocument itemDoc = new XmlDocument();
            List<Player> players = new List<Player>();
            List<PlayerMoves> moves = new List<PlayerMoves>();
            XmlNodeList playerList = itemDoc.GetElementsByTagName("player");
            PlayersName playerName = PlayersName.PLAYER_0;
            Controls controls = Controls.GAMEPAD;
            string control = "";
            string move = "";
            GamePadControls type = GamePadControls.BUTTON;
            try
            {
                itemDoc.Load("Content\\" + filename + ".xml");
            }
            catch (Exception e)
            {

                throw new Exception("File not found in content folder of game." + Environment.NewLine + e.ToString());
            }

            if (playerList.Count == 0)
            {
                throw new Exception("No players found.");
            }

            foreach (XmlNode node in playerList)
            {
                XmlElement playerElement = (XmlElement)node;

                if (playerElement.HasAttributes)
                {
                    try
                    {
                        playerName = (PlayersName)Enum.Parse(typeof(PlayersName), playerElement.Attributes["playersName"].InnerText);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Player's name not correct" + Environment.NewLine + e.ToString());
                    }
                    try
                    {
                        controls = (Controls)Enum.Parse(typeof(Controls), playerElement.Attributes["control"].InnerText);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Control device name not found." + Environment.NewLine + e.ToString());
                    }
                }
                if (node.ChildNodes.Count == 0)
                {
                    throw new Exception("Player has not profile for moves or controls." + Environment.NewLine + playerName.ToString() + Environment.NewLine + controls.ToString());
                }
                foreach (XmlNode child in node.ChildNodes)
                {
                    XmlElement moveElement = (XmlElement)child;
                    if (moveElement.HasAttributes)
                    {
                        try
                        {
                            type = (GamePadControls)Enum.Parse(typeof(GamePadControls), moveElement.Attributes["type"].InnerText);
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Control type not found." + Environment.NewLine + e.ToString());
                        }
                        control = moveElement.Attributes["control"].InnerText.ToString();
                        move = moveElement.Attributes["moverName"].InnerText.ToString();
                        if ((control == "" || control == null) || (move == "" || move == null))
                        {
                            throw new Exception("Player move or control is blank/null");
                        }
                        moves.Add(new PlayerMoves(control, move, type));
                    }
                }
                players.Add(new Player(playerName, controls, moves.ToArray()));
                moves.Clear();
            }

            CheckNumberOfPlayers(players);

            return players;
        }

        /// <summary>
        /// Create input manager
        /// </summary>
        /// <param name="players"></param>
        private void InitializeInput(List<Player> players)
        {
            //Loop through all players
            foreach (Player p in players)
            {
                //Determine player controller
                switch (p.Controller)
                {
                    //Game pad
                    case Controls.GAMEPAD:
                        //Check that gamepad exists
                        if (this.gamepads == null)
                        {
                            //Create game pad list
                            this.gamepads = new List<GamePadMonitor>();
                        }

                        //Create buttons array
                        List<Buttons> buttonsToWatchGamePads = new List<Buttons>();
                        List<InputDirection> watchDPad = new List<InputDirection>();
                        List<InputDirection> leftStick = new List<InputDirection>();
                        List<InputDirection> rightStick = new List<InputDirection>();
                        bool rightTrigger = false, leftTigger = false;
                        //Loop thorugh each button
                        foreach (PlayerMoves e in p.Moves)
                        {
                            switch (e.Type)
                            {
                                case GamePadControls.BUTTON:
                                    //Convert to button type
                                    buttonsToWatchGamePads.Add((Buttons)Enum.Parse(typeof(Buttons), e.Control.ToString()));
                                    break;
                                case GamePadControls.DPAD:
                                    watchDPad.Add((InputDirection)Enum.Parse(typeof(InputDirection), e.Control.ToString()));
                                    break;
                                case GamePadControls.LEFT_TRIGGER:
                                    leftTigger = true;
                                    break;
                                case GamePadControls.RIGHT_TRIGGER:
                                    rightTrigger = true;
                                    break;
                                case GamePadControls.LEFT_STICK:
                                    leftStick.Add((InputDirection)Enum.Parse(typeof(InputDirection), e.Control.ToString()));
                                    break;
                                case GamePadControls.RIGHT_STICK:
                                    rightStick.Add((InputDirection)Enum.Parse(typeof(InputDirection), e.Control.ToString()));
                                    break;
                            }
                        }
                        //Create game pad monitor
                        this.gamepads.Add(new GamePadMonitor(index, buttonsToWatchGamePads, watchDPad, rightStick, leftStick, rightTrigger, leftTigger));
                        //Set player controler index
                        p.Index = index;
                        //Increase player index
                        this.index++;
                        break;
                    //Mouse
                    case Controls.MOUSE:
                        //Create buttons array
                        List<MouseMonitor.MouseButtons> buttonsToWatchMouse = new List<MouseMonitor.MouseButtons>();
                        List<InputDirection> mouseAxisToWatch = new List<InputDirection>();
                        bool mouseWheel = false;
                        //Loop thorugh each button
                        foreach (PlayerMoves e in p.Moves)
                        {
                            switch (e.Type)
                            {
                                case GamePadControls.BUTTON:
                                    //Convert to button type
                                    buttonsToWatchMouse.Add((MouseMonitor.MouseButtons)Enum.Parse(typeof(MouseMonitor.MouseButtons), e.Control.ToString()));
                                    break;
                                case GamePadControls.MOUSE_AXIS:
                                    mouseAxisToWatch.Add((InputDirection)Enum.Parse(typeof(InputDirection), e.Control.ToString()));
                                    break;
                                case GamePadControls.MOUSE_WHEEL:
                                    mouseWheel = true;
                                    break;
                            }
                        }
                        //Add event listners and handlers
                        this.mouse = new MouseMonitor(buttonsToWatchMouse, mouseAxisToWatch, mouseWheel);
                        this.mouse.NewButtonPressed += new EventHandler(mouse_NewButton);
                        this.mouse.NewMousePosition += new EventHandler(mouse_NewPosition);
                        this.mouse.NewMouseWheelPosition += new EventHandler(mouse_NewWheelPosition);
                        this.mouse.OldButtonReleased += new EventHandler(mouse_OldButton);
                        break;
                    //Keyboard
                    case Controls.KEYBOARD:
                        //Check that keyboard exists
                        if (this.keyboards == null)
                        {
                            //Create keyboard list
                            this.keyboards = new List<KeyboardMonitor>();
                        }
                        //Create keys array
                        List<Keys> keys = new List<Keys>();
                        //Loop thorugh each keys
                        foreach (PlayerMoves e in p.Moves)
                        {
                            //Convert to button type
                            keys.Add((Keys)Enum.Parse(typeof(Keys), e.Control.ToString()));
                        }

                        //Add to keyboard monitor list
                        this.keyboards.Add(new KeyboardMonitor(keys));
                        //Increament count
                        break;

                }
            }

            //Check that game pads are being used
            if (gamepads != null)
            {
                //Loop through all game pads monitors
                foreach (GamePadMonitor g in gamepads)
                {
                    //Add game pad events
                    g.GamePadConnected += new EventHandler(gamepad_Connected);
                    g.GamePadDisconnected += new EventHandler(gamepad_Disconnected);
                    g.NewDPadPress += new EventHandler(gamepad_NewDPAD);
                    g.OldDPadPress += new EventHandler(gamepad_OldDPAD);
                    g.NewButtonPressed += new EventHandler(gamepad_NewButton);
                    g.OldButtonReleased += new EventHandler(gamepad_OldButton);
                    g.NewLeftStickPosition += new EventHandler(gamepad_NewLeftStickPosition);
                    g.OldLeftStickPosition += new EventHandler(gamepad_OldLeftStickPosition);
                    g.NewLeftTriggerPosition += new EventHandler(gamepad_LeftTriggerPosition);
                    g.NewRightStickPosition += new EventHandler(gamepad_NewRightStickPosition);
                    g.OldRightStickPosition += new EventHandler(gamepad_OldRightStickPosition);
                    g.NewRightTriggerPosition += new EventHandler(gamepad_RightTriggerPosition);
                }
            }

            //Check that keyboard are being used
            if (keyboards != null)
            {
                //Loop through all keyboard monitors
                foreach (KeyboardMonitor k in keyboards)
                {
                    //Add keyboard events
                    k.NewKeyPressed += new EventHandler(keyboard_NewKey);
                    k.OldKeyReleased += new EventHandler(keyboard_OldKey);
                }
            }
            this.players = players;
            //Clean up to free up memory
            System.GC.Collect();
        }

        /// <summary>
        /// Check that number of player
        /// </summary>
        /// <param name="players">List of players</param>
        private void CheckNumberOfPlayers(List<Player> players)
        {
            int gamepadsCount = 0, keyboardsCount = 0, miceCount = 0;
            List<PlayersName> names = new List<PlayersName>();
            List<Keys> keys = new List<Keys>();
            int dpasCount = 0, rightStickCount = 0, leftStickCount = 0;
            bool gamepadDpadOk = false, rightStickOk = false, leftStickOk = false;

            foreach (Player p in players)
            {
                if (!names.Contains(p.PlayerName))
                {
                    names.Add(p.PlayerName);
                }
                else
                {
                    throw new Exception("Player's name already used.");
                }

                switch (p.Controller)
                {
                    case Controls.GAMEPAD:

                        #region Check GamePad

                        gamepadsCount++;
                        if (gamepadsCount > maxGamePads)
                        {
                            throw new Exception("To many gamepad players.");
                        }
                        foreach (PlayerMoves m in p.Moves)
                        {
                            switch (m.Type)
                            {
                                case GamePadControls.DPAD:
                                    dpasCount++;
                                    if (0 == string.Compare(m.Control, InputDirection.NONE.ToString()))
                                    {
                                        gamepadDpadOk = true;
                                    }

                                    CheckInputDirection(m.Control.ToString(), p);
                                    break;
                                case GamePadControls.RIGHT_STICK:
                                    rightStickCount++;
                                    if (0 == string.Compare(m.Control, InputDirection.NONE.ToString()))
                                    {
                                        rightStickOk = true;
                                    }
                                    CheckInputDirection(m.Control.ToString(), p);
                                    break;
                                case GamePadControls.LEFT_STICK:
                                    leftStickCount++;
                                    if (0 == string.Compare(m.Control, InputDirection.NONE.ToString()))
                                    {
                                        leftStickOk = true;
                                    }
                                    CheckInputDirection(m.Control.ToString(), p);
                                    break;
                                case GamePadControls.LEFT_TRIGGER:
                                case GamePadControls.RIGHT_TRIGGER:
                                    if (0 != string.Compare(m.Control, InputDirection.CHANGE.ToString()))
                                    {
                                        throw new Exception("Gamepad must have only change for control." + Environment.NewLine + p.ToString());
                                    }
                                    CheckInputDirection(m.Control.ToString(), p);
                                    break;
                                case GamePadControls.MOUSE_AXIS:
                                case GamePadControls.MOUSE_WHEEL:
                                    throw new Exception("Gamepad does not have a mouse wheel or axis." + Environment.NewLine + p.ToString());
                                case GamePadControls.BUTTON:
                                    CheckGamePadButtons(m.Control.ToString(), p);
                                    break;
                                default:
                                    throw new Exception("Control type not found." + Environment.NewLine + p.ToString());

                            }
                        }
                        if (!gamepadDpadOk &&
                            dpasCount > 0)
                        {
                            throw new Exception("Gamepad must have a none control state for DPAD useage." + Environment.NewLine + p.ToString());
                        }
                        if (!rightStickOk &&
                             rightStickCount > 0)
                        {
                            throw new Exception("Gamepad must have a none control state for right stick useage." + Environment.NewLine + p.ToString());
                        }
                        if (!leftStickOk &&
                             leftStickCount > 0)
                        {
                            throw new Exception("Gamepad must have a none control state for left stick useage." + Environment.NewLine + p.ToString());
                        }
                        gamepadDpadOk = false;

                        #endregion

                        break;
                    case Controls.KEYBOARD:

                        # region Check Keyboard

                        keyboardsCount++;
                        if (keyboardsCount > maxKeyboard)
                        {
                            throw new Exception("To many keyboard players.");
                        }
                        foreach (PlayerMoves m in p.Moves)
                        {
                            if (0 != string.Compare(m.Type.ToString(), GamePadControls.BUTTON.ToString()))
                            {
                                throw new Exception("Keyboard can only have button control types." + Environment.NewLine + p.ToString());
                            }
                            if (!keys.Contains(CheckKeys(m.Control, p)))
                            {
                                keys.Add(CheckKeys(m.Control, p));
                            }
                            else
                            {
                                throw new Exception("Two players sharing the same keys on the keyboard." + Environment.NewLine + p.ToString());
                            }
                        }

                        #endregion

                        break;
                    case Controls.MOUSE:

                        #region Check Mouse

                        miceCount++;
                        if (miceCount > maxMouse)
                        {
                            throw new Exception("To many mouse players.");
                        }
                        foreach (PlayerMoves m in p.Moves)
                        {
                            switch (m.Type)
                            {
                                case GamePadControls.MOUSE_AXIS:
                                    if (0 == string.Compare(m.Control, InputDirection.CHANGE.ToString()) ||
                                        0 == string.Compare(m.Control, InputDirection.NONE.ToString()))
                                    {
                                        throw new Exception("Mouse control is not a known control." + Environment.NewLine + p.ToString());
                                    }
                                    break;
                                case GamePadControls.MOUSE_WHEEL:
                                    if ((0 != string.Compare(m.Control.ToString(), InputDirection.UP.ToString())) &&
                                        (0 != string.Compare(m.Control.ToString(), InputDirection.DOWN.ToString())))
                                    {
                                        throw new Exception("Mouse wheel cannot travel in given direction." + Environment.NewLine + p.ToString());
                                    }
                                    break;
                                case GamePadControls.BUTTON:
                                    CheckMouseButton(m.Control.ToString(), p);
                                    break;
                                default:
                                    throw new Exception("Control not found on mouse." + Environment.NewLine + p.ToString());
                            }
                        }

                        #endregion

                        break;
                }
            }

            this.numberOfPlayers = gamepadsCount + keyboardsCount + miceCount;
        }

        /// <summary>
        /// Check input direction
        /// </summary>
        /// <param name="s">Button</param>
        /// <param name="p">Player for error</param>
        private void CheckInputDirection(string s, Player p)
        {
            InputDirection inputDirection;
            try
            {
                inputDirection = (InputDirection)Enum.Parse(typeof(InputDirection), s);
            }
            catch
            {
                throw new Exception("Incorrect input direction" + s.ToString() + Environment.NewLine + p.ToString());
            }
        }

        /// <summary>
        /// Check game pad button
        /// </summary>
        /// <param name="s">Button</param>
        /// <param name="p">Player for error</param>
        private void CheckGamePadButtons(string s, Player p)
        {
            Buttons button;
            try
            {
                button = (Buttons)Enum.Parse(typeof(Buttons), s);
            }
            catch
            {
                throw new Exception("Incorrect button found. " + s.ToString() + Environment.NewLine + p.ToString());
            }
        }

        /// <summary>
        /// Check that keyboard button is ok
        /// </summary>
        /// <param name="s">Button</param>
        /// <param name="p">Player for error</param>
        /// <returns></returns>
        private Keys CheckKeys(string s, Player p)
        {
            Keys key;
            try
            {
                key = (Keys)Enum.Parse(typeof(Keys), s);
            }
            catch
            {
                throw new Exception("Incorrect button found. " + s.ToString() + Environment.NewLine + p.ToString());
            }
            return key;
        }

        /// <summary>
        /// Check that mouse button is ok
        /// </summary>
        /// <param name="s">Button</param>
        /// <param name="p">Player for error</param>
        private void CheckMouseButton(string s, Player p)
        {
            MouseMonitor.MouseButtons button;
            try
            {
                button = (MouseMonitor.MouseButtons)Enum.Parse(typeof(MouseMonitor.MouseButtons), s);
            }
            catch
            {
                throw new Exception("Incorrect button found. " + s.ToString() + Environment.NewLine + p.ToString());
            }
        }

        #endregion

        #region Event Handlers

        void gamepad_NewDPAD(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Create sender object for new event 
                        this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.DPAD, (Enum)this.pad.DPadState), InputStatus.START);
                        //Check input event is not null
                        if (this.InputEvent != null)
                        {
                            //Send event
                            this.InputEvent(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }
        }

        void gamepad_OldDPAD(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Create sender object for new event 
                        this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.DPAD, (Enum)this.pad.PreDPadState), InputStatus.STOP);
                        //Check input event is not null
                        if (this.InputEvent != null)
                        {
                            //Send event
                            this.InputEvent(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Game pad new button event 
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void gamepad_NewButton(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Loop through the players buttons
                        foreach (Buttons b in pad.ButtonsPressed())
                        {
                            //Create sender object for new event 
                            this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.BUTTON, (Enum)b), InputStatus.START);
                            //Check input event is not null
                            if (this.InputEvent != null)
                            {
                                //Send event
                                this.InputEvent(this.playerEvent, new EventArgs());
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Game pad old button event 
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void gamepad_OldButton(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Loop through the players buttons
                        foreach (Buttons b in pad.ButtonsReleased())
                        {
                            //Create sender object for new event 
                            this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.BUTTON, (Enum)b), InputStatus.STOP);
                            //Check input event is not null
                            if (this.InputEvent != null)
                            {
                                //Send event
                                this.InputEvent(this.playerEvent, new EventArgs());
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Game pad axis change event 
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void gamepad_NewLeftStickPosition(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Create sender object for new event 
                        this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.LEFT_STICK, (Enum)this.pad.LeftStickState), this.pad.LeftStickPosition, this.pad.LeftStickVelocity,InputStatus.START);
                        //Check input event is not null
                        if (this.InputEvent != null)
                        {
                            //Send event
                            this.InputEvent(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Game pad axis change event 
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void gamepad_OldLeftStickPosition(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Create sender object for new event 
                        this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.LEFT_STICK, (Enum)this.pad.PreLeftStickState), this.pad.LeftStickPosition, this.pad.LeftStickVelocity, InputStatus.STOP);
                        //Check input event is not null
                        if (this.InputEvent != null)
                        {
                            //Send event
                            this.InputEvent(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Game pad trigger change event 
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void gamepad_LeftTriggerPosition(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Create sender object for new event 
                        this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.LEFT_TRIGGER, InputDirection.CHANGE), this.pad.LeftTriggerPosition, this.pad.LeftTriggerVelocity);
                        //Check input event is not null
                        if (this.InputEvent != null)
                        {
                            //Send event
                            this.InputEvent(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Game pad axis change event
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void gamepad_NewRightStickPosition(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Create sender object for new event
                        this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.RIGHT_STICK, (Enum)this.pad.RightStickState), this.pad.RightStickPosition, this.pad.RightStickVelocity,InputStatus.START);
                        //Check input event is not null
                        if (this.InputEvent != null)
                        {
                            //Send event
                            this.InputEvent(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Game pad axis change event
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void gamepad_OldRightStickPosition(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Create sender object for new event
                        this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.RIGHT_STICK, (Enum)this.pad.PreRightStickState), this.pad.RightStickPosition, this.pad.RightStickVelocity,InputStatus.STOP);
                        //Check input event is not null
                        if (this.InputEvent != null)
                        {
                            //Send event
                            this.InputEvent(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Game pad trigger change event 
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void gamepad_RightTriggerPosition(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Create sender object for new event
                        this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.RIGHT_TRIGGER, InputDirection.CHANGE), this.pad.RightTriggerPosition, this.pad.RightTriggerVelocity);
                        //Check input event is not null
                        if (this.InputEvent != null)
                        {
                            //Send event
                            this.InputEvent(this.playerEvent, new EventArgs());
                        }

                    }
                }
            }
        }

        /// <summary>
        ///  Game pad connected event 
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void gamepad_Connected(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Create sender object for new event
                        this.playerEvent = new PlayerEvent(p.PlayerName, InputStatus.PLAYER_CONNECTED);
                        //Check input event is not null
                        if (this.PlayerEnteredGame != null)
                        {
                            //Send event
                            this.PlayerEnteredGame(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Game pad disconnected event 
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void gamepad_Disconnected(object sender, EventArgs e)
        {
            //Cast sender to game pad monitor
            this.pad = (GamePadMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with gamepads
                if (p.Controller == Controls.GAMEPAD)
                {
                    //Check that the player indexes match
                    if (pad.Index.Equals(p.Index))
                    {
                        //Create sender object for new event
                        this.playerEvent = new PlayerEvent(p.PlayerName, InputStatus.PLAYER_DISCONNECTED);
                        //Check input event is not null
                        if (this.PlayerLeftGame != null)
                        {
                            //Send event
                            this.PlayerLeftGame(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Keyboard new key event 
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void keyboard_NewKey(object sender, EventArgs e)
        {
            //Cast sender to keyboard monitor 
            this.keyboard = (KeyboardMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with keybaord
                if (p.Controller == Controls.KEYBOARD)
                {
                    foreach (Keys k in this.keyboard.KeysPressed())
                    {
                        //Create sender object for new event
                        this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.BUTTON, (Enum)k), InputStatus.START);
                        //Check input event is not null
                        if (this.InputEvent != null)
                        {
                            //Send event
                            this.InputEvent(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Keyboard old key event 
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void keyboard_OldKey(object sender, EventArgs e)
        {
            //Cast sender to keyboard monitor 
            this.keyboard = (KeyboardMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with keyboard
                if (p.Controller == Controls.KEYBOARD)
                {
                    foreach (Keys k in this.keyboard.KeysReleased())
                    {
                        //Create sender object for new event
                        this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.BUTTON, (Enum)k), InputStatus.STOP);
                        //Check input event is not null
                        if (this.InputEvent != null)
                        {
                            //Send event
                            this.InputEvent(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Mouse new button event
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void mouse_NewButton(object sender, EventArgs e)
        {
            //Cast sender to mouse monitor 
            this.mice = (MouseMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with mouse
                if (p.Controller == Controls.MOUSE)
                {
                    foreach (MouseMonitor.MouseButtons m in this.mouse.ButtonsPressed())
                    {
                        //Create sender object for new event
                        this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.BUTTON, (Enum)m), InputStatus.START);
                        //Check input event is not null
                        if (this.InputEvent != null)
                        {
                            //Send event
                            this.InputEvent(this.playerEvent, new EventArgs());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Mouse new position
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void mouse_NewPosition(object sender, EventArgs e)
        {
            //Cast sender to keyboard monitor 
            this.mice = (MouseMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with mouse
                if (p.Controller == Controls.MOUSE)
                {

                    //Create sender object for new event
                    this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.MOUSE_AXIS, (Enum)this.mice.MouseAxisDirection), this.mice.MousePosition, this.mouse.MouseVelocity);
                    //Check input event is not null
                    if (this.PlayerLeftGame != null)
                    {
                        //Send event
                        this.PlayerLeftGame(this.playerEvent, new EventArgs());
                    }

                }
            }
        }

        /// <summary>
        /// Mouse new wheel position
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void mouse_NewWheelPosition(object sender, EventArgs e)
        {
            //Cast sender to keyboard monitor 
            this.mice = (MouseMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with mouse
                if (p.Controller == Controls.MOUSE)
                {
                    //Create sender object for new event
                    this.playerEvent = new PlayerEvent(p.PlayerName, p.FindMove(GamePadControls.MOUSE_AXIS, (Enum)this.mice.MouseWheelDirection), this.mice.MouseWheelPostion, this.mouse.MouseWheelVelocity);
                    //Check input event is not null
                    if (this.PlayerLeftGame != null)
                    {
                        //Send event
                        this.PlayerLeftGame(this.playerEvent, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Mouse old button event
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">collision arguments</param>
        void mouse_OldButton(object sender, EventArgs e)
        {
            //Cast sender to mouse monitor 
            this.mice = (MouseMonitor)sender;
            //For every player playing
            foreach (Player p in players)
            {
                //Check on players that are playing with mouse
                if (p.Controller == Controls.MOUSE)
                {
                    //Create sender object for new event
                    this.playerEvent = new PlayerEvent(p.PlayerName, InputStatus.STOP);
                    //Check input event is not null
                    if (this.InputEvent != null)
                    {
                        //Send event
                        this.PlayerLeftGame(playerEvent, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Handle the static event sent when an object of type InputComponent is constructed
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">Empty Arguments</param>
        protected virtual void HandleComponentCreated(object sender, EventArgs e)
        {
            InputComponent component = (InputComponent)sender;
            component.NotifyEnabled += HandleComponentEnabled;
            component.NotifyDisabled += HandleComponentDisabled;
        }

        /// <summary>
        /// Handle the event sent when a component of type InputComponent is enabled
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">Empty Arguments</param>
        protected virtual void HandleComponentEnabled(object sender, EventArgs e)
        {
            InputComponent component = (InputComponent)sender;
            this.InputEvent += component.HandleInputEvent;
        }

        /// <summary>
        /// Handle the event sent when a component of type InputComponent is disabled
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">Empty Arguments</param>
        protected virtual void HandleComponentDisabled(object sender, EventArgs e)
        {
            InputComponent component = (InputComponent)sender;
            this.InputEvent -= component.HandleInputEvent;
        }

        #endregion



    }
}