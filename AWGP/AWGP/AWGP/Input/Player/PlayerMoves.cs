
namespace AWGP
{
    public class PlayerMoves
    {
        #region Variables

        private string move, control;
        //Type of control
        private GamePadControls type;

        #endregion 

        #region Constructors

        /// <summary>
        /// Create move
        /// </summary>
        /// <param name="control">Control i.e. a button on gamepad or keyboard</param>
        /// <param name="move">Move i.e. kick, punch, jump</param>
        /// <param name="type">Type of control</param>
        public PlayerMoves(string control, string move,GamePadControls type)
        {
            this.control = control;
            this.move = move;
            this.type = type;
        }

        /// <summary>
        /// Deconstructure
        /// </summary>
        ~PlayerMoves()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get move
        /// </summary>
        public string Move
        {
            get { return move; }
        }

        /// <summary>
        /// Get control 
        /// </summary>
        public string Control
        {
            get { return control; }
        }

        /// <summary>
        /// Get type of control
        /// </summary>
        public GamePadControls Type
        {
            get { return type; }
        }

        #endregion
    }
}
