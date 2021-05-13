using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat.Formats
{
    public class TMItemMap
    {
        public int id { set; get; }
        public int type { set; get; }
        public int teleporX { set; get; }
        public int teleporY { set; get; }
        public int teleporZ { set; get; }
        public string message { set; get; }
    }

    public class TMTileMap
    {
        public int type { set; get; }
        public bool pz { set; get; }
        public int id { set; get; }
        public int x { set; get; }
        public int y { set; get; }
        public int z { set; get; }
        public List<TMItemMap> items { set; get; }

        public TMTileMap()
        {
            items = new List<TMItemMap>();
        }
    }
}
