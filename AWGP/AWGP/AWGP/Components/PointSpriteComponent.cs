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
        public PointSpriteComponent(ContentManager content, RenderTargetEnum renderTarget, Vector3 position, float rotation, Vector2 scale, String albedoAsset ) : base( true )
        {
            this.Position = position;
            this.rotation = rotation;
            this.scale = scale;

            this.effect = content.Load<Effect>("Effects\\PointSpriteOpaque");
            this.albedo = content.Load<Texture2D>(albedoAsset);

            this.renderTarget = renderTarget;

            OnCreated(this, true);
        }

        protected override Matrix CalculateWorldMatrix()
        {
            Matrix scaleMatrix = Matrix.CreateScale(scale.X, scale.Y, 1.0f);
            Matrix rotationMatrix = Matrix.CreateRotationZ(rotation);
            Matrix translateMatrix = Matrix.CreateTranslation(Position);
            return Matrix.Multiply(scaleMatrix, Matrix.Multiply(rotationMatrix, translateMatrix));
        }

        public void SetEffectParameters( )
        {
            Effect.Parameters["AlbedoTexture"].SetValue(albedo);
        }

        public virtual void HandleRotationChanged(object sender, EventArgs<float> e)
        {
            rotation = -e.Value;
            WorldMatrixDirty = true;
        }

        public Effect Effect { get { return effect; } }

        public float Rotation 
        {
            get 
            { 
                return rotation; 
            }
            set
            {
                if (rotation != value)
                {
                    WorldMatrixDirty = true;
                }
                rotation = value;
            }
        }

        public Vector2 Orientation
        {
            get
            {
                return new Vector2((float)Math.Sin(Rotation), (float)Math.Cos(Rotation));
            }
            set
            {
                float newRotation = (float)Math.Atan2(value.Y, value.X);
                Rotation = newRotation;
            }
        }

        public RenderTargetEnum RenderTarget
        {
            get { return renderTarget; }
        }

        public IRendererState RendererState
        {
            get { return new States.RendererStateSprite(RenderTarget); }
        }

        public byte RenderOrder { get { return 0; } }

        private RenderTargetEnum renderTarget;
        private float rotation;
        private Vector2 scale;
        private Effect effect;
        public Texture2D albedo;
        public Texture2D normal;
        public Texture2D specular;
    }
}

namespace AWGP.Graphics
{
    public partial class Renderer
    {
        private void Render(Components.PointSpriteComponent sprite)
        {
            Matrix worldViewProjection = Matrix.Multiply(sprite.World, Matrix.Multiply(ActiveCamera.View, ActiveCamera.Projection));
            sprite.Effect.Parameters["WorldViewProjection"].SetValue(worldViewProjection);

            foreach (EffectPass pass in sprite.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.Textures[0] = sprite.albedo;

                GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

                GraphicsDevice.BlendState = BlendState.Opaque;

                device.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vertices, 0, 4, indexes, 0, 2);
            }
        }
    }
}
