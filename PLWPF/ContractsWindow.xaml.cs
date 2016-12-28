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
    /// Interaction logic for ContractsWindow.xaml
    /// </summary>
    public partial class ContractsWindow : MetroWindow
    {
        BL.BL_imp bl = new BL_imp();

        public ContractsWindow()
        {
            InitializeComponent();
            listView.ItemsSource = bl.GroupContractBySpec();
            // SimpleGrid.ItemsSource = bl.GroupContractBySpec();
            ChooseEmployeeForDeal.ItemsSource = bl.GetEmployeeList();
            ChooseEmployerForDeal.ItemsSource = bl.GetEmployerList();
            ChooseContractToRemove.ItemsSource = bl.GetContractList();
            ChooseContractToUpdate.ItemsSource = bl.GetContractList();

        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Contract c = new Contract();
                if (ChooseEmployeeForDeal.SelectedIndex == -1 || ChooseEmployerForDeal.SelectedIndex == -1 || AddContractEndingDate.SelectedDate == null || AddContractStartingDate.SelectedDate == null || AddGrossSalary.Text == "")
                    throw new Exception("All fields must be filled");

                c.ContractID = 0;
                c.EmployeeID = ((Employee)ChooseEmployeeForDeal.SelectedItem).ID;
                c.EmployerID = ((Employer)ChooseEmployerForDeal.SelectedItem).ID;
                c.EndDate = (DateTime)AddContractEndingDate.SelectedDate;
                c.StartDate = (DateTime)AddContractStartingDate.SelectedDate;


                try
                {
                    c.GrossSalary = double.Parse(AddGrossSalary.Text);

                }
                catch (Exception)
                {
                    throw new Exception("Salary must have a numeric value");
                }
                try
                {
                    c.WorkingHours = double.Parse(AddWorkingHours.Text);
                }
                catch (Exception)
                {
                    throw new Exception("The amount of working hours must have a numeric value");
                }
                if (double.Parse(AddGrossSalary.Text) < 0)
                    throw new Exception("Salary can't be negative, nice try");
                if (double.Parse(AddWorkingHours.Text) < 0)
                    throw new Exception("Testing the system uh? working hours can't be negative");

                c.IsInterviewed = (bool)AddWasInterviewed.IsChecked;
                c.IsSigned = (bool)AddIsContractSigned.IsChecked;
                c.NetSalary = bl.CalcWorkerNetSalary(c.EmployeeID, c.EmployerID, c);

                bl.AddContract(c);

                this.ShowMessageAsync("New contract was added successfully!", "Pleasure to do business with you.");


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
                if (ChooseContractToRemove.SelectedIndex == -1)
                    throw new Exception("What exactly do you want me too remove, uh? moron...");
                string messege = "Contract number " + ((Contract)ChooseContractToRemove.SelectedItem).ContractID.ToString();
                bl.RemoveContract(((Contract)ChooseContractToRemove.SelectedItem).ContractID);
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
                Contract c = new Contract();

                if (ChooseContractToUpdate.SelectedIndex == -1)
                    throw new Exception("I removed nothing, just as you requested");
                c = (Contract)ChooseContractToUpdate.SelectedItem;
                if (UpdatedContractEndingDate.SelectedDate != null)
                    c.EndDate = (DateTime)UpdatedContractEndingDate.SelectedDate;

                c.IsInterviewed = (bool)UpdateIsSinged.IsChecked;
                c.IsSigned = (bool)UpdateWasInterviewed.IsChecked;

                if (UpdateWorkingHours.Text != "")
                {
                    try
                    {
                        c.WorkingHours = double.Parse(UpdateWorkingHours.Text);
                    }
                    catch (Exception)
                    {
                        throw new Exception("The amount of working hours must have a numeric value");
                    }
                }

                if (UpdateGrossSalary.Text != "")
                {
                    try
                    {
                        c.GrossSalary = double.Parse(UpdateGrossSalary.Text);

                    }
                    catch (Exception)
                    {
                        throw new Exception("Salary must have a numeric value");
                    }
                    c.NetSalary = bl.CalcWorkerNetSalary(c.EmployeeID, c.EmployerID, c);

                }

                bl.UpdateContract(c);
                string messege = "Contract number " + ((Contract)ChooseContractToUpdate.SelectedItem).ContractID.ToString();
                this.ShowMessageAsync(messege + " was updated successfuly", "Nice updating skills you got there");


            }
            catch (Exception c)
            {
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;
            }

        }



    }




}
