using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFormat.Models;

namespace TMFormat.Creatures
{
    public static class Creature
    {
        public static CreatureModel Load(string filename)
        {
            try
            {
                var readBytes = Instance.Content.Load<byte[]>(filename);
                return CreatureReader.Deserialize(readBytes);
            }
            catch
            {
                return null;
            }
        }
    }
}
