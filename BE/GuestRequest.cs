using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities;

namespace BE
{
    public class GuestRequest
    {
        public int GuestRequestKey
        { 
            get;
            set;
        }
        public String PrivateName
        { 
            get;
            set; 
        }
        public String FamilyName
        { 
            get;
            set;
        }
        private String mailAddress;
        public String MailAddress 
        {
            
            get { return mailAddress; }
            set
            {
               //check taken from https://code.4noobz.net/c-email-validation/
                var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
                bool isValid = Regex.IsMatch(value, regex, RegexOptions.IgnoreCase);
                if (isValid)
                    mailAddress = value;
                else
                {
                    throw new ArgumentException("Non valid mail address");
                }

            }
        }
        private Status status;
        public Status Status
        {
           get { return status; }
           set 
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Status), value);
                if (success)
                    status = value;
                else
                    throw new InvalidEnumArgumentException("Non valid status");
            }
        }
        private DateTime registrationDate;
        public DateTime RegistrationDate
        {
            get { return registrationDate; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Non valid registeration date");
                registrationDate = value;
            } 
        }
        private DateTime entryDate;
        public DateTime EntryDate
        {
            get { return entryDate; }
            set
            {
                bool valid = true;
                if (value < DateTime.Now)
                    valid = false;
                int month = DateTime.Now.Month;
                if (value.Month == DateTime.Now.Month - 1) //more than 11 months forward
                    valid = false;
                if (valid)
                    entryDate = value;
                else
                    throw new ArgumentException("Non valid Entry date");
            }
        }
        private DateTime releaseDate;
        public DateTime ReleaseDate
        {
           get { return releaseDate; }
           set
            {
                bool valid = true;
                if (value <= EntryDate)
                    valid = false;
                if (value.Month == DateTime.Now.Month - 1) //more than 11 months forward
                    valid = false;
                if (valid)
                    releaseDate = value;
                else
                    throw new ArgumentException("Non valid Release date");
            }
        }
        private Area area;
        public Area Area
        {
            get { return area; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Area), value);
                if (success)
                    area = value;
                else
                    throw new InvalidEnumArgumentException("Non valid area");
            }
        }
        public String SubArea { get; set; }
        private HostingType hostingType;
        public HostingType HostingType
        {
            get { return hostingType; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(HostingType), value);
                if (success)
                    hostingType = value;
                else
                    throw new InvalidEnumArgumentException("Non valid hosting type");
            }

        }
        private int adults;
        public int Adults
        {
            get { return adults; }
            set 
            { 
                if (value > 0) 
                    adults = value;
                else
                    throw new ArgumentException("Non valid adults number"); 
            }
        }
        private int children;
        public int Children
        {
           get { return children; }
            set
            {
                if (value >= 0)
                    children = value;
                else 
                    throw new ArgumentException("Non valid children number"); 
            }
        }
        private Requirements pool;
        public Requirements Pool
        {
            get { return pool; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Requirements), value);
                if (success)
                    pool = value;
                else
                    throw new InvalidEnumArgumentException("Non valid pool requirements");
            }
        }
        private Requirements jacuzzi;
        public Requirements Jacuzzi
        {
           get { return jacuzzi; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Requirements), value);
                if (success)
                    jacuzzi = value;
                else
                    throw new InvalidEnumArgumentException("Non valid jacuzzi requirements");
            }
        }
        private Requirements garden;
        public Requirements Garden
        {
            get { return garden; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Requirements), value);
                if (success)
                    garden = value;
                else
                    throw new InvalidEnumArgumentException("Non valid garden requirements");
            }
        }
        private Requirements childrensAttractions;
        public Requirements ChildrensAttractions
        {
            get { return childrensAttractions; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Requirements), value);
                if (success)
                    childrensAttractions = value;
                else
                    throw new InvalidEnumArgumentException("Non valid children attractions' requirements");
            }
        }
        private Requirements wifi;
        public Requirements Wifi
        {
           get { return wifi; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Requirements), value);
                if (success)
                    wifi = value;
                else
                    throw new InvalidEnumArgumentException("Non valid wifi requirements");
            }
        }
        private Requirements parking;
        public Requirements Parking
        {
           get { return parking; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Requirements), value);
                if (success)
                    parking = value;
                else
                    throw new InvalidEnumArgumentException("Non valid Parkingp requirements");
            }
        }
        public override string ToString()
        {
            return Tools.GenericToString(this);
        }
    }
}
