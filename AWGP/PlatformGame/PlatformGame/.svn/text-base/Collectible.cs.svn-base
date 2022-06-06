using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using AWGP;
using AWGP.Graphics;
using AWGP.Components;
using AWGP.Graphics.Components;

namespace PlatformGame
{
    class CollectibleEntity : Entity
    {
        public CollectibleEntity(ContentManager contentManager, Vector2 position )
            : base( contentManager )
        {
            sprite = new PointSpriteComponent(contentManager, RenderTargetEnum.GBuffer, new Vector3(position.X, position.Y, 0.1f), 0, Vector2.One / 2.0f, "Sprites\\Collectible_a");
            light = new PointLightComponent(contentManager, RenderTargetEnum.BackBuffer, new Vector3(position.X, position.Y, 0.2f), Color.Yellow, 0.5f, 5);
            physics = new PhysicsComponent(position, Vector2.Zero, Vector2.Zero, float.MaxValue, true, Vector2.UnitY, 0.25f, 0, "Collectible", false);

            physics.NotifyCollision += HandleCollision;
        }

        public virtual void HandleCollision(object sender, EventArgs<collision> e)
        {
            if (e.Value.collisionType != collisionType.OUTSIDE)
            {
                if (e.Value.p1Id == "Player" || e.Value.p2Id == "Player")
                {
                    sprite.Enabled = false;
                    light.Enabled = false;
                    physics.Enabled = false;
                }
            }
        }

        private PointSpriteComponent sprite;
        private PointLightComponent light;
        private PhysicsComponent physics;
    }
}
