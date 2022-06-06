using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public class physObj
    {
        private String id;
        public String Id
        {
            get { return id; }
        }
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { if (position != value) OnPositionChanged(value); position = value; }
        }

        private Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { if (velocity != value) OnVelocityChanged(value); velocity = value; }
        }

        private Vector2 acceleration;
        public Vector2 Acceleration
        {
            get { return acceleration; }
            set { if (acceleration != value) OnAccelerationChanged(value); acceleration = value; }
        }

        private Vector2 force;
        public Vector2 Force
        {
            get { return force; }
            set { force = value; }
        }

        private float mass;
        public float Mass
        {
            get { return mass; }
            set { if (mass != value) OnMassChanged(value); mass = value; }
        }

        private float rotation;
        public float Rotation
        {
            get 
            {
                return rotation;
            }
            set
            {
                Heading = new Vector2( (float)Math.Sin( value ), (float)Math.Cos( value ) );
                if (rotation != value) OnRotationChanged(value); rotation = value; 
            }
        }

        private Vector2 heading;
        public Vector2 Heading
        {
            get { return heading; }
            set { heading = value; }
        }

        private Boolean isStatic;
        public Boolean IsStatic
        {
            get { return isStatic; }
        }



        private float halfTsquared;

        public physObj()
        {
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            Force = Vector2.Zero;
            Mass = 1;
        }

        public physObj(Vector2 velocity)
        {
            Acceleration = Vector2.Zero;
            Force = Vector2.Zero;
            Mass = 1;
            Velocity = velocity;
        }

        public physObj(Vector2 position, Vector2 velocity, Vector2 force, float mass, Vector2 initialHeading, String id, Boolean isStatic)
        {
            this.id = id;
            Position = position;
            Velocity = velocity;
            Force = force;
            Mass = mass;
            if (initialHeading != Vector2.Zero)
            {
                initialHeading.Normalize();
            }
            else
            {
                initialHeading = Vector2.UnitY;
            }

            if (initialHeading.X > 0)
            {
                if (initialHeading.Y > 0)
                {
                    Rotation = (float) Math.Atan(initialHeading.X / initialHeading.Y);
                }
                else
                {
                    Rotation = (float)(Math.PI / 2 + Math.Atan(initialHeading.Y / initialHeading.X));
                }
            }
            else
            {
                if (initialHeading.Y > 0)
                {
                    Rotation = (float)(Math.PI * 2 - Math.Atan(initialHeading.X / initialHeading.Y));
                }
                else
                {
                    Rotation = (float)(Math.PI + Math.Atan(initialHeading.X / initialHeading.Y));
                }
            }

            //this.heading = initialHeading;

            this.isStatic = isStatic;
        }

        public void rotate(float rotation)
        {
            Matrix rot = new Matrix();
            rot = Matrix.CreateRotationZ(rotation);

            heading = Vector2.Transform(heading, rot);

            //float mag = velocity.Length();

            //velocity = heading * mag;


        }

        public virtual void update(ref GameTime gameTime)
        {
            if (!isStatic)
            {
                halfTsquared = (float)(gameTime.ElapsedGameTime.TotalSeconds * gameTime.ElapsedGameTime.TotalSeconds) * 0.5f;

                Acceleration = Force / Mass;

                Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds + Acceleration * halfTsquared;

                Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;

                Force = Vector2.Zero;
            }
        }

        public event EventHandler<EventArgs<Vector2>> NotifyPositionChanged;
        protected virtual void OnPositionChanged(Vector2 position)
        {
            EventHandler<EventArgs<Vector2>> handler = NotifyPositionChanged;
            if (handler != null)
            {
                handler(this, new EventArgs<Vector2>(position));
            }
        }

        public event EventHandler<EventArgs<float>> NotifyRotationChanged;
        protected virtual void OnRotationChanged(float rotation)
        {
            EventHandler<EventArgs<float>> handler = NotifyRotationChanged;
            if (handler != null)
            {
                handler(this, new EventArgs<float>(rotation));
            }
        }

        public event EventHandler<EventArgs<float>> NotifyMassChanged;
        protected virtual void OnMassChanged(float mass)
        {
            EventHandler<EventArgs<float>> handler = NotifyMassChanged;
            if (handler != null)
            {
                handler(this, new EventArgs<float>(mass));
            }
        }

        public event EventHandler<EventArgs<Vector2>> NotifyVelocityChanged;
        protected virtual void OnVelocityChanged(Vector2 velocity)
        {
            EventHandler<EventArgs<Vector2>> handler = NotifyVelocityChanged;
            if (handler != null)
            {
                handler(this, new EventArgs<Vector2>(velocity));
            }
        }

        public event EventHandler<EventArgs<Vector2>> NotifyAccelerationChanged;
        protected virtual void OnAccelerationChanged(Vector2 acceleration)
        {
            EventHandler<EventArgs<Vector2>> handler = NotifyAccelerationChanged;
            if (handler != null)
            {
                handler(this, new EventArgs<Vector2>(acceleration));
            }
        }
    }
}
