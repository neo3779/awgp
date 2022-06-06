using System;
using AWGP;
using Microsoft.Xna.Framework.Content;
using AWGP.Components;
using AWGP.Graphics.Components;
using AWGP.Graphics;
using Microsoft.Xna.Framework;


namespace GoopGame
{
    public class Goop : Entity
    {
        public PointLightComponent light;
        public PointSpriteComponent sprite;
        public PhysicsComponent physics;
       
        private Vector2 position, initialForce;
        private float rotation;
        private int numberOfGoop, left, right;
        private float maxRad = MathHelper.ToRadians(360.0f), rad, rotDirection;
        private Random random;

        public float RotDirection
        {
            get { return this.rotDirection; }
        }

        public Goop(ContentManager content, ref Random random, ref int left, ref int right, Vector2 initialVelocity, float mass, int id, Color colour, string texCol, float layer, int numberOfGoop)
            : base(content)
        {
            this.left = left;
            this.right = right;
            this.random = random;
            this.initialForce= initialForce;
            this.numberOfGoop = numberOfGoop;

            this.sprite = new PointSpriteComponent(content, RenderTargetEnum.GBuffer, new Vector3(position.X, position.Y, layer), rotation, new Vector2(0.5f, 0.5f), "Sprites\\Goop" + texCol + "_a");
            this.physics = new PhysicsComponent(position, initialVelocity, initialForce, mass, true, Vector2.Zero, 0.25f, 0.50f, "GOOP_" + id, false);
            this.light = new PointLightComponent(content, RenderTargetEnum.BackBuffer, new Vector3(position.X, position.Y, layer + 0.5f), colour, 1.0f, 2.0f);

            physics.Enabled = true;

            rest();

            this.physics.NotifyPositionChanged += light.HandlePositionChanged;
            this.physics.NotifyPositionChanged += sprite.HandlePositionChanged;
            this.physics.NotifyRotationChanged += sprite.HandleRotationChanged;
            this.physics.NotifyCollision += HandleCollision;

            this.components.Add(sprite);
            this.components.Add(physics);
            this.components.Add(light);
        }

        public void rest()
        {
            this.rad = NextFloat(random, maxRad, 0.0f);
            this.physics.Position = new Vector2(random.Next(left, right), 4.5f);
            this.physics.Velocity = Vector2.Zero;
            this.physics.Rotation = rad;
            this.rotDirection = (float)random.NextDouble()*2 - 1;
        }


        static float NextFloat(Random random, float max, float min)
        {
            // Not a uniform distribution w.r.t. the binary floating-point number line 
            // which makes sense given that NextDouble is uniform from 0.0 to 1.0. 
            // Uniform w.r.t. a continuous number line. 
            // 
            // The range produced by this method is 6.8e38. 
            // 
            // Therefore if NextDouble produces values in the range of 0.0 to 0.1 
            // 10% of the time, we will only produce numbers less than 1e38 about 
            // 10% of the time, which does not make sense. 
            var result = (random.NextDouble()
                          * (max - min))
                          + min;
            return (float)result;
        } 

        public virtual void HandleCollision(object sender, EventArgs<collision> e)
        {
            if (e.Value.collisionType == collisionType.INTERSECTING)
            {
                
                if (e.Value.p1Id == "PLAYER_0" || e.Value.p2Id == "PLAYER_0")
                {
                    rest();
                }
                if (e.Value.p1Id == "PLAYER_1" || e.Value.p2Id == "PLAYER_1")
                {
                    rest();
                }
                if (e.Value.p1Id == "PLAYER_2" || e.Value.p2Id == "PLAYER_2")
                {
                    rest();
                }
                if (e.Value.p1Id == "PLAYER_3" || e.Value.p2Id == "PLAYER_3")
                {
                    rest();
                }

            }

            if (e.Value.collisionType == collisionType.INSIDE)
            {
                if (e.Value.p1Id == "BOTTOM_EDGE" || e.Value.p2Id == "BOTTOM_EDGE")
                {
                    rest();
                }
                if (e.Value.p1Id == "LEFT_EDGE" || e.Value.p2Id == "LEFT_EDGE")
                {
                    physics.Position = new Vector2(physics.Position.X *-1, physics.Position.Y);
                }
                if (e.Value.p1Id == "RIGHT_EDGE" || e.Value.p2Id == "RIGHT_EDGE")
                {
                    physics.Position = new Vector2(physics.Position.X * -1, physics.Position.Y);
                }
            }
            
        }
    }
}
