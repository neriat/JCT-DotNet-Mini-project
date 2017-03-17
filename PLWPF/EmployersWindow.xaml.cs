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
using SciChart.Wpf.UI.Transitionz;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for EmployeesWindow.xaml
    /// </summary>
    public partial class EmployersWindow : MetroWindow
    {
        IBL bl = FactoryBL.GetBL();
        private Employer UpdateEmployer = new Employer() { EstablishmentDate = new DateTime(2017, 1, 1) };
        private Employer AddEmployer = new Employer() { EstablishmentDate = new DateTime(2017, 1, 1) };


        public EmployersWindow()
        {
            InitializeComponent();
            AddTab.DataContext = AddEmployer;

            #region ItemsSources in the window
            AddEmpChooseField.ItemsSource = Enum.GetValues(typeof(SpecializationField)).Cast<SpecializationField>().ToList();
            UpdateEmpChooseField.ItemsSource = Enum.GetValues(typeof(SpecializationField)).Cast<SpecializationField>().ToList();
            ChooseEmployerToRemove.ItemsSource = bl.GetEmployerList();
            ChooseEmployerToUpdate.ItemsSource = bl.GetEmployerList();
            AllEmployers.ItemsSource = bl.GetEmployerList();
            #endregion
            #region Groups
            GroupByField.ItemsSource = bl.GroupEmployerByField();
            GroupByEstablishmentYear.ItemsSource = bl.GroupEmployerByEstablishmentYear();
            GroupByBusinessType.ItemsSource = bl.GroupEmployerByBusinessType();
            #endregion
        }
        /// <summary>
        /// The function refresh the items that holds outdated lists 
        /// </summary>
        private void refresh(bool createNew = true)
        {
            #region Groups
            GroupByField.ItemsSource = bl.GroupEmployerByField();
            GroupByEstablishmentYear.ItemsSource = bl.GroupEmployerByEstablishmentYear();
            GroupByBusinessType.ItemsSource = bl.GroupEmployerByBusinessType();
            #endregion

            ChooseEmployerToRemove.ItemsSource = null;
            ChooseEmployerToUpdate.ItemsSource = null;
            ChooseEmployerToRemove.ItemsSource = bl.GetEmployerList();
            ChooseEmployerToUpdate.ItemsSource = bl.GetEmployerList();
            AllEmployers.ItemsSource = bl.GetEmployerList();
            if (createNew)
            {
                AddEmployer = new Employer() { EstablishmentDate = new DateTime(2017, 1, 1) };
                AddTab.DataContext = AddEmployer;
                UpdateEmployer = new Employer() { EstablishmentDate = new DateTime(2017, 1, 1) };
                UpdateTab.DataContext = UpdateEmployer;
            }
        }

        #region clicks in window
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExceptionEmployer(AddEmployer);
                bl.AddEmployer(AddEmployer);

                this.ShowMessageAsync(AddEmployer.ToString() + " was added successfully!", "Your mom is probably very proud of you");
                refresh();
                Tools.SoundMoney.Play();
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
                //    if (ChooseEmployerToRemove.SelectedIndex == -1)
                // throw Exceptions.Employer.NoSelection;
                string messege = ((Employer)ChooseEmployerToRemove.SelectedItem).ToString();
                bl.RemoveEmployer(((Employer)ChooseEmployerToRemove.SelectedItem).ID);
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
                if (ChooseEmployerToUpdate.SelectedIndex == -1)
                    throw Exceptions.Employer.NoSelectionUpdate;
                ExceptionEmployer(UpdateEmployer);
                bl.UpdateEmployer(UpdateEmployer);
                this.ShowMessageAsync(UpdateEmployer.ToString() + " was updated successfully!", "Your mom is probably very proud of you");
                refresh();
                #region OLD_check if entered critical information
                //bool BusinessTypeChangedValue = UpdateEmpBusinessType.IsChecked == boss.IsCompany ? false : true;
                //if (
                //    UpdateEmpID.Text == "" &&
                //    !BusinessTypeChangedValue &&
                //    UpdateEmpFirstName.Text == "" &&
                //    UpdateEmpLastName.Text == "" &&
                //    UpdateEmpCompanyName.Text == "" &&
                //    UpdateEmpPhoneNumber.Text == "" &&
                //    UpdateEmpAddress.Text == "" &&
                //    UpdateEmpChooseField.SelectedIndex == -1
                //    )
                //    throw new Exception("My job is done. Nothing to update here.");
                #endregion
            }
            catch (Exception c)
            {
                Tools.SoundFailed.Play();
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;
            }
        }
        #endregion
        #region animation
        private TranslateParams translate = new TranslateParams()
        {
            From = new Point(0, 0),
            To = new Point(0, 5),
            Duration = 700,
        };
        private OpacityParams opacity_on = new OpacityParams()
        {
            From = 0,
            To = 1,
            Duration = 300,
        };
        private OpacityParams opacity_off = new OpacityParams()
        {
            From = 1,
            To = 0,
            Duration = 700,
        };
        private void Animation(UIElement element, Visibility visibilty, OpacityParams opacity, TranslateParams translate)
        {
            element.Visibility = visibilty;
            Transitionz.SetVisibility(element, visibilty);
            Transitionz.SetTranslate(element, translate);
            Transitionz.SetOpacity(element, opacity);
        }
        private void AddEmpBusinessType_IsCheckedChanged(object sender, EventArgs e)
        {
            if (AddEmpBusinessType.IsChecked == false)
            {
                Animation(AddEmpFirstName_Label, Visibility.Visible, opacity_on, translate);
                Animation(AddEmpFirstName, Visibility.Visible, opacity_on, translate);
                Animation(AddEmpLastName_Label, Visibility.Visible, opacity_on, translate);
                Animation(AddEmpLastName, Visibility.Visible, opacity_on, translate);

                Animation(AddEmpCompanyName_Label, Visibility.Collapsed, opacity_off, translate);
                Animation(AddEmpCompanyName, Visibility.Collapsed, opacity_off, translate);
                AddEmpID_Label.Text = "Employers ID:";
                //AddEmpFirstName.Text = "";
                //AddEmpLastName.Text = "";
            }
            else
            {
                Animation(AddEmpFirstName_Label, Visibility.Collapsed, opacity_off, translate);
                Animation(AddEmpFirstName, Visibility.Collapsed, opacity_off, translate);
                Animation(AddEmpLastName_Label, Visibility.Collapsed, opacity_off, translate);
                Animation(AddEmpLastName, Visibility.Collapsed, opacity_off, translate);

                Animation(AddEmpCompanyName_Label, Visibility.Visible, opacity_on, translate);
                Animation(AddEmpCompanyName, Visibility.Visible, opacity_on, translate);
                AddEmpID_Label.Text = "Company ID:";
                //AddEmpCompanyName.Text = "";


            }
        }
        private void UpdateEmpBusinessType_IsCheckedChanged(object sender, EventArgs e)
        {
            if (UpdateEmpBusinessType.IsChecked == false)
            {
                Animation(UpdateEmpFirstName_Label, Visibility.Visible, opacity_on, translate);
                Animation(UpdateEmpFirstName, Visibility.Visible, opacity_on, translate);
                Animation(UpdateEmpLastName_Label, Visibility.Visible, opacity_on, translate);
                Animation(UpdateEmpLastName, Visibility.Visible, opacity_on, translate);

                Animation(UpdateEmpCompanyName_Label, Visibility.Collapsed, opacity_off, translate);
                Animation(UpdateEmpCompanyName, Visibility.Collapsed, opacity_off, translate);

                //UpdateEmpCompanyName.Text = "";
            }
            else
            {
                Animation(UpdateEmpFirstName_Label, Visibility.Collapsed, opacity_off, translate);
                Animation(UpdateEmpFirstName, Visibility.Collapsed, opacity_off, translate);
                Animation(UpdateEmpLastName_Label, Visibility.Collapsed, opacity_off, translate);
                Animation(UpdateEmpLastName, Visibility.Collapsed, opacity_off, translate);

                Animation(UpdateEmpCompanyName_Label, Visibility.Visible, opacity_on, translate);
                Animation(UpdateEmpCompanyName, Visibility.Visible, opacity_on, translate);

                //UpdateEmpFirstName.Text = "";
                //UpdateEmpLastName.Text = "";

            }

        }
        #endregion

        private void ChooseEmployerToUpdate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseEmployerToUpdate.SelectedIndex != -1)
            {
                UpdateEmployer = (Employer)ChooseEmployerToUpdate.SelectedItem;
                UpdateTab.DataContext = UpdateEmployer;
            }
        }
        private void ExceptionEmployer(Employer emp)
        {
            #region check if entered critical information
            //if (emp.CompanyName == null) emp.CompanyName = "";
            //if (emp.LastName == null) emp.LastName = "";
            //if (emp.FirstName == null) emp.FirstName = "";

            if (
               emp.ID == "" ||
               emp.PhoneNumber == "" ||
               emp.Address == "" ||
               emp.Field == null ||
               emp.EstablishmentDate == null ||
               (emp.IsCompany == true && (emp.CompanyName == "")) ||
               (emp.IsCompany == false && (emp.FirstName == "" || emp.LastName == ""))
               )
                throw Exceptions.AllFields;

            #endregion
            int temp = -1;
            if (!int.TryParse(emp.ID, out temp))
                throw Exceptions.ID_Digits;
            else if (temp < 0)
                throw new Exception("Don't be so negative.");
            else if (temp == 0)
                throw new Exception("That's how much you worth, ZERO!");



            if (!int.TryParse(emp.PhoneNumber, out temp))
                throw Exceptions.Phonenumber_Digits;
            else if (temp <= 0)
                throw new Exception("Don't be so negative.");
        }
        private void Sort_IsCheckedChanged(object sender, EventArgs e)
        {
            #region Groups
            GroupByField.ItemsSource = bl.GroupEmployerByField(Sort.IsEnabled);
            GroupByEstablishmentYear.ItemsSource = bl.GroupEmployerByEstablishmentYear(Sort.IsEnabled);
            GroupByBusinessType.ItemsSource = bl.GroupEmployerByBusinessType(Sort.IsEnabled);
            #endregion
        }
    }
}
