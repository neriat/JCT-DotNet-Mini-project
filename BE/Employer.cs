using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Employer
    {
        public string ID { get; set; }
        public bool IsCompany { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public SpecializationField Field { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public override string ToString()
        {
            if (!IsCompany)
                return "Full name: " + FirstName + " " + LastName + " ID: " + ID;
            return "Company Name: "+ CompanyName + " ID: " + ID;
        }
        public int ContractsNum { get; set; }
    }
}
