using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Text;
using TMFormat.Attributes;
using TMFormat.Helpers;
using TMFormat.Models;

namespace TMFormat.Formats
{
    public class TMItemTexture
    {
        public byte[] Texture1 { set; get; }
        public byte[] Texture2 { set; get; }
        public byte[] Texture3 { set; get; }
        public byte[] Texture4 { set; get; }
    }

    public class TMItem : ObservableObject
    {
        public List<TMItemTexture> Textures { set; get; }
        public int Id { set; get; }
        public string Name { set; get; }
        public int Type { set; get; }
        public bool Moveable { set; get; }
        public bool Block { set; get; }
        public bool Block2 { set; get; }
        public bool Block3 { set; get; }
        public bool Block4 { set; get; }

        public bool isTop1 { set; get; }
        public bool isTop2 { set; get; }
        public bool isTop3 { set; get; }
        public bool isTop4 { set; get; }

        public bool Use { set; get; }
        public int Field { set; get; }
        public int EffectID { set; get; }
        public double Speed { set; get; }
        public int Container { set; get; }
        public double AniSpeed { set; get; }
        public int Dir { set; get; }
        public bool isDown { set; get; }
        public bool isWindow { set; get; }
        public bool isAnimation { set; get; }
        public bool isLight { set; get; }
        public bool isOffset { set; get; }
        public bool isReader { set; get; }
        public bool isEquip { set; get; }
        public int EquipSlot { set; get; }
        public ItemColor LightColor { set; get; }
        public int WeaponDistance { set; get; }
        public int WeaponValue { set; get; }
        public Vector3 Destine { set; get; }

        [NotReader]
        public string Reader { get; set; }
        [NotReader]
        public int IndexAnimation { set; get; }
        [NotReader]
        public float TimeAnimation { set; get; }

        public TMItem()
        {
            Textures = new List<TMItemTexture>();
        }

        public void Copy(object src)
        {
            if (src == null)
            {
                return;
            }
            var srcT = src.GetType();
            var dstT = this.GetType();

            foreach (var f in srcT.GetFields())
            {
                try
                {
                    var dstF = dstT.GetField(f.Name);
                    if (dstF == null)
                        continue;
                    dstF.SetValue(this, f.GetValue(src));
                }
                catch
                {
                    continue;
                }
            }

            foreach (var f in srcT.GetProperties())
            {
                try
                {
                    var dstF = dstT.GetProperty(f.Name);
                    if (dstF == null)
                        continue;

                    dstF.SetValue(this, f.GetValue(src, null), null);
                }
                catch
                {
                    continue;
                }
            }
        }

        public static List<TMItem> Load(string filename, bool UseMonoGame = false)
        {
            byte[] readBytes = new byte[0];

            if (!UseMonoGame)
            {
                if (!File.Exists(filename))
                {
                    return null;
                }

                readBytes = File.ReadAllBytes(filename);
            }
            else
            {
                readBytes = TMInstance.Content.Load<byte[]>(filename);
            }

            return Read(readBytes);

        }


        static List<TMItem> Read(byte[] data)
        {
            List<TMItem> items = new List<TMItem>();
            TMItem item = null;

            FieldInfo[] fi = typeof(TMItem).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);

            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    string Header = Encoding.ASCII.GetString(reader.ReadBytes(3)); //Obtenemos el Header
                    if (Header == "ABO")
                    {
                        int count = reader.ReadInt32(); //Obtenemos el numero de items

                        for (var i = 0; i < count; i++)
                        {
                            item = new TMItem();

                            foreach (FieldInfo info in fi)
                            {
                                if (!info.isNotReader())
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

                                    if (info.FieldType == typeof(ItemColor))
                                    {
                                        int r = reader.ReadInt32();
                                        int g = reader.ReadInt32();
                                        int b = reader.ReadInt32();
                                        info.SetValue(item, new ItemColor(r, g, b));
                                    }

                                    if (info.FieldType == typeof(List<TMItemTexture>))
                                    {
                                        int _textures = reader.ReadInt32();

                                        for (var t = 0; t < _textures; t++)
                                        {
                                            TMItemTexture texture = new TMItemTexture();
                                            int Length = reader.ReadInt32(); //Obtenemos lo largo en bytes de la textura.
                                            byte[] Texture1 = reader.ReadBytes(Length);
                                            texture.Texture1 = Texture1;

                                            Length = reader.ReadInt32();
                                            byte[] Texture2 = reader.ReadBytes(Length);
                                            texture.Texture2 = Texture2;

                                            Length = reader.ReadInt32();
                                            byte[] Texture3 = reader.ReadBytes(Length);
                                            texture.Texture3 = Texture3;

                                            Length = reader.ReadInt32();
                                            byte[] Texture4 = reader.ReadBytes(Length);
                                            texture.Texture4 = Texture4;

                                            if (TMInstance.UseTextures)
                                            {
                                                item.Textures.Add(texture);
                                            }
                                        }
                                    }
                                }
                            }
                            items.Add(item);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return items;
        }
    }
}
