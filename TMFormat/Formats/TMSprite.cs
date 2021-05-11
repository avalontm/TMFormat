using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat.Formats
{

    public class TMSpriteTexture
    {
        public Texture2D Sprite1 { set; get; }
        public Texture2D Sprite2 { set; get; }
        public Texture2D Sprite3 { set; get; }
        public Texture2D Sprite4 { set; get; }
    }

    public class TMSprite : TMItem
    {
        public List<TMSpriteTexture> Sprites { set; get; }
        public object Image { set; get; }

        public TMSprite()
        {
            Sprites = new List<TMSpriteTexture>();
        }
    }

}
