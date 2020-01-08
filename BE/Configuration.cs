using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    //MOVE TO BL?
    public static class Configuration
    {
        private static int serialGuestRequest = 10000000;
        private static int serialHostingUnit = 10000000;
        private static int serialOrder = 10000000;
        
        public static float Commission = 10;
        public static float TotalCommissionProfits = 0;

        private static Dictionary<int, DateTime> lastDiaryUpdate = new Dictionary<int, DateTime>();
        public static int GenerateKey(object obj)
        {
            if (obj is GuestRequest)
                return serialGuestRequest++;
            else if (obj is HostingUnit)
                return serialHostingUnit++;
            else if (obj is Order)
                return serialOrder++;
            else
                throw new ArgumentException("Invalid object type");
        }
        public static void AddLastUpdateDate(int key) { lastDiaryUpdate.Add(key, DateTime.Now); }
        public static void RemoveLastUpdateDate(int key) { lastDiaryUpdate.Remove(key); }
        public static DateTime GetLastUpdateDate(int key) { return lastDiaryUpdate[key]; }
        public static void UpdateLastUpdateDate(int key) { lastDiaryUpdate[key] = DateTime.Now; }
    }
}