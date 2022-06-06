using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWGP
{
    public struct particleForce
    {
        PhysicsObject p;
        iForceGenerator f;

        public particleForce(ref iForceGenerator f, ref PhysicsObject p)
        {
            this.p = p;
            this.f = f;
        }
    }

    public class ParticleForceRegistry
    {
        List<particleForce> registry;

        public ParticleForceRegistry()
        {
            registry = new List<particleForce>();
        }

        public void registerForce(ref iForceGenerator f, ref PhysicsObject p)
        {
            registry.Add(new particleForce(ref f, ref p));
        }

        public void removeForce(ref iForceGenerator f, ref PhysicsObject p)
        {
            registry.Remove(new particleForce(ref f, ref p));
        }
    }
}
