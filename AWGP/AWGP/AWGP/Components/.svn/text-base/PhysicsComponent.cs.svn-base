using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace AWGP.Components
{

    public enum Type
    {
        NONCOLLIDING, COLLIDINGCIRCLE, COLLIDINGSQUARE
    };

    public class PhysicsComponent : BaseComponent
    {
        public PhysicsComponent(Vector2 position, Vector2 initialVelocity, Vector2 initialForce, float mass, bool enabledImmediately, Vector2 initialHeading, String id, Boolean isStatic)
            : base(enabledImmediately)
        {
            physicsObject = new physObj(position, initialVelocity, initialForce, mass, initialHeading, id, isStatic);

            physicsObject.NotifyPositionChanged += HandlePositionChanged;
            physicsObject.NotifyRotationChanged += HandleRotationChanged;
            physicsObject.NotifyVelocityChanged += HandleVelocityChanged;
            physicsObject.NotifyAccelerationChanged += HandleAccelerationChanged;
            physicsObject.NotifyMassChanged += HandleMassChanged;

            OnCreated(this, enabledImmediately);
        }

        public PhysicsComponent(Vector2 position, Vector2 initialVelocity, Vector2 initialForce, float mass, Boolean enabledImmediately, Vector2 initialHeading, float radius, float elasticity, String id, Boolean isStatic)
            : base(enabledImmediately)
        {
            physicsObject = new collidingCirclePhysObj(position, initialVelocity, initialForce, mass, radius, elasticity, initialHeading, id, isStatic);

            physicsObject.NotifyPositionChanged += HandlePositionChanged;
            physicsObject.NotifyRotationChanged += HandleRotationChanged;
            physicsObject.NotifyVelocityChanged += HandleVelocityChanged;
            physicsObject.NotifyAccelerationChanged += HandleAccelerationChanged;
            physicsObject.NotifyMassChanged += HandleMassChanged;

            OnCreated(this, enabledImmediately);
        }

        public PhysicsComponent(Vector2 position, Vector2 initialVelocity, Vector2 initialForce, float mass, Boolean enabledImmediately, Vector2 initialHeading, Vector2 dimensions, float elasticity, String id, Boolean isStatic)
            : base(enabledImmediately)
        {
            physicsObject = new collidingBoxPhysObj(position, initialVelocity, initialForce, mass, dimensions, elasticity, initialHeading, id, isStatic);

            physicsObject.NotifyPositionChanged += HandlePositionChanged;
            physicsObject.NotifyRotationChanged += HandleRotationChanged;
            physicsObject.NotifyVelocityChanged += HandleVelocityChanged;
            physicsObject.NotifyAccelerationChanged += HandleAccelerationChanged;
            physicsObject.NotifyMassChanged += HandleMassChanged;

            OnCreated(this, enabledImmediately);
        }

        public Vector2 Position { get { return physicsObject.Position; } set { physicsObject.Position = value; } }
        public float Rotation { get { return physicsObject.Rotation; } set { physicsObject.Rotation = value; } }
        public float Mass { get { return physicsObject.Mass; } set { physicsObject.Mass = value; } }
        public Vector2 Velocity { get { return physicsObject.Velocity; } set { physicsObject.Velocity = value; } }
        public Vector2 Acceleration { get { return physicsObject.Acceleration; } }
        public Vector2 Force { get { return physicsObject.Force; } }
        public Vector2 Heading { get { return physicsObject.Heading; } set { physicsObject.Heading = value; } }
        public String Id { get { return physicsObject.Id; } }
        public physObj PhysicsObject { get { return physicsObject; } }

        public event EventHandler<EventArgs<forceApplier>> ForceApplied;
        public void ApplyForce(Vector2 force, int durationMilliseconds, float dragMagnitude, float magnitude, forceType type)
        {
            EventHandler<EventArgs<forceApplier>> handler = ForceApplied;
            if (handler != null)
            {
                handler(this, new EventArgs<forceApplier>(new forceApplier(physicsObject, new force(force, durationMilliseconds, dragMagnitude, magnitude, type))));
            }
        }

        public event EventHandler<EventArgs<forceApplier>> ForceRemoved;
        public void RemoveForce(Vector2 force, int durationMilliseconds, float dragMagnitude, float magnitude, forceType type)
        {
            EventHandler<EventArgs<forceApplier>> handler = ForceRemoved;
            if (handler != null)
            {
                handler(this, new EventArgs<forceApplier>(new forceApplier(physicsObject, new force(force, durationMilliseconds, dragMagnitude, magnitude, type))));
            }
        }

        public void Rotate(float rotation)
        {
            physicsObject.rotate(rotation);
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

        public override event EventHandler NotifyEnabled;
        protected override void OnEnabled()
        {
            EventHandler handler = NotifyEnabled;
            if (handler != null)
            {
                handler(this, new EventArgs<physObj>(physicsObject));
            }
        }

        public event EventHandler<EventArgs<Vector2>> NotifyPositionChanged;
        protected virtual void OnPositionChanged( Vector2 position )
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

        public event EventHandler<EventArgs<collision>> NotifyCollision;
        protected virtual void OnCollision(collision col)
        {
            EventHandler<EventArgs<collision>> handler = NotifyCollision;
            if (handler != null)
            {
                handler(this, new EventArgs<collision>(col));
            }
        }

        protected virtual void HandlePositionChanged(object sender, EventArgs<Vector2> e) { OnPositionChanged(e.Value); }
        protected virtual void HandleRotationChanged(object sender, EventArgs<float> e) { OnRotationChanged(e.Value); }
        protected virtual void HandleMassChanged(object sender, EventArgs<float> e) { OnMassChanged(e.Value); }
        protected virtual void HandleVelocityChanged(object sender, EventArgs<Vector2> e) { OnVelocityChanged(e.Value); }
        protected virtual void HandleAccelerationChanged(object sender, EventArgs<Vector2> e) { OnAccelerationChanged(e.Value); }

        public virtual void HandleCollisions(object sender, EventArgs e)
        {
            collisionEventArgs args = (collisionEventArgs)e;
            foreach( collision col in args.collisionsThisFrame )
            {
                if (col.p1 == physicsObject || col.p2 == physicsObject)
                {
                    OnCollision( col );
                }
            }
        }

        private physObj physicsObject;
    }
}
