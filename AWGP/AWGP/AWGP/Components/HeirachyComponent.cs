using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP.Components
{
    public class HeirachyComponent : BaseComponent
    {
        public HeirachyComponent( Vector2 offset, bool inheritPosition, bool inheritRotation ) 
            : base( true )
        {
            Offset = offset;
            this.inheritPosition = inheritPosition;
            this.inheritRotation = inheritRotation;
            OnCreated(this, true);
        }

        private Vector2 CalculatePosition( Vector2 position, float rotation )
        {
            return new Vector2( position.X + offsetMagnitude * (float)Math.Sin( rotation + offsetAngle ), position.Y + offsetMagnitude * (float)Math.Cos( rotation + offsetAngle ) );
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
            currentPosition = e.Value;

            if (Enabled)
            {
                if (inheritPosition)
                {
                    OnPositionChanged( CalculatePosition(e.Value, currentRotation) );
                }
            }
        }

        public virtual void HandleRotationChanged( object sender, EventArgs<float> e)
        {
            currentRotation = e.Value;

            if (Enabled)
            {
                if (inheritRotation)
                {
                    OnRotationChanged(e.Value);
                }
                if (inheritPosition)
                {
                    OnPositionChanged(CalculatePosition(currentPosition, e.Value));
                }
            }
        }

        public event EventHandler<EventArgs<Vector2>> NotifyPositionChanged;
        protected virtual void OnPositionChanged( Vector2 value )
        {
            EventHandler<EventArgs<Vector2>> handler = NotifyPositionChanged;
            if (handler != null)
            {
                handler(this, new EventArgs<Vector2>(value));
            }
        }

        public event EventHandler<EventArgs<float>> NotifyRotationChanged;
        protected virtual void OnRotationChanged(float value)
        {
            EventHandler<EventArgs<float>> handler = NotifyRotationChanged;
            if (handler != null)
            {
                handler(this, new EventArgs<float>(value));
            }
        }

        private float offsetMagnitude;
        private float offsetAngle;

        private Vector2 currentPosition;
        private float currentRotation;

        private bool inheritPosition;
        public bool InheritPosition { get { return inheritPosition; } set { inheritPosition = value; } }
        
        private bool inheritRotation;
        public bool InheritRotation { get { return inheritRotation; } set { inheritRotation = value; } }

        public Vector2 Offset 
        { 
            set 
            {
                offsetMagnitude = value.Length();
                offsetAngle = ( Math.Abs(offsetMagnitude) >= float.Epsilon ? (float)Math.Asin(value.X / offsetMagnitude) : 0.0f );
            } 
        }
    }
}
