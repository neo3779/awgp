using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public class forceApplier
    {
        private physObj p;
        public physObj P
        {
            get { return p; }
        }

        private force f;
        public force F
        {
            get { return f; }
        }
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
        }

        public forceApplier(physObj p, force f)
        {
            this.p = p;
            this.f = f;
            isActive = true;
        }

        public void applyForce(GameTime t)
        {
            if (f.isActive)
            {
                if (!f.IsPermenant)
                {
                    switch (f.Type)
                    {
                        case forceType.DIRECTION:
                            p.Force += ((f.Magnitude * p.Heading) * (f.DurationMilliseconds / t.ElapsedGameTime.Milliseconds));
                            break;
                        case forceType.DRAG:
                            p.Force += -(p.Velocity * f.DragMagnitude);
                            break;
                        case forceType.VECTOR:
                            p.Force += (f.VecForce * (f.DurationMilliseconds / t.ElapsedGameTime.Milliseconds));
                            break;
                        case forceType.ROTATION:
                            p.Rotation += f.Magnitude * (f.DurationMilliseconds / t.ElapsedGameTime.Milliseconds);
                            break;
                    }
                }
                else
                {
                    switch (f.Type)
                    {
                        case forceType.DIRECTION:
                            p.Force += (f.Magnitude * p.Heading);
                            break;
                        case forceType.DRAG:
                            p.Force += -(p.Velocity * f.DragMagnitude);
                            break;
                        case forceType.VECTOR:
                            p.Force += (f.VecForce);
                            break;
                        case forceType.ROTATION:
                            p.Rotation += f.Magnitude;
                            break;
                    }
                }

            }

            if ((f.decrementDuration(t.ElapsedGameTime.Milliseconds) < 0)&&(!f.IsPermenant))
                isActive = false;

        }
    }
}
