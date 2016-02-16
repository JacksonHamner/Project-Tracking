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
    public partial class TasksForm : Form
    {
        //constructor
        public TasksForm()
        {
            InitializeComponent();
            editing = EditMode.Unchanged;
        }
       
        enum EditMode
        {
            Adding,
            Unchanged
        }

        private EditMode editing;
        //set Mainform to this forms MDIparent
        private MainForm thisParent
        { get { return (MainForm)MdiParent; } }
        //location variable
        private int _Location;
        //get dataset from the parent form
        private ProjectTrackingDataSet thisProjectTracking
        { get { return thisParent.Tracking; } }

        //validate method
        public bool ValidateForm()
        {
            bool Valid = true;
            errorProvider1.Clear();
            DateTime start;
            DateTime end;
            int projectID;

            if (txtProjectID.Text == "")
            {
                Valid = false;
                errorProvider1.SetError(txtProjectID, "Cannot be null!");
                
            }
            else if (int.TryParse(txtProjectID.Text, out projectID) == false)
            {
                Valid = false;
                errorProvider1.SetError(txtProjectID, "Must be a number!");
            }
            else if (int.TryParse(txtProjectID.Text, out projectID) == true)
            {
                bool foundProject = false;
                foreach (DataRow dr in thisProjectTracking.Projects.Rows)
                {
                    if (dr[0].ToString() == projectID.ToString())
                    {
                        foundProject = true;
                        break;
                    }
                    
                }
                if (foundProject == false)
                {
                    Valid = false;
                    errorProvider1.SetError(txtProjectID, "No Project Found!");
                }
            }

            if (txtName.Text == "")
            {
                Valid = false;
                errorProvider1.SetError(txtName, "Cannot be null!");
            }

            if (txtDescription.Text == "false")
            {
                Valid = false;
                errorProvider1.SetError(txtDescription, "Cannot be null!");
            }

            if (txtStart.Text == "")
            {
                Valid = false;
                errorProvider1.SetError(txtStart, "Cannot be null!");
            }
            else if (DateTime.TryParse(txtStart.Text, out start) == false)
            {
                Valid = false;
                errorProvider1.SetError(txtStart, "Must be a valid Date!");
            }
            if (txtEnd.Text != "")
            {
                if (DateTime.TryParse(txtEnd.Text, out end) == false)
                {
                    Valid = false;
                    errorProvider1.SetError(txtEnd, "Must be a valid Date!");
                }
                else
                {
                    if (DateTime.TryParse(txtStart.Text, out start) == true)
                    {
                        if (end > start)
                        {
                            Valid = false;
                            errorProvider1.SetError(txtEnd, "Must be later than the start date!");
                        }
                    }
                }
            }
            return Valid;
        }

        //add new Task to ProjectTasks
        private void btnNew_Click(object sender, EventArgs e)
        {
            //is already adding data to controls
            if (editing == EditMode.Adding)
            {
                //validate entries
                bool valid = ValidateForm();
                //if all is good
                if (valid == true)
                {
                    //create instance of new row in dataset
                    DataRow newRow = thisProjectTracking.ProjectTasks.NewRow(); 
                    //set new id
                    int newID = thisProjectTracking.ProjectTasks.Rows.Count + 1;
                    bool ConfirmID = false;
                    while (ConfirmID == false)
                    {
                        foreach (DataRow dr in thisProjectTracking.ProjectTasks.Rows)
                        {
                            if ((int)dr[0] == newID)
                            { newID++; }
                            else
                            { ConfirmID = true; }
                        }
                    }
                    //set valies
                    newRow[0] = newID;
                    newRow[1] = int.Parse(txtProjectID.Text);
                    newRow[2] = txtName.Text;
                    newRow[3] = txtDescription.Text;
                    newRow[4] = DateTime.Parse(txtStart.Text);
                    newRow[5] = DateTime.Parse(txtEnd.Text);
                    newRow[6] = cbStatus.SelectedValue;
                    //add row to the Dataset
                    thisProjectTracking.ProjectTasks.Rows.Add(newRow);
                    //update location
                    _Location = thisProjectTracking.ProjectTasks.Rows.Count - 1;
                    //update button balies
                    btnNew.Text = "New";
                    btnDelete.Text = "Delete";
                    //update controls
                    if (_Location != 0) 
                    {    
                        btnPrevious.Enabled = true;
                        btnFirst.Enabled = true;
                    }
                    //set to not be adding
                    editing = EditMode.Unchanged;
                    //show new row
                    ShowRow(_Location);
                    //update status label
                    thisParent.Status = "Task Saved!";
                }
            }
                //if not adding, prepare form for adding
            else if (editing == EditMode.Unchanged) 
            {
                //clear controls
                txtID.Clear();
                txtName.Clear();
                txtDescription.Clear();
                txtStart.Clear();
                txtEnd.Clear();
                txtProjectID.Clear();
                cbStatus.SelectedIndex = 0;
                //update buttons
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                btnFirst.Enabled = false;
                btnNew.Text = "Save";
                btnDelete.Text = "Cancel";
                //set to adding
                editing = EditMode.Adding;
                //update status label
                thisParent.Status = "Ready to create new Task";
            }
        }

        //delete method
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //if not adding
            if (editing == EditMode.Unchanged)
            {
                // delete row from dataset
                thisProjectTracking.ProjectTasks.Rows[_Location].Delete();
                //if there are rows left
                if (thisProjectTracking.ProjectTasks.Rows.Count > 0)
                {
                    //update location
                    if (_Location > 0)
                    { _Location--; }
                    else
                    { _Location++; }
                    //show row
                    ShowRow(_Location);
                    //if last row
                    if (_Location == thisProjectTracking.ProjectTasks.Rows.Count - 1)
                    {
                        //update controls
                        btnPrevious.Enabled = false;
                        btnFirst.Enabled = false;
                        btnNext.Enabled = true;
                        btnLast.Enabled = true;
                    }
                    // there is only 1 row left
                    if (thisProjectTracking.ProjectTasks.Rows.Count - 1 == 0)
                    {
                        
                        btnNext.Enabled = false;
                        btnLast.Enabled = false;
                        btnPrevious.Enabled = true;
                        btnFirst.Enabled = true;
                    }
                }
                    //if no rows
                else 
                {
                    //clear / disable almost everything
                    txtID.Clear();
                    txtName.Clear();
                    txtDescription.Clear();
                    txtStart.Clear();
                    txtEnd.Clear();
                    txtProjectID.Clear();
                    cbStatus.SelectedIndex = 0;
                    btnDelete.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnLast.Enabled = false;
                    btnFirst.Enabled = false;
                }
            }
                //if adding
            else 
            {
                //if no rows
                if (thisProjectTracking.ProjectTasks.Rows.Count == 0)
                {
                    //clear and disable everything
                    txtID.Clear();
                    txtName.Clear();
                    txtDescription.Clear();
                    txtStart.Clear();
                    txtEnd.Clear();
                    txtProjectID.Clear();
                    cbStatus.SelectedIndex = 0;
                    btnDelete.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnLast.Enabled = false;
                    btnFirst.Enabled = false;
                }
                    //there are rows
                else
                {
                    //show row at current location
                    ShowRow(_Location);
                    //if last row
                    if (_Location < thisProjectTracking.ProjectTasks.Rows.Count - 1)
                    {//update controls
                        btnNext.Enabled = true;
                        btnLast.Enabled = true;
                    }
                    //if not the first row
                    if (_Location > 0)
                    {
                        btnPrevious.Enabled = true;
                        btnFirst.Enabled = true;
                    }
                }
                //update buttons
                btnDelete.Text = "Delete";
                btnNew.Text = "Add";
            }
        }

        //close form
        private void btnClose_Click(object sender, EventArgs e)
        {
            thisParent.Status = "Tasks Form Closed";
            this.Close();
            
        }
        //navigate to the first row
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
            _Location = thisProjectTracking.Employees.Rows.Count - 1;
            ShowRow(_Location);
            btnLast.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = true;
            btnFirst.Enabled = true;

        }
        //fill controls with values from a datarow at the current row position
        private void ShowRow(int location)
        {
            DataRow dr = thisProjectTracking.ProjectTasks.Rows[location];
            txtID.Text = dr[0].ToString();
            txtProjectID.Text = dr[1].ToString();
            txtName.Text = dr[2].ToString();
            txtDescription.Text = dr[3].ToString();
            txtStart.Text = dr[4].ToString();
            txtEnd.Text = dr[5].ToString();
            cbStatus.Text = dr[6].ToString();
        }

        //get values from controls and update dtatrow at location
        private void getRow(int location)
        {
            DataRow dr = thisProjectTracking.ProjectTasks.Rows[location];
            dr[1] = txtProjectID.Text;
            dr[2] = txtName.Text;
            dr[3] = txtDescription.Text;
            dr[4] = txtStart.Text;
            if (txtEnd.Text != "")
            { dr[5] = txtEnd.Text; }
            dr[6] = cbStatus.Text;
        }

        //load form method
        private void TasksForm_Load(object sender, EventArgs e)
        {
            //update status label
            thisParent.Status = "Tasks Form Ready!";
            // if mroe than 0 rows
            if (thisProjectTracking.Employees.Rows.Count > 0)
            {
                //set to first row
                _Location = 0;
                //show first row
                ShowRow(_Location);
                //update controls
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                btnNext.Enabled = (_Location < thisProjectTracking.Employees.Rows.Count - 1);
                btnLast.Enabled = (_Location < thisProjectTracking.Employees.Rows.Count - 1);
            }
            else
            {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                btnLast.Enabled = false;
                btnFirst.Enabled = false;
                btnDelete.Enabled = false;
            }

        }
    }
}
