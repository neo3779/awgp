using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    //class extending standard physics object that adds an elasticity for use in collisions.
    public class collidingPhysObj:physObj
    {

        private float elasticity;
        public float Elasticity
        {
            get { return elasticity; }
            set { elasticity = value; }
        }


        public collidingPhysObj(Vector2 position, Vector2 velocity, Vector2 force, float mass, float elasticity, Vector2 initialHeading, String id, Boolean isStatic)
            : base(position, velocity, force, mass, initialHeading, id, isStatic)
        {
            this.elasticity = elasticity;
        }


    }
}
