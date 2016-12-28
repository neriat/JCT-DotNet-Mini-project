using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{

    public class Employee : ICloneable
    {
        public string ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public District region { get; set; }
        public enumDegree Degree { get; set; }
        public bool Veteran { get; set; }
        public BankAccount BankAccount { get; set; }
        public string SpecialityID { get; set; }
        public override string ToString()
        {
            return "Full name: " + FirstName + " " + LastName + " ID: " + ID;
        }

        public object Clone()
        {

            Employee e = new Employee();
            e.Address = (string)Address.Clone();
            e.BankAccount = (BankAccount)BankAccount.Clone();
            e.BirthDate = (DateTime)BirthDate;
            e.DealsNum = (int)DealsNum;
            e.Degree = (enumDegree)Degree;
            e.FirstName = (string)FirstName.Clone();
            e.ID = (string)ID.Clone();
            e.LastName = (string)LastName.Clone();
            e.PhoneNumber = (string)PhoneNumber.Clone();
            e.region = (District)region;
            e.SpecialityID = (string)SpecialityID.Clone();
            e.Veteran = (bool)Veteran;

            return e;
        }

        public int DealsNum { get; set; }
    }
}
