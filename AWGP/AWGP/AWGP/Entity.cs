using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace AWGP
{
    public abstract class Entity
    {
        protected Entity(ContentManager content) 
        {
            components = new List<BaseComponent>();
        }

        protected List<BaseComponent> components;
    }
}
