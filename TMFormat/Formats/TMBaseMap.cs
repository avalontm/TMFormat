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
                    if (reader.ReadString() != "TMAP")
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

        public bool Save(string fileName)
        {
            int width = (int)mapInfo.Size.X;
            int height = (int)mapInfo.Size.Y;
            List<TMTileMap> tiles = new List<TMTileMap>();

            FileStream writeStream = new FileStream(fileName, FileMode.Create);

            using (BinaryWriter writer = new BinaryWriter(writeStream))
            {
                writer.Write("TMAP");
                writer.Write(mapInfo.Name);
                writer.Write(mapInfo.Version);
                writer.Write(mapInfo.Autor);

                writer.Write(width);
                writer.Write(height);

                //Escribir Tiles del Mapa.

                for (int y = 0; y < (int)mapInfo.Size.Y; y++)
                {
                    for (int x = 0; x < (int)mapInfo.Size.X; x++)
                    {
                        for (int f = 0; f < total_floors; f++)
                        {
                            if (Floors[f][x, y].item != null)
                            {
                                TMTileMap tile = new TMTileMap();

                                tile.pz = Floors[f][x, y].isPZ;
                                tile.type = Floors[f][x, y].item.Type;
                                tile.id = Floors[f][x, y].item.Id;
                                tile.x = x;
                                tile.y = y;
                                tile.z = f;

                                if (Floors[f][x, y].items != null)
                                {
                                    foreach (var item in Floors[f][x, y].items)
                                    {
                                        TMItemMap _item = new TMItemMap();
                                        _item.id = item.Id;
                                        _item.teleporX = (int)item.Destine.X;
                                        _item.teleporY = (int)item.Destine.Y;
                                        _item.teleporZ = (int)item.Destine.Z;
                                        _item.message = item.Reader ?? string.Empty;

                                        tile.items.Add(_item);
                                    }
                                }

                                tiles.Add(tile);
                            }
                        }

                    }
                }

                writer.Write(tiles.Count);

                foreach (var tile in tiles)
                {
                    writer.Write(tile.pz);
                    writer.Write(tile.id);
                    writer.Write(tile.x);
                    writer.Write(tile.y);
                    writer.Write(tile.z);

                    writer.Write(tile.items.Count);

                    foreach (var item in tile.items)
                    {
                        writer.Write(item.id);
                        writer.Write(item.teleporX);
                        writer.Write(item.teleporY);
                        writer.Write(item.teleporZ);
                        writer.Write(item.message);
                    }
                }

                return true;
            }
        }

        public void Dispose()
        {
            Floors = null;
            Items = null;
        }

    } //FIN
}
