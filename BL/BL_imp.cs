using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    public class BL_imp : IBL

    {
        //DAL.Dal_imp data = new Dal_imp();
        Idal data = FactoryDal.GetDal();

        #region Finding functions
        /// <summary>
        /// gets an specialization id as int and return its specialization type
        /// </summary>
        /// <param name="SpecializationID">Specialization ID</param>
        /// <returns> specialization type </returns>
        public Specialization FindSpecialization(int SpecializationID)
        {
            return data.FindSpecialization(SpecializationID);
        }
        public Contract FindContract(int ContractID)
        {
            return data.FindContract(ContractID);
        }
        public Employee FindWorker(string workerID)
        {
            return data.FindWorker(workerID);
        }
        public Employer FindBoss(string bossID)
        {
            return data.FindBoss(bossID);
        }
        #endregion
        #region Adding functions
        /// <summary>
        /// The function adds the contract to database right after a validification check 
        /// </summary>
        /// <param name="con">Contract</param>
        public void AddContract(Contract con)
        {
            ExceptionContract(con);

            con.NetSalary = CalcWorkerNetSalary(con);

            Employee worker = data.FindWorker(con.EmployeeID);
            Employer boss = data.FindBoss(con.EmployerID);
            worker.DealsNum++;
            boss.ContractsNum++;
            UpdateEmployee(worker);
            UpdateEmployer(boss);

            data.AddContract(con);

        }
        /// <summary>
        /// The function adds the employee to database right after a validification check 
        /// </summary>
        /// <param name="emp">Employee</param>
        public void AddEmployee(Employee emp)
        {
            ExceptionEmployee(emp);
            data.AddEmployee(emp);
        }
        /// <summary>
        /// The function adds the employer to database right after a validification check 
        /// </summary>
        /// <param name="emp">Employer</param>
        public void AddEmployer(Employer emp)
        {
            ExceptionEmployer(emp);
            data.AddEmployer(emp);
        }
        /// <summary>
        /// The function adds the specialization to database right after a validification check 
        /// </summary>
        /// <param name="sp">Specialization</param>
        public void AddSpecialization(Specialization sp)
        {
            ExceptionSpecialization(sp);
            data.AddSpecialization(sp);
        }
        #endregion
        #region Calculation functions
        /// <summary>
        /// Calculate net salary from gross salary, depands on: number of employee's contracts, number of employer's contracts and if the employee is vetern.
        /// </summary>
        /// <param name="workerID">Employee ID</param>
        /// <param name="bossID">Employer ID</param>
        /// <param name="contract">Contract type</param>
        /// <returns>Net salary after humam-resources fee</returns>
        public double CalcWorkerNetSalary(Contract contract)
        {
            // the net salary goes like that:
            //  (gross salary) - (boss fee based on number of employers under him)*(1.01 -if the worker isn't IDF vetern)/(num of deals worker did + 1)
            //  boss fee: starts at 40. downby .7 for every signed contract. state at 4
            Employee worker = data.FindWorker(contract.EmployeeID);
            Employer boss = data.FindBoss(contract.EmployerID);

            double armyDiscount = 1.01; //default mode
            if (worker.Veteran) armyDiscount = 1;

            double BossFee = 40 - 0.7 * boss.ContractsNum;
            if (BossFee < 4) BossFee = 4; //lowest fee

            if ((contract.GrossSalary - (BossFee * armyDiscount) / (worker.DealsNum + 1) <= 0))
                if (contract.GrossSalary - 0.9 <= 0) return contract.GrossSalary;
                else return contract.GrossSalary-0.9;
            else
                return contract.GrossSalary - (BossFee * armyDiscount) / (worker.DealsNum + 1);

        }
        #endregion
        #region Exception-validification void functions
        /// <summary>
        /// A check for  human-resource-terms, logical & typo errors
        /// </summary>
        /// <param name="con">Contract</param>
        private void ExceptionContract(Contract con)
        {
            // exsistence issues
            if (!WorkerExists(con.EmployeeID) && !BossExists(con.EmployerID))
                throw new Exception("Both the employee and the eployer don't exist");
            else if (!WorkerExists(con.EmployeeID))
                throw new Exception("Employee doesn't exist");
            else if (!BossExists(con.EmployerID))
                throw new Exception("Employer doesn't exist");

           
            // salary issues
            Employee worker = data.FindWorker(con.EmployeeID);
            Specialization sp = FindSpecialization(int.Parse(worker.SpecialityID));
            if (con.GrossSalary > sp.MaxSalary)
                throw new Exception("Gross salary is bigger than the max salary that defined in his specialization");
            if (con.GrossSalary < sp.MinSalary)
                throw new Exception("Gross salary is lower than the min salary that defined in his specialization");

            //company issues
            Employer boss = data.FindBoss(con.EmployerID);
            if (CompanyOldEnough(boss))
                throw new Exception("Employer has to be functional for at least a year");

            // interview issue
            if (!con.IsInterviewed)
                throw new Exception("Employer has to be interviewed");

            // timing issues
            if (con.EndDate < DateTime.Now)
                throw new Exception("Contract is already expired");

        }
        /// <summary>
        /// A check for  human-resource-terms, logical & typo errors
        /// /// </summary>
        /// <param name="emp">Employee</param>
        private void ExceptionEmployee(Employee emp)
        {
            for (int i = 0; i < emp.FirstName.Count(); i++)
                if (char.IsDigit(emp.FirstName[i]))
                    throw new Exception("I don't know a guy with digits in his name.");
            for (int i = 0; i < emp.LastName.Count(); i++)
                if (char.IsDigit(emp.LastName[i]))
                    throw new Exception("I don't know a guy with digits in his name.");

            if (!WorkerOldEnough(emp))
                throw new Exception("Employee is too young");

            if (!IsBankExist(emp.BankAccount.branch.bank.BankID))
                throw new Exception("Employee bank account doesn't exist");

            if (int.Parse(emp.BankAccount.AccountNumber) < 0)
                throw new Exception("Employee account number can't be negative");

            if (int.Parse(emp.PhoneNumber) < 0)
                throw new Exception("Employee phone number can't be negative");

        }
        /// <summary>
        /// A check for  human-resource-terms, logical & typo errors
        /// </summary>
        /// <param name="emp">Employer</param>
        private void ExceptionEmployer(Employer emp)
        {
            if (emp.EstablishmentDate > DateTime.Now)
                throw new Exception("Establishment date cannot be in the future");

            if (emp.IsCompany && emp.CompanyName == null)
                throw new Exception("Compeny must have a name");

            for (int i = 0; emp.FirstName != null && i < emp.FirstName.Count(); i++)
                if (char.IsDigit(emp.FirstName[i]))
                    throw new Exception("I don't know a guy with digits in his name.");
            for (int i = 0; emp.LastName != null && i < emp.LastName.Count(); i++)
                if (char.IsDigit(emp.LastName[i]))
                    throw new Exception("I don't know a guy with digits in his name.");
            for (int i = 0; emp.CompanyName != null && i < emp.CompanyName.Count(); i++)
                if (char.IsDigit(emp.CompanyName[i]))
                    throw new Exception("I don't know a guy with digits in his name.");

        }
        /// <summary>
        /// A check for  human-resource-terms, logical & typo errors
        /// </summary>
        /// <param name="sp">Specialization</param>
        private void ExceptionSpecialization(Specialization sp)
        {
            if (
                string.IsNullOrEmpty(sp.SpecializationName) == true &&
                sp.MaxSalary == 0 &&
                sp.MinSalary == 0
                )
                throw new Exception("All fields must have a vaild value");

            for (int i = 0; i < sp.SpecializationName.Count(); i++)
                if (char.IsDigit(sp.SpecializationName[i]))
                    throw new Exception("I don't know a guy with digits in his name.");
            if (string.IsNullOrEmpty(sp.SpecializationName))
                throw new Exception("Everyone should have a name.");
            if (sp.MinSalary > sp.MaxSalary)
                throw new Exception("Minimum salary is higher than the maximum one.");
            if (sp.MinSalary < 0)
                throw new Exception("Salary can't be negative.");
            if (sp.MaxSalary == 0)
                throw new Exception("Nobody works for free.");
        }

        private void ExceptionSpecializationUpdate(Specialization sp)
        {
            Specialization old = new Specialization();
            try
            {
                old = (Specialization)FindSpecialization(sp.SpecializationID).Clone();
            }
            catch (Exception)
            {
                throw new Exception("Task failed successfully, you didn't choose an existing specialization");
            }

            if (
                sp.SpecializationName == old.SpecializationName &&
                sp.MaxSalary == old.MaxSalary &&
                sp.MinSalary == old.MinSalary &&
                sp.Field == old.Field
                )
                throw new Exception("Task failed successfully, you didn't change anything");

        }
        private void ExceptionEmployeeUpdate(Employee emp)
        {
            Employee old = new Employee();
            try
            {
                old = (Employee)FindWorker(emp.ID).Clone();
            }
            catch (Exception)
            {
                throw new Exception("Task failed successfully, you didn't choose an existing employee");
            }

            if (
                old.ID == emp.ID &&
                old.FirstName == emp.FirstName &&
                old.LastName == emp.LastName &&
                old.BirthDate == emp.BirthDate &&
                old.PhoneNumber == emp.PhoneNumber &&
                old.Address == emp.Address &&
                old.region == emp.region &&
                old.Degree == emp.Degree &&
                old.Veteran == emp.Veteran &&
                old.BankAccount.ToString() == emp.BankAccount.ToString() &&
                old.SpecialityID == emp.SpecialityID &&
                old.DealsNum == emp.DealsNum
                )
                throw new Exception("Task failed successfully, you didn't change anything");
        }


        #endregion
        #region Getting functions
        public List<Contract> GetAllContracts(Predicate<Contract> match)
        {
            return GetContractList().FindAll(match);
        }
        //      public List<Bank> GetBankList()
        //{
        //      return data.GetBankList();
        //   }
        public List<Branch> GetBranchList()
        {
            return data.GetBranchList();
        }
        public List<Contract> GetContractList()
        {
            return data.GetContractList();
        }
        public List<Employee> GetEmployeeList()
        {
            return data.GetEmployeeList();
        }
        public List<Employer> GetEmployerList()
        {
            return data.GetEmployerList();
        }
        public int GetNumOfContracts(Predicate<Contract> match)
        {
            List<Contract> num = GetContractList().FindAll(match);
            return num.Count();
        }
        public List<Specialization> GetSpecializationList()
        {
            return data.GetSpecializationList();
        }
        #endregion
        #region Grouping functions
        #region Groups for contracts
        public IEnumerable<IGrouping<SpecializationField, Contract>> GroupContractBySpec(DateTime begin, DateTime end, bool order = false)
        {
            return from item in data.GetContractList()
                   let worker = data.FindWorker(item.EmployeeID)
                   let spec = data.FindSpecialization(int.Parse(worker.SpecialityID))
                   where item.StartDate >= begin && item.EndDate <= end
                   orderby order ? spec.Field : 0
                   group item by spec.Field;
        }
        public IEnumerable<IGrouping<SpecializationField, Contract>> GroupContractBySpec(bool order = false)
        {
            return GroupContractBySpec(DateTime.MinValue, DateTime.MaxValue, order);
        }

        public IEnumerable<IGrouping<District?, Contract>> GroupContractByDistrict(DateTime begin, DateTime end, bool order = false)
        {
            return from item in data.GetContractList()
                   let worker = data.FindWorker(item.EmployeeID)
                   where item.StartDate >= begin && item.EndDate <= end
                   orderby order ? worker.region : 0
                   group item by worker.region;
        }
        public IEnumerable<IGrouping<District?, Contract>> GroupContractByDistrict(bool order = false)
        {
            return GroupContractByDistrict(DateTime.MinValue, DateTime.MaxValue, order);
        }

        public IEnumerable<IGrouping<int, double>> GroupContractByShares(DateTime begin, DateTime end, bool order = false)
        {
            var temp = from item in data.GetContractList()
                       let worker = data.FindWorker(item.EmployeeID)
                       //where int.Parse(item.EmployerID) == bossID
                       where item.StartDate >= begin && item.EndDate <= end
                       orderby order ? item.StartDate.Year : 0
                       group (item.GrossSalary - item.NetSalary) * item.WorkingHours by item.StartDate.Year;

            return from item in temp
                   group Math.Round(item.Sum(), 2) by item.Key;

        }
        public IEnumerable<IGrouping<int, double>> GroupContractByShares(bool order = false)
        {
            return GroupContractByShares(DateTime.MinValue, DateTime.MaxValue, order);
        }
        #endregion

        #region Groups for employees
        public IEnumerable<IGrouping<District?, Employee>> GroupEmployeeByDistrict(bool order = false)
        {
            return from item in data.GetEmployeeList()
                   orderby order ? item.region : 0
                   group item by item.region;
        }
        public IEnumerable<IGrouping<string, Employee>> GroupEmployeeByBank(bool order = false)
        {
            return from item in data.GetEmployeeList()
                   let names = item.BankAccount.branch.bank.BankName
                   orderby order ? string.CompareOrdinal(names, item.BankAccount.branch.bank.BankName) : 0
                   group item by item.BankAccount.branch.bank.BankName;
        }
        public IEnumerable<IGrouping<enumDegree?, Employee>> GroupEmployeeByDegree(bool order = false)
        {
            return from item in data.GetEmployeeList()
                   orderby order ? item.Degree : 0
                   group item by item.Degree;
        }
        public IEnumerable<IGrouping<int, Employee>> GroupEmployeeByBirthYear(bool order = false)
        {
            return from item in data.GetEmployeeList()
                   orderby order ? item.BirthDate.Year : 0
                   group item by item.BirthDate.Year;
        }
        #endregion

        #region Groups for employers
        public IEnumerable<IGrouping<SpecializationField?, Employer>> GroupEmployerByField(bool order = false)
        {
            return from item in data.GetEmployerList()
                   orderby order ? item.Field : 0
                   group item by item.Field;
        }
        public IEnumerable<IGrouping<int, Employer>> GroupEmployerByEstablishmentYear(bool order = false)
        {
            return from item in data.GetEmployerList()
                   orderby order ? item.EstablishmentDate.Year : 0
                   group item by item.EstablishmentDate.Year;
        }
        public IEnumerable<IGrouping<string, Employer>> GroupEmployerByBusinessType(bool order = false)
        {
            return from item in data.GetEmployerList()
                   orderby order ? item.IsCompany.ToString()[0] : 0
                   group item by ((item.IsCompany) ? "Enterprise" : "Private business");
        }
        #endregion

        #region Groups for specialization
        public IEnumerable<IGrouping<SpecializationField, Specialization>> GroupSpecializationByField(bool order = false)
        {
            return from item in data.GetSpecializationList()
                   orderby order ? item.Field : 0
                   group item by item.Field;
        }
        public IEnumerable<IGrouping<double, Specialization>> GroupSpecializationByMinSalary(bool order = false)
        {
            return from item in data.GetSpecializationList()
                   orderby order ? item.MinSalary : 0
                   group item by Math.Round(item.MinSalary, 2);
        }
        public IEnumerable<IGrouping<double, Specialization>> GroupSpecializationByMaxSalary(bool order = false)
        {
            return from item in data.GetSpecializationList()
                   orderby order ? item.MaxSalary : 0
                   group item by Math.Round(item.MaxSalary, 2);
        }
        #endregion

        public List<Bank> GetBankList()
        {
            return data.GetBankList();
        }

        public IEnumerable<IGrouping<string, IGrouping<string, Branch>>> GetAllBranchesByBankAndCity()
        {
            return
                from branch in data.GetBranchList().Distinct()
                group branch by branch.bank.BankName into g
                from item in (from branch2 in g
                              group branch2 by branch2.BranchCity)
                group item by g.Key;

        }
        #endregion
        #region BOOLEAN functions
        /// <summary>
        /// gets an bank id and indicate if the bank exist in the system by boolean
        /// </summary>
        /// <param name="bankID">Bank ID</param>
        /// <returns>if bank exist in system or not</returns>
        private bool IsBankExist(string bankID)
        {
            return data.IsBankExist(bankID);
        }
        /// <summary>
        /// gets an employee id and indicate if the employee exist in the system by boolean
        /// </summary>
        /// <param name="workerID">Employee ID</param>
        /// <returns>if employee exist in system or not</returns>
        private bool WorkerExists(string workerID)
        {
            try
            {
                data.FindWorker(workerID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// gets an employer id and indicate if the employer exist in the system by boolean
        /// </summary>
        /// <param name="bossID">Employer ID</param>
        /// <returns>if employer exist in system or not</returns>
        private bool BossExists(string bossID)
        {
            try
            {
                data.FindBoss(bossID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Checks if company function enough years by the terms
        /// </summary>
        /// <param name="company">Conpany - employer</param>
        /// <returns>True or False</returns>
        public bool CompanyOldEnough(Employer company)
        {
            return (DateTime.Now - company.EstablishmentDate < new TimeSpan(365, 0, 0, 0));
        }
        public bool InTheSystem(string worker, string boss)
        {
            return (WorkerExists(worker) && BossExists(boss));
        }
        public bool WorkerOldEnough(Employee emp)
        {
            return (DateTime.Now - emp.BirthDate >= new TimeSpan(365 * 18, 0, 0, 0, 0));
        }
        #endregion
        #region Removing functions
        public void RemoveContract(int id)
        {
            Contract con = data.FindContract(id);
            if (con.IsSigned)
                throw new Exception("The contract is signed and so cannot be removed");
            Employee worker = data.FindWorker(con.EmployeeID);
            Employer boss = data.FindBoss(con.EmployerID);
            data.RemoveContract(id);
            worker.DealsNum--;
            boss.ContractsNum--;

        }
        public void RemoveEmployee(string id)
        {
            foreach (var item in GetAllContracts(con => con.EmployeeID == id))
                RemoveContract(item.ContractID);
            data.RemoveEmployee(id);

        }
        public void RemoveEmployer(string id)
        {
            foreach (var item in GetAllContracts(con => con.EmployerID == id))
                RemoveContract(item.ContractID);
            data.RemoveEmployer(id);
        }
        public void RemoveSpecialization(int id)
        {
            RemoveAllContract(con => data.FindWorker(con.EmployeeID).SpecialityID == id.ToString());
            RemoveAllEmployee(emp => int.Parse(emp.SpecialityID) == id);
            data.RemoveSpecialization(id);
        }
        private int RemoveAllEmployee(Predicate<Employee> match)
        {
            return data.RemoveAllEmployee(match);
        }
        private int RemoveAllContract(Predicate<Contract> match)
        {
            return data.RemoveAllContract(match);
        }
        #endregion
        #region Updating functions
        public void UpdateContract(Contract UpdatedCon)
        {
            ExceptionContract(UpdatedCon);
            UpdatedCon.NetSalary = CalcWorkerNetSalary(UpdatedCon);
            data.UpdateContract(UpdatedCon);
        }
        public void UpdateEmployee(Employee UpdatedEmp)
        {
            ExceptionEmployee(UpdatedEmp);
            ExceptionEmployeeUpdate(UpdatedEmp);
            data.UpdateEmployee(UpdatedEmp);
        }
        public void UpdateEmployer(Employer UpdatedEmp)
        {
            ExceptionEmployer(UpdatedEmp);
            data.UpdateEmployer(UpdatedEmp);
        }
        public void UpdateSpecialization(Specialization UpdatedSp)
        {
            ExceptionSpecialization(UpdatedSp);
            ExceptionSpecializationUpdate(UpdatedSp);
            data.UpdateSpecialization(UpdatedSp);
        }
        #endregion
    }
}
