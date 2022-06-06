using System;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public class PlayerEvent
    {
        #region Variables

        //Player's name
        private PlayersName playerName;
        //Player's move
        private string move;
        //Tringer postion and velocity
        private float floatPos,floatVel;
        //Vector for new postion and velocity 
        private Vector2 vectorPos,vectorVel;
        //Input status 
        private InputStatus status;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="move"></param>
        /// <param name="vectorPos"></param>
        /// <param name="status"></param>
        public PlayerEvent(PlayersName playerName, string move, Vector2 vectorPos, Vector2 vectorVel, InputStatus status)
        {
            this.playerName = playerName;
            this.move = move;
            this.floatPos = 0;
            this.vectorPos = vectorPos;
            this.vectorVel = vectorVel;
            this.status = status;
        }

        /// <summary>
        /// Create player event
        /// </summary>
        /// <param name="playerName">Player's name</param>
        /// <param name="move">Move</param>
        /// <param name="status">Status</param>
        public PlayerEvent(PlayersName playerName, string move,InputStatus status)
        {
            this.playerName = playerName;
            this.move = move;
            this.status = status;
            this.floatPos = 0;
            this.vectorPos = new Vector2(0, 0);
            this.floatVel = 0;
            this.vectorVel = new Vector2(0, 0);
        }

        /// <summary>
        /// Create player event
        /// </summary>
        /// <param name="playerName">Player's name</param>
        /// <param name="floatPos">Float position for triggers</param>
        /// <param name="floatVel">Float velocity for triggers</param>
        public PlayerEvent(PlayersName playerName, string move, float floatPos, float floatVel)
        {
            this.playerName = playerName;
            this.move = move;
            this.status = InputStatus.NONE;
            this.floatPos = floatPos;
            this.vectorPos = new Vector2(0, 0);
            this.floatVel = floatVel;
        }

        /// <summary>
        /// Create player event
        /// </summary>
        /// <param name="playerName">Player's name</param>
        /// <param name="vectorPos">Vector posistion</param>
        /// <param name="vectorVel">Vector velocity</param>
        public PlayerEvent(PlayersName playerName, string move, Vector2 vectorPos, Vector2 vectorVel)
        {
            this.playerName = playerName;
            this.move = move;
            this.status = InputStatus.NONE;
            this.floatPos = 0;
            this.vectorPos = vectorPos;
            this.vectorVel = vectorVel;
        }

        /// <summary>
        /// Create player event
        /// </summary>
        /// <param name="playerName">Player's name</param>
        /// <param name="status">Input status</param>
        public PlayerEvent(PlayersName playerName, InputStatus status)
        {
            this.playerName = playerName;
            this.move = "";
            this.status = status;
            this.floatPos = 0;
            this.vectorPos = new Vector2(0, 0);
            this.floatVel = 0;
            this.vectorVel = new Vector2(0, 0);
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        ~PlayerEvent()
        {

        }

        #endregion

        #region Properties / Methods

        /// <summary>
        /// Get player's name
        /// </summary>
        public PlayersName PlayerName
        {
            get { return this.playerName; }
        }

        /// <summary>
        /// Get player's move
        /// </summary>
        public string Move
        {
            get { return this.move; }
        }

        /// <summary>
        /// Get float postiontion
        /// </summary>
        public float FloatPos
        {
            get { return this.floatPos; }
        }

        /// <summary>
        /// Create float velocity
        /// </summary>
        public float FloatVel
        {
            get { return this.floatVel; }
        }

        /// <summary>
        /// Create vector position
        /// </summary>
        public Vector2 VectorPos
        {
            get { return this.vectorPos; }
        }

        /// <summary>
        /// Create vector velocity
        /// </summary>
        public Vector2 VectorVel
        {
            get { return this.vectorVel; }
        }

        /// <summary>
        /// Input statue
        /// </summary>
        public InputStatus Status
        {
            get { return this.status; }
        }

        /// <summary>
        /// To string for data checking 
        /// </summary>
        /// <returns>String </returns>
        public new string ToString()
        {
            //Create string
            string s = "";
            s += "Player: " + this.playerName.ToString();
            s += Environment.NewLine;
            s += "Move: " + this.move.ToString();
            s += Environment.NewLine;
            s += "Float Pos: " + this.floatPos.ToString();
            s += Environment.NewLine;
            s += "Float Vel: " + this.floatVel.ToString();
            s += Environment.NewLine;
            s += "Vector Pos: " + this.vectorPos.ToString();
            s += Environment.NewLine;
            s += "Vector Vel: " + this.vectorVel.ToString();
            s += Environment.NewLine;
            s += "Status: " + status.ToString();
            return s;
        }

        #endregion
    }
}
