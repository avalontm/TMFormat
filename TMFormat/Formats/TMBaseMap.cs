using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using TMFormat.Enums;
using TMFormat.Formats;
using TMFormat.Models;

namespace TMFormat.Formats
{
    public class TMBaseMap : IDisposable
    {
        public static readonly int total_floors = 15;
        public static readonly int TileSize = 32;

        //Variables Publicas
        public MapInfo mapInfo = new MapInfo();

        // MAP LISTS
        public List<MapProperties[,]> Floors = new List<MapProperties[,]>();
        List<TMSprite> Items = new List<TMSprite>();

        public TMBaseMap(List<TMSprite> items)
        {
            Items = items;
        }

        public void Create(int width, int height)
        {
            //Informacion del mapa
            mapInfo.Size = new Vector2();
            mapInfo.Size.X = width;
            mapInfo.Size.Y = height;

            Floors.Clear();

            for (var i = 0; i < total_floors; i++)
            {
                var floor = new MapProperties[width, height];
                Floors.Add(floor);
            }
        }

        /* +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ */
        /* READ Map*/
        /* +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ */

        public bool Load(string fileMap)
        {
            int width = 0;
            int height = 0;

            if (string.IsNullOrEmpty(fileMap))
            {
                Console.WriteLine($"[MapBase] Load => Not Root Directory.");
                return false;
            }

            string filename = fileMap;

            if (!File.Exists(filename))
            {
                return false;
            }

            FileStream readStream = new FileStream(filename, FileMode.Open);

            using (BinaryReader reader = new BinaryReader(readStream))
            {
                try
                {
                    if (reader.ReadString() != "ABO")
                    {
                        return false;
                    }

                    mapInfo.Name = reader.ReadString();
                    mapInfo.Version = reader.ReadString();
                    mapInfo.Autor = reader.ReadString();

                    width = reader.ReadInt32();
                    height = reader.ReadInt32();

                    Create(width, height);

                    //Leemos Tiles del Mapa.
                    int tiles_count = reader.ReadInt32();

                    for (int t = 0; t < tiles_count; t++)
                    {
                        bool pz = reader.ReadBoolean();
                        int tile_id = reader.ReadInt32();
                        int x = reader.ReadInt32();
                        int y = reader.ReadInt32();
                        int z = reader.ReadInt32();
                        int items_count = reader.ReadInt32();
                      
                        var real_item = Items.Where(itm => itm.Id == tile_id).FirstOrDefault();
                        TMSprite item = new TMSprite();
                        item.Copy(real_item);
                      
                        Floors[z][x, y].item = item;
                        Floors[z][x, y].isPZ = pz;

                        for (int i = 0; i < items_count; i++)
                        {
                            int item_id = reader.ReadInt32();
                            int destineX = reader.ReadInt32();
                            int destineY = reader.ReadInt32();
                            int destineZ = reader.ReadInt32();
                            string message = reader.ReadString();

                            real_item = Items.Where(itm => itm.Id == item_id).FirstOrDefault();
                            TMSprite _item = new TMSprite();
                            _item.Copy(real_item);

                            _item.Destine = new Vector3(destineX, destineY, destineZ);
                            _item.Reader = message;

                            if (Floors[z][x, y].items == null)
                            {
                                Floors[z][x, y].items = new List<TMSprite>();
                            }
                            Floors[z][x, y].items.Add(_item);
                        }
                    }

                    reader.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MapBase] Load => {ex}");
                    reader.Close();
                    return false;
                }
            }
        }

        public void Dispose()
        {
            Floors = null;
            Items = null;
        }

    } //FIN
}
