using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TMFormat.Framework.Resolution;

namespace TMFormat.Framework.Maps
{
    public class Camera
    {
        Vector2 rect = new Vector2(9, 7);
        public static Rectangle Screen;
        public static Vector2 Center = Vector2.Zero;
        MapManager map;

        public Camera(MapManager map)
        {
            this.map = map;
            Screen = new Rectangle(0, 0, (int)rect.X, (int)rect.Y);
            Center = new Vector2((ResolutionManager.TitleSafeArea.Width / 2), (ResolutionManager.TitleSafeArea.Height / 2));
        }

        public Rectangle onCameraScreen()
        {
            if (map.player == null)
            {
                return new Rectangle();
            }

            int ScreenX = ((int)map.player.pos_x - Camera.Screen.Width);
            int ScreenY = ((int)map.player.pos_y - Camera.Screen.Height);

            int ScreenWidth = ((int)map.player.pos_x + Camera.Screen.Width);
            int ScreenHeight = ((int)map.player.pos_y + Camera.Screen.Height);

            if (ScreenX < 0)
            {
                ScreenX = 0;
            }
            if (ScreenY < 0)
            {
                ScreenY = 0;
            }

            if (ScreenWidth > (int)map.MapBase.mapInfo.Size.X)
            {
                ScreenWidth = (int)map.MapBase.mapInfo.Size.X;
            }
            if (ScreenHeight > (int)map.MapBase.mapInfo.Size.Y)
            {
                ScreenHeight = (int)map.MapBase.mapInfo.Size.Y;
            }

            return new Rectangle(ScreenX, ScreenY, ScreenWidth, ScreenHeight);
        }
    }
}
