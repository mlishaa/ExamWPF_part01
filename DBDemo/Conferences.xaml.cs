using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace DBDemo
{
    /// <summary>
    /// Interaction logic for Conferences.xaml
    /// </summary>
    public partial class Conferences : Window
    {
        //string to tell us where is the database
        private string dbConnection = ConfigurationManager.ConnectionStrings["DBDemo.Properties.Settings.ConfrencesDBConnectionString"].ConnectionString;
         
        private Conference newConference;

        public Conferences()
        {
            InitializeComponent();
            this.SizeToContent = SizeToContent.Height;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            PopulateConferences();
            newConference = new Conference();
        }

        private void PopulateConferences()
        {
            DataSet ds = new DataSet();
            try
            {
                string command = @"SELECT * from Conferences";
               
                using (SqlConnection con = new SqlConnection(dbConnection))
                {
                    SqlCommand cmd = new SqlCommand(command, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds, "Cenferences");

                    cmbLoadVisitorForm.ItemsSource = ds.Tables["Cenferences"].Rows;
                    cmbLoadVisitorForm.SelectedValuePath = ".[Id]";
                    cmbLoadVisitorForm.DisplayMemberPath = ".[Name]";

                    cmbLoadVisitorForm.Items.Refresh();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnAddConference_Click(object sender, RoutedEventArgs e)
        {
            if (CheckAllInputConf())
          
                try
                {
                    using (SqlConnection con = new SqlConnection(dbConnection))
                    {
                        string command = @"INSERT INTO Conferences (Name,ContactNumber,confDate) VALUES (" +
                            @"'" + newConference.Name + @"','" + newConference.ContactNum + @"','" + newConference.ConferenceDate + @"')";
              
                        SqlCommand cmd = new SqlCommand(command, con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Succussfuly add a new conference","new conference",
                            MessageBoxButton.OK,MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            ////Repopulate my conferences list
            ///
            
                PopulateConferences();
                ClearForm();
            
        }
        // i will use it later 
        //private void CmbLoadVisitorForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //Load All Visitors form (MainWindow)
        //    //number 1 in the argument list has to be changed later.
        //    string ConferenceName = (cmbLoadVisitorForm.SelectedItem as DataRow)["Name"].ToString();
        //    MainWindow allVisitorsForm = new MainWindow(int.Parse(cmbLoadVisitorForm.SelectedValue.ToString()), ConferenceName);
        //    //Showdialog
        //    allVisitorsForm.ShowDialog();
        //}

        private void BtnLoadConf_Click(object sender, RoutedEventArgs e)
        {
            string ConferenceName = (cmbLoadVisitorForm.SelectedItem as DataRow)["Name"].ToString();
            MainWindow allVisitorsForm = new MainWindow(int.Parse(cmbLoadVisitorForm.SelectedValue.ToString()), ConferenceName);
            //Showdialog
            allVisitorsForm.ShowDialog();
        }


        ///
        private void ClearForm()
        {
            txtConferenceName.Clear();
            txtContactNumber.Clear();
            dpConfDate.SelectedDate = null;
        }


        private bool CheckAllInputConf()
        {
            StringBuilder msg = new StringBuilder();
            Conference c = new Conference();
            //Name
            if (string.IsNullOrEmpty(txtConferenceName.Text))
                msg.AppendLine("Name is a required field");
            else
                c.Name = txtConferenceName.Text;

            //Major
            if (string.IsNullOrEmpty(txtContactNumber.Text))
                msg.AppendLine("Contact number is a required field");
            else
            {
                try
                {
                    c.ContactNum = txtContactNumber.Text;
                }
                catch (ArgumentException ex)
                {
                    msg.AppendLine("Please enter phone number as 000-000-0000");
                
                   
                }
            }

            //Country

            //Check in date
            if (!dpConfDate.SelectedDate.HasValue)
                msg.AppendLine("Date is not selected");
            else
                c.ConferenceDate = dpConfDate.SelectedDate.Value;

            if (!string.IsNullOrEmpty(msg.ToString()))
            {
                MessageBox.Show(msg.ToString(), "Form Missing Data", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                newConference = c;
                return true;
            }
          
        }

        private void BtnDeleteConf_Click(object sender, RoutedEventArgs e)
        {
            int confID = int.Parse(cmbLoadVisitorForm.SelectedValue.ToString());
            try { 
            using (SqlConnection con = new SqlConnection(dbConnection))
            {
                string command = @"delete from Visitors where ConferenceID= " + confID + @"";
                    //string command = @"delete from Conferences where  Id= " + confID + @"";

                    SqlCommand cmd = new SqlCommand(command, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
                
            }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            cmbLoadVisitorForm.ItemsSource = null;
            cmbLoadVisitorForm.Items.Clear();
            cmbLoadVisitorForm.Items.Refresh();
            // delete the conference
            try { 
            using (SqlConnection con = new SqlConnection(dbConnection))
            {
                string command = @"delete from Conferences where  Id= " + confID + @"";


                SqlCommand cmd = new SqlCommand(command, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                    cmbLoadVisitorForm.Items.Clear();
                 PopulateConferences();
                  cmbLoadVisitorForm.Items.Refresh();
                }

             }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }


        }

        
    }
}
