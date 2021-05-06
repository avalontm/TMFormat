﻿using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using TMFormat.Models;

namespace TMFormat.Formats
{
    public class TMCreatureLoot
    {
        public int id { set; get; }
        public int count { set; get; }
        public double probability { set; get; }
    }

    public class TMCreatureTexture
    {
        public List<byte[]> textures { set; get; }
        public List<byte[]> masks { set; get; }

        public TMCreatureTexture()
        {
            textures = new List<byte[]>();
            masks = new List<byte[]>();
        }
    }

    public class TMCreature
    {
        public string name { set; get; }
        public int type { set; get; }
        public float apeed { set; get; }
        public bool is_agressive { set; get; }
        public int experience { set; get; }
        public int attack { set; get; }
        public int defence { set; get; }
        public int level { set; get; }
        public int heal { set; get; }
        public bool is_offset { set; get; }
        public bool use_spell { set; get; }
        public int timespawn { set; get; }
        public bool use_distance { set; get; }
        public List<TMCreatureTexture> dirs { set; get; }
        public List<TMCreatureLoot> loots { set; get; }

        public TMCreature()
        {
            dirs = new List<TMCreatureTexture>();
            loots = new List<TMCreatureLoot>();

            TMCreatureTexture _dir = new TMCreatureTexture();
            dirs.Add(_dir);

            _dir = new TMCreatureTexture();
            dirs.Add(_dir);

            _dir = new TMCreatureTexture();
            dirs.Add(_dir);

            _dir = new TMCreatureTexture();
            dirs.Add(_dir);

            _dir = new TMCreatureTexture();
            dirs.Add(_dir);
        }

        public static TMCreature Load(string filename)
        {
            byte[] readBytes = null;

            try
            {
                if (TMInstance.Content == null)
                {
                    if (!File.Exists(filename))
                    {
                        Console.WriteLine($"[CreatureBase] NotFound! => {filename}");
                        return null;
                    }

                    readBytes = File.ReadAllBytes(filename);
                    return Read(readBytes);
                }

                readBytes = TMInstance.Content.Load<byte[]>(filename);
                return Read(readBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TMCreature] [LOAD] => {ex}");
                return null;
            }
        }

        public static bool Save(TMCreature item , string file)
        {
            try
            {
                byte[] byteArray = Write(item);

                using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TMCreature] [LOAD] => {ex}");
                return false;
            }
        }

        public bool SaveToFile(string file)
        {
            try
            {
                byte[] byteArray = Write(this);

                using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TMCreature] [LOAD] => {ex}");
                return false;
            }
        }

        static TMCreature Read(byte[] data)
        {
            TMCreature item = new TMCreature();

            FieldInfo[] fi = typeof(TMCreature).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    string Header = Encoding.ASCII.GetString(reader.ReadBytes(3)); //Obtenemos el Header

                    if (Header == "TMC")
                    {
                        foreach (FieldInfo info in fi)
                        {
                            if (info.FieldType == typeof(string))
                            {
                                info.SetValue(item, reader.ReadString());
                            }

                            if (info.FieldType == typeof(int))
                            {
                                info.SetValue(item, reader.ReadInt32());
                            }

                            if (info.FieldType == typeof(bool))
                            {
                                info.SetValue(item, reader.ReadBoolean());
                            }

                            if (info.FieldType == typeof(double))
                            {
                                info.SetValue(item, reader.ReadDouble());
                            }

                            if (info.FieldType == typeof(float))
                            {
                                info.SetValue(item, reader.ReadSingle());
                            }

                            if (info.FieldType == typeof(List<TMCreatureTexture>))
                            {
                                int _counts = reader.ReadInt32();

                                for (var i = 0; i < item.dirs.Count; i++)
                                {
                                    //Texturas
                                    for (var a = 0; a < 4; a++)
                                    {
                                        int Length = reader.ReadInt32(); 
                                        byte[] bytes = reader.ReadBytes(Length);
                                        item.dirs[i].textures.Add(bytes);
                                    }

                                    //Mascaras
                                    for (var a = 0; a < 4; a++)
                                    {
                                        int Length = reader.ReadInt32(); 
                                        byte[] bytes = reader.ReadBytes(Length);
                                        item.dirs[i].masks.Add(bytes);
                                    }
                                }
                            }

                            if (info.FieldType == typeof(List<TMCreatureLoot>))
                            {
                                int _loots = reader.ReadInt32();

                                for (var i = 0; i < _loots; i++)
                                {
                                    item.loots.Add(new TMCreatureLoot());
                                    FieldInfo[] _fi = typeof(TMCreatureLoot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                                    foreach (FieldInfo _info in _fi)
                                    {
                                        try
                                        {
                                            if (_info.FieldType == typeof(int))
                                            {
                                                int _value = reader.ReadInt32();
                                                _info.SetValue(item.loots[i], _value);
                                            }

                                            if (_info.FieldType == typeof(double))
                                            {
                                                double _value = reader.ReadDouble();
                                                _info.SetValue(item.loots[i], _value);
                                            }
                                        }
                                        catch
                                        {
                                            _info.SetValue(item.loots[i], 0);
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[Creature] File Unknown Format");
                        return null;
                    }
                }
            }
            return item;
        }

        static byte[] Write(TMCreature item)
        {
            FieldInfo[] fi = typeof(TMCreature).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    byte[] Header = Encoding.ASCII.GetBytes("TMC");
                    writer.Write(Header, 0, Header.Length); //Escribimos el Header.

                    foreach (FieldInfo info in fi)
                    {
                        if (info.FieldType == typeof(string))
                        {
                            try
                            {
                                writer.Write((string)info.GetValue(item));
                            }
                            catch
                            {
                                writer.Write("");
                            }
                        }

                        if (info.FieldType == typeof(int))
                        {
                            try
                            {
                                writer.Write((int)info.GetValue(item));
                            }
                            catch
                            {
                                writer.Write(0);
                            }
                        }

                        if (info.FieldType == typeof(bool))
                        {
                            try
                            {
                                writer.Write((bool)info.GetValue(item));
                            }
                            catch
                            {
                                writer.Write(false);
                            }
                        }

                        if (info.FieldType == typeof(float))
                        {
                            try
                            {
                                writer.Write((float)info.GetValue(item));
                            }
                            catch
                            {
                                writer.Write(0);
                            }
                        }

                        if (info.FieldType == typeof(double))
                        {
                            try
                            {
                                writer.Write((double)info.GetValue(item));
                            }
                            catch
                            {
                                writer.Write(0);
                            }
                        }

                        if (info.FieldType == typeof(List<TMCreatureTexture>))
                        {
                            info.GetValue(item);

                            writer.Write(item.dirs.Count);

                            foreach (TMCreatureTexture dir in item.dirs)
                            {
                                foreach (byte[] bytes in dir.textures)
                                {
                                    writer.Write(bytes.Length);
                                    writer.Write(bytes);
                                }

                                foreach (byte[] bytes in dir.masks)
                                {
                                    writer.Write(bytes.Length);
                                    writer.Write(bytes);
                                }
                            }
                        }

                        if (info.FieldType == typeof(List<TMCreatureLoot>))
                        {
                            info.GetValue(item);

                            writer.Write(item.loots.Count);

                            foreach (TMCreatureLoot loot in item.loots)
                            {
                                FieldInfo[] _fi = typeof(TMCreatureLoot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                                foreach (FieldInfo _info in _fi)
                                {
                                    if (_info.FieldType == typeof(int))
                                    {
                                        try
                                        {
                                            writer.Write((int)_info.GetValue(loot));
                                        }
                                        catch
                                        {
                                            writer.Write(0);
                                        }
                                    }

                                    if (_info.FieldType == typeof(double))
                                    {
                                        try
                                        {
                                            writer.Write((double)_info.GetValue(loot));
                                        }
                                        catch
                                        {
                                            writer.Write(0);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                return m.ToArray();
            }
        }

        public static bool SaveToImage(byte[] bytes, string file)
        {
            try
            {
                using (Image image = Image.Load(bytes))
                {
                    image.SaveAsPng($"{file}.png");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SaveToImage] {ex}");
                return false;
            }
        }

    }
}
