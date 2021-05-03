using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFormat.Models;

namespace TMFormat.Items
{
    public static class Items
    {
        public static List<ItemModel> Data = new List<ItemModel>();

        public static bool Load(string filename)
        {
            var readBytes = Instance.Content.Load<byte[]>(filename);
            Data = Reader.Deserialize(readBytes);
            if (Data != null)
            {
                return true;
            }
            return false;
        }
    }
}
