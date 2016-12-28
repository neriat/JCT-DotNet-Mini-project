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
            #region sample employee
            Employee e = new Employee();
            e.Address = "asd";
            BankAccount acc = new BankAccount();
            acc.AccountNumber = "123";
            acc.branch = bl.GetBranchList()[0];
            e.BankAccount = acc;
            e.BirthDate = new DateTime(1970,4,8);
            e.DealsNum = 0;
            e.Degree = enumDegree.BA;
            e.FirstName = "Shoham";
            e.ID = "208720623";
            e.LastName = "Jacobsen";
            e.PhoneNumber = "1234";
            e.region = District.Central;
            e.SpecialityID = "0";
            e.Veteran = true;
            bl.AddEmployee(e);
            #endregion
            #region sample employer
            Employer boss = new Employer();
            boss.ID = "209038876";
            boss.IsCompany = false;
            boss.FirstName = "Neria";
            boss.LastName = "Tzidkani";
            boss.CompanyName = "";
            boss.PhoneNumber = "0585553340";
            boss.Address = "blabla 13";
            boss.Field = SpecializationField.GUI;
            boss.EstablishmentDate = new DateTime(1970, 4, 8);
            boss.ContractsNum = 0;

            bl.AddEmployer(boss);
            #endregion
        }
        #region menu clicks
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
        #endregion
    }

}
