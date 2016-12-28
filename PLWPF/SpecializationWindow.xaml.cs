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
    /// Interaction logic for SpecializationWindow.xaml
    /// </summary>
    public partial class SpecializationWindow : MetroWindow
    {
        BL.BL_imp bl = new BL_imp();

        public List<SpecializationField> field = new List<SpecializationField>();

        public SpecializationWindow()
        {
            InitializeComponent();

            field = Enum.GetValues(typeof(SpecializationField)).Cast<SpecializationField>().ToList();
            ChooseFieldAdd.ItemsSource = field;
            ChooseFieldUpdate.ItemsSource = field;

            ChooseSpecializationToRemove.ItemsSource = bl.GetSpecializationList();
            ChooseSpecializationToUpdate.ItemsSource = bl.GetSpecializationList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AddSpecializationName.Text == "" || AddMinSalary.Text == "" || AddMaxSalary.Text == "" || ChooseFieldAdd.SelectedIndex == -1)
                    throw new Exception("All fields must be filled");

                Specialization sp = new Specialization();
                sp.Field = (SpecializationField)ChooseFieldAdd.SelectedItem;
                try
                {
                    sp.MaxSalary = double.Parse(AddMaxSalary.Text);
                }
                catch (Exception)
                {
                    throw new Exception("Salary must have a numeric value");
                }

                try
                {
                    sp.MinSalary = double.Parse(AddMinSalary.Text);
                }
                catch (Exception)
                {
                    throw new Exception("Salary must have a numeric value");
                }
                if (sp.MinSalary < 0 || sp.MaxSalary < 0)
                    throw new Exception("Salary can't be negative");
                sp.SpecializationID = 0;
                sp.SpecializationName = AddSpecializationName.Text;
                bl.AddSpecialization(sp);
                this.ShowMessageAsync("New specialization was added successfully!", "Who cares though...");

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
                Specialization sp = new Specialization();
                if (ChooseSpecializationToRemove.SelectedIndex == -1)
                    throw new Exception("We both know you didn't choose a specialization just to test my patience ");
                sp = (Specialization)ChooseSpecializationToRemove.SelectedItem;
                bl.RemoveSpecialization(sp.SpecializationID);
                this.ShowMessageAsync("Specialization was removed successfully!", "No one liked it anyway");

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
                if (ChooseSpecializationToUpdate.SelectedIndex == -1)
                    throw new Exception("We both know you didn't choose a specialization just to test my patience ");

                Specialization sp = (Specialization)((Specialization)ChooseSpecializationToUpdate.SelectedItem).Clone();

                if (
                    UpdateSpecializationName.Text == "" &&
                    ChooseFieldUpdate.SelectedIndex == -1 &&
                        UpdateMaxSalary.Text == "" &&
                    UpdateMinSalary.Text == ""
                    )
                    throw new Exception("Task failed successfully, you didn't change anything");


                if (UpdateSpecializationName.Text != "")
                    sp.SpecializationName = UpdateSpecializationName.Text;

                if (ChooseFieldUpdate.SelectedIndex != -1)
                    sp.Field = (SpecializationField)ChooseFieldUpdate.SelectedItem;

                if (UpdateMaxSalary.Text != "")
                {
                    try
                    {
                        sp.MaxSalary = double.Parse(UpdateMaxSalary.Text);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Salary must have a numeric value");
                    }
                }
                if (UpdateMinSalary.Text != "")
                {
                    try
                    {
                        sp.MinSalary = double.Parse(UpdateMinSalary.Text);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Salary must have a numeric value");
                    }
                }
                if (sp.MinSalary < 0 || sp.MaxSalary < 0)
                    throw new Exception("Salary can't be negative");
                bl.UpdateSpecialization(sp);
                this.ShowMessageAsync("Specialization was updated successfully!", "Horray!!!");

            }
            catch (Exception c)
            {
                ErrorFlyout.Header = c.Message;
                ErrorFlyout.IsOpen = true;

            }
        }
    }
}
