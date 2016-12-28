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
        void AddSpecialization(Specialization sp);
        void RemoveSpecialization(int id);
        void UpdateSpecialization(Specialization UpdatedSp);


        void AddEmployee(Employee emp);
        void RemoveEmployee(string id);
        void UpdateEmployee(Employee UpdatedEmp);

        void AddEmployer(Employer emp);
        void RemoveEmployer(string id);
        void UpdateEmployer(Employer UpdatedEmp);

        void AddContract(Contract con);
        void RemoveContract(int id);
        void UpdateContract(Contract UpdatedCon);

        List<Specialization> GetSpecializationList();
        List<Employee> GetEmployeeList();
        List<Employer> GetEmployerList();
        List<Contract> GetContractList();
        List<Bank> GetBankList();





    }
}
