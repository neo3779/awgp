using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace AWGP.Graphics.States
{
    public class RendererStateText : IRendererState
    {
        public RendererStateText()
            : base(RenderTargetEnum.BackBuffer)
        { }

        public override void EnterState(Renderer renderer)
        {
            renderer.SpriteBatch.Begin();
        }

        public override void LeaveState(Renderer renderer)
        {
            renderer.SpriteBatch.End();
        }
    }
}
