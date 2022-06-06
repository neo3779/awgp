using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace AWGP.Graphics
{
    public abstract class GraphicsComponent : BaseComponent
    {
        public GraphicsComponent( bool enabledImmediately ) : base( enabledImmediately )
        {
        }

        public static event EventHandler NotifyCreated;
        protected override void OnCreated( BaseComponent sender )
        {
            EventHandler handler = NotifyCreated;
            if (handler != null)
                handler(sender, new EventArgs());
        }

        public virtual Parameter<Vector3> Position { get { return position; } set { position = value; } }
        protected Parameter<Vector3> position;

        //protected Effect effect;
        //protected States.IRendererState rendererState;
    }
}
