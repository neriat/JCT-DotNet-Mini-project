using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface Idal
    {
        #region Find functions
        /// <summary>
        /// Find employee by its id
        /// </summary>
        /// <param name="workerID">ID</param>
        /// <returns>type of employee that id belongs to</returns>
        Employee FindWorker(string workerID);
        /// <summary>
        /// Find specialization by its id
        /// </summary>
        /// <param name="specializationID"></param>
        /// <returns>type of specialization that id belongs to</returns>
        Specialization FindSpecialization(int specializationID);
        /// <summary>
        /// Find contract by its id
        /// </summary>
        /// <param name="conID"></param>
        /// <returns>type of contract that id belongs to</returns>
        Contract FindContract(int conID);
        /// <summary>
        /// /// Find employer by its id        /// </summary>
        /// <param name="bossID"></param>
        /// <returns>type of employer that id belongs to</returns>
        Employer FindBoss(string bossID);
        #endregion
        #region Bool functions
        bool IsBankExist(string bankID);
        #endregion
        #region Add functions
        void AddContract(Contract con);
        void AddEmployee(Employee emp);
        void AddEmployer(Employer emp);
        void AddSpecialization(Specialization sp);
        #endregion
        #region Get functions
        /// <summary>
        /// The function gets access to data structure and extract the banks
        /// </summary>
        /// <returns>banks binding in list</returns>
        List<Bank> GetBankList();
        /// <summary>
        /// The function gets access to data structure and extract the contracts
        /// </summary>
        /// <returns>contracts binding in list</returns>
        List<Contract> GetContractList();
        /// <summary>
        /// The function gets access to data structure and extract the employees
        /// </summary>
        /// <returns>employees binding in list</returns>
        List<Employee> GetEmployeeList();
        /// <summary>
        /// The function gets access to data structure and extract the employers
        /// </summary>
        /// <returns>employer binding in list</returns>
        List<Employer> GetEmployerList();
        /// <summary>
        /// The function gets access to data structure and extract the specializations
        /// </summary>
        /// <returns>specializations binding in list</returns>
        List<Specialization> GetSpecializationList();
        /// <summary>
        /// The function gets access to data structure and extract the branches
        /// </summary>
        /// <returns>branches binding in list</returns>
        List<Branch> GetBranchList();
        #endregion
        #region Remove functions
        void RemoveContract(int id);
        void RemoveEmployee(string id);
        void RemoveEmployer(string id);
        void RemoveSpecialization(int id);
        #endregion
        #region Update functions        
        void UpdateContract(Contract UpdatedCon);
        void UpdateEmployee(Employee UpdatedEmp);
        void UpdateEmployer(Employer UpdatedEmp);
        void UpdateSpecialization(Specialization UpdatedSp);
        int RemoveAllEmployee(Predicate<Employee> match);
        int RemoveAllContract(Predicate<Contract> match);
        #endregion
    }
}
