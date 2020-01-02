using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Utilities;

namespace BE
{
    public class HostingUnit
    {
        private int hostingUnitKey;
        public int HostingUnitKey 
        {
            get { return hostingUnitKey; }
            set 
            {
                if (hostingUnitKey == 0)
                    hostingUnitKey = Configuration.serialHostingUnit++;
            }
        }
        public Host Owner { get; set; }
        public String HostingUnitName { get; set; }
        private Area area;
        public Area Area
        {
            get { return area; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Area), value);
                if (success)
                    area = value;
                else
                    throw new InvalidEnumArgumentException("Non valid area");
            }
        }
        public String SubArea { get; set; }
        public bool Pool { get; set; }
        public bool Jacuzzi { get; set; }
        public bool Garden { get; set; }
        public bool ChildrensAttractions { get; set; }
        public bool Wifi { get; set; }
        public bool Parking { get; set; }
        public List<String> PicturesUris { get; set; }
        public bool[,] Diary { get; set; }
        public override string ToString()
        {
            return Tools.GenericToString(this);
        }

    }
}
