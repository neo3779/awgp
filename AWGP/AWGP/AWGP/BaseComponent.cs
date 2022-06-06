using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWGP
{
    public abstract class BaseComponent
    {
        protected BaseComponent( Boolean enabledImmediately = true )
        {
            // dchapman: Ensure that the enabled flag is initially set false; we don't want to raise a Disabled event if it was never enabled! - 29/10/2011
            this.enabled = false;
        }

        protected abstract void OnCreated(BaseComponent sender, Boolean enabledImmediately);
        protected virtual void OnEnabled()
        {
            EventHandler handler = NotifyEnabled;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected virtual void OnDisabled()
        {
            EventHandler handler = NotifyDisabled;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        public virtual event EventHandler NotifyEnabled;
        public virtual event EventHandler NotifyDisabled;

        public Boolean Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    if (enabled == true && value == false)
                    {
                        OnDisabled();
                    }
                    else
                    {
                        OnEnabled();
                    }

                    enabled = value;
                }
            }
        }

        private Boolean enabled;
    }
}
