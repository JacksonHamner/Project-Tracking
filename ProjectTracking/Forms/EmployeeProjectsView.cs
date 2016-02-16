using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectTracking
{
    public partial class EmployeeProjectsView : Form
    {
        // Initialize Components
        public EmployeeProjectsView()
        { InitializeComponent(); }
        
        //Create MDIparent Mainform
        private MainForm thisParent
        { get { return (MainForm)MdiParent; } }

        // location variable to hold current row location
        private int _Location;

        // Create isntance of the Dataset from the Mainform
        private ProjectTrackingDataSet thisProjectTracking
        { get { return thisParent.Tracking; } }

        //Form Load Method
        private void EmployeeProjectsView_Load(object sender, EventArgs e)
        {
            //Update Status Label
            thisParent.Status = "Viewing Employee's Projects";
            //if there are rows in the Employees Table
            if (thisProjectTracking.Employees.Rows.Count > 0)
            {
                //Set Location to the first row
                _Location = 0;
                //Load data from the current location into the form's controls
                ShowRow(_Location);
                // disable previous/first buttons because we're on the first row
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                // if there are rows after this row, enable Next and Last buttons
                btnNext.Enabled = (_Location < thisProjectTracking.Employees.Rows.Count - 1);
                btnLast.Enabled = (_Location < thisProjectTracking.Employees.Rows.Count - 1);
            }
            //if there are no rows
            else
            {
                //disable all controls (no rows to navigate)
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                btnFirst.Enabled = false;
            }
            // Create instance of a datarow from the employees table at the first row
            DataRow dr = thisProjectTracking.Employees.Rows[_Location];
            // Add collums to the List View
            lvProjects.Columns.Add("Project", 100);
            lvProjects.Columns.Add("Task", 100);
            lvProjects.Columns.Add("Date", 100);
            lvProjects.Columns.Add("Hours", 100);
            // Set it to Details View
            lvProjects.View = View.Details;
            // Fill the Listview with information from the DataRow
            fillListView(dr);
        }

        // Set controls to the data at a specific location
        private void ShowRow(int location)
        {
            DataRow dr = thisProjectTracking.Employees.Rows[location];
            txtEmpID.Text = dr[0].ToString();
            txtFirstName.Text = dr[1].ToString();
            txtLastName.Text = dr[2].ToString();
            txtTitle.Text = dr[3].ToString();
            fillListView(dr);
        }
        // Get data from the controls and send them to the DataRow
        private void getRow(int location)
        {
            DataRow dr = thisProjectTracking.Employees.Rows[location];
            dr[0] = txtEmpID.Text;
            dr[1] = txtFirstName.Text;
            dr[2] = txtLastName.Text;
            dr[3] = txtTitle.Text;
        }

        //Navigate to to the first row, Fill controls with new data
        private void btnFirst_Click(object sender, EventArgs e)
        {
            _Location = 0;
            ShowRow(_Location);
            btnFirst.Enabled = false;
            btnPrevious.Enabled = false;
            if (thisProjectTracking.Employees.Rows.Count > 1)
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }
        }

        //Navigate to the previous Row, Fill controls with new data
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            getRow(_Location);
            _Location--;
            ShowRow(_Location);
            if (_Location == 0)
            {
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
            }
            btnNext.Enabled = true;
            btnLast.Enabled = true;
        }

        //Navigate to the next Row, Fill controls with new data
        private void btnNext_Click(object sender, EventArgs e)
        {
            getRow(_Location);
            _Location++;
            ShowRow(_Location);
            if (_Location + 1 == thisProjectTracking.Projects.Rows.Count)
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;
            }
            btnPrevious.Enabled = true;
            btnFirst.Enabled = true;
        }

        //Navigate to the last Row, Fill controls with new data
        private void btnLast_Click(object sender, EventArgs e)
        {
            _Location = thisProjectTracking.Projects.Rows.Count - 1;
            ShowRow(_Location);
            btnLast.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = true;
            btnFirst.Enabled = true;
        }

        //Fill List View with data from a specific datarow
        private void fillListView(DataRow employee)
        {
            lvProjects.Items.Clear();
            foreach (DataRow TaskEmployeeRows in thisProjectTracking.TaskEmployees.Rows)
            {
                if (TaskEmployeeRows[1].ToString() == employee[0].ToString())
                {
                    foreach (DataRow TaskRows in thisProjectTracking.ProjectTasks.Rows)
                    {
                        if (TaskEmployeeRows[0].ToString() == TaskRows[0].ToString())
                        {
                            foreach (DataRow projectRow in thisProjectTracking.Projects.Rows)
                            {
                                if (projectRow[0].ToString() == TaskRows[1].ToString())
                                {
                                    ListViewItem itmProjEmp = new ListViewItem(projectRow[1].ToString());
                                    itmProjEmp.SubItems.Add(TaskRows[2].ToString());
                                    itmProjEmp.SubItems.Add(TaskEmployeeRows[2].ToString());
                                    itmProjEmp.SubItems.Add(TaskEmployeeRows[3].ToString());
                                    lvProjects.Items.Add(itmProjEmp);
                                }
                            }
                        }
                    }
                }
            }
        }

        //close form
        private void btnClose_Click(object sender, EventArgs e)
        {
            thisParent.Status = "View Form Closed";
            this.Close();
            
        }
    }
}
