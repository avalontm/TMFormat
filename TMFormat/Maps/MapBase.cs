using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMFormat.Items;
using TMFormat.Models;

namespace TMFormat.Maps
{
    public struct TilePlayer
    {
        public bool Send;
    }

    public struct MapInfo
    {
        public string Name;
        public Vector2 Size;
        public string Version;
        public string Autor;
    }

    public struct MapProperties
    {
        public bool isCreature;
        public bool isPZ;
        public bool isTop;
        public ItemModel item;
        public List<ItemModel> items;
    }


    public static class MapBase
    {
        static string dirData;
        public static readonly int total_floors = 15;
        public static readonly int TileSize = 32;

        //Variables Publicas
        public static MapInfo mapInfo = new MapInfo();

        // MAP LISTS
        public static List<MapProperties[,]> Floors = new List<MapProperties[,]>();

        public static void Init(string data_dir)
        {
            dirData = data_dir;
        }

        public static void Create(int width, int height)
        {
            //Informacion del mapa
            mapInfo.Size = new Vector2(width, height);

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

        public static bool Load(string name)
        {
            int width = 0;
            int height = 0;

            if (string.IsNullOrEmpty(dirData))
            {
                Console.WriteLine($"[MapBase] Load => Not Root Directory.");
                return false;
            }

            string filename = Path.Combine(dirData, "maps", $"{name}.abomap");

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
                      
                        var real_item = ItemBase.Data.Where(itm => itm.Id == tile_id).FirstOrDefault();
                        ItemModel item = new ItemModel();
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

                            real_item = ItemBase.Data.Where(itm => itm.Id == item_id).FirstOrDefault();
                            ItemModel _item = new ItemModel();
                            _item.Copy(real_item);

                            _item.Destine = new Vector3(destineX, destineY, destineZ);
                            _item.Reader = message;

                            if (Floors[z][x, y].items == null)
                            {
                                Floors[z][x, y].items = new List<ItemModel>();
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


    } //FIN


}
