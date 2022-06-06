using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AWGP.Graphics;
using AWGP.Graphics.States;

namespace AWGP.Components
{
    public class AmbientLightComponent : GraphicsComponent, IRenderable
    {
        public AmbientLightComponent(ContentManager content, RenderTargetEnum renderTarget, Color colour, float intensity )
            : base( true )
        {
            this.Position = Vector3.Zero;
            this.effect = content.Load<Effect>("Effects\\AmbientLight");
            this.colour = colour;
            this.intensity = intensity;
            this.renderTarget = renderTarget;
            OnCreated(this, true);
        }

        public void SetEffectParameters()
        {
            Effect.Parameters["LightIntensity"].SetValue(Intensity);
            Effect.Parameters["LightColour"].SetValue(Colour.ToVector3());
        }

        public byte RenderOrder { get { return 1; } }
        public Effect Effect { get { return effect; } }
        public Color Colour { get { return colour; } }
        public float Intensity { get { return intensity; } }
        public IRendererState RendererState { get { return new RendererStateSprite(RenderTarget); } }

        public RenderTargetEnum RenderTarget { get { return renderTarget; } }

        private RenderTargetEnum renderTarget;
        private Effect effect;
        private Color colour;
        private float intensity;
    }
}

namespace AWGP.Graphics
{
    public partial class Renderer
    {
        private void Render(AWGP.Components.AmbientLightComponent light)
        {
            // dchapman: Workaround by setting the textures directly on the sampler. SetValue call for these parameters was getting consistently and inexplicably ignored, verified by PIX, presumably due to some internal XNA framework optimisations/weirdness - 30/10/2011
            GraphicsDevice.Textures[0] = BufferList[RenderTargetEnum.GBuffer][1].RenderTarget;
            light.Effect.Parameters["RT1Texture"].SetValue(BufferList[RenderTargetEnum.GBuffer][1].RenderTarget);

            short[] indexes = { 0, 1, 2, 3 };
            VertexPositionTexture[] vertices = new VertexPositionTexture[4];
            vertices[0].Position = new Vector3(1.0f, -1.0f, 0.0f);
            vertices[0].TextureCoordinate = new Vector2(1.0f, 1.0f);
            vertices[1].Position = new Vector3(-1.0f, -1.0f, 0.0f);
            vertices[1].TextureCoordinate = new Vector2(0.0f, 1.0f);
            vertices[2].Position = new Vector3(1.0f, 1.0f, 1.0f);
            vertices[2].TextureCoordinate = new Vector2(1.0f, 0.0f);
            vertices[3].Position = new Vector3(-1.0f, 1.0f, 0.0f);
            vertices[3].TextureCoordinate = new Vector2(0.0f, 0.0f);

            foreach (EffectPass pass in light.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

                GraphicsDevice.BlendState = BlendState.Additive;

                device.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vertices, 0, 4, indexes, 0, 2);
            }
        }
    }
}