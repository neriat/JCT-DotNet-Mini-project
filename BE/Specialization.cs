using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Specialization
    {
        public static int SpecializationIDNum = 9999999;
        public int SpecializationID{ get; set; }
        public SpecializationField Field{ get; set; }
        public string SpecializationName{ get; set; }
        public double MinSalary{ get; set; }
        public double MaxSalary{ get; set; }
        public override string ToString()
        {
            return Field + " " + SpecializationName + " " + SpecializationID;
        }
    }
}
