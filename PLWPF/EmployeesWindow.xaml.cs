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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for EmployeesWindow.xaml
    /// </summary>
    public partial class EmployeesWindow : MetroWindow
    {
        BL.BL_imp bl = new BL_imp();

        public EmployeesWindow()
        {
            InitializeComponent();

            #region enumerators in the window
            ChooseDegree.ItemsSource = Enum.GetValues(typeof(enumDegree)).Cast<enumDegree>().ToList();
            ChooseUpdatedDegree.ItemsSource = Enum.GetValues(typeof(enumDegree)).Cast<enumDegree>().ToList();

            ChooseDistrict.ItemsSource = Enum.GetValues(typeof(District)).Cast<District>().ToList();
            ChooseUpdatedDistrict.ItemsSource = Enum.GetValues(typeof(District)).Cast<District>().ToList();

            ChooseBranch.ItemsSource = bl.GetBranchList();
            ChooseUpdatedBranch.ItemsSource = bl.GetBranchList();

            ChooseSpec.ItemsSource = bl.GetSpecializationList();
            ChooseUpdatedSpec.ItemsSource = bl.GetSpecializationList();

            ChooseEmployeeToRemove.ItemsSource = bl.GetEmployeeList();
            ChooseEmployeeToUpdate.ItemsSource = bl.GetEmployeeList();
            #endregion
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int temp = -1;
                BE.Employee worker = new Employee();
                BE.BankAccount account;

                #region check if entered critical information
                if (
                    AddEmpID.Text == null ||
                    AddEmpFirstName.Text == null ||
                    AddEmpLastName.Text == null ||
                    AddEmpBirthDate.SelectedDate == null ||
                    AddEmpPhoneNumber.Text == null ||
                    AddEmpAddress.Text == null ||
                    ChooseDistrict.SelectedItem == null ||
                    ChooseDegree.SelectedItem == null ||
                    AddEmpArmyRecord.IsChecked == null ||
                    AddEmpAccountNumber.Text == null ||
                    ChooseSpec.SelectedItem == null
                    )
                    throw new Exception("All fields must have values!");
                #endregion
                #region Assign new values , doesnt effect the database
                worker.Address = AddEmpAddress.Text;
                worker.BirthDate = (DateTime)AddEmpBirthDate.SelectedDate;
                worker.DealsNum = 0;
                worker.Degree = (enumDegree)ChooseDegree.SelectedItem;
                worker.FirstName = AddEmpFirstName.Text;
                worker.LastName = AddEmpLastName.Text;
                worker.region = (District)ChooseDistrict.SelectedItem;
                worker.SpecialityID = ((Specialization)ChooseSpec.SelectedItem).SpecializationID.ToString();
                worker.Veteran = (bool)AddEmpArmyRecord.IsChecked;

                if (int.TryParse(AddEmpAccountNumber.Text, out temp))
                {
                    account.AccountNumber = AddEmpAccountNumber.Text;
                    account.branch = (Branch)ChooseBranch.SelectedItem;
                    worker.BankAccount = account;
                }
                else throw new Exception("Account number must have numbers only!");

                if (int.TryParse(AddEmpID.Text, out temp))
                    worker.ID = AddEmpID.Text;
                else throw new Exception("Employee ID must have numbers only!");

                if (int.TryParse(AddEmpPhoneNumber.Text, out temp))
                    worker.PhoneNumber = AddEmpPhoneNumber.Text;
                else throw new Exception("Employee phone number must have numbers only!");
                #endregion

                bl.AddEmployee(worker);
                this.ShowMessageAsync(worker.FirstName + " " + worker.LastName + " was updated successfully!", "Good job, you can match boxes");
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
                if (ChooseEmployeeToRemove.SelectedIndex == -1)
                    throw new Exception("Who exactly you want me too remove, uh? moron...");
                string messege = ((Employee)ChooseEmployeeToRemove.SelectedItem).ToString();
                bl.RemoveEmployee(((Employee)ChooseEmployeeToRemove.SelectedItem).ID);
                this.ShowMessageAsync(messege + " was removed successfuly", "Good job, you chose wisly");
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
                int temp;
                BE.BankAccount account;
                Employee worker = (Employee)((Employee)ChooseEmployeeToUpdate.SelectedItem).Clone();

                #region check if no update is needed
                if (ChooseEmployeeToUpdate.SelectedIndex == -1)
                    throw new Exception("You think you can outsmart me??? no updating for you!!");
                bool veternChangedValue = UpdatedEmpArmyRecord.IsChecked == worker.Veteran ? false : true;
                if (
                    UpdatedEmpAddress.Text == "" &&
                    UpdatedEmpAccountNumber.Text == "" &&
                    ChooseUpdatedDegree.SelectedItem == null &&
                    UpdatedEmpFirstName.Text == "" &&
                    UpdatedEmpLastName.Text == "" &&
                    UpdatedEmpPhoneNumber.Text == "" &&
                    ChooseUpdatedDistrict.SelectedItem == null &&
                    ChooseSpec.SelectedItem == null &&
                    !veternChangedValue
                    )
                    throw new Exception("You think you can outsmart me??? no updating for you!!");
                #endregion
                #region Assign new values , doesnt effect the database
                if (UpdatedEmpAddress.Text != "")
                    worker.Address = UpdatedEmpAddress.Text;
                if (UpdatedEmpAccountNumber.Text != "")
                {
                    if (int.TryParse(UpdatedEmpAccountNumber.Text, out temp))
                    {
                        account.AccountNumber = UpdatedEmpAccountNumber.Text;
                        account.branch = (Branch)ChooseUpdatedBranch.SelectedItem;
                        worker.BankAccount = account;
                    }
                    else throw new Exception("Account number must have numbers only!");
                }
                if (ChooseUpdatedDegree.SelectedItem != null)
                    worker.Degree = (enumDegree)ChooseUpdatedDegree.SelectedItem;
                if (UpdatedEmpFirstName.Text != "")
                    worker.FirstName = UpdatedEmpFirstName.Text;
                if (UpdatedEmpLastName.Text != "")
                    worker.LastName = UpdatedEmpLastName.Text;
                if (UpdatedEmpPhoneNumber.Text != "")
                {
                    if (int.TryParse(UpdatedEmpPhoneNumber.Text, out temp))
                        worker.PhoneNumber = UpdatedEmpPhoneNumber.Text;
                    else throw new Exception("Employee phone number must have numbers only!");
                }
                if (ChooseUpdatedDistrict.SelectedItem != null)
                    worker.region = (District)ChooseUpdatedDistrict.SelectedItem;
                if (ChooseUpdatedSpec.SelectedItem != null)
                    worker.SpecialityID = ((Specialization)ChooseUpdatedSpec.SelectedItem).SpecializationID.ToString();

                if (UpdatedEmpArmyRecord.IsChecked != null)
                    worker.Veteran = (bool)UpdatedEmpArmyRecord.IsChecked;
                #endregion

                //updating
                bl.UpdateEmployee(worker);
                // and its done.
                this.ShowMessageAsync(worker.FirstName + " " + worker.LastName + " was updated successfully!", "Not really, though, just testing things out.");

            }
            catch (Exception c)
            {
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;
            }
        }
    }
}
