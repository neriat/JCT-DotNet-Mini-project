using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    public interface IBL
    {
        #region Find functions
        Specialization FindSpecialization(int SpecializationID);
        Contract FindContract(int ContractID);
        Employee FindWorker(string workerID);
        Employer FindBoss(string bossID);
        #endregion
        #region Adding functions
        void AddContract(Contract con);
        void AddEmployee(Employee emp);
        void AddEmployer(Employer emp);
        void AddSpecialization(Specialization sp);
        #endregion
        #region Calculation functions
        double CalcWorkerNetSalary(Contract contract);
        #endregion
        #region Getting functions
        List<Contract> GetAllContracts(Predicate<Contract> match);
        List<Bank> GetBankList();
        List<Branch> GetBranchList();
        List<Contract> GetContractList();
        List<Employee> GetEmployeeList();
        List<Employer> GetEmployerList();
        int GetNumOfContracts(Predicate<Contract> match);
        List<Specialization> GetSpecializationList();
        #endregion
        #region Grouping functions

        #region Groups for contracts
        IEnumerable<IGrouping<SpecializationField, Contract>> GroupContractBySpec(DateTime begin, DateTime end, bool order = false);
        IEnumerable<IGrouping<SpecializationField, Contract>> GroupContractBySpec(bool order = false);
        IEnumerable<IGrouping<District?, Contract>> GroupContractByDistrict(DateTime begin, DateTime end, bool order = false);
        IEnumerable<IGrouping<District?, Contract>> GroupContractByDistrict(bool order = false);
        IEnumerable<IGrouping<int, double>> GroupContractByShares(DateTime begin, DateTime end, bool order = false);
        IEnumerable<IGrouping<int, double>> GroupContractByShares(bool order = false);
        #endregion

        #region Groups for employees
        IEnumerable<IGrouping<District?, Employee>> GroupEmployeeByDistrict(bool order = false);
        IEnumerable<IGrouping<string, Employee>> GroupEmployeeByBank(bool order = false);
        IEnumerable<IGrouping<enumDegree?, Employee>> GroupEmployeeByDegree(bool order = false);
        IEnumerable<IGrouping<int, Employee>> GroupEmployeeByBirthYear(bool order = false);
        #endregion;

        #region Groups for employers
        IEnumerable<IGrouping<SpecializationField?, Employer>> GroupEmployerByField(bool order = false);
        IEnumerable<IGrouping<int, Employer>> GroupEmployerByEstablishmentYear(bool order = false);
        IEnumerable<IGrouping<string, Employer>> GroupEmployerByBusinessType(bool order = false);
        #endregion

        #region Groups for specialization
        IEnumerable<IGrouping<SpecializationField, Specialization>> GroupSpecializationByField(bool order = false);
        IEnumerable<IGrouping<double, Specialization>> GroupSpecializationByMinSalary(bool order = false);
        IEnumerable<IGrouping<double, Specialization>> GroupSpecializationByMaxSalary(bool order = false);
        #endregion

        IEnumerable<IGrouping<string, IGrouping<string, Branch>>> GetAllBranchesByBankAndCity();
        #endregion
        #region BOOLEAN functions
        bool CompanyOldEnough(Employer company);
        bool InTheSystem(string worker, string boss);
        bool WorkerOldEnough(Employee emp);
        #endregion
        #region Removing functions
        void RemoveContract(int id);
        void RemoveEmployee(string id);
        void RemoveEmployer(string id);
        void RemoveSpecialization(int id);
        #endregion
        #region Updating functions
        void UpdateContract(Contract UpdatedCon);
        void UpdateEmployee(Employee UpdatedEmp);
        void UpdateEmployer(Employer UpdatedEmp);
        void UpdateSpecialization(Specialization UpdatedSp);
        #endregion

    }
}
