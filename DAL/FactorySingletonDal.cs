using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FactorySingletonDal
    {
        private static IDAL instance = null;

        static FactorySingletonDal() { }

        public static IDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Dal_imp();
                }
                return instance;
            }
        }
    }
}
