using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    class BL_imp : IBL
    {
        static IDAL Dal = FactorySingletonDal.Instance;

        #region Guest Request methods
        public void AddGuestRequest(String privateName, String familyName, String mailAddress, DateTime entryDate, DateTime releaseDate, Area area, String subArea, HostingType hostingType, int adults, int children, Requirements pool, Requirements jacuzzi, Requirements garden, Requirements childrensAttractions, Requirements wifi, Requirements parking)
        {
            try
            {
                GuestRequest request = new GuestRequest()
                {
                    PrivateName = privateName,
                    FamilyName = familyName,
                    MailAddress = mailAddress,
                    Status = Status.NotYetApproved,
                    RegistrationDate = DateTime.Now.Date,
                    EntryDate = entryDate,
                    ReleaseDate = releaseDate,
                    Area = area,
                    SubArea = subArea,
                    HostingType = hostingType,
                    Adults = adults,
                    Children = children,
                    Pool = pool,
                    Jacuzzi = jacuzzi,
                    Garden = garden,
                    ChildrensAttractions = childrensAttractions,
                    Wifi = wifi,
                    Parking = parking
                };
                if (!Dal.AddGuestRequest(request))
                    throw new ArgumentException("Guest Request Already exists in the system");
            }
            catch (ArgumentException x)
            {
                throw x;
            }
        }
        public List<GuestRequest> GetGuestRequests()
        {
            return Dal.GetGuestRequests();
        }
        private GuestRequest GetGuestRequestFromKey(int key)
        {
            var guestRequests = Dal.GetGuestRequests();
            var request = (from gr in guestRequests
                           where gr.GuestRequestKey == key
                           select gr).FirstOrDefault();
            if (request == null)
                throw new ArgumentException("there is no guest request with the requested key");
            return request;
        }
        #endregion

        #region Hosting Unit methods
        /// <summary>
        /// Adds a new hosting unit to a new host - and meanwhile creates the host himself 
        /// </summary>
        /// <param name="hostingUnitName"></param>
        /// <param name="hostKey"></param>
        /// <param name="privateName"></param>
        /// <param name="familyName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="mailAddress"></param>
        /// <param name="bankBranchDetails"></param>
        /// <param name="bankAccountNumber"></param>
        /// <param name="collectionClearance">approves withdrawing money from his bank account</param>
        /// <returns></returns>
        public void AddHostingUnit(String hostingUnitName, Area area, String subArea, bool pool, bool jacuzzi, bool garden, bool childrensAttractions, bool wifi, bool parking, List<String> picturesUris, String hostKey, String privateName, String familyName, String phoneNumber, String mailAddress, BankBranch bankBranchDetails, String bankAccountNumber, bool collectionClearance, String password)
        {
            try
            {
                HostingUnit unit = new HostingUnit()
                {
                    Owner = new Host()
                    {
                        HostKey = hostKey,
                        PrivateName = privateName,
                        FamilyName = familyName,
                        PhoneNumber = phoneNumber,
                        MailAddress = mailAddress,
                        BankBranchDetails = bankBranchDetails,
                        BankAccountNumber = bankAccountNumber,
                        CollectionClearance = collectionClearance,
                        Password = password,
                        NumHostingUnits = 1,
                        HostingUnitsKeys = new List<int>()
                    },
                    HostingUnitName = hostingUnitName,
                    Area = area,
                    SubArea = subArea,
                    Pool = pool,
                    Jacuzzi = jacuzzi,
                    Garden = garden,
                    ChildrensAttractions = childrensAttractions,
                    Wifi = wifi,
                    Parking = parking,
                    PicturesUris = picturesUris,
                    Diary = new bool[31,12] //all false by default
                };
                bool success = Dal.AddHostingUnit(unit);
                if (!success)
                    throw new ArgumentException("Hosting unit Already exists in the system");
                unit.Owner.HostingUnitsKeys.Add(unit.HostingUnitKey); //done later because before it key is not yet generated
            }
            catch (ArgumentException x)
            {
                throw x;
            }
        }
        
        /// <summary>
        /// adds a new hosting unit to an existing host
        /// </summary>
        /// <param name="hostingUnitName"></param>
        /// <param name="area"></param>
        /// <param name="subArea"></param>
        /// <param name="pool"></param>
        /// <param name="jacuzzi"></param>
        /// <param name="garden"></param>
        /// <param name="childrensAttractions"></param>
        /// <param name="wifi"></param>
        /// <param name="parking"></param>
        /// <param name="picturesUris"></param>
        /// <param name="owner"></param>
        public void AddHostingUnit(string hostingUnitName, Area area, String subArea, bool pool, bool jacuzzi, bool garden, bool childrensAttractions, bool wifi, bool parking, List<String> picturesUris, Host owner)
        {
            try
            {
                HostingUnit unit = new HostingUnit()
                {
                    Owner = owner,
                    HostingUnitName = hostingUnitName,
                    Area = area,
                    SubArea = subArea,
                    Pool = pool,
                    Jacuzzi = jacuzzi,
                    Garden = garden,
                    ChildrensAttractions = childrensAttractions,
                    Wifi = wifi,
                    Parking = parking,
                    PicturesUris = picturesUris,
                    Diary = new bool[31, 12] //all false by default
                };
                bool success = Dal.AddHostingUnit(unit);
                if (!success)
                    throw new ArgumentException("Hosting unit Already exists in the system");
                unit.Owner.HostingUnitsKeys.Add(unit.HostingUnitKey);
                unit.Owner.NumHostingUnits++;
            }
            catch (ArgumentException x)
            {
                throw x;
            }
        }

        /// <summary>
        /// updates hosting unit (not including pictures)
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="hostingUnitName"></param>
        /// <param name="area"></param>
        /// <param name="subArea"></param>
        /// <param name="pool"></param>
        /// <param name="jacuzzi"></param>
        /// <param name="garden"></param>
        /// <param name="childrensAttractions"></param>
        /// <param name="wifi"></param>
        /// <param name="parking"></param>
        public void UpdateHostingUnit(HostingUnit unit, string hostingUnitName, Area area, String subArea, bool pool, bool jacuzzi, bool garden, bool childrensAttractions, bool wifi, bool parking)
        {
            unit.HostingUnitName = hostingUnitName;
            unit.Area = area;
            unit.SubArea = subArea;
            unit.Pool = pool;
            unit.Jacuzzi = jacuzzi;
            unit.Garden = garden;
            unit.ChildrensAttractions = childrensAttractions;
            unit.Wifi = wifi;
            unit.Parking = parking;
        }
        public void DeleteHostingUnit(HostingUnit unit)
        {
            List<Order> ordersToUnit = OrdersToUnit(unit);
            var activeOrder = (from o in ordersToUnit
                               where (o.Status == Status.MailSent) || (o.Status == Status.NotYetApproved)
                               select o).FirstOrDefault();
            if (activeOrder != null)
                throw new InvalidOperationException("Cannot delete unit while there are active orders for it");
            bool success = Dal.DeleteHostingUnit(unit);
            if (!success)
                throw new ArgumentException("Requested hosting unit does not exist in the system");
            unit.Owner.NumHostingUnits--;
            unit.Owner.HostingUnitsKeys.Remove(unit.HostingUnitKey);
        }
        public void ChangeOwner(HostingUnit unit, Host owner)
        {
            unit.Owner.NumHostingUnits--;
            unit.Owner.HostingUnitsKeys.Remove(unit.HostingUnitKey);
            unit.Owner = owner;
            unit.Owner.NumHostingUnits++;
            unit.Owner.HostingUnitsKeys.Add(unit.HostingUnitKey);
        }
        public void ChangeOwner(HostingUnit unit, string hostKey, string privateName, string familyName, string phoneNumber, string mailAddress, BankBranch bankBranchDetails, string bankAccountNumber, bool collectionClearance, string password)
        {
            Host newHost;
            try
            {
                newHost = new Host()
                {
                    HostKey = hostKey,
                    PrivateName = privateName,
                    FamilyName = familyName,
                    PhoneNumber = phoneNumber,
                    MailAddress = mailAddress,
                    BankBranchDetails = bankBranchDetails,
                    BankAccountNumber = bankAccountNumber,
                    CollectionClearance = collectionClearance,
                    Password = password
                };
            }
            catch (ArgumentException x)
            {
                throw x;
            }
            unit.Owner.NumHostingUnits--;
            unit.Owner.HostingUnitsKeys.Remove(unit.HostingUnitKey);
            unit.Owner = newHost;
            unit.Owner.NumHostingUnits = 1;
            unit.Owner.HostingUnitsKeys = new List<int> { unit.HostingUnitKey };
        }
        public void UpdateDiary(HostingUnit hostingUnit, DateTime entryDate, int numDays)
        {
            if (!IsAvailable(hostingUnit, entryDate, numDays))
                throw new ArgumentOutOfRangeException("Requested dates unavailable");
            DateTime date = entryDate;
            for (int i = 0; i < numDays - 1; i++)
            {
                hostingUnit.Diary[date.Day - 1, date.Month - 1] = true;
                date.AddDays(1);
            }
        }
        public void UpdateOwner(Host owner, string privateName, string familyName, string phoneNumber, string mailAddress, BankBranch bankBranchDetails, string bankAccountNumber, bool collectionClearance, string password)
        {
            owner.PrivateName = privateName;
            owner.FamilyName = familyName;
            owner.PhoneNumber = phoneNumber;
            owner.MailAddress = mailAddress;
            owner.BankBranchDetails = bankBranchDetails;
            owner.BankAccountNumber = bankAccountNumber;
            owner.Password = password;
            if ((collectionClearance == false) && (owner.CollectionClearance == true))
            {
                var orders = Dal.GetOrders();
                foreach (var o in orders)
                {
                    if ((o.Status == Status.MailSent) || (o.Status == Status.NotYetApproved))
                    {
                        if (OrderBelongsTo(owner, o))
                            throw new InvalidOperationException("Cannot change collection clearance to false while host has active orders");
                    }
                }
            }
            owner.CollectionClearance = collectionClearance;

        }
        private HostingUnit GetHostingUnitFromKey(int key)
        {
            var hostingUnits = Dal.GetHostingUnits();
            var unit = (from hu in hostingUnits
                        where hu.HostingUnitKey == key
                        select hu).FirstOrDefault();
            if (unit == null)
                throw new ArgumentException("there is no hosting unit with the requested key");
            return unit;
        }
        public List<HostingUnit> getHostingUnits()
        {
            return Dal.GetHostingUnits();
        }
        public void addPictureToUnit(HostingUnit unit, string uri)
        {
            unit.PicturesUris.Add(uri);
        }
        public void removePictureFromUnit(HostingUnit unit, string uri)
        {
            if (unit.PicturesUris.Contains(uri))
                unit.PicturesUris.Remove(uri);
            else
                throw new ArgumentException("uri does not exist");
        }
        #endregion

        #region Order methods
        public void CreateOrder(HostingUnit hostingUnit, GuestRequest guestRequest)
        {
            if (!IsAvailable(hostingUnit, guestRequest.EntryDate, (guestRequest.ReleaseDate - guestRequest.EntryDate).Days))
            {
                throw new ArgumentOutOfRangeException("Requested dates in order are already booked");
            }
            Order order = new Order()
            {
                HostingUnitKey = hostingUnit.HostingUnitKey,
                GuestRequestKey = guestRequest.GuestRequestKey,
                Status = Status.NotYetApproved,
                CreateDate = DateTime.Now.Date
            };
            SendMail(order);
            Dal.AddOrder(order);
        }
        public void UpdateOrder(Status status, Order order)
        {
            HostingUnit unit = GetHostingUnitFromKey(order.HostingUnitKey);
            GuestRequest request = GetGuestRequestFromKey(order.GuestRequestKey);
            if ((order.Status == Status.CloseByApp) || (order.Status == Status.CloseByClient))
                throw new InvalidOperationException("Can't change order's status after the order was closed");
            if (status == Status.CloseByClient)
            {
                UpdateDiary(unit, request.EntryDate, NumDaysInRange(request.EntryDate, request.ReleaseDate));
                Configuration.TotalCommissionProfits += (Configuration.Commission * NumDaysInRange(request.EntryDate, request.ReleaseDate));
            }
            Dal.UpdateOrder(order, status);
            Dal.UpdateRequest(request, status);
            foreach (var o in OrdersToGuest(request))
            {
                if (o.OrderKey != order.OrderKey)
                    UpdateOrder(Status.CloseByApp, o);
            }
        }
        public List<Order> getOrders()
        {
            return Dal.GetOrders();
        }
        private void SendMail(Order order)
        {
            //IMPLEMENT WITH NETWORK QUERY AT STEP 3
            try
            {
                HostingUnit unit = GetHostingUnitFromKey(order.HostingUnitKey);
                if (!unit.Owner.CollectionClearance)
                    return;
                String hostMail = unit.Owner.MailAddress;
                String hostPhoneNumber = unit.Owner.PhoneNumber;
                String hostName = unit.Owner.PrivateName;
                GuestRequest request = GetGuestRequestFromKey(order.GuestRequestKey);
                String guestMail = request.MailAddress;
                string Mail = "Dear " + request.PrivateName + ",\nI'm happy to invite you to my hosting unit, "
                    + unit.HostingUnitName + ", that should suit your needs.\nfor more details, please contact" +
                    " me at " + hostMail + " or " + hostPhoneNumber + ".\nThank you, and have a great day!\n" +
                    hostName;
                //network query here
                //add pictures of the unit to the mail
                Console.WriteLine("Mail was sent to guest");
                order.Status = Status.MailSent;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("guest request or hosting unit don't exist in the system");
            }
        }
        #endregion

        #region Bank Branch methods
        public List<BankBranch> GetBankBranches()
        {
            return Dal.GetBankBranches();
        }
        #endregion

        #region Queries
            /// <summary>
            /// finds all units available at a certain time range
            /// </summary>
            /// <param name="date">Entry date</param>
            /// <param name="numDays">number of days in range</param>
            /// <returns></returns>
            public List<HostingUnit> AvailableHostingUnits(DateTime date, int numDays)
        {
            List<HostingUnit> hostingUnits = Dal.GetHostingUnits();
            var availableUnits = from hu in hostingUnits
                                 where IsAvailable(hu, date, numDays)
                                 select hu;
            return availableUnits.ToList();
        }
        private bool IsAvailable(HostingUnit hu, DateTime date, int numDays)
        {
            for (int i = 0; i < numDays-1; i++)//assuming numDays means the nights in between
            {
                if (hu.Diary[date.Day-1,date.Month-1])
                {
                    return false;
                }
                date.AddDays(1);
            }
            return true;
        }
        private bool OrderBelongsTo(Host owner,Order o)
        {
            foreach (var hostingUnitKey in owner.HostingUnitsKeys)
            {
                if (o.HostingUnitKey == hostingUnitKey)
                    return true;
            }
            return false;
        }
        public List<Order> OrdersToGuest(GuestRequest request)
        {
            var orders = Dal.GetOrders();
            var guestOrders = (from o in orders
                               where (o.GuestRequestKey == request.GuestRequestKey)
                               select o);
            return guestOrders.ToList();
        }
        public List<Order> OrdersToUnit(HostingUnit unit)
        {
            var orders = Dal.GetOrders();
            var unitOrders = (from o in orders
                              where (o.HostingUnitKey == unit.HostingUnitKey)
                              select o);
            return unitOrders.ToList();
        }
        public List<Order> OrderOlderThan(int numDays)
        {
            var orders = Dal.GetOrders();
            var oldOrders = from o in orders
                            where NumDaysInRange(o.CreateDate) > numDays
                            select o;
            return oldOrders.ToList();
        }
        public int NumOrdersToCustomer(GuestRequest request)
        {
            var orders = Dal.GetOrders();
            var customerOrders = from o in orders
                                 where o.GuestRequestKey == request.GuestRequestKey
                                 select o;
            return customerOrders.Count();
        }
        public int NumOrdersToUnit(HostingUnit unit)
        {
            var orders = Dal.GetOrders();
            var unitOrders = from o in orders
                             where o.HostingUnitKey == unit.HostingUnitKey
                             select o;
            return unitOrders.Count();
        }
        public int NumDaysInRange(DateTime firstDate, DateTime secondDate = default)
        {
            if (secondDate == default)
                return (DateTime.Now.Date - firstDate.Date).Days;
            return (secondDate.Date - firstDate.Date).Days;
        }
        public List<GuestRequest> GuestRequestsMeetingCondition(condition condition)
        {
            var requests = Dal.GetGuestRequests();
            var rqsts = from gr in requests
                        where condition(gr)
                        select gr;
            return rqsts.ToList();
        }

        public IEnumerable<IGrouping<int, Host>> GroupHostsByNumUnits(IEnumerable<HostingUnit> hostingUnits)
        {
            var hosts = GroupUnitsByHosts(hostingUnits);
            var grouped = from h in hosts
                          group h.Key by h.Key.NumHostingUnits;
            return grouped;

        }
        public IEnumerable<IGrouping<Area, GuestRequest>> GroupRequestsByArea(IEnumerable<GuestRequest> guestRequests)
        {
            var grouped = from rqst in guestRequests
                          group rqst by rqst.Area;
            return grouped;
        }
        public IEnumerable<IGrouping<int, GuestRequest>> GroupRequestsByNumGuests(IEnumerable<GuestRequest> guestRequests)
        {
            var grouped = from rqst in guestRequests
                          let numGuests = rqst.Adults + rqst.Children
                          group rqst by numGuests;
            return grouped;
        }
        public IEnumerable<IGrouping<Area, HostingUnit>> GroupUnitsByArea(IEnumerable<HostingUnit> hostingUnits)
        {
            var grouped = from unit in hostingUnits
                          group unit by unit.Area;
            return grouped;
        }
        public IEnumerable<IGrouping<Host, HostingUnit>> GroupUnitsByHosts(IEnumerable<HostingUnit> hostingUnits)
        {
            var grouped = from unit in hostingUnits
                          group unit by unit.Owner;
            return grouped;
        }
        #endregion
    }
}
