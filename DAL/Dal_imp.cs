using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;
namespace DAL
{
    public class Dal_imp : Idal
    {
        #region Find functions
        /// <summary>
        /// Find employee by its id
        /// </summary>
        /// <param name="workerID">ID</param>
        /// <returns>type of employee that id belongs to</returns>
        public Employee FindWorker(string workerID)
        {
            var a = DataSource.EmployeeList.Find(emp => emp.ID == workerID);
            if (a == null) throw new Exception("Employee doesn't exist");
            return a;
        }
        /// <summary>
        /// Find specialization by its id
        /// </summary>
        /// <param name="specializationID"></param>
        /// <returns>type of specialization that id belongs to</returns>
        public Specialization FindSpecialization(int specializationID)
        {
            Specialization sp = GetSpecializationList().Find(spec => spec.SpecializationID == specializationID);
            if (sp == null)
                throw new Exception("No such specialization exists in the system");
            return sp;
        }
        /// <summary>
        /// Find contract by its id
        /// </summary>
        /// <param name="conID"></param>
        /// <returns>type of contract that id belongs to</returns>
        public Contract FindContract(int conID)
        {
            var a = DataSource.ContractList.Find(con => con.ContractID == conID);
            if (a == null) throw new Exception("Contract doesn't exist");
            return a;
        }
        /// <summary>
        /// /// Find employer by its id        /// </summary>
        /// <param name="bossID"></param>
        /// <returns>type of employer that id belongs to</returns>
        public Employer FindBoss(string bossID)
        {
            var a = DataSource.EmployerList.Find(emp => emp.ID == bossID);
            if (a == null) throw new Exception("Employer doesn't exist");
            return a;
        }
        #endregion
        #region Bool functions
        public bool IsBankExist(string bankID)
        {
            int index = GetBankList().FindIndex(bank => bank.BankID == bankID);
            return index != -1;
        }
        //public bool WorkerExists(Employee worker)
        //{
        //    var a = DataSource.EmployeeList.Find(emp => emp.ID == worker.ID);
        //    return a != null;
        //}
        //public bool BossExists(Employer boss)
        //{
        //    var a = DataSource.EmployerList.Find(emp => emp.ID == boss.ID);
        //    return a != null;
        //}
        #endregion
        #region Add functions
        /// <summary>
        /// Add a contract to data structure that implement via list
        /// </summary>
        /// <param name="con">contract type</param>
        public void AddContract(Contract con)
        {
            if (con.ContractID == 0)
                do
                {
                    con.ContractID = ++Contract.ContractIDNum;
                }
                while (DataSource.ContractList.Find(c => c.ContractID == con.ContractID) != null);
            else
            {
                var a = DataSource.ContractList.Find(c => c.ContractID == con.ContractID);
                if (a != null)
                    throw new Exception("Contract ID is already in the system");
            }
            DataSource.ContractList.Add(con);
        }
        /// <summary>
        /// Add a emploee to data structure that implement via list
        /// </summary>
        /// <param name="emp">employee type</param>
        public void AddEmployee(Employee emp)
        {
            var a = DataSource.EmployeeList.Find(e => e.ID == emp.ID);
            if (a != null)
                throw new Exception("Employee ID is already in the system");
            DataSource.EmployeeList.Add(emp);
        }
        /// <summary>
        /// Add a employer to data structure that implement via list
        /// </summary>
        /// <param name="emp">employer type</param>
        public void AddEmployer(Employer emp)
        {
            var a = DataSource.EmployerList.Find(e => e.ID == emp.ID);
            if (a != null)
                throw new Exception("Employer ID is already in the system");
            DataSource.EmployerList.Add(emp);
        }
        /// <summary>
        /// Add a specialization to data structure that implement via list
        /// </summary>
        /// <param name="sp">specialization type</param>
        public void AddSpecialization(Specialization sp)
        {
            var a = DataSource.SpecializationList.Find(s => s.SpecializationID == sp.SpecializationID);
            if (a != null)
                throw new Exception("Specialization ID is already in the system");
            DataSource.SpecializationList.Add(sp);
        }
        #endregion
        #region Get functions
        /// <summary>
        /// The function gets access to data structure and extract the banks
        /// </summary>
        /// <returns>banks binding in list</returns>
        public List<Bank> GetBankList()
        {
            List<Bank> lst = new List<Bank>();

            //First
            Bank a = new Bank();
            a.BankID = "11";
            a.BankName = "Leumi";
            lst.Add(a);

            //Second
            a = new Bank();
            a.BankID = "14";
            a.BankName = "Otzar Hakhayal";
            lst.Add(a);

            //Third
            a = new Bank();
            a.BankID = "7";
            a.BankName = "Yahav";
            lst.Add(a);

            //4
            a = new Bank();
            a.BankID = "4";
            a.BankName = "Hapoalim";
            lst.Add(a);

            //5
            a = new Bank();
            a.BankID = "2";
            a.BankName = "Mizrahi";
            lst.Add(a);

            return lst;
            //            return DataSource.BankList;
        }
        /// <summary>
        /// The function gets access to data structure and extract the contracts
        /// </summary>
        /// <returns>contracts binding in list</returns>
        public List<Contract> GetContractList()
        {
            return DataSource.ContractList;
        }
        /// <summary>
        /// The function gets access to data structure and extract the employees
        /// </summary>
        /// <returns>employees binding in list</returns>
        public List<Employee> GetEmployeeList()
        {
            return DataSource.EmployeeList;
        }
        /// <summary>
        /// The function gets access to data structure and extract the employers
        /// </summary>
        /// <returns>employer binding in list</returns>
        public List<Employer> GetEmployerList()
        {
            return DataSource.EmployerList;
        }
        /// <summary>
        /// The function gets access to data structure and extract the specializations
        /// </summary>
        /// <returns>specializations binding in list</returns>
        public List<Specialization> GetSpecializationList()
        {
            //List<Specialization> spec = new List<Specialization>();
            //Specialization a = new Specialization();
            //a.SpecializationName = "Hello";
            //spec.Add(a);

            //a = new Specialization();
            //a.SpecializationName = "From";
            //spec.Add(a);

            //a = new Specialization();
            //a.SpecializationName = "The";
            //spec.Add(a);

            //a = new Specialization();
            //a.SpecializationName = "Other";
            //spec.Add(a);
            //a = new Specialization();
            //a.SpecializationName = "Side";
            //spec.Add(a);

            //return spec;
            return DataSource.SpecializationList;
        }
        /// <summary>
        /// The function gets access to data structure and extract the branches
        /// </summary>
        /// <returns>branches binding in list</returns>
        public List<Branch> GetBranchList()
        {
            List<Branch> branches = new List<Branch>();
            Branch b = new Branch();
            b.bank = GetBankList()[0];
            b.BranchAddress = "dhdhd dhdhd 13";
            b.BranchCity = "dcsvcss";
            b.BranchID = "1234";
            branches.Add(b);

            b = new Branch();
            b.bank = GetBankList()[1];
            b.BranchAddress = "wwww qqqq 13";
            b.BranchCity = "ggkkgg";
            b.BranchID = "156";
            branches.Add(b);

            b = new Branch();
            b.bank = GetBankList()[2];
            b.BranchAddress = "vvvv mmm 13";
            b.BranchCity = "abcd";
            b.BranchID = "2223";
            branches.Add(b);

            b = new Branch();
            b.bank = GetBankList()[3];
            b.BranchAddress = "wwww bbbb 13";
            b.BranchCity = "zzzzz";
            b.BranchID = "111";
            branches.Add(b);

            b = new Branch();
            b.bank = GetBankList()[4];
            b.BranchAddress = "wwww uu 13";
            b.BranchCity = "cc";
            b.BranchID = "111122";
            branches.Add(b);

            b = new Branch();
            b.bank = GetBankList()[4];
            b.BranchAddress = "wwww ccccc 12";
            b.BranchCity = "x";
            b.BranchID = "188";
            branches.Add(b);

            b = new Branch();
            b.bank = GetBankList()[2];
            b.BranchAddress = "cc qqqwewqqq 13";
            b.BranchCity = "ggkkgg";
            b.BranchID = "1";
            branches.Add(b);
            return branches;

        }
        #endregion
        #region Remove functions
        /// <summary>
        /// The function gets access to data structure and remove the item that id belongs to
        /// </summary>
        /// <param name="id">id of the type we remove</param>
        public void RemoveContract(int id)
        {
            var x = DataSource.ContractList.RemoveAll(con => con.ContractID == id);
            if (x == 0)
                throw new Exception("No such contract was found");

        }
        /// <summary>
        /// The function gets access to data structure and remove the item that id belongs to
        /// </summary>
        /// <param name="id">id of the type we remove</param>
        public void RemoveEmployee(string id)
        {
            var x = DataSource.EmployeeList.RemoveAll(emp => emp.ID == id);
            if (x == 0)
                throw new Exception("No such employee was found");

        }
        /// <summary>
        /// The function gets access to data structure and remove the item that id belongs to
        /// </summary>
        /// <param name="id">id of the type we remove</param>
        public void RemoveEmployer(string id)
        {
            var x = DataSource.EmployerList.RemoveAll(emp => emp.ID == id);
            if (x == 0)
                throw new Exception("No such employer was found");
        }
        /// <summary>
        /// The function gets access to data structure and remove the item that id belongs to
        /// </summary>
        /// <param name="id">id of the type we remove</param>
        public void RemoveSpecialization(int id)
        {
            var x = DataSource.SpecializationList.RemoveAll(sp => sp.SpecializationID == id);
            if (x == 0)
                throw new Exception("No such specialization was found");

        }
        #endregion
        #region Update functions
        /// <summary>
        /// The function gets access to data structure and update the item that has the same id
        /// </summary>
        /// <param name="UpdatedCon">Updated contract</param>
        public void UpdateContract(Contract UpdatedCon)
        {
            int index = DataSource.ContractList.FindIndex(con => con.ContractID == UpdatedCon.ContractID);
            if (index == -1)
                throw new Exception("No such contract was found to update");
            DataSource.ContractList[index] = UpdatedCon;
        }
        /// <summary>
        /// The function gets access to data structure and update the item that has the same id
        /// </summary>
        /// <param name="UpdatedEmp">Updated employee</param>
        public void UpdateEmployee(Employee UpdatedEmp)
        {
            int index = DataSource.EmployeeList.FindIndex(emp => emp.ID == UpdatedEmp.ID);
            if (index == -1)
                throw new Exception("No such employee was found to update");
            DataSource.EmployeeList[index] = UpdatedEmp;

        }
        /// <summary>
        /// The function gets access to data structure and update the item that has the same id
        /// </summary>
        /// <param name="UpdatedEmp">Updated employer</param>
        public void UpdateEmployer(Employer UpdatedEmp)
        {
            int index = DataSource.EmployerList.FindIndex(emp => emp.ID == UpdatedEmp.ID);
            if (index == -1)
                throw new Exception("No such employer was found to update");
            DataSource.EmployerList[index] = UpdatedEmp;
        }
        /// <summary>
        /// The function gets access to data structure and update the item that has the same id
        /// </summary>
        /// <param name="UpdatedSp">Updated specialization</param>
        public void UpdateSpecialization(Specialization UpdatedSp)
        {
            int index = DataSource.SpecializationList.FindIndex(sp => sp.SpecializationID == UpdatedSp.SpecializationID);
            if (index == -1)
                throw new Exception("No such specialization was found to update");
            DataSource.SpecializationList[index] = UpdatedSp;
        }

        #endregion
    }
}
