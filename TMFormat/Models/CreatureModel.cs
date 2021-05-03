using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TMFormat.Models
{
    public class CreatureProperties
    {
        public Texture2D Texture1 { set; get; }
        public Texture2D Texture2 { set; get; }
        public Texture2D Texture3 { set; get; }
        public Texture2D Texture4 { set; get; }

        public Texture2D Mask1 { set; get; }
        public Texture2D Mask2 { set; get; }
        public Texture2D Mask3 { set; get; }
        public Texture2D Mask4 { set; get; }
    }

    public class CreatureAnimations
    {
        public List<CreatureProperties> Animations { set; get; }

        public CreatureAnimations()
        {
            Animations = new List<CreatureProperties>();
            CreatureProperties anim = new CreatureProperties();
            Animations.Add(anim);

            anim = new CreatureProperties();
            Animations.Add(anim);

            anim = new CreatureProperties();
            Animations.Add(anim);

        }
    }
    public class CreatureLoot
    {
        public int id { set; get; }
        public int count { set; get; }
        public int probability { set; get; }

    }

    public class CreatureModel
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
        public List<CreatureAnimations> Dirs { set; get; }
        public List<CreatureLoot> Loots { set; get; }

        public CreatureModel()
        {
            Dirs = new List<CreatureAnimations>();
            Loots = new List<CreatureLoot>();

            CreatureAnimations _dir = new CreatureAnimations();
            Dirs.Add(_dir);

            _dir = new CreatureAnimations();
            Dirs.Add(_dir);

            _dir = new CreatureAnimations();
            Dirs.Add(_dir);

            _dir = new CreatureAnimations();
            Dirs.Add(_dir);

            _dir = new CreatureAnimations();
            Dirs.Add(_dir);
        }
    }

    public static class CreatureReader
    {
        public static CreatureModel Deserialize(byte[] data)
        {
            CreatureModel item = new CreatureModel();

            FieldInfo[] fi = typeof(CreatureModel).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    string Header = Encoding.ASCII.GetString(reader.ReadBytes(3)); //Obtenemos el Header
                    if (Header == "ABO")
                    {
                        item = new CreatureModel();

                        foreach (FieldInfo info in fi)
                        {

                            if (info.FieldType == typeof(Texture2D))
                            {
                                int Length = reader.ReadInt32(); //Obtenemos lo largo en bytes de la textura.
                                info.SetValue(item, byteArrayToImage(reader.ReadBytes(Length)));
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

                            if (info.FieldType == typeof(CreatureAnimations))
                            {
                                info.SetValue(item, reader.ReadDouble());
                            }

                            if (info.FieldType == typeof(List<CreatureAnimations>))
                            {
                                for (var i = 0; i < item.Dirs.Count; i++)
                                {
                                    for (var a = 0; a < item.Dirs[i].Animations.Count; a++)
                                    {
                                        FieldInfo[] _fi = typeof(CreatureProperties).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                                        foreach (FieldInfo _info in _fi)
                                        {
                                            if (_info.FieldType == typeof(Texture2D))
                                            {
                                                int Length = reader.ReadInt32(); //Obtenemos lo largo en bytes de la textura.
                                                _info.SetValue(item.Dirs[i].Animations[a], byteArrayToImage(reader.ReadBytes(Length)));
                                            }
                                        }
                                    }
                                }
                            }

                            if (info.FieldType == typeof(List<CreatureLoot>))
                            {
                                int _loots = reader.ReadInt32();

                                for (var i = 0; i < _loots; i++)
                                {
                                    item.Loots.Add(new CreatureLoot());
                                    FieldInfo[] _fi = typeof(CreatureLoot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                                    foreach (FieldInfo _info in _fi)
                                    {
                                        if (_info.FieldType == typeof(int))
                                        {
                                            int _value = reader.ReadInt32();
                                            _info.SetValue(item.Loots[i], _value);
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

        static Texture2D byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Texture2D returnImage = Texture2D.FromStream(Instance.Graphics, ms);
                return returnImage;
            }
            catch
            {
                return null;
            }
        }

    }

}
