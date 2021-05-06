using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat
{
    public static class TMInstance
    {
        public static bool UseMonoGame { private set; get; }
        public static ContentManager Content { private set; get; }

        public static void Init(bool useMonoGame = false)
        {
            UseMonoGame = useMonoGame;
        }

        public static void Init(ContentManager content, bool useMonoGame = true)
        {
            Content = content;
            UseMonoGame = useMonoGame;
        }
    }
}
