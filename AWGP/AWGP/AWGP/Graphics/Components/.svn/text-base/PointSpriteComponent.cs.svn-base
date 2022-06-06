using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using AWGP.Graphics.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace AWGP.Graphics.Components
{
    public class PointSpriteComponent : GraphicsComponent, IRenderable
    {
        public PointSpriteComponent(ContentManager content, RenderTargetEnum renderTarget, Vector3 position, float rotation, Vector2 scale, String albedoAsset, String normalAsset, String specularAsset ) : base( true )
        {
            this.position = new Parameter<Vector3>(position);
            this.rotation = rotation;
            this.scale = scale;

            Matrix scaleMatrix = Matrix.CreateScale(scale.X, scale.Y, 1.0f);
            Matrix rotationMatrix = Matrix.CreateRotationZ(rotation);
            Matrix translateMatrix = Matrix.CreateTranslation(position);
            world = Matrix.Multiply(scaleMatrix, Matrix.Multiply(rotationMatrix, translateMatrix));

            this.effect = content.Load<Effect>("Effects\\PointSpriteOpaque");
            this.albedo = content.Load<Texture2D>(albedoAsset);
            this.normal = content.Load<Texture2D>(normalAsset);
            //this.specular = content.Load<Texture2D>(specularAsset);

            this.renderTarget = renderTarget;

            //this.specularPower = 0.0f;
            //this.specularIntensity = 0.0f;
        }

        public void SetEffectParameters()
        {
            Effect.Parameters["World"].SetValue(world);
            Effect.Parameters["AlbedoTexture"].SetValue(albedo);
            Effect.Parameters["NormalTexture"].SetValue(normal);
        }

        public Effect Effect { get { return effect; } }

        public RenderTargetEnum RenderTarget
        {
            get { return renderTarget; }
        }

        public IRendererState RendererState
        {
            get { return new States.RendererStateSprite(RenderTarget); }
        }

        public byte RenderOrder { get { return 0; } }

        public Matrix World { get { return world; } }

        private RenderTargetEnum renderTarget;
        private float rotation;
        private Vector2 scale;
        private Effect effect;
        public Texture2D albedo;
        public Texture2D normal;
        //private float specularPower;
        //private float specularIntensity;
        private Matrix world;
    }
}

namespace AWGP.Graphics
{
    public partial class Renderer
    {
        private void Render(Components.PointSpriteComponent sprite)
        {
            sprite.Effect.Parameters["View"].SetValue(ActiveCamera.View);
            sprite.Effect.Parameters["Projection"].SetValue(ActiveCamera.Projection);

            Matrix worldView = Matrix.Multiply(sprite.World, ActiveCamera.View);
            sprite.Effect.Parameters["FarClip"].SetValue(100.0f);

            sprite.Effect.Parameters["WorldView"].SetValue(worldView);

            foreach (EffectPass pass in sprite.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.Textures[0] = sprite.albedo;
                GraphicsDevice.Textures[1] = sprite.normal;
                GraphicsDevice.Textures[2] = null;

                GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;

                GraphicsDevice.BlendState = BlendState.Opaque;

                device.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, vertices, 0, 4, indexes, 0, 2);
            }
        }
    }
}
