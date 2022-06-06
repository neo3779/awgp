using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWGP.Components
{
    public class PlayerInputComponent : InputComponent
    {
        public PlayerInputComponent( PlayersName player, bool enabledImmediately)
            : base(enabledImmediately)
        {
            this.player = player;
            OnCreated(this, enabledImmediately);
        }

        public override event EventHandler NotifyInputEvent;
        public override void HandleInputEvent(object sender, EventArgs e)
        {
            PlayerEvent pe = (PlayerEvent)sender;
            EventHandler handler = NotifyInputEvent;
            if (handler != null && pe.PlayerName == player)
            {
                handler(sender, new EventArgs());
            }
        }
        
        private PlayersName player; 
    }
}
