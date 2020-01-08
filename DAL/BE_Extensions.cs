using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public static class BE_Extensions
    {
        #region Clone methods

        public static GuestRequest Clone(this GuestRequest gr)
        {
            GuestRequest request = new GuestRequest()
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
            return request;
        }

        public static HostingUnit Clone(this HostingUnit hu)
        {
            HostingUnit unit = new HostingUnit()
            {
                HostingUnitKey = hu.HostingUnitKey,
                HostingUnitName = hu.HostingUnitName,
                Owner = hu.Owner.Clone(),
                Area = hu.Area,
                SubArea = hu.SubArea,
                Pool = hu.Pool,
                Jacuzzi = hu.Jacuzzi,
                Garden = hu.Garden,
                ChildrensAttractions = hu.ChildrensAttractions,
                Wifi = hu.Wifi,
                Parking = hu.Parking
            };
            unit.PicturesUris = new List<string>();
            if(hu.PicturesUris != null)
            {
                foreach (var uri in hu.PicturesUris)
                {
                    unit.PicturesUris.Add(uri);
                }
            }
            unit.Diary = new bool[31, 12];
            for (int days = 0; days < 31; days++)
            {
                for (int months = 0; months < 12; months++)
                {
                    unit.Diary[days, months] = hu.Diary[days, months];
                }
            }
            return unit;
        }

        public static Host Clone(this Host h)
        {
            Host host = new Host()
            {
                HostKey = h.HostKey,
                PrivateName = h.PrivateName,
                FamilyName = h.FamilyName,
                PhoneNumber = h.PhoneNumber,
                MailAddress = h.MailAddress,
                BankBranchDetails = h.BankBranchDetails.Clone(),
                BankAccountNumber = h.BankAccountNumber,
                CollectionClearance = h.CollectionClearance,
                NumHostingUnits = h.NumHostingUnits
            };
            host.HostingUnitsKeys = new List<int>();
            if (h.HostingUnitsKeys != null)
            {
                foreach (var key in h.HostingUnitsKeys)
                {
                    host.HostingUnitsKeys.Add(key);
                }
            }
            return host;
        }

        public static Order Clone(this Order o)
        {
            Order order = new Order()
            {
                HostingUnitKey = o.HostingUnitKey,
                GuestRequestKey = o.GuestRequestKey,
                OrderKey = o.OrderKey,
                Status = o.Status,
                CreateDate = o.CreateDate,
                OrderDate = o.OrderDate
            };
            return order;
        }

        public static BankBranch Clone(this BankBranch bb)
        {
            BankBranch branch = new BankBranch()
            {
                BankNumber = bb.BankNumber,
                BankName = bb.BankName,
                BranchNumber = bb.BranchNumber,
                BranchAddress = bb.BranchAddress,
                BranchCity = bb.BranchCity
            };
            return branch;
        }

        #endregion

        #region copy methods

        public static void Copy(this HostingUnit hostingUnit, HostingUnit hu)
        {
            //we don't copy the key because we assume we want to change the unit's details and not the unit itself
            hostingUnit.HostingUnitName = hu.HostingUnitName;
            hostingUnit.Owner.Copy(hu.Owner);
            hostingUnit.Area = hu.Area;
            hostingUnit.SubArea = hu.SubArea;
            hostingUnit.Pool = hu.Pool;
            hostingUnit.Jacuzzi = hu.Jacuzzi;
            hostingUnit.Garden = hu.Garden;
            hostingUnit.ChildrensAttractions = hu.ChildrensAttractions;
            hostingUnit.Wifi = hu.Wifi;
            hostingUnit.Parking = hu.Parking;
            hostingUnit.PicturesUris = new List<string>();
            foreach (var uri in hu.PicturesUris)
            {
                hostingUnit.PicturesUris.Add(uri);
            }
            hostingUnit.Diary = new bool[31, 12];
            for (int days = 0; days < 31; days++)
            {
                for (int months = 0; months < 12; months++)
                {
                    hostingUnit.Diary[days, months] = hu.Diary[days, months];
                }
            }
        }

        public static void Copy(this Host host, Host h)
        {
            host.HostKey = h.HostKey;
            host.PrivateName = h.PrivateName;
            host.FamilyName = h.FamilyName;
            host.PhoneNumber = h.PhoneNumber;
            host.MailAddress = h.MailAddress;
            host.BankBranchDetails.Copy(h.BankBranchDetails);
            host.BankAccountNumber = h.BankAccountNumber;
            host.CollectionClearance = h.CollectionClearance;
            host.NumHostingUnits = h.NumHostingUnits;
            host.HostingUnitsKeys = new List<int>();
            foreach (var key in h.HostingUnitsKeys)
            {
                host.HostingUnitsKeys.Add(key);
            }

        }

        public static void Copy(this BankBranch bankBranch, BankBranch bb)
        {
            bankBranch.BankNumber = bb.BankNumber;
            bankBranch.BankName = bb.BankName;
            bankBranch.BranchNumber = bb.BranchNumber;
            bankBranch.BranchAddress = bb.BranchAddress;
            bankBranch.BranchCity = bb.BranchCity;
        }

        #endregion
    }
}
