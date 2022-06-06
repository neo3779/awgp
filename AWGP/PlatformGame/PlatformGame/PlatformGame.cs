using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using AWGP;
using AWGP.Graphics;
using AWGP.Components;
using AWGP.Graphics.Components;

namespace PlatformGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PlatformGame : Microsoft.Xna.Framework.Game
    {
        InputManager input;
        physManager physics;

        public PlatformGame()
        {
            // dchapman: Add our GraphicsSystem to the collection of game services - 21/10/2011
            this.Components.Add(new AWGP.Graphics.GraphicsSystem(this));
            Content.RootDirectory = "Content";
            try
            {
                input = new InputManager("PlatformInputManager");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            const float worldHeight = 9.0f;
            Vector2 worldDimensions = new Vector2(worldHeight * GraphicsDevice.Viewport.AspectRatio, worldHeight);

            uint noofTilesY = (uint)Math.Round(worldHeight);
            uint noofTilesX = (uint)Math.Round(worldHeight * GraphicsDevice.Viewport.AspectRatio);

            physics = new physManager(worldDimensions);

            CameraComponent camera = new OrthoCameraComponent(worldDimensions.X, worldDimensions.Y, Vector3.UnitZ, Vector3.Zero, true);

            for (uint i = 0; i < noofTilesX; ++i)
            {
                for (uint j = 0; j < noofTilesY; ++j)
                {
                    Vector2 tilePosition = new Vector2(((float)i - (float)(noofTilesX / 2)), ((float)j - (float)(noofTilesY / 2)));

                    PointSpriteComponent backgroundTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(tilePosition.X, tilePosition.Y, -0.1f), 0, Vector2.One, "Sprites\\Background_a" );

                    if (j == 0
                        || (j == 3 && i >= 5 && i < 10)
                        || (j == 6 && (i < 5 || i >= 10)))
                    {
                        PhysicsComponent wallPhysTile = new PhysicsComponent(tilePosition, Vector2.Zero, Vector2.Zero, float.MaxValue, true, Vector2.UnitY, Vector2.One, 0, "tile", true);
                        PointSpriteComponent wallSpriteTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(tilePosition.X, tilePosition.Y, 0.0f), 0, Vector2.One, "Sprites\\Wall_a");
                    }
                    else if ((j == 1 && i % 2 == 0)
                            || (j == 7 && (i < 5 || i >= 10)))
                    {
                        CollectibleEntity collectible = new CollectibleEntity(Content, tilePosition);
                    }
                }
            }

            PlayersName[] playerNames = { PlayersName.PLAYER_0, PlayersName.PLAYER_1, PlayersName.PLAYER_2 };

            for (uint i = 0; i < input.NumberOfPlayers; ++i)
            {
                PlayerEntity playeri = new PlayerEntity(Content, playerNames[i]);
            }
            
            AmbientLightComponent ambient = new AmbientLightComponent(Content, RenderTargetEnum.BackBuffer, Color.White, 0.5f);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (input != null)
            {
                input.Update();
            }
            physics.Update(ref gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
