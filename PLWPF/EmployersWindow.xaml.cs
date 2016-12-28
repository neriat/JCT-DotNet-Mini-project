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

        public List<enumDegree> degree = new List<enumDegree>();
        public List<District> region = new List<District>();
        public List<Bank> Banks = new List<Bank>();
        public List<string> EmployeesFullData = new List<string>();
        public List<Specialization> specializations = new List<Specialization>();




        public EmployersWindow()
        {
            InitializeComponent();



            ChooseFieldAdd.ItemsSource = Enum.GetValues(typeof(SpecializationField)).Cast<SpecializationField>().ToList();
            ChooseUpdatedField.ItemsSource = Enum.GetValues(typeof(SpecializationField)).Cast<SpecializationField>().ToList();
            ChooseEmployerToRemove.ItemsSource = bl.GetEmployerList();
            ChooseEmployerToUpdate.ItemsSource = bl.GetEmployerList();





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
            Employee worker;
            try { worker = bl.GetEmployeeList()[ChooseEmployerToUpdate.SelectedIndex]; }
            catch { worker = null; }
            if (worker != null)
                this.ShowMessageAsync(worker.FirstName + " " + worker.LastName + " was updated successfully!", "Not really, though, just testing things out.");
            else
            {
                ErrorFlyout.Header = "Who do you expect me to update, uh? moron...";
                ErrorFlyout.IsOpen = true;
            }
        }
    }
}
