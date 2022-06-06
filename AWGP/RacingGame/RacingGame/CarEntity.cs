using System;
using AWGP;
using Microsoft.Xna.Framework.Content;
using AWGP.Components;
using AWGP.Graphics.Components;
using AWGP.Graphics;
using Microsoft.Xna.Framework;

namespace RacingGame
{
    class CarEntity : Entity
    {
        private float preEngFrac = 0, preBrakeFrac = 0, preSteerFrac = 0, trackDrag = 0.8f, dirtDrag = 2.0f ;

        private bool isOnTrack = true;

        public CarEntity(ContentManager content, PlayersName player, String id, Vector2 initialPosition, Vector2 initialHeading)
            : base(content)
        {
            PointSpriteComponent sprite = new PointSpriteComponent(content, RenderTargetEnum.GBuffer, new Vector3(0, 0, RacingGame.CarLayerDepth), 0.0f, Vector2.One, "Sprites\\Car_a_" + player);
            physics = new PhysicsComponent(initialPosition, Vector2.Zero, Vector2.Zero, 1.0f, true, initialHeading, 0.5f, 0.4f, id, false);
            physics.NotifyPositionChanged += sprite.HandlePositionChanged;
            physics.NotifyRotationChanged += sprite.HandleRotationChanged;
            physics.NotifyCollision += HandleCollision;

            sprite.HandlePositionChanged(this, new EventArgs<Vector2>(physics.Position));
            sprite.HandleRotationChanged(this, new EventArgs<float>(physics.Rotation));

            SpotLightComponent headLightA = new SpotLightComponent(content, RenderTargetEnum.BackBuffer, new Vector3( 1.0f, 1.0f, RacingGame.LightLayerDepth), Vector3.UnitY, Color.White, MathHelper.Pi / 8.0f, MathHelper.Pi / 6.0f, 2.5f, 10.0f);
            HeirachyComponent headLightAHeirachy = new HeirachyComponent(new Vector2(0.15f, 0.45f), true, true);
            physics.NotifyPositionChanged += headLightAHeirachy.HandlePositionChanged;
            physics.NotifyRotationChanged += headLightAHeirachy.HandleRotationChanged;
            headLightAHeirachy.NotifyPositionChanged += headLightA.HandlePositionChanged;
            headLightAHeirachy.NotifyRotationChanged += headLightA.HandleRotationChanged;

            headLightAHeirachy.HandlePositionChanged(this, new EventArgs<Vector2>(physics.Position));
            headLightAHeirachy.HandleRotationChanged(this, new EventArgs<float>(physics.Rotation));

            SpotLightComponent headLightB = new SpotLightComponent(content, RenderTargetEnum.BackBuffer, new Vector3(1.0f, 1.0f, RacingGame.LightLayerDepth), Vector3.UnitY, Color.White, MathHelper.Pi / 8.0f, MathHelper.Pi / 6.0f, 2.5f, 10.0f);
            HeirachyComponent headLightBHeirachy = new HeirachyComponent(new Vector2(-0.15f, -0.45f), true, true);
            physics.NotifyPositionChanged += headLightBHeirachy.HandlePositionChanged;
            physics.NotifyRotationChanged += headLightBHeirachy.HandleRotationChanged;
            headLightBHeirachy.NotifyPositionChanged += headLightB.HandlePositionChanged;
            headLightBHeirachy.NotifyRotationChanged += headLightB.HandleRotationChanged;

            headLightBHeirachy.HandlePositionChanged(this, new EventArgs<Vector2>(physics.Position));
            headLightBHeirachy.HandleRotationChanged(this, new EventArgs<float>(physics.Rotation));

            PlayerInputComponent input = new PlayerInputComponent(player, true);
            input.NotifyInputEvent += HandleInputEvent;

            components.Add(sprite);
            components.Add(headLightA);
            components.Add(headLightAHeirachy);
            components.Add(headLightB);
            components.Add(headLightBHeirachy);
            components.Add(input);
            components.Add(physics);
        }

        protected virtual void HandleInputEvent(object sender, EventArgs e)
        {
            PlayerEvent pe = (PlayerEvent)sender;
            const float engineForce = 3.0f;
            const float brakeForce = 3.0f;
            const float steeringPower = 0.1f;

            float engfrac = engineForce/100;
            float brakeFrac = brakeForce/100;
            float steerFrac = steeringPower / 100;

            switch (pe.Move)
            {
                case "Accelerate":
                    switch (pe.Status)
                    {
                        case InputStatus.START:
                            physics.ApplyForce(Vector2.Zero, -1, 0, engineForce, forceType.DIRECTION);
                            break;
                        case InputStatus.STOP:
                            physics.RemoveForce(Vector2.Zero, -1, 0, engineForce, forceType.DIRECTION);
                            break;
                        case InputStatus.NONE:
                            physics.RemoveForce(Vector2.Zero, -1, 0, preEngFrac, forceType.DIRECTION);
                            preEngFrac = engfrac * (pe.VectorPos.Y * 100);
                            physics.ApplyForce(Vector2.Zero, -1, 0, preEngFrac, forceType.DIRECTION);
                            break;
                    }
                    break;
                case "Brake":
                    switch (pe.Status)
                    {
                        case InputStatus.START:
                            physics.ApplyForce(Vector2.Zero, -1, 0, -brakeForce, forceType.DIRECTION);
                            break;
                        case InputStatus.STOP:
                            physics.RemoveForce(Vector2.Zero, -1, 0, -brakeForce, forceType.DIRECTION);
                            break;
                        case InputStatus.NONE:
                            physics.RemoveForce(Vector2.Zero, -1, 0, -preBrakeFrac, forceType.DIRECTION);
                            preBrakeFrac = brakeFrac * (pe.VectorPos.Y * 100);
                            physics.ApplyForce(Vector2.Zero, -1, 0, preBrakeFrac, forceType.DIRECTION);
                            break;
                    }
                    break;
                case "TurnLeft":
                    switch (pe.Status)
                    {
                        case InputStatus.START:
                            physics.ApplyForce(Vector2.Zero, -1, 0, -steeringPower, forceType.ROTATION);
                            break;
                        case InputStatus.STOP:
                            physics.RemoveForce(Vector2.Zero, -1, 0, -steeringPower, forceType.ROTATION);
                            break;
                        case InputStatus.NONE:
                            physics.RemoveForce(Vector2.Zero, -1, 0, preSteerFrac, forceType.ROTATION);
                            preSteerFrac = steerFrac * (pe.VectorPos.X * 100);
                            physics.ApplyForce(Vector2.Zero, -1, 0, preSteerFrac, forceType.ROTATION);
                            break;
                    }

                   break;
                case "TurnRight":


                   switch (pe.Status)
                   {
                       case InputStatus.START:
                           physics.ApplyForce(Vector2.Zero, -1, 0, steeringPower, forceType.ROTATION);
                           break;
                       case InputStatus.STOP:
                           physics.RemoveForce(Vector2.Zero, -1, 0, steeringPower, forceType.ROTATION);
                           break;
                       case InputStatus.NONE:
                           physics.RemoveForce(Vector2.Zero, -1, 0, preSteerFrac, forceType.ROTATION);
                           preSteerFrac = steerFrac * (pe.VectorPos.X * 100);
                           physics.ApplyForce(Vector2.Zero, -1, 0, preSteerFrac, forceType.ROTATION);
                           break;
                   }
                    break;
                case  "STOP":
                    physics.RemoveForce(Vector2.Zero, -1, 0, preSteerFrac, forceType.ROTATION);
                    physics.RemoveForce(Vector2.Zero, -1, 0, preEngFrac, forceType.DIRECTION);
                    physics.RemoveForce(Vector2.Zero, -1, 0, preBrakeFrac, forceType.DIRECTION);
                    preSteerFrac = steerFrac * (pe.VectorPos.Y * 100);
                    preBrakeFrac = brakeFrac * (pe.VectorPos.Y * 100);
                    preSteerFrac = steerFrac * (pe.VectorPos.X * 100);
                    physics.ApplyForce(Vector2.Zero, -1, 0, preSteerFrac, forceType.ROTATION);
                    physics.ApplyForce(Vector2.Zero, -1, 0, preEngFrac, forceType.DIRECTION);
                    physics.ApplyForce(Vector2.Zero, -1, 0, preBrakeFrac, forceType.DIRECTION);

                    break;

                    
            }



        }

        public void applyDrag()
        {
            if (isOnTrack)
            {
                Physics.ApplyForce(Vector2.Zero, 10, trackDrag, 0, forceType.DRAG);
            }
            else
            {
                Physics.ApplyForce(Vector2.Zero, 10, dirtDrag, 0, forceType.DRAG);
            }
        }

        public void setPosition(Vector2 pos, Vector2 heading)
        {
            physics.Position = pos;
            physics.Heading = heading;
        }

        private PhysicsComponent physics;
        public PhysicsComponent Physics
        {
            get { return physics; }
        }

        public virtual void HandleCollision(object sender, EventArgs<collision> e)
        {
            //System.Console.WriteLine();
            //System.Console.Write(e.Value.p1Id);
            //System.Console.Write(" ");
            //System.Console.Write(e.Value.p2Id);
            //System.Console.Write(" ");
            //System.Console.Write(e.Value.collisionType);
            //System.Console.WriteLine();

            //if (physics.PhysicsObject == e.Value.p1)
            //{
            //    System.Console.WriteLine(e.Value.p1.Id);
            //}
            //else if (physics.PhysicsObject == e.Value.p2)
            //{
            //    System.Console.WriteLine(e.Value.p2.Id);
            //}
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
                if (e.Value.p1Id == "TOP_EDGE" || e.Value.p2Id == "TOP_EDGE")
                {
                    collisionManager.collisionReponse(e.Value, physics);
                }
                if (e.Value.p1Id == "BOTTOM_EDGE" || e.Value.p2Id == "BOTTOM_EDGE")
                {
                    collisionManager.collisionReponse(e.Value, physics);
                }
                if (e.Value.p1Id == "TYRE_WALL" || e.Value.p2Id == "TYRE_WALL")
                {
                    collisionManager.collisionReponse(e.Value, physics);
                }
            }


            if (e.Value.p1Id == "TRACK" || e.Value.p2Id == "TRACK")
            {
                if (e.Value.collisionType == collisionType.INSIDE)
                {
                    isOnTrack = true;
                }
                else
                {
                    isOnTrack = false;
                }
            }

            if (this.physics.Id == "CAR_0")
            {
                if ((e.Value.p1Id == "CAR_1" || e.Value.p2Id == "CAR_1") && e.Value.collisionType == collisionType.INTERSECTING)
                {
                    collisionManager.collisionReponse(e.Value, physics);
                }
            }
            if (this.physics.Id == "CAR_1")
            {
                if ((e.Value.p1Id == "CAR_0" || e.Value.p2Id == "CAR_0") && e.Value.collisionType == collisionType.INTERSECTING)
                {
                    collisionManager.collisionReponse(e.Value, physics);
                }
            }


        }
    }
}
