using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities;

namespace BE
{
    public class Host
    {
        public String HostKey 
        {
            get { return HostKey; }
            set //Validate id format. check taken from https://idkn.wordpress.com/2008/11/07/%D7%90%D7%9C%D7%92%D7%95%D7%A8%D7%99%D7%AA%D7%9D-%D7%9C%D7%97%D7%99%D7%A9%D7%95%D7%91-%D7%AA%D7%A2%D7%95%D7%93%D7%AA-%D7%96%D7%94%D7%95%D7%AA/
            { //MAKE SURE IT WORKS
                bool valid = true;
                String temp = value;
                int[] id_12_digits = { 1, 2, 1, 2, 1, 2, 1, 2, 1 };
                int count = 0;
                if (temp == null)
                    valid = false;
                if (value.Length != 9)
                    valid = false;
                if (valid)
                {
                    temp = temp.PadLeft(9, '0');
                    for (int i = 0; i < 9; i++)
                    {
                        int num = Int32.Parse(temp.Substring(i, 1)) * id_12_digits[i];

                        if (num > 9)
                            num = (num / 10) + (num % 10);

                        count += num;
                    }

                    valid = (count % 10 == 0);
                }
                if (valid)
                    HostKey = value;
                else
                    throw new ArgumentException("Non valid host key");
            }
        }
        public String PrivateName { get; set; }
        public String FamilyName { get; set; }
        public String PhoneNumber
        {
            get { return PhoneNumber; }
            set
            {
                //check taken from https://was955.wordpress.com/2016/12/25/israel-phone-number-regex/
                var regex = @"^\+?(972|0)(\-)?0?(([23489]{1}\d{7})|[5]{1}\d{8})$";
                //MAKE SURE IT WORKS!
                bool isValid = Regex.IsMatch(value, regex, RegexOptions.IgnoreCase);
                if (isValid)
                    PhoneNumber = value;
                else
                {
                    throw new ArgumentException("Non valid phone number");
                }
            }
        }
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
        public BankBranch BankBranchDetails { get; set; }
        public String BankAccountNumber
        {
            get { return BankAccountNumber; }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if ((BankAccountNumber[i] < '0') || (BankAccountNumber[i] > '9'))
                        throw new ArgumentException("Non valid bank account number");
                }
                BankAccountNumber = value;
            }
        }
        public bool CollectionClearance { get; set; } // yes = true, no = false

        public override string ToString()
        {
            return Tools.GenericToString(this);
        }

    }
}