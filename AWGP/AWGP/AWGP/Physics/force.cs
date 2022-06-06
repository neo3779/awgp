using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public enum forceType
    {
        DRAG, VECTOR, DIRECTION, ROTATION
    }


    public class force
    {
        private Vector2 vecForce;
        public Vector2 VecForce
        {
            get { return vecForce; }
        }

        private forceType type;
        public forceType Type
        {
            get { return type; }
        }

        private float durationMilliseconds;
        public float DurationMilliseconds
        {
            get { return durationMilliseconds; }
        }

        private float dragMagnitude;
        public float DragMagnitude
        {
            get { return dragMagnitude; }
        }

        private float magnitude;
        public float Magnitude
        {
            get { return magnitude; }
        }

        private bool isPermenant;
        public bool IsPermenant
        {
            get { return isPermenant; }
        }

        public bool isActive
        {
            get
            {
                if (durationMilliseconds < 0 && !isPermenant)
                    return false;
                else
                    return true;
            }
        }

        public force(Vector2 force, int durationMilliseconds, float dragMagnitude, float magnitude, forceType type)
        {
            this.isPermenant = false;
            this.type = type;
            this.vecForce = force;
            if (durationMilliseconds < 0)
                this.isPermenant = true;
            this.durationMilliseconds = durationMilliseconds;
            this.dragMagnitude = dragMagnitude;
            this.magnitude = magnitude;
        }

        public float decrementDuration(float milliseconds)
        {
            if(!this.isPermenant)
                durationMilliseconds -= milliseconds;

            return durationMilliseconds;
        }
    }
}
