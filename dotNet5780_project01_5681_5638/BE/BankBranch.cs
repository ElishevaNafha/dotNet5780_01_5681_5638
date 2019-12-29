//To ask: are any other checks nesseccary?
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BE
{
    public class BankBranch
    {
        public String BankNumber
        {
            get { return BankNumber; }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if ((BankNumber[i] < '0') || (BankNumber[i] > '9'))
                        throw new ArgumentException("Non valid bank number");
                }
                BankNumber = value;
            }
        }
        public String BankName { get; set; }
        public int BranchNumber
        {
            get { return BranchNumber; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Non valid branch number");
                BranchNumber = value;
            }
        }
        public String BranchAddress { get; set; }
        public String BranchCity { get; set; }

        public override String ToString()
        {
            return Tools.GenericToString(this);
        }
    }
}
