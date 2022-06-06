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
            View = Matrix.Identity;
            Projection = Matrix.Identity;
            LookAt = lookat;
        }

        protected virtual void RecalculateViewMatrix()
        {
            view = Matrix.CreateLookAt(position, lookAt, Vector3.Up); 
        }

        public Vector3 LookAt
        {
            get { return lookAt; }
            set { lookAt = value; RecalculateViewMatrix(); }
        }

        public override Parameter<Vector3> Position { get { return position; } set { position = value; RecalculateViewMatrix(); } }
        public Matrix World { get { return Matrix.Identity; } }

        public Matrix View
        {
            get { return view; }
            protected set { view = value; }
        }

        public Matrix Projection
        {
            get { return projection; }
            protected set { projection = value; }
        }

        public abstract float FOV
        {
            get;
        }

        private Matrix view;
        private Matrix projection;
        private Vector3 lookAt;
    }
}
