using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AWGP;
using System.Threading;

namespace AlexTestGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private InputManager input;
        private SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            try
            {
                input = new InputManager("InputManager");
                input.InputEvent += new EventHandler(input_event);
                input.PlayerEnteredGame += new EventHandler(input_playerEntered);
                input.PlayerLeftGame += new EventHandler(input_playerLeft);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //input = Content.Load<InputManager>(@"InputManager");           

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();
            if (input != null)
            {
                input.Update();
            }
            // TODO: Add your update logic here



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (input != null)
            {
                spriteBatch.Begin();
                string message = input.ToString();
                spriteBatch.DrawString(font, message, new Vector2(50, 50), Color.Yellow);
                spriteBatch.End();
            }



            base.Draw(gameTime);
        }

        void input_event(object sender, EventArgs e)
        {
            PlayerEvent pe = (PlayerEvent)sender;
            Console.WriteLine(pe.ToString());
            switch (pe.PlayerName)
            {
                case PlayersName.PLAYER_0:
                    break;
                case PlayersName.PLAYER_1:
                    break;
                case PlayersName.PLAYER_2:
                    break;
                case PlayersName.PLAYER_3:
                    break;
                case PlayersName.PLAYER_4:
                    break;
                case PlayersName.PLAYER_5:
                    break;
                case PlayersName.PLAYER_6:
                    break;
            }

        }
        void input_playerEntered(object sender, EventArgs e)
        {
            PlayerEvent pe = (PlayerEvent)sender;
            Console.WriteLine(pe.ToString());
        }

        void input_playerLeft(object sender, EventArgs e)
        {
            PlayerEvent pe = (PlayerEvent)sender;
            Console.WriteLine(pe.ToString());
        }
    }
}
