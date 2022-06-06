using System;
using System.Collections.Generic;
using AWGP.Graphics.States;
using AWGP.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AWGP.Components
{
    public class TextComponent : GraphicsComponent, IRenderable
    {
        public TextComponent( ContentManager content, Vector2 position, String text, String font, Color color )
            :base(true)
        {
            this.Position = new Vector3( position, 0.0f );
            this.Color = color;
            this.Text = text;
            this.font = content.Load<SpriteFont>(font);
            OnCreated(this, true);
        }

        protected override Matrix CalculateWorldMatrix()
        {
            return Matrix.Identity;
        }

        public void SetEffectParameters(){}

        public virtual void HandlePositionChanged(object sender, EventArgs<Vector3> e)
        {
            Position = e.Value;
        }

        public Effect Effect { get { return null; } }

        public IRendererState RendererState
        {
            get { return new AWGP.Graphics.States.RendererStateText(); }
        }

        public byte RenderOrder { get { return 2; } }

        public SpriteFont Font { get { return font; } }
        public String Text { get { return text; } set { text = value; } }
        public Color Color { get { return color; } set { color = value; } }

        private SpriteFont font;
        private String text;
        private Color color;
    }
}

namespace AWGP.Graphics
{
    public partial class Renderer
    {
        private void Render(AWGP.Components.TextComponent text)
        {
            SpriteBatch.DrawString(text.Font, text.Text, new Vector2( text.Position.X, text.Position.Y ), text.Color);
        }

    }
}