using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public delegate bool condition(GuestRequest request);
    public interface IBL
    {
        #region Guest Request methods

        /// <summary>
        /// Creates a new guest request
        /// </summary>
        /// <param name="guestRequest"></param>
        /// <returns></returns>
        int AddGuestRequest
        (GuestRequest guestRequest);
        
        /// <summary>
        /// Returns all guest requests
        /// </summary>
        /// <returns></returns>
        List<GuestRequest> GetGuestRequests();

        /// <summary>
        /// Returns the guest request that has the sent key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        GuestRequest GetGuestRequestFromKey(int key);

        #endregion

        #region Hosting Unit methods

        /// <summary>
        /// Adds a new hosting unit for a new host
        /// </summary>
        /// <param name="hostingUnit"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        int AddHostingUnit
            (HostingUnit hostingUnit, Host host);

        /// <summary>
        /// Adds a new hosting unit to an existing host
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
        int AddHostingUnit
            (HostingUnit hostingUnit);

        /// <summary>
        /// Updates hosting unit details, not including pictures or ownership changing
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="hostingUnitName"></param>
        /// <param name="Area"></param>
        /// <param name="SubArea"></param>
        /// <param name="Pool"></param>
        /// <param name="Jacuzzi"></param>
        /// <param name="Garden"></param>
        /// <param name="ChildrensAttractions"></param>
        /// <param name="wifi"></param>
        /// <param name="parking"></param>
        void UpdateHostingUnit
            (HostingUnit unit, HostingUnit newUnit);
        
        /// <summary>
        /// Adds pictures of a hosting unit
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="uri"></param>
        void AddPicturesToUnit(HostingUnit unit, List<String> uris);

        /// <summary>
        /// Removes pictures from a hosting unit
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="uri"></param>
        void RemovePicturesFromUnit(HostingUnit unit, List<String> uris);

        /// <summary>
        /// Change hosting unit's ownership, the new owner is an existing host
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="owner">new owner</param>
        void ChangeOwner(HostingUnit unit, Host owner);

        /// <summary>
        /// Updates hosts details - finding host through one of its hosting units
        /// </summary>
        /// <param name="newOwner">new host's details</param>
        /// <param name="ownersUnit">one of the owner's units</param>
        void UpdateOwnerDetails(Host newOwner, HostingUnit ownersUnit);

        /// <summary>
        /// Returns all hosting units
        /// </summary>
        /// <returns></returns>
        List<HostingUnit> GetHostingUnits();

        /// <summary>
        /// Returns the hosting unit that has the sent key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        HostingUnit GetHostingUnitFromKey(int key);

        /// <summary>
        /// Returns the host that has the sent key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Host GetHostFromKey(String key);

        /// <summary>
        /// Deletes a hosting unit
        /// </summary>
        /// <param name="unit"></param>
        void DeleteHostingUnit(HostingUnit unit);
       
        #endregion

        #region Order methods
       
        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="hostingUnit"></param>
        /// <param name="guestRequest"></param>
        int CreateOrder(HostingUnit hostingUnit, GuestRequest guestRequest); //check all nesseccay parameters such as dates etc.
       
        /// <summary>
        /// Updates an order's status (including guest request's update)
        /// </summary>
        /// <param name="status"></param>
        /// <param name="order"></param>
        void UpdateOrder(Status status, Order order);
        
        /// <summary>
        /// Returns all orders
        /// </summary>
        /// <returns></returns>
        List<Order> GetOrders();

        /// <summary>
        /// Returns the order that has the sent key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Order GetOrderFromKey(int key);

        #endregion

        #region Bank Branch methods

        /// <summary>
        /// Returns all bank branches
        /// </summary>
        /// <returns></returns>
        List<BankBranch> GetBankBranches();
        #endregion

        #region Queries

        /// <summary>
        /// Returns all available units at a certain time range
        /// </summary>
        /// <param name="date">requested entry days</param>
        /// <param name="numDays">number of days in the visit</param>
        /// <returns></returns>
        List<HostingUnit> AvailableHostingUnits(DateTime date, int numDays);

        /// <summary>
        /// Returns the number of days between two dates
        /// </summary>
        /// <param name="firstDate"></param>
        /// <param name="secondDate"></param>
        /// <returns></returns>
        int NumDaysInRange(DateTime firstDate, DateTime secondDate = new DateTime());

        /// <summary>
        /// Returns all orders that were created more than x days ago
        /// </summary>
        /// <param name="numDays">x - number of days</param>
        /// <returns></returns>
        List<Order> OrdersOlderThan(int numDays);

        /// <summary>
        /// Checks all guest requests and returns those who meet a certain condition
        /// </summary>
        /// <param name="condition">requested condition</param>
        /// <returns></returns>
        List<GuestRequest> GuestRequestsMeetingCondition(condition condition);

        /// <summary>
        /// Returns the number of orders that were sent to a certain customer (guest request)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        int NumOrdersToGuest(GuestRequest request);

        /// <summary>
        /// Returns the number of orders that were sent for a certain hosting unit
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        int NumOrdersToUnit(HostingUnit unit);


        /// <summary>
        /// Groups a collection of guest requests by the requested area
        /// </summary>
        /// <param name="guestRequests"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<Area, GuestRequest>> GroupRequestsByArea(List<GuestRequest> guestRequests);

        /// <summary>
        /// Groups a collection of guest requests by the number of guests included in the request
        /// </summary>
        /// <param name="guestRequests"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<int, GuestRequest>> GroupRequestsByNumGuests(List<GuestRequest> guestRequests);

        /// <summary>
        /// Groups a collection of hosts given as their hosting units' owners by the number of hosting units they own. 
        /// </summary>
        /// <param name="hostingUnits"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<int, String>> GroupHostsByNumUnits(List<HostingUnit> hostingUnits);

        /// <summary>
        /// Groups a collection of hosting units by their area
        /// </summary>
        /// <param name="hostingUnits"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<Area, HostingUnit>> GroupUnitsByArea(List<HostingUnit> hostingUnits);

        /// <summary>
        /// Groups a collection of hosting units by their owners
        /// </summary>
        /// <param name="hostingUnits"></param>
        /// <returns></returns>
        IEnumerable<IGrouping<String, HostingUnit>> GroupUnitsByHosts(List<HostingUnit> hostingUnits);
        
        #endregion
    }
}
