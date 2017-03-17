using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class Employee : ICloneable
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public District? region { get; set; }
        public enumDegree? Degree { get; set; }
        public bool Veteran { get; set; }
        public BankAccount BankAccount { get; set; }
        public string SpecialityID { get; set; }
        public int DealsNum { get; set; }
        public override string ToString()
        {
            return FirstName + " " + LastName + ", " + ID;
        }
        public object Clone()
        {
            Employee e = new Employee();
            e.Address = (string)Address.Clone();
            if (BankAccount != null)
            {
                e.BankAccount = new BankAccount();
                e.BankAccount = (BankAccount)BankAccount.Clone();
            }
        
            e.BirthDate = (DateTime)BirthDate;
            e.DealsNum = (int)DealsNum;
            if (Degree != null) e.Degree = (enumDegree)Degree;
            if (FirstName != null) e.FirstName = (string)FirstName.Clone();
            if (LastName != null) e.LastName = (string)LastName.Clone();
            if (ID != null) e.ID = (string)ID.Clone();
            if (PhoneNumber != null) e.PhoneNumber = (string)PhoneNumber.Clone();
            if (region != null) e.region = (District)region;
            if (SpecialityID != null) e.SpecialityID = (string)SpecialityID.Clone();
            e.Veteran = Veteran;

            return e;
        }
    }
}
