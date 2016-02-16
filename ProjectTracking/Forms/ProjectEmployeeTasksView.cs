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
    public partial class ProjectEmployeeTasksView : Form
    {
        //constructor
        public ProjectEmployeeTasksView()
        { InitializeComponent(); }
        
        //Set the Parent form to the Main Form
        private MainForm thisParent
        { get { return (MainForm)MdiParent; } }

        //Variable to hold the current location
        private int _Location;

        //Project Tracking DataSet Property
        private ProjectTrackingDataSet thisProjectTracking
        { get { return thisParent.Tracking; } }

        // Load the form
        private void ProjectEmployeeTasksView_Load(object sender, EventArgs e)
        {   
            //update Status Label
            thisParent.Status = "Viewing: Project Employees";
            //if there are rows
            if (thisProjectTracking.Projects.Rows.Count > 0)
            {
                //set location to the first row
                _Location = 0;
                //show row at location
                ShowRow(_Location);
                //Enable controls
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                btnNext.Enabled = (_Location < thisProjectTracking.Projects.Rows.Count - 1);
                btnLast.Enabled = (_Location < thisProjectTracking.Projects.Rows.Count - 1);
            }
                //no rows
            else
            {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                btnLast.Enabled = false;
                btnFirst.Enabled = false;
            }
            //create instance of the datarow at the current location
            DataRow dr = thisProjectTracking.Projects.Rows[_Location];
            //add columns to the ListView
            lvEmployeeDetails.Columns.Add("Employee Name", 100);
            lvEmployeeDetails.Columns.Add("Task Name", 100);
            lvEmployeeDetails.Columns.Add("Date", 100);
            lvEmployeeDetails.Columns.Add("Hours", 100);
            //Set view to Details
            lvEmployeeDetails.View = View.Details;
            //Fill the Listview with info from the datarow
            fillListView(dr);
        }

        //navigate to the last row, set controls
        private void btnLast_Click(object sender, EventArgs e)
        {
            _Location = thisProjectTracking.Projects.Rows.Count - 1;
            ShowRow(_Location);
            btnLast.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = true;
            btnFirst.Enabled = true;
        }

        //navigate to the next row, update Controls
        private void btnNext_Click(object sender, EventArgs e)
        {
            getRow(_Location);
            _Location++;
            ShowRow(_Location);

            if (_Location + 1 == thisProjectTracking.Employees.Rows.Count)
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;

            }
            btnPrevious.Enabled = true;
            btnFirst.Enabled = true;
        }

        //navigate to the previous row, update controls
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

        // navigate to the first row, update controls
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

        //show row data from the current location and fill controls
        private void ShowRow(int location)
        {
            DataRow dr = thisProjectTracking.Projects.Rows[location];
            txtID.Text = dr[0].ToString();
            txtTitle.Text = dr[1].ToString();
            txtDescription.Text = dr[2].ToString();
            cbStatus.Text = dr[3].ToString();
            txtStart.Text = dr[4].ToString();
            txtEnd.Text = dr[5].ToString();
            txtManager.Text = dr[6].ToString();
            fillListView(dr);
        }

        //set row data in datarow from controls data
        private void getRow(int location)
        {
            DataRow dr = thisProjectTracking.Projects.Rows[location];
            dr[0] = txtID.Text;
            dr[1] = txtTitle.Text;
            dr[2] = txtDescription.Text;
            dr[3] = cbStatus.Text;
            dr[4] = txtStart.Text;

            if (!string.IsNullOrEmpty(txtEnd.Text))
            { dr[5] = txtEnd.Text; }
            dr[6] = txtManager.Text;
        }

        //fill list view with data from a datarow sent in
        private void fillListView(DataRow project)
        {
            lvEmployeeDetails.Items.Clear();
            foreach (DataRow TaskRows in thisProjectTracking.ProjectTasks.Rows)
            {
                if (TaskRows[1].ToString() == project[0].ToString())
                {
                    foreach (DataRow TaskEmployeeRow in thisProjectTracking.TaskEmployees.Rows)
                    {
                        if(TaskEmployeeRow[0].ToString() == TaskRows[0].ToString())
                        {
                            foreach (DataRow EmployeeRow in thisProjectTracking.Employees.Rows)
                            { 
                                if(EmployeeRow[0].ToString() == TaskEmployeeRow[1].ToString())
                                {
                                    ListViewItem itmProjEmp = new ListViewItem(EmployeeRow[1].ToString() + " " + EmployeeRow[2].ToString());
                                    itmProjEmp.SubItems.Add(TaskRows[2].ToString());
                                    itmProjEmp.SubItems.Add(TaskEmployeeRow[2].ToString());
                                    itmProjEmp.SubItems.Add(TaskEmployeeRow[3].ToString());
                                    lvEmployeeDetails.Items.Add(itmProjEmp);

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
