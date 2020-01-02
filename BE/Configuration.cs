using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Configuration
    {
        public static int serialGuestRequest = 10000000;
        public static int serialHostingUnit = 10000000;
        public static int serialOrder = 10000000;
        
        public static float Commission = 10;
        public static float TotalCommissionProfits = 0;
        public static int generateKey(object obj)
        {
            if (obj is GuestRequest)
                return serialGuestRequest++;
            if (obj is HostingUnit)
                return serialHostingUnit++;
            if (obj is Order)
                return serialOrder++;
            else
                throw new ArgumentException("Invalid object type");
        }
    }
}