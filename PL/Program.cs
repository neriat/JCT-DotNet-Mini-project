using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using BE;
namespace PL
{
    class Program
    {
        static public BL_imp func = new BL_imp();
        static public void contracts()
        {
            char op = ' ';
            do
            {
                Console.WriteLine(@"Contracts - Enter your option:
a - Add a new contract
d - Delete a contract 
u - Update an existing contract
--
q- Move back to the Main Menu");
                op = Console.ReadKey(true).KeyChar;
                switch (op)
                {
                    case 'a':
                        Contract New = new Contract();
                        Console.WriteLine("Enter the employer ID");
                        New.EmployerID = Console.ReadLine();

                        Console.WriteLine("Enter the employee ID");
                        New.EmployeeID = Console.ReadLine();

                        Console.WriteLine("Is the employee interviewed yet? - Enter 'true' or 'false'\ninvalid input will count as 'false'");
                        bool YoN;
                        bool valid = bool.TryParse(Console.ReadLine(), out YoN);
                        if (valid) New.IsInterviewed = YoN;
                        else New.IsInterviewed = false;

                        Console.WriteLine("Is the employee signed? - Enter 'true' or 'false'\ninvalid input will count as 'false'");
                        valid = bool.TryParse(Console.ReadLine(), out YoN);
                        if (valid) New.IsSigned = YoN;
                        else New.IsSigned = false;

                        Console.WriteLine("Enter gross salary for the employer -  The net salary will be calculated automatically");
                        New.GrossSalary = double.Parse(Console.ReadLine());

                        Console.WriteLine("Enter the starting date of the contract - Use the dd/mm/yyyy format");
                        string date = Console.ReadLine();
                        New.StartDate = Convert.ToDateTime(date);

                        Console.WriteLine("Enter the ending date of the contract - Use the dd/mm/yyyy format");
                        date = Console.ReadLine();
                        New.EndDate = Convert.ToDateTime(date);

                        Console.WriteLine("Enter the amount of working hours");
                        New.GrossSalary = double.Parse(Console.ReadLine());

                        func.AddContract(New);
                        break;
                    case 'd':
                        break;
                    case 'u':
                        break;
                    default:
                        break;
                }

            } while (op != 'q');
        }
        static void Main(string[] args)
        {

            char op = ' ';
            do
            {
                Console.WriteLine(@"Main Menu - Enter your option:
b - banking
c - contracting
d - employees
e - employers
s - specialization
--
q- exit");
                op = Console.ReadKey(true).KeyChar;

                switch (op)
                {
                    case 'b':
                        Console.WriteLine("Here are the branches you can choose from: ");
                        foreach (var item in func.GetBankList())
                            Console.WriteLine(item.ToString());
                        break;
                    case 'c':
                        contracts();

                        break;

                    case 'd':
                        break;

                    case 'e':
                        break;

                    case 's':
                        break;
                    case 'q': break;
                    default:
                        break;
                }





            } while (op != 'q');
        }
    }
}
