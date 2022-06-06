using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AWGP.Graphics
{
    public enum RenderTargetEnum
    {
        BackBuffer,
        GBuffer
    };

    public partial class Renderer
    {
        public Renderer(GraphicsDevice graphicsDevice)
        {
            this.rendererState = new States.RendererStateNull();
            this.device = graphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);

            RenderTarget2D renderTarget0 = new RenderTarget2D(GraphicsDevice,
                                                              GraphicsDevice.PresentationParameters.BackBufferWidth,
                                                              GraphicsDevice.PresentationParameters.BackBufferHeight,
                                                              false,
                                                              SurfaceFormat.Single,
                                                              DepthFormat.Depth24,
                                                              0,
                                                              RenderTargetUsage.PreserveContents);

            RenderTarget2D renderTarget1 = new RenderTarget2D(GraphicsDevice,
                                                              GraphicsDevice.PresentationParameters.BackBufferWidth,
                                                              GraphicsDevice.PresentationParameters.BackBufferHeight,
                                                              false,
                                                              SurfaceFormat.Color,
                                                              DepthFormat.None,
                                                              0,
                                                              RenderTargetUsage.PreserveContents);

            BufferList = new Dictionary<RenderTargetEnum, RenderTargetBinding[]>();
            BufferList.Add(RenderTargetEnum.GBuffer, new RenderTargetBinding[2] { renderTarget0, renderTarget1 });
            BufferList.Add(RenderTargetEnum.BackBuffer, null);

            indexes = new short[4] { 0, 1, 2, 3 };
            vertices = new VertexPositionTexture[4];
            vertices[0].Position            = new Vector3(-0.5f, 0.5f, 0.0f);
            vertices[0].TextureCoordinate   = new Vector2(0.0f, 0.0f);
            vertices[1].Position            = new Vector3(0.5f, 0.5f, 0.0f);
            vertices[1].TextureCoordinate   = new Vector2(1.0f, 0.0f);
            vertices[2].Position            = new Vector3(-0.5f, -0.5f, 0.0f);
            vertices[2].TextureCoordinate   = new Vector2(0.0f, 1.0f);
            vertices[3].Position            = new Vector3(0.5f, -0.5f, 0.0f);
            vertices[3].TextureCoordinate   = new Vector2(1.0f, 1.0f);
        }

        public void Clear()
        {
            foreach (RenderTargetBinding[] renderTarget in BufferList.Values)
            {
                GraphicsDevice.SetRenderTargets(renderTarget);
                GraphicsDevice.Clear(Color.Black);
            }
            
            RendererState = new States.RendererStateNull();
        }

        public void Render( IEnumerable<IRenderable> renderables )
        {
            foreach (IRenderable renderable in renderables)
            {
                // dchapman: Set the correct renderer state for these renderable - 30/10/2011
                RendererState = renderable.RendererState;

                renderable.SetEffectParameters();

                // dchapman: The 'dynamic' keyword is nicely helping us implement the Visitor Pattern here - 21/10/2011
                Render((dynamic)renderable);
            }
        }

        public void Render( IRenderable renderable )
        {
            // dchapman: Set the correct renderer state for this renderable - 30/10/2011
            RendererState = renderable.RendererState;

            renderable.SetEffectParameters();

            // dchapman: The 'dynamic' keyword is nicely helping us implement the Visitor Pattern here - 21/10/2011
            Render((dynamic)renderable);
        }


        public Dictionary<RenderTargetEnum, RenderTargetBinding[]> BufferList;
        public SpriteBatch SpriteBatch { get { return spriteBatch;} }
        public GraphicsDevice GraphicsDevice { get { return device; } }
        public CameraComponent ActiveCamera { get { return activeCamera; } }
        
        public States.IRendererState RendererState
        {
            get{ return rendererState; }
            set
            {
                // dchapman: If we're moving to an entirely different state, trigger a state change - 29/10/2011
                if (rendererState.GetType() != value.GetType())
                {
                    rendererState.LeaveState(this);
                    value.EnterState(this);
                }

                // dchapman: Even if there hasn't been a state change, there may have been a render target change - 30/10/2011
                device.SetRenderTargets(BufferList[value.RenderTarget]);

                // dchapman: Store the State we've just applied so we can compare against it later - 29/10/2011
                rendererState = value;             
            }
        }

        private States.IRendererState rendererState;
        private SpriteBatch spriteBatch;
        private GraphicsDevice device;
        
        public CameraComponent activeCamera;

        public VertexPositionTexture[] vertices;
        public short[] indexes;
    }
}
