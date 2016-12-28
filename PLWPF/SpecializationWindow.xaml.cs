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
//            ChooseFieldRemove.ItemsSource = field;
            ChooseFieldUpdate.ItemsSource = field;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.ShowMessageAsync("New specialization was added successfully!", "Not really, though, just testing things out.");
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            this.ShowMessageAsync("Specialization was removed successfully!", "Not really, though, just testing things out.");
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            this.ShowMessageAsync("Specialization was updated successfully!", "Not really, though, just testing things out.");
        }
    }
}
