using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMFormat;
using TMFormat.Attributes;

namespace TMFormat.Models
{
    public class ItemTexture
    {
        public Texture2D Texture1 { set; get; }
        public Texture2D Texture2 { set; get; }
        public Texture2D Texture3 { set; get; }
        public Texture2D Texture4 { set; get; }
    }

    public class ItemModel
    {
        public List<ItemTexture> Textures { set; get; }
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
        public int EquipSloot { set; get; }
        public Vector3 LightColor { set; get; }
        public Vector3 Destine { set; get; }

        [NotReader]
        public string Reader { get; set; }
        [NotReader]
        public int IndexAnimation { set; get; }
        [NotReader]
        public float TimeAnimation { set; get; }
       

        public ItemModel()
        {
            Textures = new List<ItemTexture>();
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
    }

    public class Reader
    {
        static bool isNotReader(FieldInfo info)
        {
            string[] fields = info.Name.Substring(1).Split('>');
            string name = fields[0];
            bool notReader = false;
            MemberInfo memberInfo = typeof(ItemModel).GetMember(name)?[0];

            if (memberInfo != null)
            {
                object[] attributes = Attribute.GetCustomAttributes(memberInfo, true);

                foreach (NotReaderAttribute attr in attributes)
                {
                    if (attr.NotReader)
                    {
                        notReader = true;
                        break;
                    }
                }
            }

            return notReader;
        }

        public static List<ItemModel> Deserialize(byte[] data)
        {
            List<ItemModel> items = new List<ItemModel>();
            ItemModel item = new ItemModel();

            FieldInfo[] fi = typeof(ItemModel).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);

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
                            item = new ItemModel();

                            foreach (FieldInfo info in fi)
                            {
                                if (!isNotReader(info))
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

                                    if (info.FieldType == typeof(List<ItemTexture>))
                                    {
                                        int _textures = reader.ReadInt32();

                                        for (var t = 0; t < _textures; t++)
                                        {
                                            ItemTexture texture = new ItemTexture();
                                            int Length = reader.ReadInt32(); //Obtenemos lo largo en bytes de la textura.
                                            texture.Texture1 = byteArrayToImage(reader.ReadBytes(Length));

                                            Length = reader.ReadInt32();
                                            texture.Texture2 = byteArrayToImage(reader.ReadBytes(Length));

                                            Length = reader.ReadInt32();
                                            texture.Texture3 = byteArrayToImage(reader.ReadBytes(Length));

                                            Length = reader.ReadInt32();
                                            texture.Texture4 = byteArrayToImage(reader.ReadBytes(Length));

                                            item.Textures.Add(texture);
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
                return new Texture2D(Instance.Graphics, 1, 1);
            }
        }

    }
}
