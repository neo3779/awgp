using System;
using AWGP;
using AWGP.Components;
using AWGP.Graphics;
using AWGP.Graphics.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GoopGame
{
    class GoopCollector : Entity
    {
        public PhysicsComponent physics;
        public SpotLightComponent light;
        public PointSpriteComponent sprite;
        private Vector2 position, initialVelocity, initialForce;
        private float rotation;


        public GoopCollector(ContentManager content, Vector2 position, Vector2 initialVelocity, Vector2 initalForce, float mass, String id, Color color, string texCol, float layer)
            :base(content)
        {
            this.position = position;
            this.initialVelocity = initialVelocity;
            this.initialForce = initialForce;
            this.rotation = rotation;

            this.sprite = new PointSpriteComponent(content, RenderTargetEnum.GBuffer, new Vector3(position.X, position.Y, layer), 0.0f, Vector2.One, "Sprites\\GoopCollector" + texCol + "_a");
            this.physics = new PhysicsComponent(position, initialVelocity, initalForce, mass, false, Vector2.UnitX, id, false);
            this.light = new SpotLightComponent(content, RenderTargetEnum.BackBuffer, new Vector3(position.X, position.Y, layer + 0.5f), Vector3.UnitY, color, MathHelper.Pi / 8.0f, MathHelper.Pi / 6.0f, 2.5f, 5.0f);

            this.physics.Enabled = true;

            this.physics.NotifyPositionChanged += light.HandlePositionChanged;
            this.physics.NotifyPositionChanged += sprite.HandlePositionChanged;
            this.physics.NotifyRotationChanged += sprite.HandleRotationChanged;
            this.physics.NotifyCollision += HandleCollision;

            this.components.Add(sprite);
            this.components.Add(physics);
            this.components.Add(light);
        }
        public virtual void HandleCollision(object sender, EventArgs<collision> e)
        {

            if (e.Value.collisionType == collisionType.INTERSECTING)
            {
                if (e.Value.p1Id == "LEFT_EDGE" || e.Value.p2Id == "LEFT_EDGE")
                {
                    collisionManager.collisionReponse(e.Value, physics);
                }
                if (e.Value.p1Id == "RIGHT_EDGE" || e.Value.p2Id == "RIGHT_EDGE")
                {
                    collisionManager.collisionReponse(e.Value, physics);
                }
            }
        }

        public void rest()
        {
            physics.Position = position;
            physics.Velocity = initialVelocity;
            physics.Rotation = rotation;
        }
    }
}
