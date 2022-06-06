using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace AWGP.Graphics.Components
{
    public class PerspCameraComponent : CameraComponent
    {
        public PerspCameraComponent(float width, float height, Vector3 position, Vector3 lookat, Boolean makeActiveCamera) 
            : base(position, lookat, makeActiveCamera )
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, width / height, 0.1f, 100.0f);
            //Projection = Matrix.CreatePerspective(width/height, 1.0f, 0.1f, 100.0f);
        }

        public override float FOV
        {
            get { return MathHelper.PiOver4; }
        }
    }
}
