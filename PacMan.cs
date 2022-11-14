using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;
using System.Collections.Generic;

namespace PacMan
{
    internal class PacMan : GameObject
    {

        CoolDownTimer cooldownTimer = new CoolDownTimer();
        
        private int frame = 100;
        public static int Lives = 3;
        public static int Score = 0;
        private double frameTimer, frameInterval = 100;

        
        private Vector2 direction;
        private Vector2 destination;

        public Rectangle hitBox;

        private bool moving = false;
        private bool invisible = false;
        private const float speed = 100.0f;

        private Keys currentDirection;


        public PacMan(Texture2D tex, Vector2 pos, Rectangle srcRec, Color color, Vector2 origin, float scale, float rotation, SpriteEffects spriteEffects) : base(tex, pos, srcRec, color, origin, scale, rotation, spriteEffects) { }
     
        public void Update(GameTime gameTime)
        {
            if (invisible)
            {
                color = Color.Red;
            }

            hitBox = new Rectangle((int)pos.X - 20, (int)pos.Y - 20, tex.Width / 4, tex.Height);
            
            cooldownTimer.Update(gameTime.ElapsedGameTime.TotalSeconds);

            Animation(gameTime);
           

            if (moving)
            {
                pos += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Vector2.Distance(pos, destination) < 1)
                {
                    pos = destination;

                    moving = Game1.GetTileAtPosition(destination);

                    
                }
            }

            if (!moving)
            {
                frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) currentDirection = Keys.Left;
                if (Keyboard.GetState().IsKeyDown(Keys.Right)) currentDirection = Keys.Right;
                if (Keyboard.GetState().IsKeyDown(Keys.Up)) currentDirection = Keys.Up;
                if (Keyboard.GetState().IsKeyDown(Keys.Down)) currentDirection = Keys.Down;

                if (currentDirection == Keys.Left)
                {
                    ChangeDirection(new Vector2(-1, 0));
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    rotation = MathHelper.ToRadians(0);
                }
                else if (currentDirection == Keys.Right)
                {
                    ChangeDirection(new Vector2(1, 0));
                    spriteEffects = SpriteEffects.None;
                    rotation = MathHelper.ToRadians(0);

                }
                if (currentDirection == Keys.Up)
                {
                    ChangeDirection(new Vector2(0, -1));
                    rotation = MathHelper.ToRadians(-90);
                    spriteEffects = SpriteEffects.None;
                }
                else if (currentDirection == Keys.Down)
                {
                    ChangeDirection(new Vector2(0, 1));
                    rotation = MathHelper.ToRadians(-90);
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }

            }
        }

        public void ChangeDirection(Vector2 dir)
        {
            direction = dir;
            Vector2 newDestination = pos + direction * AssetManager.floorTex.Width;

            if (!Game1.GetTileAtPosition(newDestination))
            {
                destination = newDestination;
                moving = true;
            }

        }

        public void Collision(List<Ghost>ghostList)
        {
            foreach(Ghost ghost in ghostList)
            {
                if (hitBox.Intersects(ghost.hitBox) && cooldownTimer.IsDone())
                {
                   Lives--;
                   color = Color.Red;
                   cooldownTimer.ResetAndStart(2.0);
                }
                if(!hitBox.Intersects(ghost.hitBox) && cooldownTimer.IsDone())
                {
                    color = Color.White;
                }
               
             
            }
        }
        public void EatFood(List<Food> foodList, List<SpecialFruit> specialfruitList)
        {
            foreach(Food food in foodList)
            {
                if (hitBox.Intersects(food.hitBox))
                {
                    AssetManager.eatSound.Play();
                    Score += 100;
                    foodList.Remove(food);
                    break;
                }

            }

            foreach(SpecialFruit specialFruit in specialfruitList)
            {
                if (hitBox.Intersects(specialFruit.hitBox))
                {
                    AssetManager.eatSound.Play();
                    Score += 500;
                    specialfruitList.Remove(specialFruit);
                    break;
                }
            }
        }

        public void Animation(GameTime gameTime)
        {
            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (frameTimer <= 0)
            {
                frameTimer = frameInterval; frame++;
                srcRec.X = (frame % 4) * 40;
            }
        }

    }
}
