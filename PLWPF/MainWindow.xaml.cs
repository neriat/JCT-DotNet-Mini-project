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
using BL;
using BE;
using System.Threading;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using MahApps.Metro;
using SciChart.Wpf.UI.Transitionz;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow


    {
        public MainWindow()
        {
            InitializeComponent();

            // set the Red accent and dark theme only to the current window
            ThemeManager.ChangeAppStyle(this,
                                        ThemeManager.GetAccent("Blue"),
                                        ThemeManager.GetAppTheme("BaseLight"));

            IBL bl = FactoryBL.GetBL();

            #region examples
            //#region ExampleSpecialization
            //Specialization sp = new Specialization();
            //sp.Field = SpecializationField.Data_Base;
            //sp.MaxSalary = 100000;
            //sp.MinSalary = 0;
            //sp.SpecializationID = 10000000;
            //sp.SpecializationName = "Something";
            //bl.AddSpecialization(sp);
            //#endregion
            //#region ExampleEmployee
            //Employee e = new Employee();
            //e.Address = "asd";
            //BankAccount acc = new BankAccount();
            //acc.AccountNumber = "123";
            //acc.branch = bl.GetBranchList()[0];
            //e.BankAccount = acc;
            //e.BirthDate = new DateTime(1970, 4, 8);
            //e.DealsNum = 0;
            //e.Degree = enumDegree.BA;
            //e.FirstName = "Shoham";
            //e.ID = "208720623";
            //e.LastName = "Jacobsen";
            //e.PhoneNumber = "1234";
            //e.region = District.Central;
            //e.SpecialityID = "10000000";
            //e.Veteran = true;
            //bl.AddEmployee(e);
            //#endregion
            //#region ExampleEmployer
            //Employer emp = new Employer();
            //emp.Address = "asd";
            //acc.AccountNumber = "123";
            //acc.branch = bl.GetBranchList()[0];
            //emp.ContractsNum = 0;
            //emp.FirstName = "Big";
            //emp.ID = "208720624";
            //emp.LastName = "Boss";
            //emp.PhoneNumber = "1234";
            //emp.IsCompany = false;
            //emp.Field = SpecializationField.Data_Base;
            //emp.EstablishmentDate = new DateTime(1990, 2, 3);
            //bl.AddEmployer(emp);
            //#endregion
            //#region ExampleContract
            //Contract c = new Contract();
            //c.ContractID = 0;
            //c.IsInterviewed = true;
            //c.IsSigned = true;
            //c.EmployeeID = "208720623";
            //c.EmployerID = "208720624";
            //c.EndDate = new DateTime(2020, 3, 2);
            //c.GrossSalary = 400;
            //c.NetSalary = 300;
            //c.StartDate = new DateTime(1990, 3, 2);
            //c.WorkingHours = 3;
            //bl.AddContract(c);
            //#endregion
            #endregion
        }

        #region window clicks
        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.SoundHover.Play();
            EmployeesWindow window = new EmployeesWindow();
            window.ShowDialog();
        }
        private void SpecializationButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.SoundHover.Play();
            SpecializationWindow window = new SpecializationWindow();
            window.ShowDialog();
        }
        private void EmployersButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.SoundHover.Play();
            EmployersWindow window = new EmployersWindow();
            window.ShowDialog();
        }
        private void ContractsButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.SoundHover.Play();
            ContractsWindow window = new ContractsWindow();
            window.ShowDialog();
        }
        private void SendMail_Click(object sender, RoutedEventArgs e)
        {
            MailProgressRing.Visibility = System.Windows.Visibility.Visible;

            string body = new TextRange(MailBody.Document.ContentStart, MailBody.Document.ContentEnd).Text;
            string name = MailName.Text;
            MailBody.Document.Blocks.Clear();
            MailName.Text = "";

            try
            {
                if (!Tools.CheckForInternetConnection()) throw new Exception("No Internet Conection. Try buying a router");
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Tools.mail(name, body);
                }).Start();
                Information_Click(sender, e);
                this.ShowMessageAsync("E-Mail was sent successfully!", "Thanks for your help");
            }
            catch (Exception c)
            {
                if (c != null) ErrorFlyout.Header = c.Message;
                else ErrorFlyout.Header = ("\tE-Mail wasn't sent!\n\tYour fault, your internet fault. I don't care");
                ErrorFlyout.IsOpen = true;
            }
            MailProgressRing.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void Information_Click(object sender, RoutedEventArgs e)
        {
            if (InformationFlyout.IsOpen == false) InformationFlyout.IsOpen = true;
            else InformationFlyout.IsOpen = false;
        }

        #endregion
        #region Tiles font size
        /// <summary>
        /// Make the font size of the tile bigger when mouse enters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_MouseEnter(object sender, MouseEventArgs e)
        {
            Tile thisTile = (Tile)sender;
            thisTile.TitleFontSize += 3;
        }
        /// <summary>
        /// Make the font size of the tile smaller when mouse leaves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tile_MouseLeave(object sender, MouseEventArgs e)
        {
            Tile thisTile = (Tile)sender;
            thisTile.TitleFontSize -= 3;
        }
        #endregion



    }

}
