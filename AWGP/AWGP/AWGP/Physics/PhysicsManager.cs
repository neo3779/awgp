using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;


namespace AWGP
{
    public class PhysicsManager
    {
        //global object lists
        private List<PhysicsObject> physObjects;
        private List<Emitter> emitters; 

        //bounding rectangles for game world
        private Rectangle []boundingRects;

        //force registry
        private ParticleForceRegistry forceRegistry;

        Texture2D tex;
        Color[] color;
        
        public PhysicsManager(Rectangle gameBounds, GraphicsDevice gd)
        {
            physObjects = new List<PhysicsObject>();
            boundingRects = new Rectangle[4];
            forceRegistry = new ParticleForceRegistry();

            //calculate bounding rectangles
            boundingRects[0] = new Rectangle(-10, 0, 10, gameBounds.Height);   //left
            boundingRects[1] = new Rectangle(0, gameBounds.Height, gameBounds.Width, 10);   //bottom
            boundingRects[2] = new Rectangle(gameBounds.Width, 0, 10, gameBounds.Height);   //right
            boundingRects[3] = new Rectangle(0, -10, gameBounds.Width, 10); //top

            tex = new Texture2D(gd, 100, 50);
            color = new Color[100 *50];
            for(int i = 0; i< color.Length; i++)
            {
                color[i] = new Color(1,0,0);
            }
            tex.SetData(color);
        }


        public int CreatePhysObject()
        {
            PhysicsObject p = new PhysicsObject();
            physObjects.Add(p);
            return p.ObjectId;
        }

        public int CreatePhysObject(float invMass, Vector2 velocity, Vector2 position, Rectangle boundingRect)
        {
            PhysicsObject p = new PhysicsObject(invMass, velocity, position, new BoundingSquares(boundingRect,new Rectangle[0]));
            physObjects.Add(p);
            return p.ObjectId;
        }

        public Emitter CreateEmitter(Emitter.updateMethod update, int ttl, int numberOfParticles, Vector2 emitterPosition, bool singleEmission)
        {
            Emitter e = new Emitter(update, ttl, numberOfParticles, emitterPosition, singleEmission);
            emitters.Add(e);
            return e;
        }


        public void Update(GameTime t)
        {
            foreach(PhysicsObject p in physObjects)
            {

                p.ResolveForces(t);
                p.ResetForces();

            }

            foreach (Emitter e in emitters)
            {
                e.Update(t);
            }
        }


        public void collide()
        {

        }
         
        public void render(SpriteBatch sb)
        {
            for(int i = 0; i<physObjects.Count; i++)
            {
                //if ( == typeof(BoundingSquares))
                if(physObjects[i].BoundingShape is BoundingSquares)
                    sb.Draw(tex,((BoundingSquares)physObjects[i].BoundingShape).L1, null, Color.White, physObjects[i].Rotation, Vector2.Zero, SpriteEffects.None, 0);
            }

        }
      

    }
}
