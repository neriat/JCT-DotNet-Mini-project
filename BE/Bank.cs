using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public struct Bank : ICloneable
    {
        public string BankID { get; set; }
        public string BankName { get; set; }
        public object Clone()
        {
            Bank b = new Bank();
            b.BankID = (string)BankID;
            b.BankName = (string)BankName;
            return b;
        }
        public override string ToString()
        {
            return "Name :  " + BankName + "  Bank ID :  " + BankID;
        }
    }

    public struct Branch : ICloneable
    {
        public Bank bank { get; set; }
        public string BranchID { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCity { get; set; }
        public override string ToString()
        {
            return bank.ToString() + " Branch ID :  " + BranchID;
        }
        public object Clone()
        {
            Branch b = new Branch();
            b.bank = (Bank)bank.Clone();
            b.BranchID = (string)BranchID.Clone();
            b.BranchAddress = (string)BranchAddress.Clone();
            b.BranchCity = (string)BranchCity.Clone();

            return b;
        }
    }

    public struct BankAccount : ICloneable
    {
        public Branch branch { get; set; }
        public string AccountNumber { get; set; }

        public object Clone()
        {
            BankAccount b = new BankAccount();
            b.AccountNumber = (string)AccountNumber.Clone();
            b.branch = (Branch)branch.Clone();
            return b;
        }
    }
}
