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
        DAL.Dal_imp data = new Dal_imp();
        #region Finding functions
        /// <summary>
        /// gets an specialization id as int and return its specialization type
        /// </summary>
        /// <param name="SpecializationID">Specialization ID</param>
        /// <returns> specialization type </returns>
        private Specialization FindSpecialization(int SpecializationID)
        {
            return data.FindSpecialization(SpecializationID);
        }
        private Contract FindContract(int ContractID)
        {
            return data.FindContract(ContractID);
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
            data.AddContract(con);

            Employee worker = data.FindWorker(con.EmployeeID);
            Employer boss = data.FindBoss(con.EmployerID);
            worker.DealsNum++;
            boss.ContractsNum++;
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
            //  (gross salary) - (boss fee based on number of employers under him)*(1.01 -if the worker isn't IDF vetern)/(num of deals worker did + 1) -(-10% -boss gain)
            //  boss fee: starts at 40. downby .7 for every signed contract. state at 4
            Employee worker = data.FindWorker(contract.EmployeeID);
            Employer boss = data.FindBoss(contract.EmployerID);

            double armyDiscount = 1.01; //default mode
            if (worker.Veteran) armyDiscount = 1;

            double BossFee = 40 - 0.7 * boss.ContractsNum;
            if (BossFee < 4) BossFee = 4; //lowest fee

            return (contract.GrossSalary - (BossFee * armyDiscount) / (worker.DealsNum + 1)) * 0.9; //using in 0.9 in grouping!!!

        }
        public double CalcEmployerGain(string bossID)
        {
            //NOT IMPLEMENTED YET
            var qr = from item in GetContractList()
                     where item.EmployerID == bossID
                     select item.NetSalary / 0.9;
            return qr.Sum();
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

            // bank issues
            Employee worker = data.FindWorker(con.EmployeeID);
            if (!IsBankExist(worker.BankAccount.branch.bank.BankID))
                throw new Exception("Employee bank account doesn't exist");

            // salary issues
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
            if (!WorkerOldEnough(emp))
                throw new Exception("Employee is too young");

            if (!IsBankExist(emp.BankAccount.branch.bank.BankID))
                throw new Exception("Employee bank account doesn't exist");
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
        }
        /// <summary>
        /// A check for  human-resource-terms, logical & typo errors
        /// </summary>
        /// <param name="sp">Specialization</param>
        private void ExceptionSpecialization(Specialization sp)
        {
            if (sp.MinSalary > sp.MaxSalary)
                throw new Exception("Minimum salary is higher than the maximum one");
        }
        #endregion
        #region Getting functions
        public List<Contract> GetAllContracts(Predicate<Contract> match)
        {
            return GetContractList().FindAll(match);
        }
        public List<Bank> GetBankList()
        {
            return data.GetBankList();
        }
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
        public IEnumerable<IGrouping<SpecializationField, Contract>> GroupContractBySpec(DateTime begin, DateTime end, bool order = false)
        {
            return from item in data.GetContractList()
                   let worker = data.FindWorker(item.EmployeeID)
                   let spec = data.FindSpecialization(int.Parse(worker.SpecialityID))
                   where item.StartDate >= begin && item.EndDate <= end
                   orderby order ? item.ContractID : 0
                   group item by spec.Field;
        }
        public IEnumerable<IGrouping<SpecializationField, Contract>> GroupContractBySpec(bool order = false)
        {
            return GroupContractBySpec(DateTime.MinValue, DateTime.MaxValue, order);
        }
        public IEnumerable<IGrouping<District, Contract>> GroupContractByDistrict(DateTime begin, DateTime end, bool order = false)
        {
            return from item in data.GetContractList()
                   let worker = data.FindWorker(item.EmployeeID)
                   where item.StartDate >= begin && item.EndDate <= end
                   orderby order ? item.ContractID : 0
                   group item by worker.region;
        }
        public IEnumerable<IGrouping<District, Contract>> GroupContractByDistrict(bool order = false)
        {
            return GroupContractByDistrict(DateTime.MinValue, DateTime.MaxValue, order);
        }
        public IEnumerable<IGrouping<int, double>> GroupGainByStartYear(int bossID, DateTime begin, DateTime end, bool order = false)
        {
            return from item in data.GetContractList()
                   let worker = data.FindWorker(item.EmployeeID)
                   where int.Parse(item.EmployerID) == bossID
                   where item.StartDate >= begin && item.EndDate <= end
                   orderby order ? item.StartDate.Year : 0
                   group item.NetSalary / 0.9 by item.StartDate.Year;
        }
        public IEnumerable<IGrouping<int, double>> GroupGainByStartYear(int bossID, bool order = false)
        {
            return GroupGainByStartYear(bossID, DateTime.MinValue, DateTime.MaxValue, order);
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
            data.RemoveSpecialization(id);
        }
        #endregion
        #region Updating functions
        public void UpdateContract(Contract UpdatedCon)
        {
            ExceptionContract(UpdatedCon);
            data.UpdateContract(UpdatedCon);
        }
        public void UpdateEmployee(Employee UpdatedEmp)
        {
            ExceptionEmployee(UpdatedEmp);
            data.UpdateEmployee(UpdatedEmp);
        }
        public void UpdateEmployer(Employer UpdatedEmp)
        {
            ExceptionEmployer(UpdatedEmp);
            data.UpdateEmployer(UpdatedEmp);
        }
        public void UpdateSpecialization(Specialization UpdatedSp)
        {
            data.UpdateSpecialization(UpdatedSp);
        }
        #endregion
    }
}
