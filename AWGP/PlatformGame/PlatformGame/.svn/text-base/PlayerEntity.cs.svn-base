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
    class PlayerEntity : Entity
    {
        static readonly Vector2[]initialPositions   = { new Vector2(-1.5f,1), new Vector2(0,1), new Vector2(1.5f,1) };
        static readonly Color[] colours             = { Color.Red, Color.Green, Color.Blue };
        static uint playersCreated = 0;

        public PlayerEntity(ContentManager contentManager, PlayersName player)
            : base( contentManager )
        {
            isAtRest = false;

            sprite = new PointSpriteComponent(contentManager, RenderTargetEnum.GBuffer, new Vector3(initialPositions[playersCreated].X, initialPositions[playersCreated].Y, 0), 0, Vector2.One, "Sprites\\Player"+playersCreated+"_a");
            physics = new PhysicsComponent(initialPositions[playersCreated], Vector2.Zero, Vector2.Zero, 1, true, Vector2.UnitX, 0.5f, 0, "Player", false);
            PlayerInputComponent input = new PlayerInputComponent(player, true);

            light = new SpotLightComponent(contentManager, RenderTargetEnum.BackBuffer, new Vector3(initialPositions[playersCreated].X, initialPositions[playersCreated].Y, 0.1f), -Vector3.UnitY, colours[playersCreated], MathHelper.Pi / 8.0f, MathHelper.Pi / 6.0f, 2.0f, 5.0f);

            physics.NotifyPositionChanged += sprite.HandlePositionChanged;
            physics.NotifyPositionChanged += HandlePositionChanged;
            physics.NotifyPositionChanged += light.HandlePositionChanged;
            physics.NotifyVelocityChanged += HandleVelocityChanged;
            physics.NotifyCollision += HandleCollision;
            input.NotifyInputEvent += HandleInputEvent;

            physics.ApplyForce(-Vector2.UnitY, -1, 0, physics.Mass * gravityAcc, forceType.VECTOR);

            components.Add(sprite);
            components.Add(physics);
            components.Add(input);
            components.Add(light);

            ++playersCreated;
        }

        protected virtual void HandleInputEvent(object sender, EventArgs e)
        {
            const float walkForce = 500.0f;
            const float jumpForce = 35.0f;

            PlayerEvent pe = (PlayerEvent)sender;
            switch( pe.Move )
            {
                case "MoveLeft":
                    if (pe.Status == InputStatus.START)
                    {
                        physics.ApplyForce(-Vector2.UnitX, -1, 0, walkForce, forceType.VECTOR);
                    }
                    else if (pe.Status == InputStatus.STOP)
                    {
                        physics.RemoveForce(-Vector2.UnitX, -1, 0, walkForce, forceType.VECTOR);
                    }
                    break;
                case "MoveRight":
                    if (pe.Status == InputStatus.START)
                    {
                        physics.ApplyForce(Vector2.UnitX, -1, 0, walkForce, forceType.VECTOR);
                    }
                    else if (pe.Status == InputStatus.STOP)
                    {
                        physics.RemoveForce(Vector2.UnitX, -1, 0, walkForce, forceType.VECTOR);
                    }
                    break;
                case "Jump":
                    if (pe.Status == InputStatus.START && physics.Velocity.Y >= -float.Epsilon && physics.Velocity.Y <= float.Epsilon)
                    {
                        physics.ApplyForce(Vector2.UnitY, 285, 0, jumpForce, forceType.VECTOR);
                    }
                    break;
            }
        }

        protected virtual void HandlePositionChanged(object sender, EventArgs<Vector2> e)
        {
            if (isAtRest)
            {
                physics.RemoveForce(Vector2.UnitY, -1, 0, physics.Mass * gravityAcc, forceType.VECTOR);
            }

            isAtRest = false;
        }

        protected virtual void HandleVelocityChanged(object sender, EventArgs<Vector2> e)
        {
            float rotation = Math.Min(Math.Max(e.Value.X, -MathHelper.PiOver4), MathHelper.PiOver4);
            sprite.Rotation = -rotation;
            light.Orientation = new Vector3((float)Math.Sin(rotation + MathHelper.Pi), (float)Math.Cos(rotation + MathHelper.Pi), 0);
        }

        public virtual void HandleCollision(object sender, EventArgs<collision> e)
        {
            if (e.Value.collisionType == collisionType.INTERSECTING || e.Value.collisionType == collisionType.INSIDE)
            {
                if (e.Value.p1Id != "Collectible" && e.Value.p2Id != "Collectible")
                {
                    Vector2 previousVelocity = new Vector2(physics.Velocity.X, physics.Velocity.Y);

                    collisionManager.collisionReponse(e.Value, physics);

                    Vector2 collisionVector = (e.Value.p1 == physics.PhysicsObject ? e.Value.collisionVector : -e.Value.collisionVector);
                    if (!isAtRest && Math.Sign(collisionVector.Y) > float.Epsilon)
                    {
                        isAtRest = true;
                        physics.ApplyForce(Vector2.UnitY, -1, 0, physics.Mass * gravityAcc, forceType.VECTOR);
                    }

                    if (isAtRest)
                    {
                        physics.Velocity = new Vector2(previousVelocity.X, physics.Velocity.Y);
                    }
                }
            }
        }

        private const float gravityAcc = 9.81f;
        private Boolean isAtRest;
        private PhysicsComponent physics;
        private PointSpriteComponent sprite;
        private SpotLightComponent light;
    }
}
