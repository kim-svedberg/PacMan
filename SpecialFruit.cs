using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PacMan
{
    internal class SpecialFruit : Food
    {
        public SpecialFruit(Texture2D tex, Vector2 pos, Rectangle hitBox) : base(tex, pos, hitBox) { }
    }
}
