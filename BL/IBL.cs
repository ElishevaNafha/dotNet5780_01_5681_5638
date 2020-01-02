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
        void AddGuestRequest
        (String privateName, String familyName, String mailAddress, DateTime entryDate,
         DateTime releaseDate, Area area, String subArea, HostingType hostingType, int adults, int children,
         Requirements pool, Requirements jacuzzi, Requirements garden, Requirements childrensAttractions,
         Requirements wifi, Requirements parking);
        List<GuestRequest> GetGuestRequests();
        #endregion

        #region Hosting Unit methods
        void AddHostingUnit //for a new host
            (String hostingUnitName, Area area, String subArea, bool pool, bool jacuzzi, bool garden,
            bool childrensAttractions, bool wifi, bool parking, List<String> picturesUris, String hostKey, 
            String privateName, String familyName, String phoneNumber, String mailAddress, 
            BankBranch bankBranchDetails, String bankAccountNumber, bool collectionClearance,
            String password);
        void AddHostingUnit //for an existing host
            (String hostingUnitName, Area area, String subArea, bool pool, bool jacuzzi, bool garden,
            bool childrensAttractions, bool wifi, bool parking, List<String> picturesUris, Host owner);
        void UpdateHostingUnit //not including ownership changing
            (HostingUnit unit, String hostingUnitName, Area Area, String SubArea, bool Pool, bool Jacuzzi, bool Garden,
            bool ChildrensAttractions, bool wifi, bool parking);
        void addPictureToUnit(HostingUnit unit, String uri);
        void removePictureFromUnit(HostingUnit unit, String uri);
        void ChangeOwner(HostingUnit unit, Host owner);//for an existing host
        void ChangeOwner //for a new host
            (HostingUnit unit, String hostKey, String privateName, String familyName, String phoneNumber,
            String mailAddress, BankBranch bankBranchDetails, String bankAccountNumber, bool collectionClearance,
            String password);
        void UpdateOwner
            (Host owner, String privateName, String familyName, String phoneNumber, String mailAddress, 
            BankBranch bankBranchDetails, String bankAccountNumber, bool collectionClearance,
            String password);
        List<HostingUnit> getHostingUnits();
        void DeleteHostingUnit(HostingUnit unit);
        #endregion

        #region Order methods
        void CreateOrder(HostingUnit hostingUnit, GuestRequest guestRequest); //check all nesseccay parameters such as dates etc.
        void UpdateOrder(Status status, Order order);
        List<Order> getOrders();
        #endregion

        #region Bank Branch methods
        List<BankBranch> GetBankBranches();
        #endregion

        #region Queries
        List<HostingUnit> AvailableHostingUnits(DateTime date, int numDays);
        int NumDaysInRange(DateTime firstDate, DateTime secondDate = new DateTime());
        List<Order> OrderOlderThan(int numDays);
        List<GuestRequest> GuestRequestsMeetingCondition(condition condition);
        int NumOrdersToCustomer(GuestRequest request);
        int NumOrdersToUnit(HostingUnit unit);
        IEnumerable<IGrouping<Area, GuestRequest>> GroupRequestsByArea(IEnumerable<GuestRequest> guestRequests);
        IEnumerable<IGrouping<int, GuestRequest>> GroupRequestsByNumGuests(IEnumerable<GuestRequest> guestRequests);
        IEnumerable<IGrouping<int, Host>> GroupHostsByNumUnits(IEnumerable<HostingUnit> hostingUnits);
        IEnumerable<IGrouping<Area, HostingUnit>> GroupUnitsByArea(IEnumerable<HostingUnit> hostingUnits);
        IEnumerable<IGrouping<Host, HostingUnit>> GroupUnitsByHosts(IEnumerable<HostingUnit> hostingUnits);
        #endregion

        //from instructions
    }
}
