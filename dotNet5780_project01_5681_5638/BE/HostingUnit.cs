using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Utilities;

namespace BE
{
    public class HostingUnit
    {
        public String HostingUnitKey { get; private set; }
        public Host Owner { get; set; } //validation?
        public String HostingUnitName { get; set; }
        public bool[,] Diary { get; private set; } //MAKE SURE IT'S CHANGABLE SOMEHOW

        public override string ToString()
        {
            return Tools.GenericToString(this);
        }

    }
}
