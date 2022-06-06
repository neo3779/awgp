using Microsoft.Xna.Framework.Graphics;

namespace AWGP.Graphics.States
{
    public class RendererStateNull : IRendererState
    {
        public RendererStateNull() : base(RenderTargetEnum.BackBuffer) { }
        public override void EnterState(Renderer renderer) { }
        public override void LeaveState(Renderer renderer) { }
    }
}
