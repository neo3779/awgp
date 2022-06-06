using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public enum collisionType
    {
        INSIDE, OUTSIDE, INTERSECTING
    }

    //struct representing a collision between 2 objects
    public struct collision
    {
        public collidingPhysObj p1; //object 1
        public collidingPhysObj p2; //object 2
        public Vector2 collisionVector; //the vector between the centres of the colliding objects
        public collisionType collisionType;
        public String p1Id; //the physics ID of object 1
        public String p2Id; //the physics ID of object 2
    }

    //custom event arguments for the collsion event.
    //each frame a list of the collision state of each pair of objects is sent with the event
    public class collisionEventArgs : EventArgs
    {
        public List<collision> collisionsThisFrame; //the list of collisions

        public collisionEventArgs()
        {
            collisionsThisFrame = new List<collision>();           
        }

        public void addCollision(collision c)
        {
            collisionsThisFrame.Add(c);
        }

        public void clear()
        {
            collisionsThisFrame.Clear();
        }
    }

    //an instance of this class is held by the physics manager to handle all collision detection and provide methods for response
    public class collisionManager
    {
        private List<collidingPhysObj>  collidingObjects;       //List of all objects that can collide in the game world
        private Vector2                 worldEdge;              //the size of the game world
        private collisionEventArgs      collisionsThisFrame;    
        public event EventHandler       collision;              //this event is fired once a frame, the args containing a list of the collision state of each pair of objects

        
        public collisionManager(Vector2 worldEdge)
        {
            collidingObjects = new List<collidingPhysObj>();
            collisionsThisFrame = new collisionEventArgs();
            this.worldEdge = worldEdge;

            //create bounding boxes for each edge of the game world, to object-worldedge collision detection 
            collidingBoxPhysObj leftWall = new collidingBoxPhysObj(new Vector2(-worldEdge.X/2 - 2, 0), Vector2.Zero, Vector2.Zero, 0, new Vector2(4, worldEdge.Y), 0, Vector2.Zero, "LEFT_EDGE", true);
            collidingBoxPhysObj rightWall = new collidingBoxPhysObj(new Vector2(worldEdge.X/2 + 2, 0), Vector2.Zero, Vector2.Zero, 0, new Vector2(4, worldEdge.Y), 0, Vector2.Zero, "RIGHT_EDGE", true);
            collidingBoxPhysObj topWall = new collidingBoxPhysObj(new Vector2(0, worldEdge.Y / 2 + 2), Vector2.Zero, Vector2.Zero, 0, new Vector2(worldEdge.X, 4), 0, Vector2.Zero, "TOP_EDGE", true);
            collidingBoxPhysObj bottomtWall = new collidingBoxPhysObj(new Vector2(0, -worldEdge.Y / 2 - 2), Vector2.Zero, Vector2.Zero, 0, new Vector2(worldEdge.X, 4), 0, Vector2.Zero, "BOTTOM_EDGE", true);
            
            //add these to the list of colliding objects
            collidingObjects.Add((collidingPhysObj)leftWall);
            collidingObjects.Add((collidingPhysObj)rightWall);
            collidingObjects.Add((collidingPhysObj)topWall);
            collidingObjects.Add((collidingPhysObj)bottomtWall);
        }

        //called once per frame, checks collisions between each pair of objects in game world and adds collisions to a collisionEventArgs object
        //which is sent off with the collision event, which is fired at the end of the method.
        public void checkCollisions()
        {
            collisionsThisFrame.clear();    //clear the list in the event args, ready for this frames collisions

            //nested loops, so each pair of objects is checked once 
            for(int i = 0; i<collidingObjects.Count; i++)
            {
                for (int j = i+1; j < collidingObjects.Count; j++)
                {
                    if (collidingObjects[i].GetType() == typeof(collidingCirclePhysObj))    //if object has a bounding circle...
                    {
                        if (collidingObjects[j].GetType() == typeof(collidingCirclePhysObj)) //if object has a bounding circle...
                        {
                            //both objects are circles
                            collisionsThisFrame.addCollision(circleCircleCollision((collidingCirclePhysObj)collidingObjects[i], (collidingCirclePhysObj)collidingObjects[j]));                            
                        }
                        else
                        {
                            //obj 1 = circle, obj 2 = square
                            collisionsThisFrame.addCollision(circleSquareCollision((collidingCirclePhysObj)collidingObjects[i], (collidingBoxPhysObj)collidingObjects[j]));                             
                        }
                    }
                    else
                    {
                        if(collidingObjects[j].GetType() == typeof(collidingCirclePhysObj))
                        {
                            //obj 1 = square, obj 2 = circle
                            collisionsThisFrame.addCollision(circleSquareCollision((collidingCirclePhysObj)collidingObjects[j], (collidingBoxPhysObj)collidingObjects[i])); 
                        }
                        else
                        {
                            //both objects are square
                            collisionsThisFrame.addCollision(squareSquareCollision((collidingBoxPhysObj)collidingObjects[i], (collidingBoxPhysObj)collidingObjects[j])); 
                        }
                    }
                }
            }

            //fire the collision event, with the eventArgs containing the collision list
            if (this.collision != null)
            {
                this.collision(this, collisionsThisFrame);
            }
        }

        //register an object with the collision system
        public void addObject(collidingPhysObj  p)
        {
            collidingObjects.Add(p);
        }

        //method to calculate the collision status of a bounding circle and bounding square object
        private static collision circleSquareCollision(collidingCirclePhysObj p1, collidingBoxPhysObj p2)
        {
            collision c = new AWGP.collision();

            //calculate rectangles that bound each object,
            //this sacrifices some accuracy for the circles, if the collision happens in the corner.
            fRectangle p1Box = new fRectangle(p1.Position.X - p1.Radius, p1.Position.Y + p1.Radius, p1.Radius * 2, p1.Radius * 2);
            fRectangle p2Box = new fRectangle(p2.Position.X - (p2.CollisionBox.X / 2), p2.Position.Y + (p2.CollisionBox.Y / 2), p2.CollisionBox.X, p2.CollisionBox.Y);
            
            //calculate the union of the 2 rectangles
            fRectangle union = fRectangle.Union(p1Box,p2Box);

            //if the width or height of the union is greater than the greater than
            //the combined width or height of the colliding objects, they cant be colliding
            if ((union.Width > (p1Box.Width + p2Box.Width)) || (union.Height > (p1Box.Height + p2Box.Height)))
            {
                c.p1 = p1;
                c.p1Id = p1.Id;
                c.p2 = p2;
                c.p2Id = p2.Id;
                c.collisionVector = p1.Position - p2.Position;
                c.collisionType = collisionType.OUTSIDE;
                return c;
            }

            //if the union is the same size as p1, p2 must be inside
            if (((union.Width > (p1Box.Width - 0.00001f)) && (union.Width < (p1Box.Width + 0.00001f))) && (union.Height > (p1Box.Height - 0.00001f)) && (union.Height < (p1Box.Height + 0.00001f)))
            {
                    c.p1 = p2;
                    c.p1Id = p2.Id;
                    c.p2 = p1;
                    c.p2Id = p1.Id;
                    
                    c.collisionVector = p2.Position - p1.Position;
                    c.collisionType = collisionType.INSIDE;
                    return c;
             }

            //if the union is the same size as p2, p1 must be inside
            if (((union.Width > (p2Box.Width - 0.00001f)) && (union.Width < (p2Box.Width + 0.00001f))) && (union.Height > (p2Box.Height - 0.00001f)) && (union.Height < (p2Box.Height + 0.00001f)))
            {
                c.p1 = p1;
                c.p1Id = p1.Id;
                c.p2 = p2;
                c.p2Id = p2.Id;
                c.collisionVector = p1.Position - p2.Position;
                c.collisionType = collisionType.INSIDE;
                return c;
            }

            //if the objects are not inside or outside each other, they must be intersecting
            c.p1 = p1;
            c.p1Id = p1.Id;
            c.p2 = p2;
            c.p2Id = p2.Id;
            c.collisionVector = p1.Position - p2.Position;
            c.collisionType = collisionType.INTERSECTING;
            return c;
                
        }



        //method to calculate the collision status of 2 bounding circle objects
        private static collision circleCircleCollision(collidingCirclePhysObj p1, collidingCirclePhysObj p2)
        {
            collision c = new AWGP.collision();
            Vector2 vecBetweenCentres = p1.Position - p2.Position;
            if (vecBetweenCentres.Length() < (p1.Radius + p2.Radius)) //if length of vec between centres is less than sum of radii..
            {
                if (p1.Radius > p2.Radius) //if circle 1 is bigger, check if crircle 2 is inside it
                {

                    if ((vecBetweenCentres.Length() + p2.Radius) < (p1.Radius))
                    {
                        c.p1 = p2;
                        c.p1Id = p2.Id;
                        c.p2 = p1;
                        c.p2Id = p1.Id;
                        c.collisionVector = p2.Position - p1.Position;
                        c.collisionType = collisionType.INSIDE;
                        return c;
                    }
                    else //if its not inside, must be intersecting
                    {
                        c.p1 = p1;
                        c.p1Id = p1.Id;
                        c.p2 = p2;
                        c.p2Id = p2.Id;
                        c.collisionVector = p1.Position - p2.Position;
                        c.collisionType = collisionType.INTERSECTING;
                        return c;
                    }
                }
                else //if circle 2 is bigger, check if crircle 1 is inside it
                {
                    if ((vecBetweenCentres.Length() + (p1.Radius) < (p2.Radius)))
                    {
                        c.p1 = p1;
                        c.p1Id = p1.Id;
                        c.p2 = p2;
                        c.p2Id = p2.Id;
                        c.collisionVector = p1.Position - p2.Position;
                        c.collisionType = collisionType.INSIDE;
                        return c;
                    }
                    else //if its not inside, must be intersecting
                    {
                        c.p1 = p2;
                        c.p1Id = p2.Id;
                        c.p2 = p1;
                        c.p2Id = p1.Id;
                        c.collisionVector = p2.Position - p1.Position;
                        c.collisionType = collisionType.INTERSECTING;
                        return c;
                    }
                }
            }
            else    //if the circles are not inside each other or intersecting, they must be outside each other
            {
                c.p1 = p1;
                c.p1Id = p1.Id;
                c.p2 = p2;
                c.p2Id = p2.Id;
                c.collisionVector = p1.Position - p2.Position;
                c.collisionType = collisionType.OUTSIDE;
                return c;
            }

        }

        //method to calculate the collision status of 2 bounding square objects
        private static collision squareSquareCollision(collidingBoxPhysObj p1, collidingBoxPhysObj p2)
        {
            collision c = new AWGP.collision();
            //calulate bounding rectangles for the objects, and their union
            fRectangle p1Box = new fRectangle(p1.Position.X - (p1.CollisionBox.X / 2), p1.Position.Y + (p1.CollisionBox.Y / 2), p1.CollisionBox.X, p1.CollisionBox.Y);
            fRectangle p2Box = new fRectangle(p2.Position.X - (p2.CollisionBox.X / 2), p2.Position.Y + (p2.CollisionBox.Y / 2), p2.CollisionBox.X, p2.CollisionBox.Y);
            fRectangle union = fRectangle.Union(p1Box, p2Box);

            //if the width or height of the union is greater than the greater than
            //the combined width or height of the colliding objects, they cant be colliding
            if ((union.Width >= (p1Box.Width + p2Box.Width)) || (union.Height >= (p1Box.Height + p2Box.Height)))
            {
                c.p1 = p1;
                c.p1Id = p1.Id;
                c.p2 = p2;
                c.p2Id = p2.Id;
                c.collisionVector = p1.Position - p2.Position;
                c.collisionType = collisionType.OUTSIDE;
                return c;
            }

            //if the union is the same size as p1, p2 must be inside
            if ((union.Width == p1Box.Width) && (union.Height == p1Box.Height))
            {
                c.p1 = p2;
                c.p1Id = p2.Id;
                c.p2 = p1;
                c.p2Id = p1.Id;
                c.collisionVector = p2.Position - p1.Position;
                c.collisionType = collisionType.INSIDE;
                return c;
            }

            //if the union is the same size as p2, p1 must be inside
            if ((union.Width == p2Box.Width) && (union.Height == p2Box.Height))
            {
                c.p1 = p1;
                c.p1Id = p1.Id;
                c.p2 = p2;
                c.p2Id = p2.Id;
                c.collisionVector = p1.Position - p2.Position;
                c.collisionType = collisionType.INSIDE;
                return c;
            }

            //otherwise, they must be intersecting
            c.p1 = p1;
            c.p1Id = p1.Id;
            c.p2 = p2;
            c.p2Id = p2.Id;
            c.collisionVector = p1.Position - p2.Position;
            c.collisionType = collisionType.INTERSECTING;
            return c;
        }


        //static method to handle collision response between 2 objects, second arg specifys which object to calculate response for
        public static void collisionReponse(collision c, Components.PhysicsComponent obj)
        {
            if (c.p1.GetType() == typeof(collidingCirclePhysObj))
            {
                if (c.p2.GetType() == typeof(collidingCirclePhysObj))
                {
                    //both circles
                    //move objects apart so they are just touching
                    Vector2 vecMovementNeeded;
                    Vector2 normalisedCollisionVector = c.collisionVector;
                    normalisedCollisionVector.Normalize();
                    float movementNeeded;
                    movementNeeded = (((collidingCirclePhysObj)c.p1).Radius + ((collidingCirclePhysObj)c.p2).Radius) - c.collisionVector.Length();
                    vecMovementNeeded = normalisedCollisionVector * movementNeeded;
                    if (!c.p1.IsStatic && !c.p2.IsStatic)
                    {
                        c.p1.Position += vecMovementNeeded / 2;
                        c.p2.Position -= vecMovementNeeded / 2;
                    }
                    else
                    {
                        if (!c.p1.IsStatic)
                        {
                            c.p1.Position += vecMovementNeeded;
                        }
                        else
                        {
                            if (!c.p2.IsStatic)
                            {
                                c.p2.Position -= vecMovementNeeded;
                            }
                        }
                    }

                    //perform collision response for the selected object
                    if (obj.PhysicsObject == c.p1)
                    {
                        bounce(c.collisionVector, (collidingCirclePhysObj)obj.PhysicsObject);
                    }
                    else
                    {
                        bounce(-c.collisionVector, (collidingCirclePhysObj)obj.PhysicsObject);
                    }
                    
                }
                else
                {
                    //obj 1 = circle, obj 2 = square
                    fRectangle p1Box = new fRectangle(c.p1.Position.X - ((collidingCirclePhysObj)c.p1).Radius, c.p1.Position.Y + ((collidingCirclePhysObj)c.p1).Radius, ((collidingCirclePhysObj)c.p1).Radius * 2, ((collidingCirclePhysObj)c.p1).Radius * 2);
                    fRectangle p2Box = new fRectangle(c.p2.Position.X - (((collidingBoxPhysObj)c.p2).CollisionBox.X / 2), c.p2.Position.Y + (((collidingBoxPhysObj)c.p2).CollisionBox.Y / 2), ((collidingBoxPhysObj)c.p2).CollisionBox.X, ((collidingBoxPhysObj)c.p2).CollisionBox.Y);
                    fRectangle union = fRectangle.Union(p1Box, p2Box);

                    //if there is more overlap in the x axis than the y.....
                    if (((p1Box.Height + p2Box.Height) - union.Height) < ((p1Box.Width + p2Box.Width) - union.Width))
                    {
                        //if the first object is higher than the second
                        if (c.p1.Position.Y > c.p2.Position.Y)
                        {
                            //then move the first object up until they are just touching
                            c.p1.Position = new Vector2(c.p1.Position.X, c.p1.Position.Y + ((p1Box.Height + p2Box.Height) - union.Height));                        
                        }
                        else
                        {
                            //then move the first down up until they are just touching
                            c.p1.Position = new Vector2(c.p1.Position.X, c.p1.Position.Y - ((p1Box.Height + p2Box.Height) - union.Height));
                        }
                        
                    }
                    else
                    {
                        if (c.p1.Position.X > c.p2.Position.X)
                        {
                            //then move the first object right until they are just touching
                            c.p1.Position = new Vector2(c.p1.Position.X + ((p1Box.Width + p2Box.Width) - union.Width), c.p1.Position.Y);
                        }
                        else
                        {
                            //then move the first object left until they are just touching
                            c.p1.Position = new Vector2(c.p1.Position.X - ((p1Box.Width + p2Box.Width) - union.Width), c.p1.Position.Y);
                        }
                    }

                    //y - y1 = m(x - x1)
                    float m = c.collisionVector.Y / c.collisionVector.X;

                    //calc where line intercepts top of square
                    //calc if this is within square
                    //if circle is higher must be hitting top
                    //else hitting bottom
                    //if circle is to left must be hitting left

                    float xIntersectWithTopBox = ((p2Box.Top - c.p2.Position.Y) / m) + c.p2.Position.Y;
                    if (xIntersectWithTopBox > p2Box.Left && xIntersectWithTopBox < p2Box.Right)
                    {
                        if (c.p1.Position.Y > c.p2.Position.Y)
                        {
                            //hits top line
                            if (obj.PhysicsObject == c.p1)
                            {
                                bounce(Vector2.UnitY, (collidingPhysObj)obj.PhysicsObject);
                            }
                            else
                            {
                                bounce(-Vector2.UnitY, (collidingPhysObj)obj.PhysicsObject);
                            }
                        }
                        else
                        {
                            //hits bottom line
                            if (obj.PhysicsObject == c.p1)
                            {
                                bounce(-Vector2.UnitY, (collidingPhysObj)obj.PhysicsObject);
                            }
                            else
                            {
                                bounce(Vector2.UnitY, (collidingPhysObj)obj.PhysicsObject);
                            }
                            
                        }
                    }
                    else
                    {
                        if (c.p1.Position.X > c.p2.Position.X)
                        {
                            //hits Right line
                            if (obj.PhysicsObject == c.p1)
                            {
                                bounce(Vector2.UnitX, (collidingPhysObj)obj.PhysicsObject);
                            }
                            else
                            {
                                bounce(-Vector2.UnitX, (collidingPhysObj)obj.PhysicsObject);
                            }
                        }
                        else
                        {
                            //hits Left line
                            if (obj.PhysicsObject == c.p1)
                            {
                                bounce(-Vector2.UnitX, (collidingPhysObj)obj.PhysicsObject);
                            }
                            else
                            {
                                bounce(Vector2.UnitX, (collidingPhysObj)obj.PhysicsObject);
                            }
                            
                        }
                    }
                }
            }
            else
            {
                if (c.p2.GetType() == typeof(collidingCirclePhysObj))
                {
                    //obj 1 = square, obj 2 = circle
                    fRectangle p2Box = new fRectangle(c.p2.Position.X - ((collidingCirclePhysObj)c.p2).Radius, c.p2.Position.Y + ((collidingCirclePhysObj)c.p2).Radius, ((collidingCirclePhysObj)c.p2).Radius * 2, ((collidingCirclePhysObj)c.p2).Radius * 2);
                    fRectangle p1Box = new fRectangle(c.p1.Position.X - (((collidingBoxPhysObj)c.p1).CollisionBox.X / 2), c.p1.Position.Y + (((collidingBoxPhysObj)c.p1).CollisionBox.Y / 2), ((collidingBoxPhysObj)c.p1).CollisionBox.X, ((collidingBoxPhysObj)c.p1).CollisionBox.Y);
                    fRectangle union = fRectangle.Union(p1Box, p2Box);

                    if (((p1Box.Height + p2Box.Height) - union.Height) < ((p1Box.Width + p2Box.Width) - union.Width))
                    {
                        if (c.p1.Position.Y > c.p2.Position.Y)
                        {
                            c.p1.Position = new Vector2(c.p1.Position.X, c.p1.Position.Y + (p1Box.Height + p2Box.Height) - union.Height);
                        }
                        else
                        {
                            c.p1.Position = new Vector2(c.p1.Position.X, c.p1.Position.Y - (p1Box.Height + p2Box.Height) - union.Height);
                        }

                    }
                    else
                    {
                        if (c.p1.Position.X > c.p2.Position.X)
                        {
                            c.p1.Position = new Vector2(c.p1.Position.X + (p1Box.Width + p2Box.Width) - union.Width, c.p1.Position.Y);
                        }
                        else
                        {
                            c.p1.Position = new Vector2(c.p1.Position.X - (p1Box.Width + p2Box.Width) - union.Width, c.p1.Position.Y);
                        }
                    }
                    //y - y1 = m(x - x1)
                    float m = c.collisionVector.Y / c.collisionVector.X;

                    //calc where line intercepts top of square
                    //calc if this is within square
                    //if circle is higher must be hitting top
                    //else hitting bottom
                    //if circle is to left must be hitting left

                    float xIntersectWithTopBox = ((p2Box.Top - c.p2.Position.Y) / m) + c.p2.Position.Y;
                    if (xIntersectWithTopBox > p2Box.Left && xIntersectWithTopBox < p2Box.Right)
                    {
                        if (c.p2.Position.Y > c.p1.Position.Y)
                        {
                            //hits top line
                            if (obj.PhysicsObject == c.p2)
                            {
                                bounce(Vector2.UnitY, (collidingPhysObj)obj.PhysicsObject);
                            }
                            else
                            {
                                bounce(-Vector2.UnitY, (collidingPhysObj)obj.PhysicsObject);
                            }
                        }
                        else
                        {
                            //hits bottom line
                            if (obj.PhysicsObject == c.p2)
                            {
                                bounce(-Vector2.UnitY, (collidingPhysObj)obj.PhysicsObject);
                            }
                            else
                            {
                                bounce(Vector2.UnitY, (collidingPhysObj)obj.PhysicsObject);
                            }

                        }
                    }
                    else
                    {
                        if (c.p2.Position.X > c.p1.Position.X)
                        {
                            //hits Right line
                            if (obj.PhysicsObject == c.p2)
                            {
                                bounce(Vector2.UnitX, (collidingPhysObj)obj.PhysicsObject);
                            }
                            else
                            {
                                bounce(-Vector2.UnitX, (collidingPhysObj)obj.PhysicsObject);
                            }
                        }
                        else
                        {
                            //hits Left line
                            if (obj.PhysicsObject == c.p2)
                            {
                                bounce(-Vector2.UnitX, (collidingPhysObj)obj.PhysicsObject);
                            }
                            else
                            {
                                bounce(Vector2.UnitX, (collidingPhysObj)obj.PhysicsObject);
                            }

                        }
                    }


                }
                else
                {
                    //both square
                    fRectangle p1Box = new fRectangle(c.p1.Position.X - (((collidingBoxPhysObj)c.p1).CollisionBox.X / 2), c.p1.Position.Y + (((collidingBoxPhysObj)c.p1).CollisionBox.Y / 2), ((collidingBoxPhysObj)c.p1).CollisionBox.X, ((collidingBoxPhysObj)c.p1).CollisionBox.Y);
                    fRectangle p2Box = new fRectangle(c.p2.Position.X - (((collidingBoxPhysObj)c.p2).CollisionBox.X / 2), c.p2.Position.Y + (((collidingBoxPhysObj)c.p2).CollisionBox.Y / 2), ((collidingBoxPhysObj)c.p2).CollisionBox.X, ((collidingBoxPhysObj)c.p2).CollisionBox.Y);
                    fRectangle union = fRectangle.Union(p1Box, p2Box);
                    Vector2 v = new Vector2();
                    if (((p1Box.Height + p2Box.Height) - union.Height) < ((p1Box.Width + p2Box.Width) - union.Width))
                    {
                        if (c.p1.Position.Y > c.p2.Position.Y)
                        {
                            c.p1.Position = new Vector2(c.p1.Position.X, c.p1.Position.Y + (p1Box.Height + p2Box.Height) - union.Height);
                            v = new Vector2(0, 1);
                        }
                        else
                        {
                            c.p1.Position = new Vector2(c.p1.Position.X, c.p1.Position.Y - (p1Box.Height + p2Box.Height) - union.Height);
                            v = new Vector2(0, -1);
                        }

                    }
                    else
                    {
                        if (c.p1.Position.X > c.p2.Position.X)
                        {
                            c.p1.Position = new Vector2(c.p1.Position.X + (p1Box.Width + p2Box.Width) - union.Width, c.p1.Position.Y);
                            v = new Vector2(1, 0);
                        }
                        else
                        {
                            c.p1.Position = new Vector2(c.p1.Position.X - (p1Box.Width + p2Box.Width) - union.Width, c.p1.Position.Y);
                            v = new Vector2(-1, 0);
                        }
                    }

                    if (obj.Id == c.p1Id)
                    {
                        bounce(v, (collidingPhysObj)obj.PhysicsObject);
                    }
                    else
                    {
                        bounce(-v, (collidingPhysObj)obj.PhysicsObject);
                    }
                }
            }
        }

        public static void bounce(Vector2 collisionSurfaceNormal, collidingPhysObj p)
        {
            collisionSurfaceNormal.Normalize();
            float projectionMagnitude = Vector2.Dot(-p.Velocity, collisionSurfaceNormal);
            Vector2 projection = projectionMagnitude * collisionSurfaceNormal;
            Vector2 V = p.Velocity + projection;
            p.Velocity = (projection + V) * p.Elasticity;
        }

    }
}
