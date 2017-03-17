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
using BE;
using BL;
using System.Globalization;
using System.ComponentModel;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for EmployeesWindow.xaml
    /// </summary>
    public partial class EmployeesWindow : MetroWindow
    {
        BackgroundWorker worker = new BackgroundWorker();
        IEnumerable<IGrouping<string, IGrouping<string, Branch>>> branches;
        IBL bl = FactoryBL.GetBL();
        private static BankAccount WindowEmplyoee_BankAccount = new BankAccount() { branch = new Branch() { bank = new Bank() } };
        private Employee AddEmplyoee = new Employee()
        {
            BirthDate = new DateTime(2017, 1, 1),
            BankAccount = WindowEmplyoee_BankAccount,
        };
        private Employee UpdateEmplyoee = new Employee();

        private bool BanksDownloaded = true;



        public EmployeesWindow()
        {
            InitializeComponent();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
            AddTab.DataContext = AddEmplyoee;
            #region ItemsSources
            ChooseDegree.ItemsSource = Enum.GetValues(typeof(enumDegree)).Cast<enumDegree>().ToList();
            ChooseUpdatedDegree.ItemsSource = Enum.GetValues(typeof(enumDegree)).Cast<enumDegree>().ToList();

            ChooseDistrict.ItemsSource = Enum.GetValues(typeof(District)).Cast<District>().ToList();
            ChooseUpdatedDistrict.ItemsSource = Enum.GetValues(typeof(District)).Cast<District>().ToList();

            ChooseSpec.ItemsSource = bl.GetSpecializationList();
            ChooseUpdatedSpec.ItemsSource = bl.GetSpecializationList();

            ChooseEmployeeToRemove.ItemsSource = bl.GetEmployeeList();
            ChooseEmployeeToUpdate.ItemsSource = bl.GetEmployeeList();

            AllEmployees.ItemsSource = bl.GetEmployeeList();

            #endregion
            #region Groups
            GroupByDistricts.ItemsSource = bl.GroupEmployeeByDistrict();
            GroupByDegree.ItemsSource = bl.GroupEmployeeByDegree();
            GroupByBirthYear.ItemsSource = bl.GroupEmployeeByBirthYear();
            GroupByBankName.ItemsSource = bl.GroupEmployeeByBank();
            #endregion
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (BanksDownloaded)
            {
                LoadingBanks.Visibility = Visibility.Collapsed;
                ChooseBank.Visibility = Visibility.Visible;
                UpdateLoadingBanks.Visibility = Visibility.Collapsed;
                UpdateChooseBank.Visibility = Visibility.Visible;
                EmployeeLoadingBanks.Visibility = Visibility.Collapsed;
                ChooseEmployeeToUpdate.Visibility = Visibility.Visible;
                ChooseEmployeeToUpdate.IsEnabled = true;
                ChooseBank.IsEnabled = true;
                UpdateChooseBank.IsEnabled = true;
                ChooseBank.ItemsSource = branches;
                UpdateChooseBank.ItemsSource = branches;
            }
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {

                branches = bl.GetAllBranchesByBankAndCity().ToList();

            }
            catch (Exception)
            {
                Dispatcher.Invoke(() =>
                    {
                        BanksDownloaded = false;
                    }
                );
            }
        }


        /// <summary>
        /// The function refresh the items that holds outdated lists 
        /// </summary>
        private void refresh(bool createNew = true)
        {
            #region Groups
            GroupByDistricts.ItemsSource = bl.GroupEmployeeByDistrict();
            GroupByDegree.ItemsSource = bl.GroupEmployeeByDegree();
            GroupByBirthYear.ItemsSource = bl.GroupEmployeeByBirthYear();
            GroupByBankName.ItemsSource = bl.GroupEmployeeByBank();
            #endregion

            ChooseEmployeeToRemove.ItemsSource = null;
            ChooseEmployeeToUpdate.ItemsSource = null;
            ChooseEmployeeToRemove.ItemsSource = bl.GetEmployeeList();
            ChooseEmployeeToUpdate.ItemsSource = bl.GetEmployeeList();
            ChooseBranch.SelectedIndex = -1;
            AddEmpAccountNumber.Text = "";
            UpdateChooseBank.SelectedIndex = -1;
            UpdatedEmpAccountNumber.Text = "";
            ChooseBank.SelectedIndex = -1;
            ChooseCity.SelectedIndex = -1;
            AllEmployees.ItemsSource = bl.GetEmployeeList();

            if (createNew)
            {
                WindowEmplyoee_BankAccount = new BankAccount() { branch = new Branch() { bank = new Bank() } };
                AddEmplyoee = new Employee()
                {
                    BirthDate = new DateTime(2017, 1, 1),
                    BankAccount = WindowEmplyoee_BankAccount,
                };
                AddTab.DataContext = AddEmplyoee;
                UpdateEmplyoee = new Employee();
                UpdateTab.DataContext = UpdateEmplyoee;
            }
        }

        #region window clicks
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddEmplyoee.BankAccount.AccountNumber = AddEmpAccountNumber.Text;
                ExceptionEmployee(AddEmplyoee);

                bl.AddEmployee(AddEmplyoee);

                this.ShowMessageAsync(AddEmplyoee.ToString() + " was added successfully!", "Good job, you can match boxes");
                refresh();
                Tools.SoundWhip.Play();
            }
            catch (Exception c)
            {
                Tools.SoundFailed.Play();
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;

            }
            
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ChooseEmployeeToRemove.SelectedIndex == -1)
                    throw new Exception("Who exactly you want me too remove, uh? moron...");
                string messege = ((Employee)ChooseEmployeeToRemove.SelectedItem).ToString();
                bl.RemoveEmployee(((Employee)ChooseEmployeeToRemove.SelectedItem).ID);
                this.ShowMessageAsync(messege + " was removed successfuly", "Good job, you chose wisly");

                refresh(false);
            }
            catch (Exception c)
            {
                Tools.SoundFailed.Play();
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;
            }
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (ChooseEmployeeToUpdate.SelectedIndex == -1)
                    throw new Exception("You think you can outsmart me??? no updating for you!!");
                
                UpdateEmplyoee.BankAccount = new BankAccount()
                {
                    branch = (Branch)UpdateChooseBranch.SelectedItem,
                    AccountNumber = UpdatedEmpAccountNumber.Text,
                };
                ExceptionEmployee(UpdateEmplyoee);
                bl.UpdateEmployee(UpdateEmplyoee);
                this.ShowMessageAsync(UpdateEmplyoee.FirstName + " " + UpdateEmplyoee.LastName + " was updated successfully!", "Good job, you can match boxes");

                refresh();
            }
            catch (Exception c)
            {
                Tools.SoundFailed.Play();
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;
            }
        }
        
        #endregion
        private void ExceptionEmployee(Employee emp)
        {
            int temp = -1;
            if (
                       emp.ID == ""
                    || emp.LastName == ""
                    || emp.Address == ""
                    || emp.FirstName == ""
                    || emp.PhoneNumber == ""
                    || emp.SpecialityID == null
                    || emp.BankAccount.AccountNumber == ""
                    || emp.BankAccount.branch ==null
                    || emp.Degree == null
                    || emp.region == null
                    || emp.BirthDate == null
                     )
                throw new Exception("All fields must have values!");

            if (!int.TryParse(emp.BankAccount.AccountNumber, out temp))
                throw new Exception("Account number must have numbers only!");
            else if (temp < 0)
                throw new Exception("Don't be so negative.");
            else if (temp == 0)
                throw new Exception("That's how much you worth, ZERO!");

            if (!int.TryParse(emp.ID, out temp))
                throw new Exception("Invalid ID format!");
            else if (temp < 0)
                throw new Exception("Don't be so negative.");
            else if (temp == 0)
                throw new Exception("That's how much you worth, ZERO!");

            if (!int.TryParse(emp.PhoneNumber, out temp))
                throw new Exception("Employee phone number must have numbers only!");
            else if (temp < 0)
                throw new Exception("Don't be so negative.");
            else if (temp == 0)
                throw new Exception("That's how much you worth, ZERO!");



        }
        private void AddBranchChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseBranch.SelectedIndex != -1)
            {
                AddEmplyoee.BankAccount.branch = (Branch)ChooseBranch.SelectedItem;
            }
        }
        private void UpdateBranchChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UpdateChooseBranch.SelectedIndex != -1)
            {
                UpdateEmplyoee.BankAccount = new BankAccount()
                {
                    branch = (Branch)((Branch)UpdateChooseBranch.SelectedItem).Clone(),
                };

            }
        }
        private void ChooseEmployeeToUpdate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseEmployeeToUpdate.SelectedIndex != -1)
            {
                UpdateEmplyoee = (Employee)ChooseEmployeeToUpdate.SelectedItem;
                if (branches != null)
                {
                    UpdateChooseBank.SelectedItem = branches.ToList().Find(x => x.Key == UpdateEmplyoee.BankAccount.branch.bank.BankName);
                    UpdatedEmpAccountNumber.Text = UpdateEmplyoee.BankAccount.AccountNumber;
                    var citylist = branches.ToList().Find(x => x.Key == UpdateEmplyoee.BankAccount.branch.bank.BankName).ToList();
                    UpdateChooseCity.SelectedItem = citylist.Find(x => x.Key == UpdateEmplyoee.BankAccount.branch.BranchCity);
                    var branchlist = citylist.Find(x => x.Key == UpdateEmplyoee.BankAccount.branch.BranchCity).ToList();
                    UpdateChooseBranch.SelectedItem = branchlist.Find(x => x.BranchID == UpdateEmplyoee.BankAccount.branch.BranchID);
                }
                else
                {
                    ErrorFlyout.Header = "Couldn't load banks, add them manually or restart the window";
                    ErrorFlyout.IsOpen = true;
                }
                UpdateTab.DataContext = UpdateEmplyoee;
            }
        }



        private void Sort_IsCheckedChanged(object sender, EventArgs e)
        {
            GroupByDistricts.ItemsSource = bl.GroupEmployeeByDistrict(Sort.IsEnabled);
            GroupByDegree.ItemsSource = bl.GroupEmployeeByDegree(Sort.IsEnabled);
            GroupByBirthYear.ItemsSource = bl.GroupEmployeeByBirthYear(Sort.IsEnabled);
            GroupByBankName.ItemsSource = bl.GroupEmployeeByBank(Sort.IsEnabled);
        }
    }
    public class SpecialityToSpecialityID : IValueConverter
    {
        IBL bl = FactoryBL.GetBL();
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return bl.GetSpecializationList()[(int)value].SpecializationID.ToString();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return bl.GetSpecializationList().FindIndex(x => x.SpecializationID.ToString() == ((string)value));
        }
    }
}

