using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Contract
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
            return "Employee ID: " + EmployeeID + " Employer ID: " + EmployerID + " Contract ID: " + ContractID;
        }
    }
}
