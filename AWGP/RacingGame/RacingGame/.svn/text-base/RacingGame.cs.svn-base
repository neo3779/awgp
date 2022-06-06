using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using AWGP;
using AWGP.Components;
using AWGP.Graphics;
using AWGP.Graphics.Components;

namespace RacingGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RacingGame : Microsoft.Xna.Framework.Game
    {
        InputManager input;
        physManager physics;
        List<Entity> entities;

        public static readonly float DirtLayerDepth    = -0.05f;
        public static readonly float RoadLayerDepth    = -0.025f;
        public static readonly float WalllayerDepth    = 0.25f;
        public static readonly float CarLayerDepth     = 0.0f;
        public static readonly float LightLayerDepth   = 0.25f;

        //apologies for the magic number Dan, will refactor this out later, but at the moment cant access the aspect ratio as graphics havent been initialized at this point!
        public static float screenWidth = 13.7499996975f;
        public static float screenHeight = 8.25f;

        public RacingGame()
        {
            // dchapman: Add our GraphicsSystem to the collection of game services - 21/10/2011
            this.Components.Add(new AWGP.Graphics.GraphicsSystem(this));
            Content.RootDirectory = "Content";
            try
            {
                input = new InputManager("InputDefinitions");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            physics = new physManager(new Vector2(screenWidth, screenHeight));
            physics.CollisionsManager.collision +=new EventHandler(handleCollisions); //add collision handling event

            entities = new List<Entity>();
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
            AmbientLightComponent ambient = new AmbientLightComponent(Content, RenderTargetEnum.BackBuffer, Color.White, 0.5f);

            //CameraComponent camera = new PerspCameraComponent(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, new Vector3(0.0f, 0.0f, 10.0f), new Vector3(0.0f, 0.0f, 0.0f), true);
            float worldWidth = 8.25f;
            float aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            CameraComponent camera = new OrthoCameraComponent(worldWidth * GraphicsDevice.Viewport.AspectRatio, worldWidth, new Vector3(0.0f, 0.0f, 10.0f), new Vector3(0.0f, 0.0f, 0.0f), true);

            // dchapman: Top row of dirt - 08/11/2011
            const float dirtScale = 1.65f;
            const int horizCount = 9;
            const int vertCount = 5;
            for (int i = -horizCount / 2; i <= horizCount / 2; ++i)
            {
                PointSpriteComponent sprite = new PointSpriteComponent(Content,
                                                                        RenderTargetEnum.GBuffer,
                                                                        new Vector3((float)i * dirtScale, (float)(vertCount / 2) * dirtScale, RacingGame.DirtLayerDepth),
                                                                        0.0f,
                                                                        new Vector2(dirtScale, dirtScale),
                                                                        "Sprites\\Dirt_a");
            }

            // dchapman: Bottom row of dirt - 08/11/2011
            for (int i = -horizCount / 2; i <= horizCount / 2; ++i)
            {
                PointSpriteComponent sprite = new PointSpriteComponent(Content,
                                                                        RenderTargetEnum.GBuffer,
                                                                        new Vector3((float)i * dirtScale, (float)(-vertCount / 2) * dirtScale, RacingGame.DirtLayerDepth),
                                                                       0.0f,
                                                                        new Vector2(dirtScale, dirtScale),
                                                                        "Sprites\\Dirt_a");
            }

            // dchapman: Left row of dirt - 08/11/2011
            for (int i = -vertCount / 2; i <= vertCount / 2; ++i)
            {
                PointSpriteComponent sprite = new PointSpriteComponent(Content,
                                                                        RenderTargetEnum.GBuffer,
                                                                        new Vector3((float)(-horizCount / 2) * dirtScale, (float)i * dirtScale, RacingGame.DirtLayerDepth),
                                                                        0.0f,
                                                                        new Vector2(dirtScale, dirtScale),
                                                                        "Sprites\\Dirt_a");
            }

            // dchapman: Right row of dirt - 08/11/2011
            for (int i = -vertCount / 2; i <= vertCount / 2; ++i)
            {
                PointSpriteComponent sprite = new PointSpriteComponent(Content,
                                                                        RenderTargetEnum.GBuffer,
                                                                        new Vector3((float)(horizCount / 2) * dirtScale, (float)i * dirtScale, RacingGame.DirtLayerDepth),
                                                                        0.0f,
                                                                        new Vector2(dirtScale, dirtScale),
                                                                        "Sprites\\Dirt_a");
            }

            // dchapman: Road tiles - 08/11/2011
            float roadTileScale = ((float)(Math.Min(vertCount, horizCount) - 2) * dirtScale) / 2.0f;
            { PointSpriteComponent roadTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(-roadTileScale * 2.0f, roadTileScale / 2.0f, RacingGame.RoadLayerDepth), 0.0f, new Vector2(roadTileScale, roadTileScale), "Sprites\\Road_a"); }
            { PointSpriteComponent roadTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(-roadTileScale, roadTileScale / 2.0f, RacingGame.RoadLayerDepth), 0.0f, new Vector2(roadTileScale, roadTileScale), "Sprites\\Road_a"); }
            { PointSpriteComponent roadTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(0.0f, roadTileScale / 2.0f, RacingGame.RoadLayerDepth), 0.0f, new Vector2(roadTileScale, roadTileScale), "Sprites\\Road_a"); }
            { PointSpriteComponent roadTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(roadTileScale, roadTileScale / 2.0f, RacingGame.RoadLayerDepth), 0.0f, new Vector2(roadTileScale, roadTileScale), "Sprites\\Road_a"); }
            { PointSpriteComponent roadTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(roadTileScale * 2.0f, roadTileScale / 2.0f, RacingGame.RoadLayerDepth), 0.0f, new Vector2(roadTileScale, roadTileScale), "Sprites\\Road_a"); }
            { PointSpriteComponent roadTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(-roadTileScale * 2.0f, -roadTileScale / 2.0f, RacingGame.RoadLayerDepth), 0.0f, new Vector2(roadTileScale, roadTileScale), "Sprites\\Road_a"); }
            { PointSpriteComponent roadTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(-roadTileScale, -roadTileScale / 2.0f, RacingGame.RoadLayerDepth), 0.0f, new Vector2(roadTileScale, roadTileScale), "Sprites\\Road_a"); }
            { PointSpriteComponent roadTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(0.0f, -roadTileScale / 2.0f, RacingGame.RoadLayerDepth), 0.0f, new Vector2(roadTileScale, roadTileScale), "Sprites\\Road_a"); }
            { PointSpriteComponent roadTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(roadTileScale, -roadTileScale / 2.0f, RacingGame.RoadLayerDepth), 0.0f, new Vector2(roadTileScale, roadTileScale), "Sprites\\Road_a"); }
            { PointSpriteComponent roadTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(roadTileScale * 2.0f, -roadTileScale / 2.0f, RacingGame.RoadLayerDepth), 0.0f, new Vector2(roadTileScale, roadTileScale), "Sprites\\Road_a"); }

            // dchapman: Wall tiles - 08/11/2011
            { PointSpriteComponent wallTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(-0.5f, 0.0f, RacingGame.WalllayerDepth), 0.0f, Vector2.One, "Sprites\\Wall_a"); }
            { PointSpriteComponent wallTile = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, new Vector3(0.5f, 0.0f, RacingGame.WalllayerDepth), 0.0f, Vector2.One, "Sprites\\Wall_a"); }

            //physics object for the track, to allow collision detection
            collidingBoxPhysObj trackArea = new collidingBoxPhysObj(new Vector2(0, 0), Vector2.Zero, Vector2.Zero, 1.0f, new Vector2(roadTileScale * 5, roadTileScale * 2), 0, Vector2.Zero, "TRACK", true);
            collidingBoxPhysObj tyreWall = new collidingBoxPhysObj(new Vector2(0, 0), Vector2.Zero, Vector2.Zero, 1.0f, new Vector2(2, 1), 0.5f, Vector2.Zero, "TYRE_WALL", true);
            physics.registerParticle(trackArea);
            physics.registerParticle(tyreWall);

            entities.Add( new CarEntity( Content, PlayersName.PLAYER_0, "CAR_0", new Vector2(0, 1.5f), -Vector2.UnitX ));
            entities.Add(new CarEntity(Content, PlayersName.PLAYER_1, "CAR_1", new Vector2(0, -1.5f), Vector2.UnitX));
            //entities.Add(new CarEntity(Content, PlayersName.PLAYER_0, "CAR_2", new Vector2(0, -1.01f), Vector2.UnitX));
            //entities.Add(new CarEntity(Content, PlayersName.PLAYER_1, "CAR_3", new Vector2(0, -2.02f), Vector2.UnitX));

   
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
            if(input != null)
            {
                input.Update();
            }
            foreach (CarEntity c in entities)
            {
                c.applyDrag();
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

        private void handleCollisions(object sender, EventArgs e)
        {
            collisionEventArgs ce = (collisionEventArgs)e;
            //collision handling here
        }

    }
}
