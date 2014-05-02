using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcadia.Gamestates.Pong
{
    class Arena
    {
        private ArenaComponent[] acComponents;

        public ArenaComponent[] Components
        {
            get { return acComponents; }
            set { acComponents = value; }
        }

        public Arena()
        {
            acComponents = null;
        }

        public Arena(ArenaComponent component)
        {
            ArenaComponent[] components = new ArenaComponent[1];
            components[0] = component;
            acComponents = components;
        }

        public Arena(ArenaComponent[] components)
        {
            acComponents = components;
        }

        public bool IsCollidingWith(Rectangle collisionBox)
        {
            foreach (ArenaComponent wall in acComponents)
            {
                if (wall.CollisionBox.Intersects(collisionBox))
                {
                    return true;
                }
            }

            return false;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ArenaComponent component in acComponents)
            {
                component.Draw(spriteBatch);
            }
        }
    }
}
