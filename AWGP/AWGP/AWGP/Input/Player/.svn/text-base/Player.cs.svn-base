using System;
using Microsoft.Xna.Framework;

namespace AWGP
{

    public class Player
    {
        #region Variables

        //Player's controller
        private Controls controller;
        //Player's name
        private PlayersName playerName;
        //Array of moves 
        private PlayerMoves[] moves;
        //Players index if they have a gamepad
        private PlayerIndex index;

        #endregion

        #region Constructor

        /// <summary>
        /// Create player
        /// </summary>
        /// <param name="playerName">Player's name</param>
        /// <param name="controller">Player's controller</param>
        /// <param name="moves">Player's moves</param>
        public Player(PlayersName playerName, Controls controller, PlayerMoves[] moves)
        {
            //Assign object values
            this.playerName = playerName;
            this.controller = controller;
            this.moves = moves;
            //Initialize index object
            this.index = new PlayerIndex();
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        ~Player()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the control array
        /// </summary>
        public PlayerMoves[] Moves
        {
            get { return moves; }
        }

        /// <summary>
        /// Get or set player's index
        /// </summary>
        public PlayerIndex Index
        {
            get { return this.index; }
            set { this.index = value; }
        }

        /// <summary>
        /// Get or set player's controller
        /// </summary>
        public Controls Controller
        {
            get { return this.controller; }
        }

        /// <summary>
        /// Get or set player's name
        /// </summary>
        public PlayersName PlayerName
        {
            get { return this.playerName; }
        }

        /// <summary>
        /// To string for testing
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            //Create string for player's information 
            string s = "";
            s += "Player's Name: " + playerName;
            s += Environment.NewLine;
            s += "Control: " + controller.ToString();
            s += Environment.NewLine;
            s += "GamePad Index: " + index.ToString();
            return s;
        }

        /// <summary>
        /// Find a move with in the player object
        /// </summary>
        /// <param name="p">Move</param>
        /// <returns>Move string</returns>
        public string FindMove(Enum type, Enum move)
        {
            //Loop thorugh moves
            foreach (PlayerMoves m in moves)
            {
                if (0 == m.Type.CompareTo(type))
                {
                    //Compare
                    if (0 == m.Control.CompareTo(move.ToString()))
                    {
                        return m.Move.ToString();
                    }
                }
            }
            //If not found
            return null;
        }

        #endregion
    }
}
