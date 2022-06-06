using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWGP.Components
{
    public class InputComponent : BaseComponent
    {
        public InputComponent( bool enabledImmediately ) : base( enabledImmediately )
        {
            OnCreated(this, enabledImmediately);
        }

        public static event EventHandler NotifyCreated;
        protected override void OnCreated(BaseComponent sender, Boolean enabledImmediately)
        {
            EventHandler handler = NotifyCreated;
            if (handler != null)
                handler(sender, new EventArgs());

            // dchapman: Ensure that the enabled flag is initially set false; we don't want to raise a Disabled event if it was never enabled! - 29/10/2011
            Enabled = enabledImmediately;
        }

        public virtual event EventHandler NotifyInputEvent;
        public virtual void HandleInputEvent(object sender, EventArgs e)
        {
            EventHandler handler = NotifyInputEvent;
            if (handler != null)
            {
                handler(sender, new EventArgs());
            }
        }
    }
}
