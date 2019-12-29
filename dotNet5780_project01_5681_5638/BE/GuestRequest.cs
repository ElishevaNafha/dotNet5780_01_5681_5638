using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BE
{
    public class GuestRequest
    {
        public int GuestRequestKey { get; private set; }
        public String PrivateName { get; set; }
        public String FamilyName { get; set; }
        public String MailAddress
        {
            get { return MailAddress; }
            set
            {
                //check taken from https://code.4noobz.net/c-email-validation/
                var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
                bool isValid = Regex.IsMatch(value, regex, RegexOptions.IgnoreCase);
                if (isValid)
                    MailAddress = value;
                else
                {
                    throw new ArgumentException("Non valid mail address");
                }
            }
        }
        public Status Status
        {
            get { return Status; }
            set 
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Status), value);
                if (success)
                    Status = value;
                else
                    throw new InvalidEnumArgumentException("Non valid status");
            }
        }
        public DateTime RegistrationDate
        {
            get { return RegistrationDate; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Non valid registeration date");
                RegistrationDate = value;
            } 
        }
        public DateTime EntryDate
        {
            get { return EntryDate; }
            set
            {
                bool valid = true;
                if (value < DateTime.Now)
                    valid = false;
                if (value.Month == DateTime.Now.Month - 1) //more than 11 months forward
                    valid = false;
                if (valid)
                    EntryDate = value;
                else
                    throw new ArgumentException("Non valid Entry date");
            }
        }
        public DateTime ReleaseDate
        {
            get { return ReleaseDate; }
            set
            {
                bool valid = true;
                if (ReleaseDate <= EntryDate)
                    valid = false;
                if (value.Month == DateTime.Now.Month - 1) //more than 11 months forward
                    valid = false;
                if (valid)
                    ReleaseDate = value;
                else
                    throw new ArgumentException("Non valid Release date");
            }
        }
        public Area Area
        {
            get { return Area; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Area), value);
                if (success)
                    Area = value;
                else
                    throw new InvalidEnumArgumentException("Non valid area");
            }
        }
        public String SubArea { get; set; }
        public HostingType HostingType
        {
            get { return HostingType; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(HostingType), value);
                if (success)
                    HostingType = value;
                else
                    throw new InvalidEnumArgumentException("Non valid hosting type");
            }
        }
        public int Adults
        {
            get { return Adults; }
            set 
            { 
                if (value > 0) 
                    Adults = value;
                else
                    throw new ArgumentException("Non valid adults number"); 
            }
        }
        public int Children
        {
            get { return Children; }
            set
            {
                if (value >= 0)
                    Children = value;
                else 
                    throw new ArgumentException("Non valid children number"); 
            }
        }
        public Requirements Pool
        {
            get { return Pool; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Requirements), value);
                if (success)
                    Pool = value;
                else
                    throw new InvalidEnumArgumentException("Non valid pool requirements");
            }
        }
        public Requirements Jacuzzi
        {
            get { return Jacuzzi; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Requirements), value);
                if (success)
                    Jacuzzi = value;
                else
                    throw new InvalidEnumArgumentException("Non valid jacuzzi requirements");
            }
        }
        public Requirements Garden
        {
            get { return Garden; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Requirements), value);
                if (success)
                    Garden = value;
                else
                    throw new InvalidEnumArgumentException("Non valid garden requirements");
            }
        }
        public Requirements ChildrensAttractions
        {
            get { return ChildrensAttractions; }
            set
            {
                //enum checks taken from https://www.c-sharpcorner.com/code/1307/validate-enum-value-in-c-sharp.aspx
                bool success = Enum.IsDefined(typeof(Requirements), value);
                if (success)
                    ChildrensAttractions = value;
                else
                    throw new InvalidEnumArgumentException("Non valid children attractions' requirements");
            }
        }
    }
}
