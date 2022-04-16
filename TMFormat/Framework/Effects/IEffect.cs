using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TMFormat.Formats;
using TMFormat.Framework.Items;
using TMFormat.Helpers;

namespace TMFormat.Framework.Effects
{
    public class IEffect
    {
        public TMSprite item { private set; get; }
        float ani_speed { set; get; }
        float ani_count { set; get; }
        public int ani_index { private set; get; }
        public VectorInt3 pos { private set; get; }
        public VectorInt3 destine { set; get; }
        string message;
        Color color;
        public float tmpX;
        public float tmpY;

        public IEffect(int item_id, VectorInt3 position)
        {
            pos = position;
            ani_count = 0;
            ani_index = 0;

            item = ItemManager.GetItem(item_id);

            if (item != null)
            {
                ani_speed = (float)item.AniSpeed;
            }
        }

        public IEffect(int item_id, VectorInt3 position, VectorInt3 destine)
        {
            this.pos = position;
            this.destine = destine;
            ani_count = 0;
            ani_index = 0;

            item = ItemManager.GetItem(item_id);

            if (item != null)
            {
                ani_speed = (float)item.AniSpeed;
            }
        }

        public IEffect(string item_name, VectorInt3 position)
        {
            pos = position;
            ani_count = 0;
            ani_index = 0;

            item = ItemManager.GetItem(item_name);

            if (item != null)
            {
                ani_speed = (float)item.AniSpeed;
            }
        }

        public IEffect(string message, VectorInt3 position, Color color)
        {
            pos = position;
            this.message = message;
            this.color = color;
            ani_speed = 0.5f;
            ani_count = 0;
            ani_index = 0;
        }

        public IEffect(string title, string message, VectorInt3 position, Color color)
        {
            pos = position;
            this.message = message;
            this.color = color;
            ani_speed = 0.5f;
            ani_count = 0;
            ani_index = 0;
        }

        public void onSetAnimIndex(int _index)
        {
            ani_index = _index;
        }

        public virtual void Update(GameTime gameTime)
        {
            ani_count += (float)(ani_speed * gameTime.ElapsedGameTime.TotalSeconds);

            if (ani_count > 0.2)
            {
                ani_count = 0;
                ani_index++;

                if (ani_index > (item.Textures.Count - 1))
                {
                    ani_index = (item.Textures.Count - 1);
                    TMInstance.Map.effects.Remove(this);
                }
            }
        }

        public bool onRange()
        {
            var resultX = Math.Abs(pos.X - TMInstance.Map.player.pos_x);
            var resultY = Math.Abs(pos.Y - TMInstance.Map.player.pos_y);

            if (resultX <= 8 && resultY <= 8)
            {
                bool isDungeon = (pos.Z > TMInstance.Map.FloorDefault);

                if (isDungeon && TMInstance.Map.isDungeon)
                {
                    return true;
                }
                if ((int)pos.Z <= TMInstance.Map.FloorDefault)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            //COORDENADAS
            tmpX = ((pos.X * TMBaseMap.TileSize) - TMInstance.Map.camera.Screen.X);
            tmpY = ((pos.Y * TMBaseMap.TileSize) - TMInstance.Map.camera.Screen.Y);

            if (pos.Z != TMInstance.Map.player.pos_z)
            {
                if (pos.Z > TMInstance.Map.player.pos_z)
                {
                    //COORDENADAS
                    tmpX += (1 * TMBaseMap.TileSize);
                    tmpY += (1 * TMBaseMap.TileSize);
                }
                else
                {
                    //COORDENADAS
                    tmpX -= (1 * TMBaseMap.TileSize);
                    tmpY -= (1 * TMBaseMap.TileSize);

                }
            }

            if (onRange())
            {
                if (item != null)
                {
                    _spriteBatch.Draw(item.Sprites[ani_index].Sprite1, Utils.GetTileDestine(tmpX, tmpY), null, Color.White);
                }
            }

        }
    }
}
