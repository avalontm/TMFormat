using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMFormat.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class NotReaderAttribute : Attribute
    {
        public bool NotReader
        {
            get
            {
                return _notreader;
            }
        }

        bool _notreader;
        public NotReaderAttribute() : base()
        {
            this.IsDefaultAttribute();
            _notreader = true;
        }
    }
}
