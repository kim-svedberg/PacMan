using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace PacMan
{
    internal class Ghost : GameObject
    {
        int frame = 100;
        double frameTimer, frameInterval = 150;

        Vector2 direction;
        Vector2 destination;
        Vector2 newDestination;
        Vector2 randomDir;

        bool moving = false;
        float speed = 80.0f;

        public Rectangle hitBox;

        Random rnd;


        public Ghost(Texture2D tex, 
            Vector2 pos, 
            Rectangle srcRec,
            Color color,
            Vector2 origin, 
            float scale, 
            float rotation, 
            SpriteEffects spriteEffects,
            Rectangle hitBox) 

            : base(tex, 
                  pos, 
                  srcRec, 
                  color,
                  origin, 
                  scale, 
                  rotation, 
                  spriteEffects)
        {
            this.hitBox = hitBox;
        }
        

        public void Update(GameTime gameTime)
        {
            hitBox.Location = new Vector2((int)pos.X - 20, (int)pos.Y - 20).ToPoint();


            rnd = new Random();
            int randX = rnd.Next(-1, 2);
            int randY = rnd.Next(-1, 2); 
            randomDir = new Vector2(randX, randY);

            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (frameTimer <= 0)
            {
                frameTimer = frameInterval; frame++;
                srcRec.X = (frame % 8) * 40;
            }

            //Chooses a random direction when not moving.
            //If both the x & y value = 0, changes the x-value to prevent the ghost from standing still. 
            if (!moving)
            {
               ChooseDirection(randomDir);
               direction = randomDir;
                    
                if(randX == 0 && randY == 0)
                {
                    randX = 1;
                }
            }

            //Moving
            else
            {
                pos += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Standing on a tile changes moving = false. 
                if (Vector2.Distance(pos, destination) < 1)
                {
                    pos = destination;
                    moving = false;
                }
            }

        }

        public void ChooseDirection(Vector2 dir)
        {

            direction = dir;
            newDestination = pos + direction * AssetManager.floorTex.Width;

            if (!Game1.GetTileAtPosition(newDestination))
            {
                destination = newDestination;
                moving = true;
            }

        }
    }
}
