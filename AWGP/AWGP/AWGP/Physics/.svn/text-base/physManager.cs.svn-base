using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using AWGP.Components;

namespace AWGP
{
    public class ForceAppliedEventArgs : EventArgs
    {
        public ForceAppliedEventArgs( forceApplier force )
            : base()
        {
            this.force = force;
        }

        public forceApplier Force{ get{ return force; } }
        private forceApplier force;
    }

    public class physManager
    {
        private List<physObj> physObjects;
        private collisionManager collisionManager;
        public collisionManager CollisionsManager
        {
            get { return collisionManager; }
        }
        private Vector2 worldBounds;
        private forceManager forceManager;
        public forceManager ForceManager
        {
            get { return forceManager; }
        }

        public physManager(Vector2 worldBounds)
        {
            physObjects = new List<physObj>();
            collisionManager = new collisionManager(worldBounds);
            forceManager = new forceManager();
            this.worldBounds = worldBounds;
            PhysicsComponent.NotifyCreated += HandleComponentCreated;
        }

        public void registerParticle(physObj p)
        {
            physObjects.Add(p);
            if (p.GetType() == typeof(collidingBoxPhysObj) || p.GetType() == typeof(collidingCirclePhysObj))
            {
                collisionManager.addObject((collidingPhysObj)p);
            }
        }

        public void Update(ref GameTime gameTime)
        {
            forceManager.applyForces(gameTime);
            foreach (physObj ob in physObjects)
            {
                ob.update(ref gameTime);
            }
            collisionManager.checkCollisions();
        }
        /// <summary>
        /// Handle the static event sent when an object of type InputComponent is constructed
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">Empty Arguments</param>
        protected virtual void HandleComponentCreated(object sender, EventArgs e)
        {
            PhysicsComponent component = (PhysicsComponent)sender;
            component.NotifyEnabled += HandleComponentEnabled;
            component.NotifyDisabled += HandleComponentDisabled;
        }

        /// <summary>
        /// Handle the event sent when a component of type InputComponent is enabled
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">Empty Arguments</param>
        protected virtual void HandleComponentEnabled(object sender, EventArgs e)
        {
            PhysicsComponent component = (PhysicsComponent)sender;
            component.ForceApplied += HandleForceApplied;
            component.ForceRemoved += HandleForceRemoved;

            collisionManager.collision += component.HandleCollisions;

            EventArgs<physObj> args = (EventArgs<physObj>)e;

            registerParticle(args.Value);
        }

        /// <summary>
        /// Handle the event sent when a component of type InputComponent is disabled
        /// </summary>
        /// <param name="sender">Object that sent </param>
        /// <param name="e">Empty Arguments</param>
        protected virtual void HandleComponentDisabled(object sender, EventArgs e)
        {
            PhysicsComponent component = (PhysicsComponent)sender;
            component.ForceApplied -= HandleForceApplied;
            component.ForceRemoved -= HandleForceRemoved;

            collisionManager.collision -= component.HandleCollisions;
        }

        protected virtual void HandleForceApplied(object sender, EventArgs<forceApplier> e)
        {
            ForceManager.addForce(e.Value);
        }

        protected virtual void HandleForceRemoved(object sender, EventArgs<forceApplier> e)
        {
            ForceManager.removeForce(e.Value);
        }
    }
}
