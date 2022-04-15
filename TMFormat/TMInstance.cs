using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TMFormat.Formats;
using TMFormat.Framework.Items;
using TMFormat.Framework.Loaders;
using TMFormat.Framework.Maps;
using TMFormat.Framework.Outfits;
using TMFormat.Framework.Resolution;

namespace TMFormat
{
    public static class TMInstance
    {
        public static float targetFPS = 60f;
        public static bool UseMonoGame { private set; get; }
        public static ContentManager Content { private set; get; }
        public static bool UseTextures { get; private set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static MapManager Map { get; private set; }

        public static void Init(bool useMonoGame = false, bool useTextures = false)
        {
            UseMonoGame = useMonoGame;
            UseTextures = useTextures;
        }

        public static void Init(ContentManager content, bool useMonoGame = true, bool useTextures = true)
        {
            Content = content;
            UseMonoGame = useMonoGame;
            UseTextures = useTextures;
        }

        public static bool InitGame(GraphicsDevice device, ContentManager content, string fileMap)
        {
            GraphicsDevice = device;

            Content = content;
            UseMonoGame = true;
            UseTextures = true;

            Colors.Init();
            Shaders.Init();
            Fonts.Init();
            Textures.Init();

            bool items_status = ItemManager.Init(Path.Combine(Content.RootDirectory, "items.dat"));

            if (items_status)
            {
                Map = new MapManager(GraphicsDevice, ItemManager.Items);
            }

            return items_status;
        }

        public static bool InitWorld(string fileMap = "")
        {
            if (string.IsNullOrEmpty(fileMap))
            {
                return Map.MapBase.Load(Path.Combine(Content.RootDirectory, "world.tmap"));
            }
            return Map.MapBase.Load(Path.Combine(Content.RootDirectory, $"{fileMap}.tmap"));
        }

    }
}
