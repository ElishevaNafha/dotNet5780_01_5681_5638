using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BE;
using DS;

namespace DAL
{
    internal class Dal_imp : IDAL
    {
        #region Guest Request methods
        public int AddGuestRequest(GuestRequest request)
        {
            var rqst = (from r in DataSource.GuestRequestsList
                        where r.GuestRequestKey == request.GuestRequestKey
                        select r).FirstOrDefault();
            if (rqst != null)
                throw new ArgumentException("DAL: guest request already exists in the system");
            request.GuestRequestKey = Configuration.GenerateKey(request);
            DataSource.GuestRequestsList.Add(request.Clone());
            return request.GuestRequestKey;
        }
        public bool UpdateRequest(GuestRequest request, Status newStatus)
        {
            var rqst = (from gr in DataSource.GuestRequestsList
                        where gr.GuestRequestKey == request.GuestRequestKey
                        select gr).FirstOrDefault();
            if (rqst != null)
            {
                rqst.Status = newStatus;
                return true;
            }
            return false;
        }
        public List<GuestRequest> GetGuestRequests()
        {
            var requests = from gr in DataSource.GuestRequestsList
                           select gr.Clone();
            return requests.ToList();
        }
        #endregion

        #region Hosting Unit methods
        public int AddHostingUnit(HostingUnit unit)
        {
            var unt = (from u in DataSource.HostingUnitsList
                       where u.HostingUnitKey == unit.HostingUnitKey
                       select u).FirstOrDefault();

            if (unt != null)
                throw new ArgumentException("DAL: hosting unit already exists in the system");
            unit.HostingUnitKey = Configuration.GenerateKey(unit);
            DataSource.HostingUnitsList.Add(unit.Clone());
            return unit.HostingUnitKey;
        }
        public bool DeleteHostingUnit(HostingUnit unit)
        {
            var unt = (from hu in DataSource.HostingUnitsList
                       where hu.HostingUnitKey == unit.HostingUnitKey
                       select hu).FirstOrDefault();
            if (unt != null)
            {
                DataSource.HostingUnitsList.Remove(unt);
                return true;
            }
            return false;
        }
        public bool UpdateHostingUnit(HostingUnit unit, HostingUnit newUnit)
        {
            var unt = (from hu in DataSource.HostingUnitsList
                       where hu.HostingUnitKey == unit.HostingUnitKey
                       select hu).FirstOrDefault();
            if (unt != null)
            {
                unt.Copy(newUnit);

                return true;
            }
            return false;
        }
        public List<HostingUnit> GetHostingUnits()
        {
            var units = from hu in DataSource.HostingUnitsList
                        select hu.Clone();
            return units.ToList();
        }
        #endregion

        #region Order methods
       
        public int AddOrder(Order order)
        {
            var ordr = (from o in DataSource.OrdersList
                        where o.OrderKey == order.OrderKey
                        select o).FirstOrDefault();
            if (ordr != null)
                throw new ArgumentException("DAL: order already exists in the system");
            order.OrderKey = Configuration.GenerateKey(order);
            DataSource.OrdersList.Add(order.Clone());
            return order.OrderKey;
        }
        public bool UpdateOrder(Order order, Status newStatus)
        {
            var ordr = (from o in DataSource.OrdersList
                        where o.OrderKey == order.OrderKey
                        select o).FirstOrDefault();
            if (ordr != null)
            {
                ordr.Status = newStatus;
                return true;
            }
            return false;
        }
        public List<Order> GetOrders()
        {
            var orders = from o in DataSource.OrdersList
                         select o.Clone();
            return orders.ToList();
        }

        #endregion

        #region Bank Branch methods
        public List<BankBranch> GetBankBranches()
        {
            List<BankBranch> bbList = new List<BankBranch>()
            {
                new BankBranch()
                {
                    BankNumber = "111",
                    BankName = "name1",
                    BranchNumber = 1,
                    BranchAddress = "address1",
                    BranchCity = "city1"
                },
                  new BankBranch()
                {
                    BankNumber = "222",
                    BankName = "name2",
                    BranchNumber = 2,
                    BranchAddress = "address2",
                    BranchCity = "city2"
                },
                 new BankBranch()
                {
                    BankNumber = "333",
                    BankName = "name3",
                    BranchNumber = 3,
                    BranchAddress = "address3",
                    BranchCity = "city3"
                },
                 new BankBranch()
                {
                    BankNumber = "444",
                    BankName = "name4",
                    BranchNumber = 4,
                    BranchAddress = "address4",
                    BranchCity = "city4"
                },
                 new BankBranch()
                {
                    BankNumber = "555",
                    BankName = "name5",
                    BranchNumber = 5,
                    BranchAddress = "address5",
                    BranchCity = "city5"
                }
            };
            var bankBranches = from bb in bbList
                               let branch = bb // We were instructed to use let :)
                               select branch.Clone();
            return bankBranches.ToList();
        }
        #endregion
   }
}
