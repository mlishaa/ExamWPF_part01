using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for VisitorsWindow.xaml
    /// </summary>
    public partial class VisitorsWindow : Window
    {
        public Visitor VisitorInfo { get; }
        private bool isModification = false;
        //private Visitor originalVisitorInfo;
        
        //New Visitor Scenario
        public VisitorsWindow(DataRowCollection countries)
        {
            InitializeComponent();
            SetupWindow(countries);
            VisitorInfo = new Visitor();
        }

        //Modify Visitor Scenario
        public VisitorsWindow(DataRowCollection countries, Visitor existingVisitor)
        {
            InitializeComponent();
            SetupWindow(countries);
            VisitorInfo = existingVisitor;
            
            //Change the title, change the button content
            this.Title = "Modify Visitor Info";
            btnSave.Content = "Modify";
            btnClear.IsEnabled = false;
            LoadVisitorInfo();
            isModification = true;
        }

        private void LoadVisitorInfo()
        {
            txtName.Text = VisitorInfo.FullName;
            txtMajor.Text = VisitorInfo.Major;
            cmbCountries.SelectedValue = VisitorInfo.Country;
            chkbSpeaker.IsChecked = VisitorInfo.IsSpeaker;
            dpCheckIn.SelectedDate = VisitorInfo.CheckInDate;

            if (VisitorInfo.VisitorStatus == Status.Teacher)
                rbtnTeacher.IsChecked = true;
            else if (VisitorInfo.VisitorStatus == Status.Student)
                rbtnStudent.IsChecked = true;
            else
                rbtnProf.IsChecked = true;
        }

        private void SetupWindow(DataRowCollection countries)
        {
            this.SizeToContent = SizeToContent.Height;
            this.SizeToContent = SizeToContent.Width;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //Populating countries combo box
            cmbCountries.ItemsSource = countries;
            cmbCountries.SelectedValuePath = ".[Name]";
            cmbCountries.DisplayMemberPath = ".[Name]";
            cmbCountries.Items.Refresh();
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtMajor.Clear();
            cmbCountries.SelectedIndex = -1;
            rbtnProf.IsChecked = false;
            rbtnTeacher.IsChecked = false;
            rbtnStudent.IsChecked = false;
            chkbSpeaker.IsChecked = false;
            dpCheckIn.SelectedDate = null;
        }

        private bool CheckAllInput()
        {
            StringBuilder msg = new StringBuilder();

            //Name
            if (string.IsNullOrEmpty(txtName.Text))
                msg.AppendLine("Name is a required field");

            //Major
            if (string.IsNullOrEmpty(txtMajor.Text))
                msg.AppendLine("Major is a required field");

            //Country
            if (cmbCountries.SelectedValue == null)
                msg.AppendLine("No Country Selected");

            //Status
            if (!(rbtnTeacher.IsChecked.Value || rbtnStudent.IsChecked.Value || rbtnProf.IsChecked.Value))
                msg.AppendLine("Status is not chosen");

            //Check in date
            if (!dpCheckIn.SelectedDate.HasValue)
                msg.AppendLine("Date is not selected");

            if (!string.IsNullOrEmpty(msg.ToString()))
            {
                MessageBox.Show(msg.ToString(), "Form Missing Data", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            bool update = true;
            if (isModification)
                update = CompareVisitorInfo();

            if(CheckAllInput() && update)
            {
                VisitorInfo.FullName = txtName.Text;
                VisitorInfo.Major = txtMajor.Text;
                VisitorInfo.Country = cmbCountries.SelectedValue.ToString();
                VisitorInfo.IsSpeaker = chkbSpeaker.IsChecked.Value;
                VisitorInfo.CheckInDate = dpCheckIn.SelectedDate.Value;
                VisitorInfo.VisitorStatus = rbtnTeacher.IsChecked.Value ? Status.Teacher :
                                rbtnStudent.IsChecked.Value ? Status.Student : Status.Proffessional;

                ClearForm();
                this.DialogResult = true;
                
            }

            this.Close();
        }
        //Will compare the form with the object// if there is not change it will close the form.
        private bool CompareVisitorInfo()
        {
            if (txtName.Text != VisitorInfo.FullName)
                return true;
            if (txtMajor.Text != VisitorInfo.Major)
                return true;
            if (cmbCountries.SelectedValue.ToString() != VisitorInfo.Country)
                return true;
            if (chkbSpeaker.IsChecked != VisitorInfo.IsSpeaker)
                return true;
            if (dpCheckIn.SelectedDate != VisitorInfo.CheckInDate)
                return true;
            string currentStatus = rbtnTeacher.IsChecked.Value ? Status.Teacher.ToString() :
                                   rbtnStudent.IsChecked.Value ? Status.Student.ToString() : Status.Proffessional.ToString();
            if (currentStatus != VisitorInfo.VisitorStatus.ToString())
                return true;

            return false;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
