using Microsoft.Xna.Framework.Graphics;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMFormat.Formats;
using TMFormat.Maps;

namespace TMFormat.Models
{
    public static class CreatureReader
    {
        public static TMCreature Deserialize(byte[] data)
        {
            TMCreature item = new TMCreature();
            texture_index = 0;

            FieldInfo[] fi = typeof(TMCreature).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    string Header = Encoding.ASCII.GetString(reader.ReadBytes(3)); //Obtenemos el Header
                    if (Header == "ABO")
                    {
                        item = new TMCreature();

                        foreach (FieldInfo info in fi)
                        {

                            if (info.FieldType == typeof(byte[]))
                            {
                                int Length = reader.ReadInt32(); //Obtenemos lo largo en bytes de la textura.
                                byte[] bytes = reader.ReadBytes(Length);
                                info.SetValue(item, BytesToImage(bytes));
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
                                        FieldInfo[] _fi = typeof(TMCreatureAnimations).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                                        foreach (FieldInfo _info in _fi)
                                        {
                                            if (_info.FieldType == typeof(byte[]))
                                            {
                                                int Length = reader.ReadInt32(); //Obtenemos lo largo en bytes de la textura.
                                                byte[] bytes = reader.ReadBytes(Length);
                                                byte[] _texture = BytesToImage(bytes);
                                                _info.SetValue(item.Dirs[i].Animations[a], _texture);
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

        static int texture_index = 0;
        static byte[] BytesToImage(byte[] byteArrayIn)
        {
            Console.WriteLine("[BytesToImage]");
            using (Image image = Image.Load(byteArrayIn))
            {
                string root = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "textures");

                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                image.SaveAsPng(Path.Combine(root, $"texture_{texture_index}.png"));
                texture_index++;
            }

            return byteArrayIn;
        }

    }
}
