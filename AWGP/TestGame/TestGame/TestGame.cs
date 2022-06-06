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
using AWGP.Graphics.Components;

namespace TestGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class TestGame : Microsoft.Xna.Framework.Game
	{
		//List<Entity> entityList;
		TestPlayerComponent test;
		InputManager input;

		public TestGame()
		{
			// dchapman: Add our GraphicsSystem to the collection of game services - 21/10/2011
			this.Components.Add(new AWGP.Graphics.GraphicsSystem(this));
			Content.RootDirectory = "Content";

			//input = new InputManager();

			TestPlayerComponent.NotifyCreated += HandleComponentCreated;
		}

		protected virtual void HandleComponentCreated( object sender, EventArgs e)
		{
			test = (TestPlayerComponent)sender;
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

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// TODO: use this.Content to load your game content here

			CameraComponent camera = new PerspCameraComponent(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, new Vector3(0.0f, 0.0f, 10.0f), new Vector3(0.0f, 0.0f, 0.0f), true);
			
			const int noofSprites = 121;
			PointSpriteComponent[] sprites = new PointSpriteComponent[noofSprites];

			int grid = (int)Math.Ceiling(Math.Sqrt( (float)noofSprites ));
			const float separation = 1.0f;

			for (int x = 0; x < grid; ++x)
			{
				for (int y = 0; y < grid; ++y)
				{
					Vector3 position = new Vector3(((float)(2 * x - (grid - 1)) / 2.0f) * separation,
													((float)(2 * y - (grid - 1)) / 2.0f) * separation,
													0.0f);
					//sprites[x + (y * grid)] = new PointSpriteComponent(Content, RenderTargetEnum.GBuffer, position, 0.0f, Vector2.One);
				}
			}

			PointLightComponent light1 = new AWGP.Graphics.Components.PointLightComponent(Content, RenderTargetEnum.BackBuffer, new Vector3(0.0f, 0.0f, 0.25f), Color.White, 30.0f, 1.5f);
			PointLightComponent light2 = new AWGP.Graphics.Components.PointLightComponent(Content, RenderTargetEnum.BackBuffer, Vector3.Zero, Color.Red, 1.5f, 1.5f);

			TestPlayerComponent testy = new TestPlayerComponent(new Vector3(2.0f, 1.0f, 0.25f), input);

			// dchapman: Link the position of the light to the position of the TestPlayerClass instance - 04/11/2011
			//light2.Position = testy.Position;
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
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

			// TODO: Add your update logic here

			input.Update();

			test.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			// dchapman: GraphicsSystem will draw the scene when base.Draw() is called. - 21/10/2011
			base.Draw(gameTime);
		}
	}
}
