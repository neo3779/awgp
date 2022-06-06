using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using AWGP.Components;
using AWGP;
using AWGP.Graphics;
using AWGP.Graphics.Components;

namespace GoopGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GoopGame : Microsoft.Xna.Framework.Game
    {
        private static Random random = new Random();
        private InputManager input;
        private physManager physics;
        private List<Entity> entities;
        private int numberOfGloops = 3;
        private CameraComponent camera;
        private AmbientLightComponent ambientLight;
        private int numberOfPlayers = 2;
        private float startTime = 10000;
        private TextComponent text;
        private float layers;
        private float gravity = -0.5f;
        //apologies for the magic number Dan, will refactor this out later, but at the moment cant access the aspect ratio as graphics havent been initialized at this point!
        public static float screenWidth = 13.7499996975f;
        public static float screenHeight = 8.25f;
        private int left = (int)screenWidth / 2 * -1;
        private int right = (int)screenWidth / 2;
        private float maxRad = MathHelper.ToRadians(360.0f);
        private bool gamego = false;
        private int gameDuration = 30000;
        private Vector3 middle = new Vector3(300, 200, 0);
        private Vector3 topRight = new Vector3(700, 0, 0);
        private bool reset = false;

        /// <summary>
        /// 
        /// </summary>
        public GoopGame()
        {
            // dchapman: Add our GraphicsSystem to the collection of game services - 21/10/2011
            this.Components.Add(new AWGP.Graphics.GraphicsSystem(this));
            Content.RootDirectory = "Content";
            try
            {
                //Create the input check for errors on load
                input = new InputManager("GoopInputManager");
                //Number of goop
                numberOfGloops = numberOfGloops * input.NumberOfPlayers;
                GoopPlayer.Numgoop = numberOfGloops;
                GoopPlayer.Numplayers = numberOfPlayers;

                numberOfPlayers = input.NumberOfPlayers;
            }
            catch (Exception ex)
            {
                //Print errors out
                Console.Write(ex.ToString());
            }
            //Load the physic component 
            physics = new physManager(new Vector2(screenWidth, screenHeight));

            //Add the collision manager to the physics objects
            physics.CollisionsManager.collision += new EventHandler(handleCollisions);

            //Create a list of all the game movable objects.
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
            //Create an ambient light so that you can see all the game wourld
            ambientLight = new AmbientLightComponent(Content, RenderTargetEnum.BackBuffer, Color.White, 0.5f);
            //Create a camera to see the world
            float worldWidth = 8.25f;
            float aspectRatio = (GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height);
            camera = new OrthoCameraComponent(worldWidth * GraphicsDevice.Viewport.AspectRatio, worldWidth, new Vector3(0.0f, 0.0f, 10.0f), new Vector3(0.0f, 0.0f, 0.0f), true);
            //Text
            text = new TextComponent(Content, new Vector2(300, 200), "Pres start to start the game", "font", Color.White);
            //Create a textured backgorund from the textures
            int finishYBackgound = 5, startyBackgornd = finishYBackgound * -1, finishXBackgound = 8, startXBackgornd = finishXBackgound * -1;

            //Bakcground
            for (int j = startyBackgornd; j < finishYBackgound; j++)
            {
                for (int i = startXBackgornd; i < finishXBackgound; i++)
                {
                    PointSpriteComponent background = new PointSpriteComponent(Content,
                                                                               RenderTargetEnum.GBuffer,
                                                                               new Vector3(i, j, layers),
                                                                               0.0f,
                                                                                Vector2.One,
                                                                               "Sprites\\Background_a");
                }
            }

            Color[] color = { Color.Red, Color.Green, Color.Blue, Color.Yellow };
            List<float> forces = new List<float>();
            forces.Add(0.5f); 
            forces.Add(0.5f); 
            forces.Add(-0.5f); 
            forces.Add(-0.5f);
            forces = new List<float>(shuffleList(forces));
            float nextPlayerPlace = -1.75f;
            AWGP.PlayersName name = PlayersName.PLAYER_0;
            float half = numberOfPlayers / 2;
            float startPos = half * -1;
            int colourCount = 0;
            string colString = "";
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (colourCount == numberOfPlayers)
                {
                    colourCount = 0;
                }
                switch (colourCount)
                {
                    case 0:
                        colString = "Red";
                        break;
                    case 1:
                        colString = "Green";
                        break;
                    case 2:
                        colString = "Blue";
                        break;
                    case 3:
                        colString = "Yellow";
                        break;
                }
                layers += 0.00001f;
                //Add player one to the world
                entities.Add(new GoopPlayer(Content, new Vector2(startPos + nextPlayerPlace, -3.5f), name, name.ToString(), color[colourCount], colString, layers, new Vector2(i * 125, 0)));
                nextPlayerPlace += 1.75f;
                layers += 0.00001f;
                //entities.Add(new GoopCollector(Content, new Vector2(startPos, -3.5f), Vector2.Zero, Vector2.Zero, forces[i],  "COLLECTOR_" + name, color[colourCount], colString, layers));
                colourCount++;
                startPos++;
                name++;
            }
            foreach (var p in entities.OfType<GoopPlayer>())
            {
                p.exitGame += new EventHandler(closeGame);
                p.start_stop += new EventHandler(startStop);
                
            }

            foreach (var g in entities.OfType<GoopCollector>())
            {
                g.physics.ApplyForce(new Vector2(-0.5f, 0), -1, 0, 0, AWGP.forceType.VECTOR);
            }

     

            colString = "";
            colourCount = 0;
            //Create the goop for the world
            //GoopPlayer.Numplayers = numberOfPlayers;
            //GoopPlayer.Numgoop = numberOfGloops;
            int intHalf = 0;
            intHalf = numberOfGloops / 2;
            for (int i = -intHalf; i < intHalf; i++)
            {
                if (colourCount == numberOfPlayers)
                {
                    colourCount = 0;
                }

                switch (colourCount)
                {
                    case 0:
                        colString = "Red";
                        break;
                    case 1:
                        colString = "Green";
                        break;
                    case 2:
                        colString = "Blue";
                        break;
                    case 3:
                        colString = "Yellow";
                        break;
                }

                layers += 0.00001f;

                entities.Add(new Goop(Content, ref random, ref left, ref right, Vector2.Zero, (float)random.NextDouble(), i, color[colourCount], colString, layers, numberOfGloops));

                colourCount++;
            }

            foreach (var g in entities.OfType<Goop>())
            {
                g.physics.ApplyForce(new Vector2((float)random.NextDouble() * 2 - 1, gravity), -1, 0, 0, AWGP.forceType.VECTOR);
            }

            resetEverything();
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

            if (input != null)
            {
                input.Update();
            }

            if (gamego)
            {
                if (startTime > 0)
                {
                    startTime -= gameTime.ElapsedGameTime.Milliseconds;
                    text.Text = "Game will start in " + (int)(startTime / 1000);
                    text.Position = middle;
                }
                else
                {
                    if (gameDuration > 0)
                    {
                        reset = false;
                        //Update the input if the input is not null
                        gameDuration -= gameTime.ElapsedGameTime.Milliseconds;
                        text.Text = "Time: " + +(int)(gameDuration / 1000);
                        text.Position = topRight;
                        foreach (var g in entities.OfType<Goop>())
                        {
                            if (g.physics.Rotation >= maxRad || g.physics.Rotation <= maxRad * -1)
                            {
                                g.physics.Rotation = 0.0f;
                            }
                            else
                            {
                                g.physics.Rotation += g.RotDirection;
                            }

                        }
                        //Update the physic component
                        physics.Update(ref gameTime);
                    }
                    else
                    {
                        text.Text = "Game Over.";
                        text.Position = middle;
                        if (!reset)
                        {
                            reset = true;
                            resetEverything();
                        }
                    }
                }

            }
            else
            {
                text.Text = "Press start to start game.";
                text.Position = middle;
                if (startTime != 5000)
                {
                    startTime = 5000;
                }
                if (gameDuration <= 0)
                {
                    gameDuration = 30000;
                }
            }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startStop(object sender, EventArgs e)
        {
            gamego = switchBool(gamego);
        }

        private void scoreIncrease(object sender, EventArgs e)
        {

        }

        private void resetEverything()
        {
            foreach (var g in entities.OfType<Goop>())
            {
                g.rest();

            }
            foreach (var p in entities.OfType<GoopPlayer>())
            {
                p.rest();
            }
            foreach (var c in entities.OfType<GoopCollector>())
            {
                c.rest();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeGame(object sender, EventArgs e)
        {
            Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleCollisions(object sender, EventArgs e)
        {
            collisionEventArgs ce = (collisionEventArgs)e;
            //collision handling here

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool switchBool(bool b)
        {
            return !b;
        }

        private List<E> shuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }
    }
}
