using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PacMan
{
    internal class GameObject
    {
        protected Texture2D tex;
        protected Vector2 pos;
        protected Rectangle srcRec;
        protected Color color;
        protected Vector2 origin;
        protected float scale;
        protected float rotation;
        protected SpriteEffects spriteEffects;
        

        public GameObject(Texture2D tex, Vector2 pos, Rectangle srcRec, Color color, Vector2 origin, float scale, float rotation, SpriteEffects spriteEffects)
        {
            this.tex = tex;
            this.pos = pos;
            this.srcRec = srcRec;
            this.color = color;
            this.origin = origin;
            this.scale = scale;
            this.rotation = rotation;
            this.spriteEffects = spriteEffects;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, 
                pos,
                srcRec,  
                color,
                rotation,
                origin,
                scale,
                spriteEffects,
                1);
        }
    }
}
