using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FactoryDal
    {
        static Idal dl = null;
        public static Idal GetDal()
        {
            if (dl == null)
                dl = new Dal_XML_imp();
            return dl;
        }
    }
}

