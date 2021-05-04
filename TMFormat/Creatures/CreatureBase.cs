using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMFormat.Models;

namespace TMFormat.Creatures
{
    public static class CreatureBase
    {
        public static CreatureModel Load(string filename)
        {
            byte[] readBytes = new byte[0];

            try
            {
                if (Instance.Content == null)
                {
                    if (!File.Exists(filename))
                    {
                        Console.WriteLine($"[CreatureBase] NotFound! => {filename}");
                        return null;
                    }

                    readBytes = File.ReadAllBytes(filename);
                    return CreatureReader.Deserialize(readBytes);
                }

                readBytes = Instance.Content.Load<byte[]>(filename);
                return CreatureReader.Deserialize(readBytes);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[CreatureBase] Error => {ex}");
                return null;
            }
        }
    }
}
