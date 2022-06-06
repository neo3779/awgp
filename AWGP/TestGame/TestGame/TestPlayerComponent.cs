using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AWGP;
using Microsoft.Xna.Framework;

namespace TestGame
{
    public class TestPlayerComponent : BaseComponent
    {
        public TestPlayerComponent(Vector3 position, InputManager input) : base(true)
        {
            this.position = new Parameter<Vector3>( position );
            input.InputEvent += new EventHandler(HandleInputEvent);
        }

        public static event EventHandler NotifyCreated;
        protected override void OnCreated(BaseComponent sender)
        {
            EventHandler handler = NotifyCreated;
            if (handler != null)
                handler(sender, new EventArgs());
        }

        void HandleInputEvent(object sender, EventArgs e)
        {
            PlayerEvent pe = (PlayerEvent)sender;
            if ( pe.Move == "Right" )
            {
                velocity.X = 1.0f; velocity.Y = 0.0f; velocity.Z = 0.0f;
            }
            else if ( pe.Move == "Left" )
            {
                velocity.X = -1.0f; velocity.Y = 0.0f; velocity.Z = 0.0f;
            }
            else if (pe.Move == "Up")
            {
                velocity.X = 0.0f; velocity.Y = 1.0f; velocity.Z = 0.0f;
            }
            else if (pe.Move == "Down")
            {
                velocity.X = 0.0f; velocity.Y = -1.0f; velocity.Z = 0.0f;
            }
            Console.WriteLine(pe.ToString());
        }

        public void Update(GameTime gameTime)
        {
            position.Data += Vector3.Multiply( velocity, (float)gameTime.ElapsedGameTime.TotalSeconds );
        }

        public virtual Parameter<Vector3> Position { get { return position; } }
        public Parameter<Vector3> position;
        protected Vector3 velocity;
      }
}
