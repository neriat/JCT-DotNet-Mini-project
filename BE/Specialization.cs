using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Specialization : ICloneable
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
        object ICloneable.Clone()
        {
            Specialization s = new Specialization();
            s.SpecializationID = (int)SpecializationID;
            s.Field = (SpecializationField)Field;
            s.SpecializationName = (string)SpecializationName.Clone();
            s. MinSalary =(double)MinSalary;
            s. MaxSalary =(double)MaxSalary;
            return s;
        }
    }
}
