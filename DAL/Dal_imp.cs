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
    //NEEDS TO BE FULLY IMPLEMENTED
    //THINGS TO REMEMBER: we must use linq at least 4 times, in different ways.
    // required: check whether a key already exists in all addition functions.
    //get functions will return a copy of the list and not the list itself.
    public class Dal_imp : IDAL
    {
        #region Guest Request methods
        public bool AddGuestRequest(GuestRequest request)
        {
            request.GuestRequestKey = Configuration.generateKey(request);
            var rqst = (from r in DataSource.GuestRequestsList
                        where r.GuestRequestKey == request.GuestRequestKey
                        select r).FirstOrDefault();
            if (rqst == null)
            {
                DataSource.GuestRequestsList.Add(request);
                return true;
            }
            return false;
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
                           select new GuestRequest() //we would use a clone function, but we were instructed to use select new.
                           {
                               GuestRequestKey = gr.GuestRequestKey,
                               PrivateName = gr.PrivateName,
                               FamilyName = gr.FamilyName,
                               MailAddress = gr.MailAddress,
                               Status = gr.Status,
                               RegistrationDate = gr.RegistrationDate,
                               EntryDate = gr.EntryDate,
                               ReleaseDate = gr.ReleaseDate,
                               Area = gr.Area,
                               SubArea = gr.SubArea,
                               HostingType = gr.HostingType,
                               Adults = gr.Adults,
                               Children = gr.Children,
                               Pool = gr.Pool,
                               Jacuzzi = gr.Jacuzzi,
                               Garden = gr.Garden,
                               Wifi = gr.Wifi,
                               Parking = gr.Parking,
                               ChildrensAttractions = gr.ChildrensAttractions
                           };
            return requests.ToList();
        }
        #endregion

        #region Hosting Unit methods
        public bool AddHostingUnit(HostingUnit unit)
        {
            if (unit.HostingUnitKey != 0)
            {
                return false;
            }
            unit.HostingUnitKey = Configuration.generateKey(unit);
            DataSource.HostingUnitsList.Add(unit);
            return true;
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
                //we didn't update the key because we assumed the unit stays the same unit
                unt.Owner = newUnit.Owner;
                unt.HostingUnitName = newUnit.HostingUnitName;
                unt.Area = newUnit.Area;
                unt.SubArea = newUnit.SubArea;
                unt.Pool = newUnit.Pool;
                unt.Jacuzzi = newUnit.Jacuzzi;
                unt.Garden = newUnit.Garden;
                unt.ChildrensAttractions = newUnit.ChildrensAttractions;
                unt.Wifi = newUnit.Wifi;
                unt.Parking = newUnit.Parking;
                unt.PicturesUris = newUnit.PicturesUris;
                unt.Diary = newUnit.Diary;
                return true;
            }
            return false;
        }
        public List<HostingUnit> GetHostingUnits()
        {
            var units = from hu in DataSource.HostingUnitsList
                        select new HostingUnit() //we would use a clone function, but we were instructed to use select new.
                        {
                            HostingUnitKey = hu.HostingUnitKey,
                            Owner = hu.Owner,
                            HostingUnitName = hu.HostingUnitName,
                            Area = hu.Area,
                            SubArea = hu.SubArea,
                            Pool = hu.Pool,
                            Jacuzzi = hu.Jacuzzi,
                            Garden = hu.Garden,
                            ChildrensAttractions = hu.ChildrensAttractions,
                            Wifi = hu.Wifi,
                            Parking = hu.Parking,
                            PicturesUris = hu.PicturesUris,
                            Diary = hu.Diary,
                        };
            return units.ToList();
        }
        #endregion

        #region Order methods
        public bool AddOrder(Order order)
        {
            order.OrderKey = Configuration.generateKey(order);
            var ordr = (from o in DataSource.OrdersList
                        where o.OrderKey == order.OrderKey
                        select o).FirstOrDefault();
            if (ordr == null)
            {
                DataSource.OrdersList.Add(order);
                return true;
            }
            return false;
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
                         select new Order() //we would use a clone function, but we were instructed to use select new.
                         {
                             HostingUnitKey = o.HostingUnitKey,
                             GuestRequestKey = o.GuestRequestKey,
                             OrderKey = o.OrderKey,
                             Status = o.Status,
                             CreateDate = o.CreateDate,
                             OrderDate = o.OrderDate
                         };
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
            return bbList;
        }
        #endregion
   }
}
