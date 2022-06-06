using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AWGP.Graphics
{
    public interface IRenderable
    {
        void SetEffectParameters();

        States.IRendererState RendererState { get; }
        //RenderTargetEnum RenderTarget { get; }
        Effect Effect { get; }
        byte RenderOrder { get; }
    }
}