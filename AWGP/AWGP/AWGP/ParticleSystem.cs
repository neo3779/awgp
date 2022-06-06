using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public class ParticleSystem
    {
        private Particle [] particles;

        public ParticleSystem(int num)
        {
            particles = new Particle[num];

            for (int i = 0; i < num; i++)
            {
                particles[i] = new Particle( 0.0f, Vector2.Zero, Vector2.Zero );
            }
        }

        ~ParticleSystem()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public Particle GetParticle(int i)
        {
            return particles[i];
        }
    }
}
