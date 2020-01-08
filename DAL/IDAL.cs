using System;
using System.Collections.Generic;
using BE;

namespace DAL
{
    public interface IDAL
    { 
        #region Guest Request methods
        int AddGuestRequest(GuestRequest request);
        bool UpdateRequest(GuestRequest request, Status newStatus);
        List<GuestRequest> GetGuestRequests();
        #endregion

        #region Hosting Unit methods
        int AddHostingUnit(HostingUnit unit);
        bool DeleteHostingUnit(HostingUnit unit);
        bool UpdateHostingUnit(HostingUnit unit, HostingUnit newUnit);
        List<HostingUnit> GetHostingUnits();
        #endregion

        #region Order methods
        int AddOrder(Order order);
        bool UpdateOrder(Order order, Status newStatus);
        List<Order> GetOrders();
        #endregion

        #region Bank Branch methods
        List<BankBranch> GetBankBranches();
        #endregion
    }
}
