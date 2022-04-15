using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat.Framework
{
    public class VectorInt3
    {
        public int X { set; get; }
        public int Y { set; get; }
        public int Z { set; get; }

        public VectorInt3()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        public VectorInt3(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override string ToString()
        {
            return string.Format($"<{X}, {Y}, {Z}>");
        }
    }
}
