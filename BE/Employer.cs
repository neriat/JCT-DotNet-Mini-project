using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class Employer : ICloneable
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public bool IsCompany { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public SpecializationField? Field { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public int ContractsNum { get; set; }
        public override string ToString()
        {
            if (IsCompany)
                return CompanyName;
            return FirstName + " " + LastName;
        }
        public object Clone()
        {
            Employer e = new Employer();

            e.ID = (string)ID;
            e.IsCompany = (bool)IsCompany;
            if(LastName!=null) e.LastName = (string)LastName.Clone();
            if (FirstName != null) e.FirstName = (string)FirstName.Clone();
            if (CompanyName != null) e.CompanyName = (string)CompanyName.Clone();
            if (PhoneNumber!= null) e.PhoneNumber = (string)PhoneNumber.Clone();
            e.Address = (string)Address.Clone();
            e.Field = (SpecializationField)Field;
            e.EstablishmentDate = (DateTime)EstablishmentDate;
            e.ContractsNum = (int)ContractsNum;

            return e;
        }

    }
}
