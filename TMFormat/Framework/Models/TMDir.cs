using System;
using System.Collections.Generic;
using System.Text;
using TMFormat.Formats;

namespace TMFormat.Framework.Models
{
    public class TMDir
    {
        public List<TMSpriteTexture> sprites;

        public TMDir()
        {
            sprites = new List<TMSpriteTexture>();

            sprites.Add(new TMSpriteTexture());
            sprites.Add(new TMSpriteTexture());
            sprites.Add(new TMSpriteTexture());
            sprites.Add(new TMSpriteTexture());
            sprites.Add(new TMSpriteTexture());

        }
    }
}
