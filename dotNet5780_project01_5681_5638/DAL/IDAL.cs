using System;
using System.Collections.Generic;
using BE;

namespace DAL
{
    public interface Idal
    {
        bool addRequest(GuestRequest request);
        bool updateRequest(GuestRequest request, Status newStatus);

        bool addHostingUnit(HostingUnit unit);
        bool deleteHostingUnit(HostingUnit unit);
        bool updateHostingUnit(HostingUnit unit, HostingUnit newUnit);

        bool addOrder(Order order);
        bool updateOrder(Order order, Status newStatus);

        List<HostingUnit> getHostingUnits();
        List<GuestRequest> getGuestRequests();
        List<Order> getOrders();
        List<BankBranch> getBankBranches();
    }
}
