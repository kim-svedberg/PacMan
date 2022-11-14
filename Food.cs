using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace PacMan
{
    internal class Food
    {
        Texture2D tex;
        Vector2 pos;
        public Rectangle hitBox;

        public Food(Texture2D tex, Vector2 pos, Rectangle hitBox)
        {
            this.tex = tex;
            this.pos = pos;
            this.hitBox = hitBox;
        }

        public void Update()
        {
            hitBox.Location = new Vector2((int)pos.X, (int)pos.Y).ToPoint();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, 
                pos, 
                null, 
                Color.White,
                0f,
                new Vector2(5,5),
                1f,
                SpriteEffects.None,
                0
                );

            //spriteBatch.DrawRectangle(hitBox,
            //    Color.Red,
            //    2f,
            //    0);
        }
    }


}
