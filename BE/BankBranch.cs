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
        private String bankNumber;
        public String BankNumber
        {
            get { return bankNumber; }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if ((value[i] < '0') || (value[i] > '9'))
                        throw new ArgumentException("Non valid bank number");
                }
                bankNumber = value;
            }
        }
        public String BankName { get; set; }
        private int branchNumber;
        public int BranchNumber
        {
            get { return branchNumber; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Non valid branch number");
                branchNumber = value;
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
