using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat.Models
{
    public class ItemColor
    {
        public int R { set; get; }
        public int G { set; get; }
        public int B { set; get; }

        public ItemColor()
        {

        }
        public ItemColor(int _r, int _g, int _b)
        {
            R = _r;
            G = _g;
            B = _b;
        }
    }
}
