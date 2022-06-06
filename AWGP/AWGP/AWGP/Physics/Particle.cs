using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public class Particle : PhysicsObject
    {
        int ttl;
        public int Ttl
        {
            set { ttl = value; }
            get { return ttl; }
        }

        Boolean isActive;
        public Boolean IsActive
        {
            set { isActive = value; }
            get { return isActive; }
        }

        int spawnTime;
        public int SpawnTime
        {
            set { spawnTime = value; }
            get { return spawnTime; }
        }

        bool isDead;
        public bool IsDead
        {
            set { isDead = value; }
            get { return isDead; }
        }
        
        public Particle(float mass, Vector2 velocity, Vector2 position):
            base(mass, velocity, position, new BoundingSquares(Rectangle.Empty,new Rectangle[0]))
        {

        }
    }
}
