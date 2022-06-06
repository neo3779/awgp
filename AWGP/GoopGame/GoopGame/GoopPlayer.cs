using System;
using AWGP;
using Microsoft.Xna.Framework.Content;
using AWGP.Components;
using AWGP.Graphics.Components;
using AWGP.Graphics;
using Microsoft.Xna.Framework;

namespace GoopGame
{
    internal class GoopPlayer : Entity
    {
        public event EventHandler exitGame, start_stop, win;
        public PhysicsComponent physics;
        public PlayerInputComponent input;
        public SpotLightComponent light;
        public PointSpriteComponent sprite;
        public TextComponent text;

        private static int numplayers;
        public static int Numplayers
        {
            set
            {
                numplayers = value;
            }
        }
        private String texCol;

        private static int numgoop;
        public static int Numgoop
        {
            set
            {
                numgoop = value;
            }
        }

        private static int GoopPerPlayer;


        public int[] goop;

        private int score;

        private Vector2 position;


        public GoopPlayer(ContentManager content, Vector2 position, PlayersName player, String id, Color color, string texCol, float layer,
                          Vector2 texPos)
            : base(content)
        {
            this.position = position;

            this.sprite = new PointSpriteComponent(content, RenderTargetEnum.GBuffer,
                                                   new Vector3(position.X, position.Y, layer), 0.0f,
                                                   new Vector2(1.0f, 1.0f), "Sprites\\GoopCollector" + texCol + "_a");
            this.physics = new PhysicsComponent(position, Vector2.Zero, Vector2.Zero, 1.0f, true, Vector2.UnitX,
                                                new Vector2(0.8f,1.0f), 0.0f, id, false);
            this.light = new SpotLightComponent(content, RenderTargetEnum.BackBuffer,
                                                 new Vector3(position.X, position.Y, layer + 0.3f), Vector3.UnitY, color, 
                                                 MathHelper.Pi / 4.0f, MathHelper.Pi / 3.5f, 3.0f, 3.0f);
            this.input = new PlayerInputComponent(player, true);
            this.text = new TextComponent(content, texPos, "Player " + texCol + Environment.NewLine + "Score: " + score,
                                          "font", color);

            physics.Enabled = true;

            this.texCol = texCol;

            //this.physics.NotifyPositionChanged += lightHeirachy.HandlePositionChanged;
            this.physics.NotifyPositionChanged += light.HandlePositionChanged;
            this.physics.NotifyPositionChanged += sprite.HandlePositionChanged;
            this.physics.NotifyRotationChanged += sprite.HandleRotationChanged;
            this.physics.NotifyCollision += HandleCollision;
            this.input.NotifyInputEvent += HandleInputEvent;

            this.components.Add(sprite);
            this.components.Add(physics);
            this.components.Add(light);
            this.components.Add(input);

            GoopPerPlayer = numgoop / numplayers;
            goop = new int[GoopPerPlayer];
            
            int goopindex = 0;
            

            if (color == Color.Red)
            {
                for (int i = 0; i < numgoop; i+=numplayers)
                {
                    goop[goopindex] = i - numgoop / 2;
                    goopindex++;
                }
            }
            if (color == Color.Green)
            {
                for (int i = 1; i < numgoop; i += numplayers)
                {
                    goop[goopindex] = i - numgoop / 2;
                    goopindex++;
                }
            }
            if (color == Color.Blue)
            {
                for (int i = 2; i < numgoop; i += numplayers)
                {
                    goop[goopindex] = i - numgoop / 2;
                    goopindex++;
                }
            }
            if (color == Color.Yellow)
            {
                for (int i = 3; i < numgoop; i += numplayers)
                {
                    goop[goopindex] = i - numgoop / 2;
                    goopindex++;
                }
            }

            //for (int i = 0; i < numgoop; i++)
            //{
            //    if(i % numgoop
            //}

        }

        public void rest()
        {
            //physics.Position = position;
            //physics.Velocity = initialVelocity;
            //physics.Rotation = rotation;
        }

        protected virtual void HandleInputEvent(object sender, EventArgs e)
        {
            PlayerEvent pe = (PlayerEvent)sender;

            switch (pe.Move)
            {
                case "START_STOP":
                    switch (pe.Status)
                    {
                        case InputStatus.START:
                            if (this.start_stop != null)
                            {
                                this.start_stop(this, new EventArgs());
                            }
                            break;
                    }
                    break;
                case "EXIT":
                    switch (pe.Status)
                    {
                        case InputStatus.START:
                            if (this.exitGame != null)
                            {
                                this.exitGame(this, new EventArgs());
                            }
                            break;
                    }
                    break;
                case "RIGHT":
                    switch (pe.Status)
                    {
                        case InputStatus.START:
                            physics.ApplyForce(new Vector2(4,0), -1, 0, 1.0f, forceType.VECTOR);
                            break;
                        case InputStatus.STOP:
                            physics.RemoveForce(new Vector2(4, 0), -1, 0, 1.0f, forceType.VECTOR);
                            break;
                    }
                    break;
                case "LEFT":


                    switch (pe.Status)
                    {
                        case InputStatus.START:
                            physics.ApplyForce(new Vector2(-4, 0), -1, 0, 1.0f, forceType.VECTOR);
                            break;
                        case InputStatus.STOP:
                            physics.RemoveForce(new Vector2(-4, 0), -1, 0, 1.0f, forceType.VECTOR);
                            break;
                    }
                    break;

            }
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
                for (int i = 0; i < GoopPerPlayer; i++)
                {
                    if (e.Value.p1Id == "GOOP_" + goop[i].ToString() || e.Value.p2Id == "GOOP_" + goop[i].ToString())
                    {
                        score++;
                        text.Text = "Player " + texCol + Environment.NewLine + "Score: " + score;
                        if (score == 10)
                        {
                            if (this.win != null)
                            {
                                this.win(this, new EventArgs());
                            }
                        }
                    }
                }
            }
        }
    }
}
