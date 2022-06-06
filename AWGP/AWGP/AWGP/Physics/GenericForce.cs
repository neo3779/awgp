using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    class GenericForce:iForceGenerator
    {
        Vector2 force;

        public GenericForce(Vector2 force)
        {
            this.force = force;
        }

        public void applyForce(PhysicsObject p)
        {
            p.ApplyForce(force);
        }
    }
}
