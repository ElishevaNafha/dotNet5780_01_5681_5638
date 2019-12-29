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
        public String HostingUnitKey { get; private set; }
        public String GuestRequestKey { get; private set; }
        public int OrderKey { get; private set; }
        public Status Status
        {
            get { return Status; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Status), value);
                if (success)
                    Status = value;
                else
                    throw new InvalidEnumArgumentException("Non valid status");
            }
        }
        public DateTime CreateDate
        {
            get { return CreateDate; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Non valid create date");
                CreateDate = value;
            }
        }
        public DateTime OrderDate
        {
            get { return OrderDate; }
            set
            {
                if (value < CreateDate)
                    throw new ArgumentException("Non valid order date");
                OrderDate = value;
            }
        }
        public override string ToString()
        {
            return Tools.GenericToString(this);
        }
    }
}