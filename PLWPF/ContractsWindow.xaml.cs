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

            ChooseEmployeeForDeal.ItemsSource = bl.GetEmployeeList();
            ChooseEmployerForDeal.ItemsSource = bl.GetEmployerList();
            ChooseContractToRemove.ItemsSource = bl.GetContractList();
            ChooseContractToUpdate.ItemsSource = bl.GetContractList();
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ErrorFlyout.Header = "Couldn't add employee, probably your mistake, fix it...";
            ErrorFlyout.IsOpen = true;
            //  this.ShowMessageAsync("New employee was added successfully!", "Not really, though, just testing things out.");
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            ErrorFlyout.Header = "Not going to delete this employee, deal with it...";
            ErrorFlyout.IsOpen = true;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Contract worker;
            try { worker = bl.GetContractList()[ChooseContractToUpdate.SelectedIndex]; }
            catch { worker = null; }
            if (worker != null)
                this.ShowMessageAsync("Contract" + " " + "blablala" + " was updated successfully!", "Not really, though, just testing things out.");
            else
            {
                ErrorFlyout.Header = "Who do you expect me to update, uh? moron...";
                ErrorFlyout.IsOpen = true;
            }
        }



    }




}
