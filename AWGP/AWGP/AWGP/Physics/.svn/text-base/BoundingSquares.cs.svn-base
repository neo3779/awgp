using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public class BoundingSquares
    {
        protected Rectangle l1;
        public Rectangle L1
        {
            get { return l1; }
        }
        protected List<Rectangle> l2;


        protected Vector4[] verts;
        public Vector4 []Verts
        {
            get { return verts; }
            set { verts = value; }
        }

        public BoundingSquares(Rectangle L1, Rectangle []L2)
        {
            this.l1 = L1;
            for (int i = 0; i < L2.Length; i++)
            {
                this.l2.Add(L2[i]);
            }

            verts = new Vector4[4];
            verts[0] = Vector4.Zero;
            verts[1] = new Vector4(L1.Width, 0, 0, 1);
            verts[2] = new Vector4(0, L1.Height, 0, 1);
            verts[3] = new Vector4(L1.Width, L1.Height, 0, 1);
        }

    }
}
