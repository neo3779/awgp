using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public class forceManager
    {
        private List<forceApplier> forces;

        public forceManager()
        {
            forces = new List<forceApplier>();
            
        }

        public void addForce(forceApplier f)
        {
            forces.Add(f);
        }

        public void removeForce(forceApplier f)
        {
            //**WARNING** those with an aversion to long 'if' statements or a weak heart should probably skip this method, dont say i didnt warn you...
            //had to add this monstrosity of an 'if' statement as for some reason forces.Remove(f) would not work, possibly because i am supposed to overide the equals() method of forceApplier, but i dont know.
            for(int i = 0; i<forces.Count; i++)
            {
                if ((forces[i].F.DragMagnitude == f.F.DragMagnitude) 
                    && (forces[i].F.DurationMilliseconds == f.F.DurationMilliseconds) 
                    && (forces[i].F.IsPermenant == f.F.IsPermenant) 
                    && (forces[i].F.Magnitude == f.F.Magnitude) 
                    && (forces[i].F.Type == f.F.Type) 
                    && (forces[i].F.VecForce == f.F.VecForce) 
                    && (forces[i].P.Acceleration == f.P.Acceleration) 
                    && (forces[i].P.Force == f.P.Force) 
                    && (forces[i].P.Heading == f.P.Heading) 
                    && (forces[i].P.Mass == f.P.Mass) 
                    && (forces[i].P.Position == f.P.Position))
                {
                    forces.RemoveAt(i);
                    break;
                }
            }
           
        }

        public void applyForces(GameTime t)
        {
            foreach (forceApplier f in forces)
            {
                f.applyForce(t);
            }

            for (int i = 0; i < forces.Count; i++)
            {
                if (!forces[i].IsActive)
                    forces.RemoveAt(i);
            }
        }



    }
}
