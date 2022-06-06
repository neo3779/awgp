using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace AWGP
{
    public class fRectangle
    {
        #region Variables
        private Vector2 position;
        private Vector2 size;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public float Width
        {
            get{ return size.X; }
        }

        public float Height
        {
            get { return size.Y; }
        }

        public float Left
        {
            get { return position.X; } 
        }

        public float Right
        {
            get { return position.X + size.X; }
        }

        public float Top
        {
            get { return position.Y; }
        }

        public float Bottom
        {
            get { return position.Y - size.Y; }
        }
        #endregion

        #region Constructors
        public fRectangle(float x, float y, float width, float height)
        {
            position = new Vector2(x,y);
            size = new Vector2(width, height);
        }
        public fRectangle(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }
        #endregion

        #region Static Methods
        public static fRectangle Union(fRectangle rectA, fRectangle rectB)
        {
            float x = Math.Min(rectA.Left, rectB.Left);
            float y = Math.Max(rectA.Top, rectB.Top);

            float width = Math.Abs(Math.Max(rectA.Right, rectB.Right)- x);
            float height = Math.Abs(Math.Min(rectA.Bottom, rectB.Bottom) - y);

            return new fRectangle(x, y, width, height);
        }
        #endregion
    }
}
