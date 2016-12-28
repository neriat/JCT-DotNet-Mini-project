using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Contract : ICloneable
    {

        public static int ContractIDNum = 9999999;
        public int ContractID { get; set; }
        public string EmployerID { get; set; }
        public string EmployeeID { get; set; }
        public bool IsInterviewed { get; set; }
        public bool IsSigned { get; set; }
        public double GrossSalary { get; set; }
        public double NetSalary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double WorkingHours { get; set; }
        public override string ToString()
        {
            return "Not implemeneted yet!!!";
        }
        object ICloneable.Clone()
        {
            Contract c = new Contract();

            c.ContractID = (int)ContractID;
            c.EmployeeID = (string)EmployeeID.Clone();
            c.EmployerID = (string)EmployerID.Clone();
            c.IsInterviewed = (bool)IsInterviewed;
            c.IsSigned = (bool)IsSigned;
            c.GrossSalary = (double)GrossSalary;
            c.NetSalary = (double)NetSalary;
            c.StartDate = (DateTime)StartDate;
            c.EndDate = (DateTime)EndDate;
            c.WorkingHours = (double)WorkingHours;

            return c;
        }
    }
}
