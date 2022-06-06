using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public class PhysicsObject
    {
        static int nextId;

        protected int objectId;
        public int ObjectId
        {
            get { return objectId; }
        }

        protected float inverseMass;
        public float InverseMass
        {
            get { return inverseMass; }
        }
        protected Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected Vector2 force;
        protected Vector2 acceleration;
        public Vector2 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        protected float collisionDamping;
        public float CollisionDamping
        {
            get { return collisionDamping; }
            set { collisionDamping = value; }
        }

        protected Object boundingShape;
        public Object BoundingShape
        {
            get { return boundingShape; }
        }

        //protected Rectangle l1Box;
        //public Rectangle L1Box
        //{
        //    get { return l1Box; }
        //    set { l1Box = value; }
        //}


  

        //protected List<Rectangle> l2Boxes;
        //public List<Rectangle> L2Box
        //{
        //    get { return L2Box; }
        //}

        protected float rotation;
        public float Rotation
        {
            get { return rotation; }
        }

        protected Matrix rotationMatrix;



        public PhysicsObject()
        {
            inverseMass = 0.0f;
            
            velocity.X = 0;
            velocity.Y = 0;

            position.X = 0;
            position.Y = 0;
            
            force.X = 0;
            force.Y = 0;


        }

        public PhysicsObject(float inverseMass, Vector2 velocity, Vector2 position, Object boundingShape)
        {
            this.boundingShape = boundingShape;

            this.inverseMass = inverseMass;

            this.velocity = velocity;

            this.position = position;

            force.X = 0;
            force.Y = 0;

        }

        public PhysicsObject(float inverseMass, Vector2 velocity, Vector2 position, Rectangle boundingRectangle)
        {
            this.inverseMass = inverseMass;

            this.velocity = velocity;

            this.position = position;

            force.X = 0;
            force.Y = 0;

            rotationMatrix = new Matrix();

            
        }

        public void setPosX(float x)
        {
            position.X = x;
        }

        public void setPosY(float y)
        {
            position.Y = y;
        }

        public void ApplyForce(Vector2 f)
        {
            force += f;
        }


        public void ResetForces()
        {
            force.X = 0;
            force.Y = 0;
        }

        public void ResolveForces(GameTime t)
        {
            acceleration.X = force.X * inverseMass;
            acceleration.Y = force.Y * inverseMass;

            velocity = velocity + (acceleration * (t.ElapsedGameTime.Milliseconds/1000.0f));
        }

        public void UpdatePosition(GameTime t)
        {
            position = position + (velocity * (t.ElapsedGameTime.Milliseconds / 1000.0f));
        }


        public void rotate(float angle)
        {
            rotationMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(angle));

            //only need to ratate bounding shape if its a rectangle
            if (boundingShape.GetType() == typeof(BoundingSquares))
            {
                for (int i = 0; i < 4; i++)
                {
                    ((BoundingSquares)boundingShape).Verts[i] = Vector4.Transform(((BoundingSquares)boundingShape).Verts[i], rotationMatrix);
                }
            }
            
        }

        public static int getId()
        {
            return nextId++;
        }

    }
}
