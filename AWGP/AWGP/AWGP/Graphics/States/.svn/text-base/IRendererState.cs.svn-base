using Microsoft.Xna.Framework.Graphics;

namespace AWGP.Graphics.States
{
    public abstract class IRendererState
    {
        protected IRendererState(RenderTargetEnum renderTarget)
        {
            this.renderTarget = renderTarget;
        }

        readonly private RenderTargetEnum renderTarget;

        public RenderTargetEnum RenderTarget
        {
            get { return renderTarget; }
        }

        public abstract void EnterState(Renderer renderer);
        public abstract void LeaveState(Renderer renderer);
    }
}
