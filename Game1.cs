using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using MonoGame.Extended;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Microsoft.Xna.Framework.Media;


namespace PacMan
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        enum GameState { Menu, InGame, GameOver, Win }
        GameState gameState;
        
        Texture2D[] gameStateTex;
        static Tile[,] tileArray;

        PacMan pacMan;
        Ghost ghost;
        Food food;
        SpecialFruit specialFruit;


        StreamReader reader;
       
       
        List<string> strings = new List<string>();
        List<Ghost> ghostList = new List<Ghost>();
        List<Food> foodList = new List<Food>();
        List<SpecialFruit> specialFruitList = new List<SpecialFruit>();

        Rectangle pacManSrcRec;
        Rectangle ghostSrcRec;
        Rectangle ghostHitBox;
        Rectangle foodHitBox;
        Rectangle specialFruitHitBox;


        float pacManRotation;
        float ghostRotation;

        public static int TexAdjustment = 10;
        int spaceBetweenLives = 10;

        Vector2 pacManOrigin;
        Vector2 pacManPos;
        Vector2 ghostOrigin;
        Vector2 ghostPos;
        Vector2 foodPos;
        Vector2 lifePos;
        Vector2 scorePos;
        Vector2 specialFruitPos;

        SpriteEffects pacManFx = SpriteEffects.None;
        SpriteEffects ghostFx = SpriteEffects.None;

        Color pacManColor = Color.White;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 440;
            graphics.ApplyChanges();


        }

        protected override void Initialize()
        {
            gameState = GameState.Menu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
           
            AssetManager.LoadAssets(Content);
            ReadFromTextFile();

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(AssetManager.themeSong);

            pacManRotation = 0;
            pacManOrigin = new Vector2(20, 20);
            ghostOrigin = new Vector2(20, 20);

            pacManSrcRec = new Rectangle(0,0, AssetManager.pacManTex.Width / 4, AssetManager.pacManTex.Height);
            ghostSrcRec = new Rectangle(0, 0, AssetManager.multiSheet.Width / 8, AssetManager.multiSheet.Height / 6);

            lifePos = new Vector2(10, 8);
            scorePos = new Vector2(50, 5);

            gameStateTex = new Texture2D[4];
            gameStateTex[(int)GameState.InGame] = AssetManager.wallTex;
            gameStateTex[(int)GameState.Menu] = AssetManager.menuTex;
            gameStateTex[(int)GameState.GameOver] = AssetManager.gameOverTex;
            gameStateTex[(int)GameState.Win] = AssetManager.winTex;

           
            Map();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState)
            {
                case GameState.Menu:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            gameState = GameState.InGame;
                        }
                    }
                    break;

                case GameState.InGame:
                    {

                        foreach (Ghost ghost in ghostList)
                        {
                            ghost.Update(gameTime);
                        }

                        foreach(Food food in foodList)
                        {
                            food.Update();
                        }

                        pacMan.Update(gameTime);
                        pacMan.EatFood(foodList, specialFruitList);
                        pacMan.Collision(ghostList);

                        //Win condition
                        if(foodList.Count == 0)
                        {
                            gameState = GameState.Win;
                        }

                        //Losing condition
                        if(PacMan.Lives == 0)
                        {
                            AssetManager.defeatSound.Play();
                            gameState = GameState.GameOver;
                        }

                    }
                    break;

                case GameState.GameOver:
                    {
                        MediaPlayer.Stop();
                    }
                    break;

                case GameState.Win:
                    break;

            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Draw(gameStateTex[(int)gameState],
                new Vector2(0, 0),
                null, Color.White);

            if (gameState == GameState.InGame)
            {

                foreach (Tile tile in tileArray)
                {
                    tile.Draw(spriteBatch);
                }

                foreach(Food food in foodList)
                {
                
                    food.Draw(spriteBatch);

                }

                foreach(SpecialFruit specialFruit in specialFruitList)
                {
                    specialFruit.Draw(spriteBatch);
                }

                foreach(Ghost ghost in ghostList)
                {
                    ghost.Draw(spriteBatch);
                }
               
                pacMan.Draw(spriteBatch);

                //spriteBatch.DrawRectangle(pacMan.hitBox,
                //    Color.Red,
                //    2f,
                //    0);

                //spriteBatch.DrawRectangle(ghost.hitBox,
                //    Color.Red,
                //    2f,
                //    0);

                //Drawing the lives
                for (int i = 0; i < PacMan.Lives; i++)
                {
                    spriteBatch.Draw(AssetManager.lifeTex, new Vector2(lifePos.X + i * spaceBetweenLives, lifePos.Y), Color.White);
                }

                //Draw score 
                spriteBatch.DrawString(AssetManager.font, "SCORE: " + PacMan.Score, scorePos, Color.Gold);
            }

            if(gameState == GameState.GameOver || gameState == GameState.Win)
            {
                spriteBatch.DrawString(AssetManager.font, "SCORE: " + PacMan.Score, new Vector2(220, 320), Color.Gold);

            }


            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void ReadFromTextFile()
        {
            reader = new StreamReader(@"C:\Users\kimas\OneDrive\Desktop\Projects_and_Solutions\PacMan\Content\map.txt");
            while (!reader.EndOfStream)
            {
                strings.Add(reader.ReadLine());
            }
            reader.Close();
        }

        public static bool GetTileAtPosition(Vector2 vec)
        {
            return tileArray[(int)vec.X / AssetManager.floorTex.Width, (int)vec.Y / AssetManager.floorTex.Height].wall;
        }

        public void Map()
        {
            tileArray = new Tile[strings[0].Length, strings.Count];
            for(int i = 0; i < tileArray.GetLength(0); i++)
            {
                for (int j = 0; j < tileArray.GetLength(1); j++)
                {
                    if (strings[j][i] == 'F')
                    {
                        tileArray[i, j] = new Tile
                            (AssetManager.floorTex, 
                            new Vector2(AssetManager.floorTex.Width * i, AssetManager.floorTex.Height * j), 
                            false);

                        food = new Food
                            (AssetManager.foodTex, 
                            foodPos = new Vector2(AssetManager.floorTex.Width * i, AssetManager.floorTex.Height * j), 
                            foodHitBox = new Rectangle((int)foodPos.X, (int)foodPos.Y, 
                            AssetManager.foodTex.Width, 
                            AssetManager.foodTex.Height)); 

                        foodList.Add(food); 

                    }
                    if (strings[j][i] == 'W')
                    {
                        tileArray[i, j] = new Tile
                            (AssetManager.wallTex, 
                            new Vector2(AssetManager.wallTex.Width * i, AssetManager.wallTex.Height * j), 
                            true);
                    }
                    if (strings[j][i] == 'p')
                    {
                        tileArray[i, j] = new Tile
                            (AssetManager.floorTex, 
                            new Vector2(AssetManager.floorTex.Width * i, AssetManager.floorTex.Height * j), 
                            false);

                        pacMan = new PacMan
                            (AssetManager.pacManTex, 
                            pacManPos = new Vector2(AssetManager.floorTex.Width * i, AssetManager.floorTex.Height * j), 
                            pacManSrcRec,
                            pacManColor,
                            pacManOrigin, 
                            1, 
                            pacManRotation, 
                            pacManFx);
                    }
                    if (strings[j][i] == 'g')
                    {
                        tileArray[i, j] = new Tile
                            (AssetManager.floorTex, 
                            new Vector2(AssetManager.floorTex.Width * i, AssetManager.floorTex.Height * j), 
                            false);

                        ghost = new Ghost(AssetManager.multiSheet, 
                            ghostPos = new Vector2(AssetManager.floorTex.Width * i, AssetManager.floorTex.Height * j), 
                            ghostSrcRec,
                            Color.White,
                            ghostOrigin, 
                            1, 
                            ghostRotation,
                            ghostFx, 
                            ghostHitBox = new Rectangle
                            ((int)ghostPos.X - 20, (int)ghostPos.Y - 20, 
                            AssetManager.multiSheet.Width / 8, 
                            AssetManager.multiSheet.Height / 6));

                        ghostList.Add(ghost);

                        food = new Food
                            (AssetManager.foodTex, 
                            new Vector2(AssetManager.floorTex.Width * i, AssetManager.floorTex.Height * j), 
                            foodHitBox);

                        foodList.Add(food);

                    }
                    if (strings[j][i] == 's')
                    {
                        tileArray[i, j] = new Tile
                            (AssetManager.floorTex,
                            new Vector2(AssetManager.floorTex.Width * i, AssetManager.floorTex.Height * j),
                            false);

                        specialFruit = new SpecialFruit(AssetManager.cherryTex,
                            specialFruitPos = new Vector2
                            (AssetManager.floorTex.Width * i, 
                            AssetManager.floorTex.Height * j),
                            specialFruitHitBox = new Rectangle
                            ((int)specialFruitPos.X, 
                            (int)specialFruitPos.Y, 
                            AssetManager.cherryTex.Width,
                            AssetManager.cherryTex.Height));
                            
                        specialFruitList.Add(specialFruit);


                    }

                }

            }



        }
       
    }
}
//SpriteManager s.78
//AI s.112
//High score s.140
//Power-Ups s164