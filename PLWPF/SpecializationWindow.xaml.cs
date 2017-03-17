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
        IBL bl = FactoryBL.GetBL();
        private Specialization AddSpesialization = new Specialization();
        private Specialization UpdateSpesialization = new Specialization();


        public SpecializationWindow()
        {
            InitializeComponent();
            AddTab.DataContext = AddSpesialization;

            #region ItemsSources
            ChooseFieldAdd.ItemsSource = Enum.GetValues(typeof(SpecializationField)).Cast<SpecializationField>().ToList();
            ChooseFieldUpdate.ItemsSource = Enum.GetValues(typeof(SpecializationField)).Cast<SpecializationField>().ToList();

            ChooseSpecializationToRemove.ItemsSource = bl.GetSpecializationList();
            ChooseSpecializationToUpdate.ItemsSource = bl.GetSpecializationList();
            AllSpecializations.ItemsSource = bl.GetSpecializationList();
            #endregion
            #region Groups
            GroupByField.ItemsSource = bl.GroupSpecializationByField();
            GroupByMaxSalary.ItemsSource = bl.GroupSpecializationByMaxSalary();
            GroupByMinSalary.ItemsSource = bl.GroupSpecializationByMinSalary();
            #endregion

        }
        /// <summary>
        /// The function refresh the items that holds outdated lists 
        /// </summary>
        private void refresh(bool createNew = true)
        {
            #region Groups
            GroupByField.ItemsSource = bl.GroupSpecializationByField();
            GroupByMaxSalary.ItemsSource = bl.GroupSpecializationByMaxSalary();
            GroupByMinSalary.ItemsSource = bl.GroupSpecializationByMinSalary();
            #endregion
            AllSpecializations.ItemsSource = bl.GetSpecializationList();

            ChooseSpecializationToRemove.ItemsSource = null;
            ChooseSpecializationToUpdate.ItemsSource = null;
            ChooseSpecializationToRemove.ItemsSource = bl.GetSpecializationList();
            ChooseSpecializationToUpdate.ItemsSource = bl.GetSpecializationList();
            if (createNew)
            {
                AddSpesialization = new Specialization();
                AddTab.DataContext = AddSpesialization;
                UpdateSpesialization = new Specialization();
                UpdateTab.DataContext = UpdateSpesialization;
            }
        }

        #region window clicks
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddSpesialization.SpecializationID = 0;
                if (AddSpesialization.SpecializationName == "" || AddSpesialization.SpecializationName == null)
                    throw new Exception("Specialization must have name!");
                #region space killer

                #endregion
                bl.AddSpecialization(AddSpesialization);
                this.ShowMessageAsync("New specialization was added successfully!", "Who cares though...");
                refresh();
                Tools.SoundSuccsses.Play();
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
                Specialization sp = new Specialization();
                if (ChooseSpecializationToRemove.SelectedIndex == -1)
                    throw new Exception("We both know you didn't choose a specialization just to test my patience ");
                sp = (Specialization)ChooseSpecializationToRemove.SelectedItem;
                bl.RemoveSpecialization(sp.SpecializationID);
                this.ShowMessageAsync("Specialization was removed successfully!", "No one liked it anyway");
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
                if (ChooseSpecializationToUpdate.SelectedIndex == -1)
                    throw new Exception("We both know you didn't choose a specialization just to test my patience ");
                if (UpdateSpesialization.SpecializationName == "")
                    throw new Exception("Specialization must have name!");
                bl.UpdateSpecialization(UpdateSpesialization);
                this.ShowMessageAsync("Specialization was updated successfully!", "Horray!!!");
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

        private void ChooseSpecializationToUpdate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseSpecializationToUpdate.SelectedIndex != -1)
            {
                UpdateSpesialization = (Specialization)ChooseSpecializationToUpdate.SelectedItem;
                UpdateTab.DataContext = UpdateSpesialization;
            }
        }

        private void Sort_IsCheckedChanged(object sender, EventArgs e)
        {

            GroupByField.ItemsSource = bl.GroupSpecializationByField(Sort.IsEnabled);
            GroupByMaxSalary.ItemsSource = bl.GroupSpecializationByMaxSalary(Sort.IsEnabled);
            GroupByMinSalary.ItemsSource = bl.GroupSpecializationByMinSalary(Sort.IsEnabled);

        }
    }
}
