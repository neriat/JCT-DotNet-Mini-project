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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for ContractsWindow.xaml
    /// </summary>
    public partial class ContractsWindow : MetroWindow
    {
        //BL.BL_imp bl = new BL_imp();
        IBL bl = FactoryBL.GetBL();
        private Contract AddContract = new Contract() { StartDate = new DateTime(2017, 1, 1), EndDate = new DateTime(2017, 1, 1) };
        private Contract UpdateContract = new Contract() { StartDate = new DateTime(2017, 1, 1), EndDate = new DateTime(2017, 1, 1) };



        public ContractsWindow()
        {
            InitializeComponent();
            AddTab.DataContext = AddContract;
            AddGrossSalary.ToolTip = "Choose salary:\nChoose an employee to see the range you can pick from";
            UpdateGrossSalary.ToolTip = "Choose salary:\nChoose an employee to see the range you can pick from";
            AllContracts.ItemsSource = bl.GetContractList();
            #region group section
            GroupBySpec.ItemsSource = bl.GroupContractBySpec();
            GroupByDistrict.ItemsSource = bl.GroupContractByDistrict();
            GroupByShares.ItemsSource = bl.GroupContractByShares();
            #endregion
            #region ItemsSources section
            ChooseEmployeeForDeal.ItemsSource = bl.GetEmployeeList();
            ChooseEmployerForDeal.ItemsSource = bl.GetEmployerList();
            ChooseContractToRemove.ItemsSource = bl.GetContractList();
            ChooseContractToUpdate.ItemsSource = bl.GetContractList();
            #endregion
        }
        /// <summary>
        /// The function refresh the items that holds outdated lists 
        /// </summary>
        private void refresh(bool createNew = true)
        {
            AllContracts.ItemsSource = bl.GetContractList();
            ChooseContractToRemove.ItemsSource = null;
            ChooseContractToUpdate.ItemsSource = null;
            ChooseContractToRemove.ItemsSource = bl.GetContractList();
            ChooseContractToUpdate.ItemsSource = bl.GetContractList();
            #region group section
            GroupBySpec.ItemsSource = bl.GroupContractBySpec();
            GroupByDistrict.ItemsSource = bl.GroupContractByDistrict();
            GroupByShares.ItemsSource = bl.GroupContractByShares();
            #endregion

            if (createNew)
            {
                AddContract = new Contract() { StartDate = new DateTime(2017, 1, 1), EndDate = new DateTime(2017, 1, 1) };
                AddTab.DataContext = AddContract;
                UpdateContract = new Contract() { StartDate = new DateTime(2017, 1, 1), EndDate = new DateTime(2017, 1, 1) };
                UpdateTab.DataContext = UpdateContract;
            }
        }

        #region clicks in window
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExceptionContract(AddContract);
                bl.AddContract(AddContract);
                this.ShowMessageAsync("New contract was added successfully!", "Pleasure to do business with you.");
                refresh();
            }
            catch (Exception c)
            {
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;
            }
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ChooseContractToRemove.SelectedIndex == -1)
                    throw new Exception("What exactly do you want me too remove, uh? moron...");
                string messege = "Contract no. " + ((Contract)ChooseContractToRemove.SelectedItem).ContractID.ToString();
                bl.RemoveContract(((Contract)ChooseContractToRemove.SelectedItem).ContractID);
                this.ShowMessageAsync(messege + " was removed successfuly", "Good job, you chose wisly");
                refresh(false);
            }
            catch (Exception c)
            {
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;
            }
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (ChooseContractToUpdate.SelectedIndex == -1)
                    throw new Exception("Choose something moron");


                ExceptionContract(UpdateContract);
                bl.UpdateContract(UpdateContract);
                string messege = "Contract no. " + UpdateContract.ContractID;
                this.ShowMessageAsync(messege + " was updated successfuly", "Nice updating skills you got there");
                refresh();
                #region OLD
                #region check if entered critical information
                //bool TypeChangedValueSigned = UpdateIsSinged.IsChecked == c.IsSigned ? false : true;
                //bool TypeChangedValueInterviewed = UpdateWasInterviewed.IsChecked == c.IsInterviewed ? false : true;
                //if (
                //    UpdatedContractEndingDate.SelectedDate == null &&
                //    UpdateGrossSalary.Value == null &&
                //    !TypeChangedValueSigned &&
                //    !TypeChangedValueInterviewed
                //    )
                //    throw new Exception("I updated successfully. nothing.");
                #endregion
                #region Assign new values , doesnt effect the database
                //if (UpdatedContractEndingDate.SelectedDate != null)
                //    c.EndDate = (DateTime)UpdatedContractEndingDate.SelectedDate;

                //c.IsInterviewed = (bool)UpdateWasInterviewed.IsChecked;
                //c.IsSigned = (bool)UpdateIsSinged.IsChecked;
                //c.WorkingHours = (double)UpdateWorkingHours.Value;
                //c.GrossSalary = (double)UpdateGrossSalary.Value;

                #endregion
                #endregion
            }
            catch (Exception c)
            {
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;
            }

        }
        #endregion

        private void ChooseContractToUpdate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseContractToUpdate.SelectedIndex != -1)
            {
                UpdateContract = (Contract)ChooseContractToUpdate.SelectedItem;
                UpdateTab.DataContext = UpdateContract;

            }

            try
            {
                Specialization temp = (Specialization)bl.FindSpecialization(int.Parse(bl.FindWorker(UpdateContract.EmployeeID).SpecialityID));
                UpdateGrossSalary.ToolTip = "Choose salary between " + temp.MinSalary + " and " + temp.MaxSalary;
                UpdateGrossSalary.Maximum = temp.MaxSalary;
                UpdateGrossSalary.Minimum = temp.MinSalary;
            }
            catch (Exception)
            {
                // Just dont show the right tooltip
            }
        }
        private void ExceptionContract(Contract c)
        {
            if (
                c.EmployeeID == null ||
                c.EmployerID == null ||
                c.EndDate == null ||
                c.StartDate == null
                )
                throw new Exception("All fields must be filled");
            if (c.GrossSalary <= 0)
                throw new Exception("No one works for free");

            if (c.WorkingHours <= 0)
                throw new Exception("Working hours must have a positive value");

        }

        private void Sort_IsCheckedChanged(object sender, EventArgs e)
        {
            GroupBySpec.ItemsSource = bl.GroupContractBySpec(Sort.IsEnabled);
            GroupByDistrict.ItemsSource = bl.GroupContractByDistrict(Sort.IsEnabled);
            GroupByShares.ItemsSource = bl.GroupContractByShares(Sort.IsEnabled);
        }

        private void ChooseEmployeeForDeal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Specialization temp = (Specialization)bl.FindSpecialization(int.Parse(((Employee)ChooseEmployeeForDeal.SelectedItem).SpecialityID));
                AddGrossSalary.ToolTip = "Choose salary between " + temp.MinSalary + " and " + temp.MaxSalary;
                AddGrossSalary.Maximum = temp.MaxSalary;
                AddGrossSalary.Minimum = temp.MinSalary;
            }
            catch (Exception)
            {
                // Just dont show the right tooltip
            }
        }
    }
    #region     CONVERTER USED IN XAML
    public class EmployeeToEmployeeID : IValueConverter
    {
        IBL bl = FactoryBL.GetBL();
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return bl.GetEmployeeList()[(int)value].ID;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return bl.GetEmployeeList().FindIndex(x => x.ID == ((string)value));
        }
    }

    public class EmployerToEmployerID : IValueConverter
    {
        IBL bl = FactoryBL.GetBL();
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return bl.GetEmployerList()[(int)value].ID;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return bl.GetEmployerList().FindIndex(x => x.ID == ((string)value));
        }
    }
    public class EmployeeIdToName : IValueConverter
    {
        IBL bl = FactoryBL.GetBL();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Employee worker = bl.FindWorker((string)value);
            return worker.FirstName + " " + worker.LastName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class EmployerIdToName : IValueConverter
    {
        IBL bl = FactoryBL.GetBL();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Employer boss = bl.FindBoss((string)value);
            if (boss.IsCompany)
                return boss.CompanyName;
            else return boss.FirstName + " " + boss.LastName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    #endregion
}
