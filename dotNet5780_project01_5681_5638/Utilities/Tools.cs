using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Tools
    {
        
        public static string GenericToString<T>(T obj)
        {
            String str="";
            foreach (var prop in obj.GetType().GetProperties())
            {
                str += prop.Name + "    " + prop.GetMethod.ToString()+"\n";
            }
            return str;
        }

    }
}
