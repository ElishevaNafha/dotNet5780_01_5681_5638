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
    internal class BL_imp : IBL
    {
        static readonly IDAL Dal = FactorySingletonDal.Instance;

        #region Guest Request methods

        public int AddGuestRequest(GuestRequest guestRequest)
        {
            if (guestRequest.GuestRequestKey != 0)
                throw new ArgumentException("BL: new guest request can't have an initialized key");
            guestRequest.RegistrationDate = DateTime.Now;
            return Dal.AddGuestRequest(guestRequest);
        }
        public List<GuestRequest> GetGuestRequests()
        {
            return Dal.GetGuestRequests();
        }
        public GuestRequest GetGuestRequestFromKey(int key)
        {
            try
            {
                GuestRequest request = Dal.GetGuestRequests().Where(gr => gr.GuestRequestKey == key).SingleOrDefault();
                if (request == null)
                    throw new ArgumentException("BL: there is no guest request with the requested key");
                return request;
            }
            catch (ArgumentException x)
            {
                throw x;
            }
            catch (InvalidOperationException x)
            {
                throw x;
            }
        }

        #endregion

        #region Hosting Unit methods

        public int AddHostingUnit(HostingUnit hostingUnit, Host host)
        {
            try
            {
                if (hostingUnit.HostingUnitKey != 0)
                    throw new ArgumentException("BL: new hosting unit can't have an initialized key");
                if ((host.NumHostingUnits != 0) || (host.HostingUnitsKeys != null))
                    throw new ArgumentException("BL: A new host should have 0 hosting units");
                if (hostingUnit.Diary != null)
                    throw new ArgumentException("BL: new hosting unit can't have an initialized diary");

                hostingUnit.Diary = new bool[31, 12]; //all false by default
                hostingUnit.PicturesUris = new List<string>();
                hostingUnit.Owner = host;
                int key = Dal.AddHostingUnit(hostingUnit);

                //update host's hosting units' details
                hostingUnit.Owner.NumHostingUnits = 1;
                hostingUnit.Owner.HostingUnitsKeys = new List<int>();
                hostingUnit.Owner.HostingUnitsKeys.Add(key);
                FullUpdateHostingUnit(hostingUnit, hostingUnit);

                Configuration.AddLastUpdateDate(key);

                return key;
            }
            catch (ArgumentException x)
            {
                throw x;
            }
        }
        public int AddHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                if (hostingUnit.HostingUnitKey != 0)
                    throw new ArgumentException("BL: new hosting unit can't have an initialized key");
                if (hostingUnit.Diary != null)
                    throw new ArgumentException("BL: new hosting unit can't have an initialized diary");

                hostingUnit.Diary = new bool[31, 12]; //all false by default
                hostingUnit.PicturesUris = new List<string>();
                int key = Dal.AddHostingUnit(hostingUnit);

                //update host's hosting units' details
                hostingUnit.Owner.NumHostingUnits += 1;
                hostingUnit.Owner.HostingUnitsKeys.Add(key);
                FullUpdateHostingUnit(GetHostingUnitFromKey(key), hostingUnit);

                Configuration.AddLastUpdateDate(key);

                return key;
            }
            catch (ArgumentException x)
            {
                throw x;
            }
        }
        public void UpdateHostingUnit(HostingUnit unit, HostingUnit newUnit)
        {
            if ((newUnit.HostingUnitKey != unit.HostingUnitKey) || (newUnit.Owner != unit.Owner) || (newUnit.Diary != unit.Diary))
                throw new ArgumentException("BL: one or more of the changed properties cannot be updated in this method");
            Dal.UpdateHostingUnit(unit, newUnit);
        }

        /// <summary>
        /// Updates all details of hosting unit, relaying on validitions from other methods using this method
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="newUnit"></param>
        private void FullUpdateHostingUnit(HostingUnit unit, HostingUnit newUnit)
        {
            Dal.UpdateHostingUnit(unit, newUnit);
            UpdateOwnerDetails(newUnit.Owner, newUnit);
        }
        public void DeleteHostingUnit(HostingUnit unit)
        {
            if(OrdersToUnit(unit).Any(o => (o.Status == Status.MailSent) || (o.Status == Status.NotYetApproved)))
                throw new InvalidOperationException("BL: Cannot delete unit while there are active orders for it");
            bool success = Dal.DeleteHostingUnit(unit);
            if (!success)
                throw new ArgumentException("BL: Requested hosting unit does not exist in the system");
            Configuration.RemoveLastUpdateDate(unit.HostingUnitKey);
            unit.Owner.NumHostingUnits--;
            unit.Owner.HostingUnitsKeys.Remove(unit.HostingUnitKey);
            UpdateOwnerDetails(unit.Owner, unit);
        }
        public void ChangeOwner(HostingUnit unit, Host owner)
        {
            //Updates cloned Host, then updates the actual host's details in the system using the hosting unit
            unit.Owner.NumHostingUnits--;
            unit.Owner.HostingUnitsKeys.Remove(unit.HostingUnitKey);
            UpdateOwnerDetails(unit.Owner, unit);

            //Updates cloned unit's owner to the new host, then updates the actual hosting unit's and host's details
            unit.Owner = owner;
            if (owner.HostingUnitsKeys == null)
                owner.HostingUnitsKeys = new List<int>();
            Dal.UpdateHostingUnit(unit, unit); //key stayed the same, so we can send the same unit twice
            unit.Owner.NumHostingUnits++;
            unit.Owner.HostingUnitsKeys.Add(unit.HostingUnitKey);
            UpdateOwnerDetails(unit.Owner, unit);
        }
        private void UpdateDiary(HostingUnit hostingUnit, DateTime entryDate, int numDays)
        {
            //Update Diary to hold data of past month and the next 11
            DateTime lastUpdate = Configuration.GetLastUpdateDate(hostingUnit.HostingUnitKey);
            int outdatedMonths = ((DateTime.Now.Year - lastUpdate.Year) * 12) + DateTime.Now.Month - lastUpdate.Month;
            for (int i = 0; i < outdatedMonths; i++)
            {
                int month = DateTime.Now.AddMonths(-2 - i).Month; 
                for (int j = 0; j < 31; j++)
                {
                    hostingUnit.Diary[j, month] = false;
                }
            }
            Configuration.UpdateLastUpdateDate(hostingUnit.HostingUnitKey);

            //Update requested order dates
            if (!IsAvailable(hostingUnit, entryDate, numDays))
                throw new ArgumentOutOfRangeException("BL: Requested dates unavailable");
            DateTime date = entryDate;
            for (int i = 0; i < numDays; i++)
            {
                hostingUnit.Diary[date.Day - 1, date.Month - 1] = true;
                date = date.AddDays(1);
            }
            Dal.UpdateHostingUnit(hostingUnit, hostingUnit); //key stayed the same, so we can send the same unit twice
        }
        public void UpdateOwnerDetails(Host newOwner, HostingUnit ownersUnit)
        {
            if ((newOwner.CollectionClearance == false) && (ownersUnit.Owner.CollectionClearance == true))
            {
                var orders = Dal.GetOrders();
                foreach (var o in orders)
                {
                    if ((o.Status == Status.MailSent) || (o.Status == Status.NotYetApproved))
                    {
                        if (OrderBelongsTo(ownersUnit.Owner, o))
                            throw new InvalidOperationException("BL: Cannot change collection clearance to false while host has active orders");
                    }
                }
            }
            if (newOwner.HostKey != ownersUnit.Owner.HostKey)
                throw new InvalidOperationException("BL: Cannot change host's id");
            ownersUnit.Owner = newOwner;
            //Updates host's details in all his hosting units
            foreach (var key in newOwner.HostingUnitsKeys)
            {
                HostingUnit u = GetHostingUnitFromKey(key);
                u.Owner = newOwner;
                Dal.UpdateHostingUnit(u, u);
            }
        }
        public HostingUnit GetHostingUnitFromKey(int key)
        {
            try
            {
                HostingUnit unit = Dal.GetHostingUnits().Where(hu => hu.HostingUnitKey == key).SingleOrDefault();
                if (unit == null)
                    throw new ArgumentException("BL: there is no hosting unit with the requested key");
                return unit;
            }
            catch (ArgumentException x)
            {
                throw x;
            }
            catch (InvalidOperationException x)
            {
                throw x;
            }
        }
        public Host GetHostFromKey(String key)
        {
            var hostingUnits = GetHostingUnits();
            var host = (from hu in hostingUnits
                        where (hu.Owner.HostKey == key)
                        select hu.Owner).FirstOrDefault();
            return host;
        }
        public List<HostingUnit> GetHostingUnits()
        {
            return Dal.GetHostingUnits();
        }
        public void AddPicturesToUnit(HostingUnit unit, List<string> uris)
        {
            foreach (var uri in uris)
            {
                unit.PicturesUris.Add(uri);
            }
            Dal.UpdateHostingUnit(unit, unit); //key stayed the same, so we can send the same unit twice
        }
        public void RemovePicturesFromUnit(HostingUnit unit, List<string> uris)
        {
            foreach (var uri in uris)
            {
                if (unit.PicturesUris.Contains(uri))
                    unit.PicturesUris.Remove(uri);
                else
                    throw new ArgumentException("BL: one of the requested uris does not exist");
            }
            Dal.UpdateHostingUnit(unit, unit); //key stayed the same, so we can send the same unit twice
        }

        #endregion

        #region Order methods

        public int CreateOrder(HostingUnit hostingUnit, GuestRequest guestRequest)
        {
            if (!IsAvailable(hostingUnit, guestRequest.EntryDate, (guestRequest.ReleaseDate - guestRequest.EntryDate).Days))
            {
                throw new ArgumentOutOfRangeException("BL: Requested dates in order are already booked");
            }
            Order order = new Order()
            {
                HostingUnitKey = hostingUnit.HostingUnitKey,
                GuestRequestKey = guestRequest.GuestRequestKey,
                Status = Status.NotYetApproved,
                CreateDate = DateTime.Now.Date
            };
            SendMail(order);
            return Dal.AddOrder(order);
        }
        public void UpdateOrder(Status status, Order order)
        {
            HostingUnit unit = GetHostingUnitFromKey(order.HostingUnitKey);
            GuestRequest request = GetGuestRequestFromKey(order.GuestRequestKey);
            if ((GetOrderFromKey(order.OrderKey).Status == Status.CloseByApp) || (GetOrderFromKey(order.OrderKey).Status == Status.CloseByClient))
                throw new InvalidOperationException("BL: Can't change order's status after the order was closed");
            if (status == Status.CloseByClient)
            {
                UpdateDiary(unit, request.EntryDate, NumDaysInRange(request.EntryDate, request.ReleaseDate));
                Configuration.TotalCommissionProfits += (Configuration.Commission * NumDaysInRange(request.EntryDate, request.ReleaseDate));
            }
            Dal.UpdateOrder(order, status);
            Dal.UpdateRequest(request, status);
            if (status == Status.CloseByClient)
            {
                foreach (var o in OrdersToGuest(request))
                {
                    if (o.OrderKey != order.OrderKey)
                        UpdateOrder(Status.CloseByApp, o);
                }
            }
        }
        public List<Order> GetOrders()
        {
            return Dal.GetOrders();
        }
        public Order GetOrderFromKey(int key)
        {
            try
            {
                Order order = Dal.GetOrders().Where(o => o.OrderKey == key).SingleOrDefault();
                if (order == null)
                    throw new ArgumentException("BL: there is no order with the requested key");
                return order;
            }
            catch (ArgumentException x)
            {
                throw x;
            }
            catch (InvalidOperationException x)
            {
                throw x;
            }
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
                    + unit.HostingUnitName + ", that should suit your needs.\nFor more details, please contact" +
                    " me at " + hostMail + " or " + hostPhoneNumber + ".\nThank you, and have a great day!\n" +
                    hostName;
                //network query here
                //add pictures of the unit to the mail
                Console.WriteLine("Mail was sent to guest\n");
                order.Status = Status.MailSent;
                order.OrderDate = DateTime.Now.Date;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("BL: guest request or hosting unit don't exist in the system");
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
            for (int i = 0; i < numDays - 1; i++)//assuming numDays means the nights in between
            {
                if (hu.Diary[date.Day - 1, date.Month - 1])
                {
                    return false;
                }
                date.AddDays(1);
            }
            return true;
        }
       
        /// <summary>
        /// Checks weather an order was created by a certain host
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool OrderBelongsTo(Host owner, Order o)
        {
            foreach (var hostingUnitKey in owner.HostingUnitsKeys)
            {
                if (o.HostingUnitKey == hostingUnitKey)
                    return true;
            }
            return false;
        }
        private List<Order> OrdersToGuest(GuestRequest request)
        {
            var orders = Dal.GetOrders();
            var guestOrders = (from o in orders
                               where (o.GuestRequestKey == request.GuestRequestKey)
                               select o);
            return guestOrders.ToList();
        }
        private List<Order> OrdersToUnit(HostingUnit unit)
        {
            var orders = Dal.GetOrders();
            var unitOrders = (from o in orders
                              where (o.HostingUnitKey == unit.HostingUnitKey)
                              select o);
            return unitOrders.ToList();
        }
        public List<Order> OrdersOlderThan(int numDays)
        {
            var orders = Dal.GetOrders();
            var oldOrders = from o in orders
                            where NumDaysInRange(o.CreateDate) > numDays
                            select o;
            return oldOrders.ToList();
        }
        public int NumOrdersToGuest(GuestRequest request)
        {
            var orders = Dal.GetOrders();
            var guestOrders = from o in orders
                              where o.GuestRequestKey == request.GuestRequestKey
                              select new { o.OrderKey };
            return guestOrders.Count();
        }
        public int NumOrdersToUnit(HostingUnit unit)
        {
            var orders = Dal.GetOrders();
            var unitOrders = from o in orders
                             where o.HostingUnitKey == unit.HostingUnitKey
                             select new { o.OrderKey };
            return unitOrders.Count();
        }
        public int NumDaysInRange(DateTime firstDate, DateTime secondDate = default)
        {
            if (secondDate == default)
                return (DateTime.Now.Date - firstDate.Date).Days;
            return (secondDate.Date - firstDate.Date).Days + 1;
        }
        public List<GuestRequest> GuestRequestsMeetingCondition(condition condition)
        {
            var requests = Dal.GetGuestRequests();
            var rqsts = from gr in requests
                        where condition(gr)
                        select gr;
            return rqsts.ToList();
        }
        
        public IEnumerable<IGrouping<int,String>> GroupHostsByNumUnits(List<HostingUnit> hostingUnits)
        {
            var hosts = (GroupUnitsByHosts(hostingUnits));
            var grouped = from h in hosts
                          group h.Key by GetHostFromKey(h.Key).NumHostingUnits;
            return grouped;
        }
        public IEnumerable<IGrouping<Area, GuestRequest>> GroupRequestsByArea(List<GuestRequest> guestRequests)
        {
            var grouped = from rqst in guestRequests
                          group rqst by rqst.Area;
            return grouped;
        }
        public IEnumerable<IGrouping<int, GuestRequest>> GroupRequestsByNumGuests(List<GuestRequest> guestRequests)
        {
            var grouped = from rqst in guestRequests
                          let numGuests = rqst.Adults + rqst.Children
                          group rqst by numGuests;
            return grouped;
        }
        public IEnumerable<IGrouping<Area, HostingUnit>> GroupUnitsByArea(List<HostingUnit> hostingUnits)
        {
            var grouped = from unit in hostingUnits
                          group unit by unit.Area;
            return grouped;
        }
        public IEnumerable<IGrouping<String, HostingUnit>> GroupUnitsByHosts(List<HostingUnit> hostingUnits)
        {
            var grouped = from unit in hostingUnits
                          group unit by unit.Owner.HostKey;
            return grouped;
        }
        
        #endregion
    }
}
