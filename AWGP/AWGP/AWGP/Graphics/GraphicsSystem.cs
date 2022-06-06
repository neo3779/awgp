//=============================================================================
//     File: GraphicsSystem.cs
//  Created: 16/10/2011 by dchapman
//=============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AWGP.Graphics
{
    public partial class GraphicsSystem : DrawableGameComponent
    {
        private List<IRenderable> renderables;
        private Renderer renderer;
        
        private GraphicsDeviceManager graphicsDevice;

        //-----------------------------------------------------------------------------
        // Function: GraphicsSystem
        //  Created: 16/10/2011 by dchapman
        //-----------------------------------------------------------------------------
        public GraphicsSystem(Game game) : base(game)
        {
            GraphicsComponent.NotifyCreated += HandleComponentCreated;

            graphicsDevice = new GraphicsDeviceManager(game);
            graphicsDevice.PreparingDeviceSettings += HandlePreparingDeviceSettings;
            
            renderables = new List<IRenderable>();
        }

        ~GraphicsSystem()
        {
           
        }

        protected virtual void HandlePreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }

        protected virtual void HandleComponentCreated(object sender, EventArgs e)
        {
            GraphicsComponent component = (GraphicsComponent)sender;
            component.NotifyEnabled     += HandleComponentEnabled;
            component.NotifyDisabled    += HandleComponentDisabled;
        }

        protected virtual void HandleComponentEnabled(object sender, EventArgs e)
        {
            if (sender is IRenderable)
            {
                renderables.Add((IRenderable)sender);
                return;
            }
            else if (sender is CameraComponent)
            {
                renderer.activeCamera = (CameraComponent)sender;
                return;
            }
        }
        
        protected virtual void HandleComponentDisabled(object sender, EventArgs e)
        {
            if (sender is IRenderable)
            {
                renderables.Remove((IRenderable)sender);
                return;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

       protected override void LoadContent()
       {
           renderer = new Renderer(GraphicsDevice);        
            base.LoadContent();
       }

       protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            // dchapman: Clears all Render Targets and prepares the RendererState for a new frame - 23/10/2011
            renderer.Clear();

            // dchapman: Sorts the collection of renderables according to their desired draw order - 23/10/2011
            renderables.Sort(CompareIRenderables);

            // dchapman: Pass the sorted list of renderables to the Renderer object so that it can visit them each and call the appropriate Render function - 23/10/2011
            renderer.Render(renderables);          

            base.Draw(gameTime);
        }

        private int CompareIRenderables( IRenderable a, IRenderable b )
        {
            if (a.RenderOrder > b.RenderOrder)
                return 1;
            else if (a.RenderOrder < b.RenderOrder)
                return -1;
            else
                return 0;
        }
    }
}