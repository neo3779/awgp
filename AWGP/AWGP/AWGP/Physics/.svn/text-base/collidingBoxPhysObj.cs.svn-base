using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    //class extending the standard colliding physics object that adds an AABB for collision handling
    public class collidingBoxPhysObj:collidingPhysObj
    {
        //vector representing the dimensions of the bounding box
        private Vector2 collisionBox;
        public Vector2 CollisionBox
        {
            get { return collisionBox; }
            set { collisionBox = value; }
        }

        public collidingBoxPhysObj(Vector2 position, Vector2 velocity, Vector2 force, float mass, Vector2 collisionBox, float elasticity, Vector2 initialHeading, String id, Boolean isStatic)
            : base(position, velocity, force, mass, elasticity, initialHeading, id, isStatic)
        {
            this.collisionBox = collisionBox;
        }
    }
}
