using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public enum effect
    {
        EXPLOSION, SMOKE, EMITTER
    };

    public class particleEmitter
    {
        private List<physObj> particles;
        private int numberOfParticles;
        private Random rnd;
        private effect effect;
    
        public particleEmitter(int numberOfParticles)
        {
            this.numberOfParticles = numberOfParticles;
            rnd = new Random();

            for (int i = 0; i < numberOfParticles; i++)
            {
                particles.Add(new physParticle());
            }

        }

        public void intialiseSystem(Vector2 position, effect effect)
        {
            this.effect = effect;
            foreach (physObj p in particles)
            {
                p.Position = position;
                resetParticle(p);
            }
        
        }

        public void update(ref GameTime gameTime)
        {
            foreach(physObj p in particles)
            {
                if (((iPhysParticle)p).IsActive)
                {
                    p.update(ref gameTime);
                    ((iPhysParticle)p).TimeToLive -= gameTime.ElapsedGameTime.Milliseconds;
                    if (((iPhysParticle)p).TimeToLive < 0)
                    {
                        if (this.effect == effect.EXPLOSION)
                        {
                            ((iPhysParticle)p).IsActive = false;
                        }
                        else
                        {
                            resetParticle(p);
                        }
                    }
                }
                
            }

        }

        public void resetParticle(physObj p)
        {
            switch (this.effect)
            {
                case effect.EXPLOSION:
                    p.Mass = (rnd.Next(1, 10) / 10.0f);
                    p.Velocity = new Vector2((rnd.Next(-100, 100) / 10.0f), (rnd.Next(-100, -10) / 10.0f));
                    p.Force = new Vector2(0, p.Mass * 9.81f);
                    ((iPhysParticle)p).TimeToLive = 2000;
                    ((iPhysParticle)p).IsActive = true;
                    break;

                case effect.EMITTER:
                    p.Mass = (rnd.Next(1, 10) / 10.0f);
                    p.Velocity = new Vector2((rnd.Next(-50, 50) / 10.0f), (rnd.Next(-100, -10) / 10.0f));
                    p.Force = new Vector2(0, p.Mass * 9.81f);
                    ((iPhysParticle)p).TimeToLive = 2000;
                    ((iPhysParticle)p).IsActive = true;
                    break;

                case effect.SMOKE:
                    p.Mass = (rnd.Next(1, 10) / 10.0f);
                    p.Velocity = new Vector2((rnd.Next(-50, 50) / 10.0f), (rnd.Next(-100, -10) / 10.0f));
                    p.Force = new Vector2(0, p.Mass * 9.81f);
                    ((iPhysParticle)p).TimeToLive = 2000;
                    ((iPhysParticle)p).IsActive = true;
                    break;
            }
        }
    }
}
