using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Exceptions
    {
        public static Exception ID_Digits = new Exception("ID must have numbers only!");
        public static Exception Phonenumber_Digits = new Exception("Phone number must have numbers only!");
        public static Exception AllFields = new Exception("All fields must have values!");

        public class Employer
        {
            public static Exception NoSelectionDelete  = new Exception("Can't touch this. too doo doo doh");
            public static Exception NoSelectionUpdate = new Exception("My job is done. Nothing to update here. no selection");
            public static Exception Exist = new Exception("Employer is already in the system");
            public static Exception ExistN = new Exception("Employer doesn't exist");
        }

        public class Employee
        {
            public static Exception NoSelectionDelete = new Exception("Can't touch this. too doo doo doh");
            public static Exception NoSelectionUpdate = new Exception("My job is done. Nothing to update here. no selection");
            public static Exception Exist = new Exception("Employee is already in the system");
            public static Exception ExistN = new Exception("Employee doesn't exist");
        }

        public class Specialization
        {
           // public static Exception NoSelectionDelete = 
           // public static Exception NoSelectionUpdate =
            public static Exception Exist = new Exception("Specialization is already in the system");
            public static Exception ExistN = new Exception("No such specialization exists in the system");
        }

        public class Contract
        {
         //   public static Exception NoSelectionDelete =
            public static Exception Exist = new Exception("Contract is already in the system");
            public static Exception ExistN = new Exception("Contract doesn't exist");
        }


        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();
        //public static Exception  = new Exception();

                                          


    }
}
