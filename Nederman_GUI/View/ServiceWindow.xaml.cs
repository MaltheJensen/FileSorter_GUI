using Nederman_GUI.Model;
using Nederman_GUI.ModelView;
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

namespace Nederman_GUI.View
{
    /// <summary>
    /// Interaction logic for ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window
    {
        ServiceController serviceController;
        public ServiceWindow()
        {
            InitializeComponent();
            serviceController = new ServiceController();
        }

        private void searchCompanyButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchCompanyBox.Text))
            {
                MessageBox.Show("Indtast venligst et firma at søge efter");
                CleanWindow();
            }
            else 
            {
                var company = serviceController.GetCompany(searchCompanyBox.Text);

                if (company == null)
                {
                    MessageBox.Show($"Kunne ikke finde firmaet {searchCompanyBox.Text}");
                    CleanWindow();
                }
                else 
                {
                    DisplayChosenCompany(company);
                }
            }
        }

        private void DisplayChosenCompany(Company company) 
        {
            InsertCompanyInfo(company);
            InsertJobinstructions(company);
            InsertReports(company);
        }

        private void InsertCompanyInfo(Company company) 
        {
            displayNameBox.Text = company.Name;
            displayAddressBox.Text = company.Address;
            displayContactPersonBox.Text = company.ContactPerson;
            displayZipCodeBox.Text = company.ZipCode;
            displayCityBox.Text = company.City;
            displayCompanyPhone.Text = company.PhoneNumber;
        }
        private void InsertJobinstructions(Company company)
        {
            jobInstructionsListBox.Items.Clear();
            if (company.JobInstrutionsFileName != null)
            {
                foreach (string s in company.JobInstrutionsFileName)
                {
                    jobInstructionsListBox.Items.Add(s);
                }
            }
        }

        private void InsertReports(Company company)
        {
            rapportsListBox.Items.Clear();
            if (company.RapportsFileName != null)
            {
                foreach (string s in company.RapportsFileName)
                {
                    rapportsListBox.Items.Add(s);
                }
            }
        }

        private void CleanWindow() 
        {
            searchCompanyBox.Text = "";
            displayNameBox.Text = "";
            displayAddressBox.Text = "";
            displayContactPersonBox.Text = "";
            displayZipCodeBox.Text = "";
            displayCityBox.Text = "";
            displayCompanyPhone.Text = "";
            jobInstructionsListBox.Items.Clear();
            rapportsListBox.Items.Clear();
        }

        private void jobInstructionsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                serviceController.OpenFile(jobInstructionsListBox.SelectedItem.ToString());
            }
            catch (NullReferenceException)
            {
                if (jobInstructionsListBox.Items.IsEmpty) 
                {
                    MessageBox.Show("No company is selected");
                }

            }
        }

        private void rapportsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                serviceController.OpenFile(rapportsListBox.SelectedItem.ToString());
            }
            catch (NullReferenceException)
            {
                if (rapportsListBox.Items.IsEmpty) 
                {
                    MessageBox.Show("No company is selected");
                }
                
            }
        }

        private void newRepportButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var newReportName = serviceController.SaveNewReport(displayNameBox.Text);

            rapportsListBox.Items.Add(newReportName);
        }
    }
}
