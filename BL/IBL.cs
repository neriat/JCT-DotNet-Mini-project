using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    public interface IBL : Idal
    {
        bool WorkerOldEnough(Employee emp);
        bool InTheSystem(string worker, string boss);
        bool CompanyOldEnough(Employer company);
        double CalcWorkerNetSalary(string workerID, string bossID, Contract contract);
        List<Contract> GetAllContracts(Predicate<Contract> match);
        int GetNumOfContracts(Predicate<Contract> match);


    }
}
