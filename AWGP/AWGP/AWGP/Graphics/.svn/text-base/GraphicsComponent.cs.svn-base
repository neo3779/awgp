using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace AWGP.Graphics
{
    public abstract class GraphicsComponent : BaseComponent
    {
        public GraphicsComponent( bool enabledImmediately ) : base( enabledImmediately )
        {
            position = new Vector3();
            world = new Matrix();
            worldMatrixDirty = true;
        }

        ~GraphicsComponent()
        {
            
        }

        protected virtual Matrix CalculateWorldMatrix()
        {
            return Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);
        }

        private Matrix UpdateWorldMatrix(bool forceAlwaysUpdate = false)
        {
            if (forceAlwaysUpdate || WorldMatrixDirty)
            {
                WorldMatrixDirty = false;
                World = CalculateWorldMatrix();
            }
            return World;
        }

        public static event EventHandler NotifyCreated;
        protected override void OnCreated(BaseComponent sender, Boolean enabledImmediately)
        {
            EventHandler handler = NotifyCreated;
            if (handler != null)
                handler(sender, new EventArgs());

            Enabled = enabledImmediately;
        }

        public virtual void HandlePositionChanged( object sender, EventArgs<Vector2> e )
        {
            Position = new Vector3(e.Value, position.Z);
        }

        public virtual Vector3 Position { get { return position; } set { position = value; WorldMatrixDirty = true; } }
        private Vector3 position;

        public Matrix World { get { if (WorldMatrixDirty) UpdateWorldMatrix(); return world; } protected set { world = value; } }
        private Matrix world;
        
        private bool worldMatrixDirty;
        protected bool WorldMatrixDirty { get { return worldMatrixDirty; } set { worldMatrixDirty = value; } }
    }
}
