using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PacMan
{
    internal class Tile 
    {
        Texture2D texture;
        public Vector2 position;
        public bool wall;




        public Tile(Texture2D texture, Vector2 position, bool wall)
        {
            this.texture = texture;
            this.position = position;
            this.wall = wall;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,
                position,
                null,
                Color.White,
                0f,
                new Vector2(20,20),
                1f,
                SpriteEffects.None,
                1f);
                
                
               

        }


    }
}
