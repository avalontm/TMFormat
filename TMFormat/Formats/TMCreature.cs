using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using TMFormat.Models;

namespace TMFormat.Formats
{
    public class TMCreatureProperties
    {
        public byte[] Texture1 { set; get; }
        public byte[] Texture2 { set; get; }
        public byte[] Texture3 { set; get; }
        public byte[] Texture4 { set; get; }

        public byte[] Mask1 { set; get; }
        public byte[] Mask2 { set; get; }
        public byte[] Mask3 { set; get; }
        public byte[] Mask4 { set; get; }
    }

    public class TMCreatureLoot
    {
        public int id { set; get; }
        public int count { set; get; }
        public int probability { set; get; }
    }

    public class TMCreatureAnimations
    {
        public List<TMCreatureProperties> Animations { set; get; }

        public TMCreatureAnimations()
        {
            Animations = new List<TMCreatureProperties>();
            TMCreatureProperties anim = new TMCreatureProperties();
            Animations.Add(anim);

            anim = new TMCreatureProperties();
            Animations.Add(anim);

            anim = new TMCreatureProperties();
            Animations.Add(anim);

        }
    }

    public class TMCreature
    {
        public string Name { set; get; }
        public int Type { set; get; }
        public float Speed { set; get; }
        public bool isAgressive { set; get; }
        public int Experience { set; get; }
        public int Attack { set; get; }
        public int Defence { set; get; }
        public int Level { set; get; }
        public int Heal { set; get; }
        public bool isOffset { set; get; }
        public bool useSpell { set; get; }
        public int TimeSpawn { set; get; }
        public bool UseDistance { set; get; }
        public List<TMCreatureAnimations> Dirs { set; get; }
        public List<TMCreatureLoot> Loots { set; get; }

        public TMCreature()
        {
            Dirs = new List<TMCreatureAnimations>();
            Loots = new List<TMCreatureLoot>();

            TMCreatureAnimations _dir = new TMCreatureAnimations();
            Dirs.Add(_dir);

            _dir = new TMCreatureAnimations();
            Dirs.Add(_dir);

            _dir = new TMCreatureAnimations();
            Dirs.Add(_dir);

            _dir = new TMCreatureAnimations();
            Dirs.Add(_dir);

            _dir = new TMCreatureAnimations();
            Dirs.Add(_dir);
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

        static TMCreature Read(byte[] data)
        {
            TMCreature item = new TMCreature();

            FieldInfo[] fi = typeof(TMCreature).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    string Header = Encoding.ASCII.GetString(reader.ReadBytes(3)); //Obtenemos el Header

                    if (Header == "ABO")
                    {
                        foreach (FieldInfo info in fi)
                        {
                            if (info.FieldType == typeof(byte[]))
                            {
                                int Length = reader.ReadInt32(); //Obtenemos lo largo en bytes de la textura.
                                byte[] bytes = reader.ReadBytes(Length);
                                info.SetValue(item, bytes);
                            }

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

                            if (info.FieldType == typeof(TMCreatureAnimations))
                            {
                                info.SetValue(item, reader.ReadDouble());
                            }

                            if (info.FieldType == typeof(List<TMCreatureAnimations>))
                            {
                                for (var i = 0; i < item.Dirs.Count; i++)
                                {
                                    for (var a = 0; a < item.Dirs[i].Animations.Count; a++)
                                    {
                                        FieldInfo[] _fi = typeof(TMCreatureProperties).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                                        foreach (FieldInfo _info in _fi)
                                        {
                                            if (_info.FieldType == typeof(byte[]))
                                            {
                                                int Length = reader.ReadInt32(); //Obtenemos lo largo en bytes de la textura.
                                                byte[] bytes = reader.ReadBytes(Length);
                                                _info.SetValue(item.Dirs[i].Animations[a], bytes);
                                                Console.WriteLine($"[ANIM] {_info.Name}");
                                            }
                                        }
                                    }
                                }
                            }

                            if (info.FieldType == typeof(List<TMCreatureLoot>))
                            {
                                int _loots = reader.ReadInt32();

                                for (var i = 0; i < _loots; i++)
                                {
                                    item.Loots.Add(new TMCreatureLoot());
                                    FieldInfo[] _fi = typeof(TMCreatureLoot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                                    foreach (FieldInfo _info in _fi)
                                    {
                                        try
                                        {
                                            if (_info.FieldType == typeof(int))
                                            {
                                                int _value = reader.ReadInt32();
                                                _info.SetValue(item.Loots[i], _value);
                                            }

                                            if (_info.FieldType == typeof(double))
                                            {
                                                double _value = reader.ReadDouble();
                                                _info.SetValue(item.Loots[i], _value);
                                            }
                                        }
                                        catch
                                        {
                                            _info.SetValue(item.Loots[i], 0);
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return item;
        }

        public bool SaveToImage(byte[] bytes, string file)
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
