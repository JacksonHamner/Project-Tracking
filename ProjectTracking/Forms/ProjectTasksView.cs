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
    public partial class ProjectTasksView : Form
    {
        // constructor
        public ProjectTasksView()
        { InitializeComponent(); }

        //Parent Form 
        private MainForm thisParent
        { get { return (MainForm)MdiParent; } }

        //Location Variable
        private int _Location;

        //Get Dataset from Parent Form
        private ProjectTrackingDataSet thisProjectTracking
        { get { return thisParent.Tracking; } }

        //Load Form
        private void ProjectTasksView_Load(object sender, EventArgs e)
        {
            // update status label
            thisParent.Status = "Viewing Projects";
            //if there are rows
            if (thisProjectTracking.Projects.Rows.Count > 0)
            {
                //set location to the first row
                _Location = 0;
                //update controls
                ShowRow(_Location);
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                btnNext.Enabled = (_Location < thisProjectTracking.Projects.Rows.Count - 1);
                btnLast.Enabled = (_Location < thisProjectTracking.Projects.Rows.Count - 1);
            }
            else
            {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                btnLast.Enabled = false;
                btnFirst.Enabled = false;
            }
            //create a new datarow based on the current location
            DataRow dr = thisProjectTracking.Projects.Rows[_Location];
            //Add colums to listview
            lvTaskDetails.Columns.Add("Task", 100);
            lvTaskDetails.Columns.Add("Description", 300);
            lvTaskDetails.Columns.Add("status", 100);
            //details view
            lvTaskDetails.View = View.Details;
            //fill method with datarow
            fillListView(dr);
        }

        // show a row at a given location
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

        // fill list view method
        private void fillListView(DataRow project)
        {
            //clear listview
            lvTaskDetails.Items.Clear();

            //for each project tasks
            foreach (DataRow TaskRows in thisProjectTracking.ProjectTasks.Rows)
            {
                if (TaskRows[1].ToString() == project[0].ToString())
                {
                    //create new listview item
                    ListViewItem itmTaskEmployees = new ListViewItem(TaskRows[2].ToString());
                    //fille subitems with values
                    itmTaskEmployees.SubItems.Add(TaskRows[3].ToString());
                    itmTaskEmployees.SubItems.Add(TaskRows[6].ToString());
                    //add items to listview
                    lvTaskDetails.Items.Add(itmTaskEmployees);
                }
            }
        }

        // set row values from controls
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

        //navigate to the first row, fill controls with data
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

        //navigate to the previous row
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

        //navigate to the next row
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

        //navigate to the last row
        private void btnLast_Click(object sender, EventArgs e)
        {
            _Location = thisProjectTracking.Projects.Rows.Count - 1;
            ShowRow(_Location);
            btnLast.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = true;
            btnFirst.Enabled = true;
        }
        //close form method, update status label
        private void btnClose_Click(object sender, EventArgs e)
        {
            thisParent.Status = "View Form Closed";
            this.Close();
            
        }
    }
}
