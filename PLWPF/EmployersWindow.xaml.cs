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
    public partial class EmployersWindow : MetroWindow
    {
        BL.BL_imp bl = new BL_imp();

        public EmployersWindow()
        {
            InitializeComponent();

            #region ItemsSources in the window
            AddEmpChooseField.ItemsSource = Enum.GetValues(typeof(SpecializationField)).Cast<SpecializationField>().ToList();
            UpdateEmpChooseField.ItemsSource = Enum.GetValues(typeof(SpecializationField)).Cast<SpecializationField>().ToList();
            ChooseEmployerToRemove.ItemsSource = bl.GetEmployerList();
            ChooseEmployerToUpdate.ItemsSource = bl.GetEmployerList();
            #endregion
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int temp = -1;
                BE.Employer boss = new Employer();

                #region check if entered critical information
                if (
                    AddEmpID.Text == "" ||
                    AddEmpBusinessType == null ||
                    AddEmpFirstName.Text == "" ||
                    AddEmpLastName.Text == "" ||
                    AddEmpCompanyName.Text == "" ||
                    AddEmpPhoneNumber.Text == "" ||
                    AddEmpAddress.Text == "" ||
                    AddEmpChooseField == null ||
                    AddEmpEstablishmentDates == null
                    )
                    throw new Exception("All fields must have values!");
                #endregion
                #region Assign new values , doesnt effect the database
                if (int.TryParse(AddEmpID.Text, out temp))
                    boss.ID = AddEmpID.Text;
                else throw new Exception("Employee ID must have numbers only!");

                boss.IsCompany = (bool)AddEmpBusinessType.IsChecked;
                boss.FirstName = AddEmpFirstName.Text;
                boss.LastName = AddEmpLastName.Text;
                boss.CompanyName = AddEmpCompanyName.Text;
                if (int.TryParse(AddEmpPhoneNumber.Text, out temp))
                    boss.PhoneNumber = AddEmpPhoneNumber.Text;
                else throw new Exception("Employee phone number must have numbers only!");

                boss.Address = AddEmpAddress.Text;
                boss.Field = (SpecializationField)AddEmpChooseField.SelectedItem;
                boss.EstablishmentDate = (DateTime)AddEmpEstablishmentDates.SelectedDate;
                boss.ContractsNum = 0;
                #endregion

                bl.AddEmployer(boss);
                this.ShowMessageAsync(boss.FirstName + " " + boss.LastName + " was added successfully!", "Your mom probably very proud of you");
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
                if (ChooseEmployerToRemove.SelectedIndex == -1)
                    throw new Exception("Can't touch this. too doo doo doh");
                string messege = ((Employer)ChooseEmployerToRemove.SelectedItem).ToString();
                bl.RemoveEmployer(((Employer)ChooseEmployerToRemove.SelectedItem).ID);
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
                int temp = -1;
                Employer boss = (Employer)((Employer)ChooseEmployerToUpdate.SelectedItem).Clone();
                
                #region check if entered critical information
                if (ChooseEmployerToUpdate.SelectedIndex == -1)
                    throw new Exception("My job is done. Nothing to update here.");
                bool BusinessTypeChangedValue = UpdateEmpBusinessType.IsChecked == boss.IsCompany ? false : true;
                if (
                    UpdateEmpID.Text == "" &&
                    !BusinessTypeChangedValue &&
                    UpdateEmpFirstName.Text == "" &&
                    UpdateEmpLastName.Text == "" &&
                    UpdateEmpCompanyName.Text == "" &&
                    UpdateEmpPhoneNumber.Text == "" &&
                    UpdateEmpAddress.Text == "" &&
                    UpdateEmpChooseField.SelectedIndex == -1
                    )
                    throw new Exception("My job is done. Nothing to update here.");
                #endregion
                #region Assign new values , doesnt effect the database
                if (UpdateEmpID.Text != "")
                {
                    if (int.TryParse(UpdateEmpID.Text, out temp))
                        boss.ID = UpdateEmpID.Text;
                    else throw new Exception("Employee ID must have numbers only!");
                }

                if (UpdateEmpBusinessType.IsChecked != null)
                    boss.IsCompany = (bool)UpdateEmpBusinessType.IsChecked;

                if (UpdateEmpFirstName.Text != "")
                    boss.FirstName = UpdateEmpFirstName.Text;

                if (UpdateEmpLastName.Text != "")
                    boss.LastName = UpdateEmpLastName.Text;

                if (UpdateEmpCompanyName.Text != "")
                    boss.CompanyName = UpdateEmpCompanyName.Text;

                if (UpdateEmpPhoneNumber.Text != "")
                {
                    if (int.TryParse(UpdateEmpPhoneNumber.Text, out temp))
                        boss.PhoneNumber = UpdateEmpPhoneNumber.Text;
                    else throw new Exception("Employee phone number must have numbers only!");
                }

                if (UpdateEmpAddress.Text !="")
                    boss.Address = UpdateEmpAddress.Text;

                if (UpdateEmpChooseField.SelectedItem !=null)
                    boss.Field = (SpecializationField)UpdateEmpChooseField.SelectedItem;
                #endregion

                bl.UpdateEmployer(boss);
                this.ShowMessageAsync(boss.FirstName + " " + boss.LastName + " was updated successfully!", "Your mom probably very proud of you");
            }
            catch (Exception c)
            {
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;
            }
        }


    }
}
