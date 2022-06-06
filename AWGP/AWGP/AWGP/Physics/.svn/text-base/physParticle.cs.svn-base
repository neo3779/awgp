using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public class physParticle : physObj, iPhysParticle
    {
        private int timeToLive;
        public int TimeToLive
        {
            get { return timeToLive; }
            set { timeToLive = value; }
        }
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        private float radius;
        public float Radius
        {
            get { return radius; }
        }

        public physParticle()
            : base()
        {
            timeToLive = 0;
            isActive = false;
        }


        public physParticle(Vector2 position, Vector2 velocity, Vector2 force, float mass, int timeToLive, float radius, Vector2 initialHeading, String id)
            : base(position, velocity, force, mass, initialHeading, id, false)
        {
            this.radius = radius;
            this.timeToLive = timeToLive;
        }

    }

    public class collidingPhysParticle : collidingCirclePhysObj, iPhysParticle
    {
        private int timeToLive;
        public int TimeToLive
        {
            get { return timeToLive; }
            set { timeToLive = value; }
        }
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        //public collidingPhysParticle()
        //    : base()
        //{
        //    timeToLive = 0;
        //    isActive = false;
        //}

        public collidingPhysParticle(Vector2 position, Vector2 velocity, Vector2 force, float mass, float radius, float elasticity, int timeToLive, Vector2 initialHeading, String id)
            : base(position, velocity, force, mass, radius, elasticity, initialHeading, id, false)
        {
            this.timeToLive = timeToLive;
        }

    }
}
