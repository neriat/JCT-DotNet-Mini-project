using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using BL;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow


    {
        public MainWindow()
        {
            InitializeComponent();

            BL.BL_imp bl = new BL_imp();

            #region ExampleSpecialization
            Specialization sp = new Specialization();
            sp.Field = SpecializationField.Data_Base;
            sp.MaxSalary = 100000;
            sp.MinSalary = 0;
            sp.SpecializationID = 10000000;
            sp.SpecializationName = "Something";
            bl.AddSpecialization(sp);
            #endregion

            #region ExampleEmployee
            Employee e = new Employee();
            e.Address = "asd";
            BankAccount acc = new BankAccount();
            acc.AccountNumber = "123";
            acc.branch = bl.GetBranchList()[0];
            e.BankAccount = acc;
            e.BirthDate = new DateTime(1970, 4, 8);
            e.DealsNum = 0;
            e.Degree = enumDegree.BA;
            e.FirstName = "Shoham";
            e.ID = "208720623";
            e.LastName = "Jacobsen";
            e.PhoneNumber = "1234";
            e.region = District.Central;
            e.SpecialityID = "10000000";
            e.Veteran = true;
            bl.AddEmployee(e);
            #endregion

            #region ExampleEmployer
            Employer emp = new Employer();
            emp.Address = "asd";
            acc.AccountNumber = "123";
            acc.branch = bl.GetBranchList()[0];
            emp.ContractsNum = 0;
            emp.FirstName = "Big";
            emp.ID = "208720624";
            emp.LastName = "Boss";
            emp.PhoneNumber = "1234";
            emp.CompanyName = "good company";
            emp.Field = SpecializationField.Data_Base;
            bl.AddEmployer(emp);
            #endregion

            #region ExampleContract
            Contract c = new Contract();
            c.ContractID = 0;
            c.IsInterviewed = true;
            c.IsSigned = true;
            c.EmployeeID = "208720623";
            c.EmployerID = "208720624";
            c.EndDate = new DateTime(2020, 3, 2);
            c.GrossSalary = 400;
            c.NetSalary = 300;
            c.StartDate = new DateTime(1990, 3, 2);
            c.WorkingHours = 3;
            bl.AddContract(c);
            #endregion







        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeesWindow window = new EmployeesWindow();
            window.ShowDialog();
        }

        private void SpecializationButton_Click(object sender, RoutedEventArgs e)
        {
            SpecializationWindow window = new SpecializationWindow();
            window.ShowDialog();
        }

        private void EmployersButton_Click(object sender, RoutedEventArgs e)
        {
            EmployersWindow window = new EmployersWindow();
            window.ShowDialog();
        }

        private void ContractsButton_Click(object sender, RoutedEventArgs e)
        {
            ContractsWindow window = new ContractsWindow();
            window.ShowDialog();
        }

        private void Tile_MouseEnter(object sender, MouseEventArgs e)
        {
            Tile thisTile = (Tile)sender;
            thisTile.TitleFontSize += 3;           
        }

        private void Tile_MouseLeave(object sender, MouseEventArgs e)
        {
            Tile thisTile = (Tile)sender;
            thisTile.TitleFontSize -= 3;
        }

      
    }

}
