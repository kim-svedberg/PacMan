using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using Microsoft.Xna.Framework.Content;
using System.Drawing.Drawing2D;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace PacMan
{
    internal class AssetManager
    {
        public static Texture2D floorTex;
        public static Texture2D wallTex;
        public static Texture2D pacManTex;    
        public static Texture2D multiSheet;
        public static Texture2D foodTex;
        public static Texture2D lifeTex;
        public static Texture2D menuTex;
        public static Texture2D gameOverTex;
        public static Texture2D winTex;
        public static Texture2D cherryTex;

        public static Song themeSong;
        public static SoundEffect eatSound;
        public static SoundEffect defeatSound;

        public static SpriteFont font;

        public static void LoadAssets(ContentManager content)
        {
            floorTex = content.Load<Texture2D>(@"floortile_");
            wallTex = content.Load<Texture2D>("walltile_");
            pacManTex = content.Load<Texture2D>("PacMan");
            multiSheet = content.Load<Texture2D>("MultiSheet_");
            foodTex = content.Load<Texture2D>("food");
            lifeTex = content.Load<Texture2D>("PacManLife");
            menuTex = content.Load<Texture2D>("menuPM");
            gameOverTex = content.Load<Texture2D>("GOPacMan");
            winTex = content.Load<Texture2D>("win2");
            cherryTex = content.Load<Texture2D>("cherry");

            themeSong = content.Load<Song>("themePM");
            eatSound = content.Load<SoundEffect>("waka");
            defeatSound = content.Load<SoundEffect>("defeat");

            font = content.Load<SpriteFont>("font");

        }

    }
}
