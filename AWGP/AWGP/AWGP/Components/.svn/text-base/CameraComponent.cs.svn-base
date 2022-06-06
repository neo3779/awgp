using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP.Graphics
{
    public abstract class CameraComponent : GraphicsComponent
    {
        public CameraComponent(Vector3 position, Vector3 lookat, Boolean makeActiveCamera)
            : base(makeActiveCamera)
        {
            Position = new Parameter<Vector3>(position);
            Projection = Matrix.Identity;
            LookAt = lookat;
        }

        protected override Matrix CalculateWorldMatrix()
        {
            return Matrix.CreateLookAt(Position, lookAt, Vector3.Up);
        }

        public Vector3 LookAt
        {
            get { return lookAt; }
            set { lookAt = value; WorldMatrixDirty = true; }
        }

        public Matrix View
        {
            get { return World; }
            protected set { World = value; }
        }

        public Matrix Projection
        {
            get { return projection; }
            protected set { projection = value; }
        }

        public Matrix ProjectionInverse
        {
            get { return Matrix.Invert(projection); }
        }

        public abstract float FOV
        {
            get;
        }

        private Matrix projection;
        private Vector3 lookAt;
    }
}
