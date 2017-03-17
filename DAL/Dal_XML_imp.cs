using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Net;

namespace DAL
{
    class Dal_XML_imp : Idal
    {
        #region XML
        private static int SpecializationIDNum
        {
            get
            {
                return GetSpecializationIDNum();
            }
            set
            {
                UpdateSpecializationIDNum(value);
            }

        }
        private static int ContractIDNum
        {
            get
            {
                return GetContractIDNum();
            }
            set
            {
                UpdateContractIDNum(value);
            }

        }
        private static int GetSpecializationIDNum()
        {
            const string filename = "XMLinitialization.xml";
            try
            {
                XElement Root = XElement.Load(filename);
                return Convert.ToInt32(Root.Element("SpecializationIDNum").Value);
            }
            catch (Exception)
            {
                return 9999999;
            }
        }
        private static int GetContractIDNum()
        {
            const string filename = "XMLinitialization.xml";
            try
            {
                XElement Root = XElement.Load(filename);
                return Convert.ToInt32(Root.Element("ContractIDNum").Value);
            }
            catch (Exception)
            {
                return 9999999;
            }
        }
        private static void UpdateSpecializationIDNum(int value = 0)
        {
            const string filename = "XMLinitialization.xml";
            try
            {
                new XElement
                    ("root",
                        new XElement("SpecializationIDNum", value),
                        new XElement("ContractIDNum", GetContractIDNum())
                    ).Save(filename);
            }
            catch (Exception)
            {
                throw new Exception("Cannot reach initialization file");
            }
        }
        private static void UpdateContractIDNum(int value = 0)
        {
            const string filename = "XMLinitialization.xml";
            try
            {
                new XElement
                    ("root",
                        new XElement("SpecializationIDNum", GetSpecializationIDNum()),
                        new XElement("ContractIDNum", value)
                    ).Save(filename);
            }
            catch (Exception)
            {
                throw new Exception("Cannot reach initialization file");
            }

        }


        private static bool isDownloaded = false;
        private void WriteToFile<T>(List<T> data, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            try
            {
                File.Delete(filename + ".xml");
                StreamWriter writer = File.CreateText(filename + ".xml");
                serializer.Serialize(writer, data);
                writer.Close();
            }
            catch (Exception)
            {
                throw new Exception("Cannot read file");
            }
        }
        private List<T> ReadFromFile<T>(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            if (!File.Exists(filename + ".xml"))
                WriteToFile<T>(new List<T>(), filename);
            using (StreamReader reader = new StreamReader(filename + ".xml"))
            {
                return (List<T>)serializer.Deserialize(reader);
            }
        }

        private void WriteToFile_LINQ(List<Specialization> data, string filename)
        {
            XElement SpecializationRoot = new XElement(
                "Specializations",
                 from sp in data
                 select new XElement("Specialization",
                            new XElement("SpecializationID", sp.SpecializationID),
                            new XElement("Field", sp.Field),
                            new XElement("SpecializationName", sp.SpecializationName),
                            new XElement("MinSalary", sp.MinSalary),
                            new XElement("MaxSalary", sp.MaxSalary)
                            )
                );
            SpecializationRoot.Save(filename + ".xml");
        }
        private List<Specialization> ReadFromFile_LINQ(string filename)
        {
            try
            {
                if (!File.Exists(filename + ".xml"))
                    WriteToFile<Specialization>(new List<Specialization>(), filename);
                XElement spRoot = XElement.Load(filename + ".xml");
                return
                    (
                    from sp in spRoot.Elements()
                    select new Specialization()
                    {
                        SpecializationID = Convert.ToInt32(sp.Element("SpecializationID").Value),
                        Field = (SpecializationField)Enum.Parse(typeof(SpecializationField), sp.Element("Field").Value, true),
                        SpecializationName = sp.Element("SpecializationName").Value,
                        MinSalary = Convert.ToDouble(sp.Element("MinSalary").Value),
                        MaxSalary = Convert.ToDouble(sp.Element("MaxSalary").Value)
                    }).ToList();
            }
            catch (Exception)
            {
                throw new Exception("Couldn't read from file");
            }
        }
        private void AddToFile(Specialization sp, string filename)
        {
            try
            {
                XElement spRoot = XElement.Load(filename + ".xml");
                XElement ID = new XElement("SpecializationID", sp.SpecializationID);
                XElement Field = new XElement("Field", sp.Field);
                XElement SpecializationName = new XElement("SpecializationName", sp.SpecializationName);
                XElement MinSalary = new XElement("MinSalary", sp.MinSalary);
                XElement MaxSalary = new XElement("MaxSalary", sp.MaxSalary);
                spRoot.Add(new XElement("Specialization", ID, Field, SpecializationName, MinSalary, MaxSalary));
                spRoot.Save(filename + ".xml");
            }
            catch
            {
                throw new Exception("Cannot add to file");
            }
        }
        private void RemoveFromFile(int id, string filename)
        {
            XElement sp;
            try
            {
                XElement spRoot = XElement.Load(filename + ".xml");

                sp = (from item in spRoot.Elements()
                      where Convert.ToInt32(item.Element("SpecializationID").Value) == id
                      select item).FirstOrDefault();
                sp.Remove();
                spRoot.Save(filename + ".xml");
                // TODO
            }
            catch
            {
                throw new Exception("Cannot open file - REMOVE");
            }
        }
        private void UpdateToFile(Specialization sp, string filename)
        {

            RemoveFromFile(sp.SpecializationID, filename);
            AddToFile(sp, filename);
        }
        private void DownloadBank()
        {
            const string url = @"http://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/snifim_he.xml";
            const string url_Backup = @"https://dl.dropboxusercontent.com/s/o64cejeti5joykt/snifim_he.xml";
            const string filepath = @"XMLbank.xml";
            var wc = new WebClient();
            try
            {
                wc.DownloadFile(url, filepath);
                isDownloaded = true;
            }
            catch (Exception)
            {
                wc.DownloadFile(url_Backup, filepath);
            }
            finally
            {
                wc.Dispose();
            }
        }
        #endregion

        #region Get functions
        /// <summary>
        /// The function gets access to XML data file and extract the banks
        /// </summary>
        /// <returns>banks binding in list</returns>
        public List<Bank> GetBankList()
        {
            return (from item in GetAllBranch()
                    select item.bank).Distinct().ToList();
            #region old
            //List<Bank> lst = new List<Bank>();

            ////First
            //Bank a = new Bank();
            //a.BankID = "11";
            //a.BankName = "Leumi";
            //lst.Add(a);

            ////Second
            //a = new Bank();
            //a.BankID = "14";
            //a.BankName = "Otzar Hakhayal";
            //lst.Add(a);

            ////Third
            //a = new Bank();
            //a.BankID = "7";
            //a.BankName = "Yahav";
            //lst.Add(a);

            ////4
            //a = new Bank();
            //a.BankID = "4";
            //a.BankName = "Hapoalim";
            //lst.Add(a);

            ////5
            //a = new Bank();
            //a.BankID = "2";
            //a.BankName = "Mizrahi";
            //lst.Add(a);

            //return lst;
            #endregion
        }

        private List<Branch> GetAllBranch()
        {
            
            if (isDownloaded == false) DownloadBank();
            XElement banks = XElement.Load("XMLbank.xml");
            List<Branch> temp = new List<Branch>();
            foreach (var item in banks.Elements())
            {
                temp.Add(new Branch()
                {
                    bank = new Bank()
                    {
                        BankID = item.Element("Bank_Code").Value,
                        BankName = item.Element("Bank_Name").Value.Replace('\n', ' ').Trim()
                    },
                    BranchID = item.Element("Branch_Code").Value,
                    BranchAddress = item.Element("Branch_Address").Value,
                    BranchCity = item.Element("City").Value.Replace('\n', ' ').Trim(),
                });
            }
            return temp;
            #region old
            //List<Bank> lst = new List<Bank>();

            ////First
            //Bank a = new Bank();
            //a.BankID = "11";
            //a.BankName = "Leumi";
            //lst.Add(a);

            ////Second
            //a = new Bank();
            //a.BankID = "14";
            //a.BankName = "Otzar Hakhayal";
            //lst.Add(a);

            ////Third
            //a = new Bank();
            //a.BankID = "7";
            //a.BankName = "Yahav";
            //lst.Add(a);

            ////4
            //a = new Bank();
            //a.BankID = "4";
            //a.BankName = "Hapoalim";
            //lst.Add(a);

            ////5
            //a = new Bank();
            //a.BankID = "2";
            //a.BankName = "Mizrahi";
            //lst.Add(a);

            //return lst;
            #endregion
        }
        /// <summary>
        /// The function gets access to XML data file and extract the contracts
        /// </summary>
        /// <returns>contracts binding in list</returns>
        public List<Contract> GetContractList()
        {
            return ReadFromFile<Contract>("XMLcontract").ToList();
        }
        /// <summary>
        /// The function gets access to XML data file and extract the employees
        /// </summary>
        /// <returns>employees binding in list</returns>
        public List<Employee> GetEmployeeList()
        {
            return ReadFromFile<Employee>("XMLemployee").ToList();
        }
        /// <summary>
        /// The function gets access to XML data file and extract the employers
        /// </summary>
        /// <returns>employer binding in list</returns>
        public List<Employer> GetEmployerList()
        {
            return ReadFromFile<Employer>("XMLemployer").ToList();

        }
        /// <summary>
        /// The function gets access to XML data file and extract the specializations
        /// </summary>
        /// <returns>specializations binding in list</returns>
        public List<Specialization> GetSpecializationList()
        {
            return ReadFromFile_LINQ("XMLspecialization").ToList();
        }
        /// <summary>
        /// The function gets access to XML data file and extract the branches
        /// </summary>
        /// <returns>branches binding in list</returns>
        public List<Branch> GetBranchList()
        {
            return GetAllBranch();
        }
        #endregion
        #region Add functions
        /// <summary>
        /// Add a contract to data structure that implement via list
        /// </summary>
        /// <param name="con">contract type</param>
        public void AddContract(Contract con)
        {
            List<Contract> ContractList = GetContractList();
            if (con.ContractID == 0)
                do
                {
                    con.ContractID = ++ContractIDNum;
                }
                while (ContractList.Find(c => c.ContractID == con.ContractID) != null);
            else
            {
                var a = ContractList.Find(c => c.ContractID == con.ContractID);
                if (a != null)
                    throw new Exception("Contract ID is already in the system");
            }

            ContractList.Add(con);
            WriteToFile<Contract>(ContractList, "XMLcontract");


        }
        /// <summary>
        /// Add a emploee to data structure that implement via list
        /// </summary>
        /// <param name="emp">employee type</param>
        public void AddEmployee(Employee emp)
        {
            var EmployeeList = GetEmployeeList();
            var a = EmployeeList.Find(e => e.ID == emp.ID);
            if (a != null)
                throw new Exception("Employee ID is already in the system");
            EmployeeList.Add(emp);
            WriteToFile<Employee>(EmployeeList, "XMLemployee");
        }
        /// <summary>
        /// Add a employer to data structure that implement via list
        /// </summary>
        /// <param name="emp">employer type</param>
        public void AddEmployer(Employer emp)
        {
            var EmployerList = GetEmployerList();
            var a = EmployerList.Find(e => e.ID == emp.ID);
            if (a != null)
                throw new Exception("Employer ID is already in the system");
            EmployerList.Add(emp);
            WriteToFile<Employer>(EmployerList, "XMLemployer");
        }
        /// <summary>
        /// Add a specialization to data structure that implement via list
        /// </summary>
        /// <param name="sp">specialization type</param>
        public void AddSpecialization(Specialization sp)
        {
            var SpecializationList = GetSpecializationList();
            if (sp.SpecializationID == 0)
                do
                {
                    sp.SpecializationID = ++SpecializationIDNum;
                }
                while (SpecializationList.Find(s => s.SpecializationID == sp.SpecializationID) != null);
            else
            {
                var a = SpecializationList.Find(s => s.SpecializationID == sp.SpecializationID);
                if (a != null)
                    throw new Exception("Specialization ID is already in the system");
            }
            SpecializationList.Add(sp);
            WriteToFile_LINQ(SpecializationList, "XMLspecialization");
        }
        #endregion
        #region Update functions
        /// <summary>
        /// The function gets access to data structure and update the item that has the same id
        /// </summary>
        /// <param name="UpdatedCon">Updated contract</param>
        public void UpdateContract(Contract UpdatedCon)
        {
            List<Contract> ContractList = GetContractList();
            int index = ContractList.FindIndex(con => con.ContractID == UpdatedCon.ContractID);
            if (index == -1)
                throw new Exception("No such contract was found to update");
            ContractList[index] = UpdatedCon;
            WriteToFile<Contract>(ContractList, "XMLcontract");
        }
        /// <summary>
        /// The function gets access to data structure and update the item that has the same id
        /// </summary>
        /// <param name="UpdatedEmp">Updated employee</param>
        public void UpdateEmployee(Employee UpdatedEmp)
        {
            var EmployeeList = GetEmployeeList();
            int index = EmployeeList.FindIndex(emp => emp.ID == UpdatedEmp.ID);
            if (index == -1)
                throw new Exception("No such employee was found to update");
            EmployeeList[index] = UpdatedEmp;
            WriteToFile<Employee>(EmployeeList, "XMLemployee");
        }
        /// <summary>
        /// The function gets access to data structure and update the item that has the same id
        /// </summary>
        /// <param name="UpdatedEmp">Updated employer</param>
        public void UpdateEmployer(Employer UpdatedEmp)
        {
            var EmployerList = GetEmployerList();
            int index = EmployerList.FindIndex(emp => emp.ID == UpdatedEmp.ID);
            if (index == -1)
                throw new Exception("No such employer was found to update");
            EmployerList[index] = UpdatedEmp;
            WriteToFile<Employer>(EmployerList, "XMLemployer");
        }
        /// <summary>
        /// The function gets access to data structure and update the item that has the same id
        /// </summary>
        /// <param name="UpdatedSp">Updated specialization</param>
        public void UpdateSpecialization(Specialization UpdatedSp)
        {
            XElement sp;
            try
            {
                XElement spRoot = XElement.Load("XMLspecialization.xml");
                sp = (from item in spRoot.Elements()
                      where int.Parse(item.Element("SpecializationID").Value) == UpdatedSp.SpecializationID
                      select item).FirstOrDefault();
            }
            catch
            {
                sp = null;
                throw new Exception("Cannot open file");
            }
            if (sp == null)
                throw new Exception("No such specialization was found to update");
            else UpdateToFile(UpdatedSp, "XMLspecialization");
        }
        #endregion
        #region Remove functions
        /// <summary>
        /// The function gets access to data structure and remove the item that id belongs to
        /// </summary>
        /// <param name="id">id of the type we remove</param>
        public void RemoveContract(int id)
        {
            List<Contract> ContractList = GetContractList();
            var x = ContractList.RemoveAll(con => con.ContractID == id);
            if (x == 0)
                throw new Exception("No such contract was found");
            WriteToFile<Contract>(ContractList, "XMLcontract");
        }
        /// <summary>
        /// The function gets access to data structure and remove the item that id belongs to
        /// </summary>
        /// <param name="id">id of the type we remove</param>
        public void RemoveEmployee(string id)
        {
            var EmployeeList = GetEmployeeList();
            var x = EmployeeList.RemoveAll(emp => emp.ID == id);
            if (x == 0)
                throw new Exception("No such employee was found");
            WriteToFile<Employee>(EmployeeList, "XMLemployee");
        }
        /// <summary>
        /// The function gets access to data structure and remove the item that id belongs to
        /// </summary>
        /// <param name="id">id of the type we remove</param>
        public void RemoveEmployer(string id)
        {
            var EmployerList = GetEmployerList();
            var x = EmployerList.RemoveAll(emp => emp.ID == id);
            if (x == 0)
                throw new Exception("No such employer was found");
            WriteToFile<Employer>(EmployerList, "XMLemployer");
        }
        /// <summary>
        /// The function gets access to data structure and remove the item that id belongs to
        /// </summary>
        /// <param name="id">id of the type we remove</param>
        public void RemoveSpecialization(int id)
        {
            var SpecializationList = GetSpecializationList();
            var x = SpecializationList.RemoveAll(sp => sp.SpecializationID == id);
            if (x == 0)
                throw new Exception("No such specialization was found");
            WriteToFile_LINQ(SpecializationList, "XMLspecialization");
        }

        public int RemoveAllEmployee(Predicate<Employee> match)
        {
            var EmployeeList = GetEmployeeList();
            var howmany = EmployeeList.RemoveAll(match);
            WriteToFile<Employee>(EmployeeList, "XMLemployee");
            return howmany;
        }
        public int RemoveAllContract(Predicate<Contract> match)
        {
            var ContractList = GetContractList();
            var howmany = ContractList.RemoveAll(match);
            WriteToFile<Contract>(ContractList, "XMLcontract");
            return howmany;
        }
        #endregion
        #region Find functions
        /// <summary>
        /// Find employee by its id
        /// </summary>
        /// <param name="workerID">ID</param>
        /// <returns>type of employee that id belongs to</returns>
        public Employee FindWorker(string workerID)
        {
            var a = GetEmployeeList().Find(emp => emp.ID == workerID);
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
            var a = GetContractList().Find(con => con.ContractID == conID);
            if (a == null) throw new Exception("Contract doesn't exist");
            return a;
        }
        /// <summary>
        /// /// Find employer by its id        /// </summary>
        /// <param name="bossID"></param>
        /// <returns>type of employer that id belongs to</returns>
        public Employer FindBoss(string bossID)
        {
            var a = GetEmployerList().Find(emp => emp.ID == bossID);
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



        #region blabla bla
        //#region add
        //public void AddContract(Contract con)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddEmployee(Employee emp)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddEmployer(Employer emp)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddSpecialization(Specialization sp)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion
        //#region find
        //public Employer FindBoss(string bossID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Contract FindContract(int conID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Specialization FindSpecialization(int specializationID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Employee FindWorker(string workerID)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion
        //#region get
        //public List<Bank> GetBankList()
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Branch> GetBranchList()
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Contract> GetContractList()
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Employee> GetEmployeeList()
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Employer> GetEmployerList()
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Specialization> GetSpecializationList()
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion
        //#region bool
        //public bool IsBankExist(string bankID)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion
        //#region remove
        //public int RemoveAllContract(Predicate<Contract> match)
        //{
        //    throw new NotImplementedException();
        //}

        //public int RemoveAllEmployee(Predicate<Employee> match)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RemoveContract(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RemoveEmployee(string id)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RemoveEmployer(string id)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RemoveSpecialization(int id)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion
        //#region update
        //public void UpdateContract(Contract UpdatedCon)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateEmployee(Employee UpdatedEmp)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateEmployer(Employer UpdatedEmp)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdateSpecialization(Specialization UpdatedSp)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion
        #endregion
    }
}
