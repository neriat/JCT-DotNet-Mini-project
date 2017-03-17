using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class Bank : ICloneable
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
            return BankName + ", " + BankID;
        }

        //public static bool operator ==(Bank b, Bank c)
        //{
        //    return (b.ToString()==c.ToString());
        //}
        //public static bool operator !=(Bank b, Bank c)
        //{
        //    return (b.ToString() != c.ToString());
        //}

    }

    [Serializable]
    public class Branch : ICloneable
    {
        public Branch()
        {
            bank = new Bank();
            BranchID = null;
            BranchAddress = null;
            BranchCity = null;
        }
        public Bank bank { get; set; }
        public string BranchID { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCity { get; set; }
        public override string ToString()
        {
            return bank.ToString() + "  " + BranchID;
        }
        //public static bool operator ==(Branch b, Branch c)
        //{
        //    return (b.ToString() == c.ToString());
        //}
        //public static bool operator !=(Branch b, Branch c)
        //{
        //    return (b.ToString() != c.ToString());
        //}

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
    [Serializable]
    public class BankAccount : ICloneable
    {
        public BankAccount()
        {
            branch = new Branch();
            AccountNumber = null;
        }
        public Branch branch { get; set; }
        public string AccountNumber { get; set; }
        public override string ToString()
        {
            return branch.ToString() + "  " + AccountNumber;
        }
        //public static bool operator ==(BankAccount b, BankAccount c)
        //{
        //    return (b.ToString() == c.ToString());
        //}
        //public static bool operator !=(BankAccount b, BankAccount c)
        //{
        //    return (b.ToString() != c.ToString());
        //}
        public object Clone()
        {
            BankAccount b = new BankAccount();
            b.AccountNumber = (string)AccountNumber.Clone();
            b.branch = (Branch)branch.Clone();
            return b;
        }
    }
}
