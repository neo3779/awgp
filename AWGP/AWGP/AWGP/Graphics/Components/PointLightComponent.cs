using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace AWGP.Graphics.Components
{
    public class PointLightComponent : GraphicsComponent, IRenderable
    {
        public PointLightComponent(ContentManager content, RenderTargetEnum renderTarget, Vector3 position, Color colour, float radius = 10.0f, float intensity = 1.0f)
            : base( true )
        {
            this.position = new Parameter<Vector3>(position);
            this.effect = content.Load<Effect>("Effects\\PointLight");
            this.radius = radius;
            this.colour = colour;
            this.intensity = intensity;
            this.renderTarget = renderTarget;
        }

        public void SetEffectParameters()
        {
            Effect.Parameters["Radius"].SetValue(Radius);
            Effect.Parameters["LightPosition"].SetValue(Position);
            Effect.Parameters["LightIntensity"].SetValue(Intensity);
            Effect.Parameters["LightColour"].SetValue(Colour.ToVector3());
            Effect.Parameters["DistanceMax"].SetValue(2.0f * Radius);
        }
        
        public byte RenderOrder { get { return 1; } }
        public Effect Effect { get { return effect; } }
        public float Radius { get { return radius; } }
        public Color Colour { get { return colour; } }
        public float Intensity { get { return intensity; } }
        public States.IRendererState RendererState { get { return new States.RendererStateSprite(RenderTarget); } }
        public Matrix World { get { return Matrix.Identity; } }
        public RenderTargetEnum RenderTarget { get { return renderTarget; } }

        private RenderTargetEnum renderTarget;
        private Effect effect;
        private float radius;
        private Color colour;
        private float intensity;
    }
}

namespace AWGP.Graphics
{
    public partial class Renderer
    {
        private void Render(Components.PointLightComponent light)
        {
            Matrix world = Matrix.CreateWorld(light.Position, Vector3.Forward, Vector3.Up);
            Matrix viewProjection = Matrix.Multiply(ActiveCamera.View, ActiveCamera.Projection);
            Matrix worldViewProjection = Matrix.Multiply(world, viewProjection);

            light.Effect.Parameters["WorldViewProjection"].SetValue(worldViewProjection);
            light.Effect.Parameters["World"].SetValue(world);
            light.Effect.Parameters["WorldView"].SetValue(Matrix.Multiply(world, ActiveCamera.View));
            light.Effect.Parameters["View"].SetValue(ActiveCamera.View);
            light.Effect.Parameters["ViewProjection"].SetValue(viewProjection);
            light.Effect.Parameters["ScreenDimensions"].SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            light.Effect.Parameters["TanHalfFOV"].SetValue((float)Math.Tan(ActiveCamera.FOV));
            light.Effect.Parameters["FarClip"].SetValue(100.0f);

            // dchapman: Workaround by setting the textures directly on the sampler. SetValue call for these parameters was getting consistently and inexplicably ignored, verified by PIX. - 30/10/2011
            GraphicsDevice.Textures[0] = BufferList[RenderTargetEnum.GBuffer][0].RenderTarget;
            GraphicsDevice.Textures[1] = BufferList[RenderTargetEnum.GBuffer][1].RenderTarget;
            GraphicsDevice.Textures[2] = BufferList[RenderTargetEnum.GBuffer][2].RenderTarget;
            //GraphicsDevice.Textures[0] = null;
            //GraphicsDevice.Textures[1] = null;
            //GraphicsDevice.Textures[2] = null;

            light.Effect.Parameters["RT0Texture"].SetValue(BufferList[RenderTargetEnum.GBuffer][0].RenderTarget);
            light.Effect.Parameters["RT1Texture"].SetValue(BufferList[RenderTargetEnum.GBuffer][1].RenderTarget);
            light.Effect.Parameters["RT2Texture"].SetValue(BufferList[RenderTargetEnum.GBuffer][2].RenderTarget);

            float hypotenuse = (float)Math.Sqrt( Math.Pow(light.Radius * 2.0f, 2.0) );

            short[] indexes = { 0, 1, 2,
                                2, 1, 3 };
            VertexPositionTexture[] vertices = new VertexPositionTexture[4];
            vertices[0].Position = new Vector3( -hypotenuse, hypotenuse, 0 );
            vertices[0].TextureCoordinate = new Vector2(1.0f, 0.0f);
            vertices[1].Position = new Vector3( hypotenuse, hypotenuse, 0 );
            vertices[1].TextureCoordinate = new Vector2(0.0f, 0.0f);
            vertices[2].Position = new Vector3( -hypotenuse, -hypotenuse, 0 );
            vertices[2].TextureCoordinate = new Vector2(1.0f, 1.0f);
            vertices[3].Position = new Vector3( hypotenuse, -hypotenuse, 0 );
            vertices[3].TextureCoordinate = new Vector2(0.0f, 1.0f);

            foreach (EffectPass pass in light.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
                GraphicsDevice.SamplerStates[2] = SamplerState.LinearClamp;

                GraphicsDevice.BlendState = BlendState.Additive;

                device.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 4, indexes, 0, 2);
            }
        }
    }
}