using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BE
{
    public class Order
    {
        public int HostingUnitKey { get; set; }
        public int GuestRequestKey { get; set; }
        public int OrderKey { get; set;}
        private Status status;
        public Status Status
        {
            get { return status; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Status), value);
                if (success)
                    status = value;
                else
                    throw new InvalidEnumArgumentException("Non valid status");
            }
        }
        private DateTime createDate;
        public DateTime CreateDate
        {
            get { return createDate; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Non valid create date");
                createDate = value;
            }
        }
        private DateTime orderDate;
        public DateTime OrderDate
        {
            get { return orderDate; }
            set
            {
                orderDate = value;
            }
        }
        public override string ToString()
        {
            return Tools.GenericToString(this);
        }
    }
}