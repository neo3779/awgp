using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    //class extending standard colliding physics object that adds a radius to represent a bounding circle
    public class collidingCirclePhysObj : collidingPhysObj
    {
        private float radius;
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public collidingCirclePhysObj(Vector2 position, Vector2 velocity, Vector2 force, float mass, float radius, float elasticity, Vector2 initialHeading, String id, Boolean isStatic)
            : base(position, velocity, force, mass, elasticity, initialHeading, id, isStatic)
        {
            this.radius = radius;
        }
    }
}
