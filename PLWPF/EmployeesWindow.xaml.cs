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

        public List<enumDegree> degree = new List<enumDegree>();
        public List<District> region = new List<District>();
        public List<Bank> Banks = new List<Bank>();
        public List<string> EmployeesFullData = new List<string>();
        public List<Specialization> specializations = new List<Specialization>();




        public EmployeesWindow()
        {
            InitializeComponent();

            degree = Enum.GetValues(typeof(enumDegree)).Cast<enumDegree>().ToList();
            ChooseDegree.ItemsSource = degree;
            ChooseUpdatedDegree.ItemsSource = degree;

            region = Enum.GetValues(typeof(District)).Cast<District>().ToList();
            ChooseDistrict.ItemsSource = region;
            ChooseUpdatedDistrict.ItemsSource = region;


            ChooseBranch.ItemsSource = bl.GetBranchList();
            ChooseUpdatedBranch.ItemsSource = bl.GetBranchList();



            ChooseSpec.ItemsSource = bl.GetSpecializationList();
            ChooseUpdatedSpec.ItemsSource = bl.GetSpecializationList();

            ChooseEmployeeToRemove.ItemsSource = bl.GetEmployeeList();
            ChooseEmployeeToUpdate.ItemsSource = bl.GetEmployeeList();







        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int temp;
                BE.Employee worker = new Employee();

                if (AddEmpAddress.Text == null || AddEmpAccountNumber.Text == null || AddEmpBirthDate.SelectedDate == null || ChooseDegree.SelectedItem == null || AddEmpFirstName.Text == null || AddEmpLastName.Text == null || AddEmpID.Text == null || AddEmpPhoneNumber.Text == null || ChooseDistrict.SelectedItem == null || ChooseSpec.SelectedItem == null || AddEmpArmyRecord.IsChecked == null)
                    throw new Exception("All fields must have values!");

                worker.Address = AddEmpAddress.Text;
                ////////////////////////////////////
                BE.BankAccount account;
                if (int.TryParse(AddEmpAccountNumber.Text, out temp))
                {
                    account.AccountNumber = AddEmpAccountNumber.Text;
                    account.branch = (Branch)ChooseBranch.SelectedItem;
                    worker.BankAccount = account;
                }
                else throw new Exception("Account number must have numbers only!");
                //////////////////////////////////////////
                worker.BirthDate = (DateTime)AddEmpBirthDate.SelectedDate;
                ///////////////////////////////////////////
                worker.DealsNum = 0;
                ///////////////////////////////////////////
                worker.Degree = (enumDegree)ChooseDegree.SelectedItem;
                //////////////////////////////////////////
                worker.FirstName = AddEmpFirstName.Text;
                //////////////////////////////////////////
                worker.LastName = AddEmpLastName.Text;
                ////////////////////////////////////////
                if (int.TryParse(AddEmpID.Text, out temp))
                    worker.ID = AddEmpID.Text;
                else throw new Exception("Employee ID must have numbers only!");
                ////////////////////////////////////////
                if (int.TryParse(AddEmpPhoneNumber.Text, out temp))
                    worker.PhoneNumber = AddEmpPhoneNumber.Text;
                else throw new Exception("Employee phone number must have numbers only!");
                ////////////////////////////////////////
                worker.region = (District)ChooseDistrict.SelectedItem;
                ////////////////////////////////////////
                worker.SpecialityID = ((Specialization)ChooseSpec.SelectedItem).SpecializationID.ToString();
                ////////////////////////////////////////
                worker.Veteran = (bool)AddEmpArmyRecord.IsChecked;

                bl.AddEmployee(worker);

                this.ShowMessageAsync(worker.FirstName + " " + worker.LastName + " was updated successfully!", "Good job, you can match boxes");



            }
            catch (Exception c)
            {

                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;
            }



            //ErrorFlyout.Header = "Couldn't add employee, probably your mistake, fix it...";
            //ErrorFlyout.IsOpen = true;
            //  this.ShowMessageAsync("New employee was added successfully!", "Not really, though, just testing things out.");
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
                if (ChooseEmployeeToUpdate.SelectedIndex == -1)
                    throw new Exception("You think you can outsmart me??? no updating for you!!");
                Employee worker = (Employee)((Employee)ChooseEmployeeToUpdate.SelectedItem).Clone();
                //if (AddEmpAddress.Text == "" && AddEmpAccountNumber.Text == "" && AddEmpBirthDate.SelectedDate == null && ChooseDegree.SelectedItem == null && AddEmpFirstName.Text == "" && AddEmpLastName.Text == "" && AddEmpID.Text == "" && AddEmpPhoneNumber.Text == "" && ChooseDistrict.SelectedItem == null && ChooseSpec.SelectedItem == null)
                //    throw new Exception("You think you can outsmart me??? no updating for you!!");
                ////////////////////////////////
                if (UpdatedEmpAddress.Text != "")
                    worker.Address = UpdatedEmpAddress.Text;
                ////////////////////////////////////
                BE.BankAccount account;
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
               
                ///////////////////////////////////////////
                if (ChooseUpdatedDegree.SelectedItem != null)
                    worker.Degree = (enumDegree)ChooseUpdatedDegree.SelectedItem;
                //////////////////////////////////////////
                if (UpdatedEmpFirstName.Text != "")
                    worker.FirstName = UpdatedEmpFirstName.Text;
                //////////////////////////////////////////
                if (UpdatedEmpLastName.Text != "")
                    worker.LastName = UpdatedEmpLastName.Text;
                ////////////////////////////////////////                             
                if (UpdatedEmpPhoneNumber.Text != "")
                {
                    if (int.TryParse(UpdatedEmpPhoneNumber.Text, out temp))
                        worker.PhoneNumber = UpdatedEmpPhoneNumber.Text;
                    else throw new Exception("Employee phone number must have numbers only!");
                }
                ////////////////////////////////////////
                if (ChooseUpdatedDistrict.SelectedItem != null)
                    worker.region = (District)ChooseUpdatedDistrict.SelectedItem;
                ////////////////////////////////////////
                if (ChooseUpdatedSpec.SelectedItem != null)
                    worker.SpecialityID = ((Specialization)ChooseUpdatedSpec.SelectedItem).SpecializationID.ToString();
                ////////////////////////////////////////
                if (UpdatedEmpArmyRecord.IsChecked != null)
                    worker.Veteran = (bool)UpdatedEmpArmyRecord.IsChecked;

                bl.UpdateEmployee(worker);
                this.ShowMessageAsync(worker.FirstName + " " + worker.LastName + " was updated successfully!", "Not really, though, just testing things out.");

            }
            catch (Exception c)
            {
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;
            }


            //Employee worker;
            //try { worker = bl.GetEmployeeList()[ChooseEmployeeToUpdate.SelectedIndex]; }
            //catch { worker = null; }
            //if (worker != null)
            //    this.ShowMessageAsync(worker.FirstName + " " + worker.LastName + " was updated successfully!", "Not really, though, just testing things out.");
            //else
            //{
            //    ErrorFlyout.Header = "Who do you expect me to update, uh? moron...";
            //    ErrorFlyout.IsOpen = true;
            //}
        }
    }
}
