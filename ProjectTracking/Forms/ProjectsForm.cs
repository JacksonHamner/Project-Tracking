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
    public partial class ProjectsForm : Form
    {
        public ProjectsForm()
        {
            InitializeComponent();
            //initialize the Editmode to unchanged
            editing = EditMode.Unchanged;
        }
        //Edit Mode Enumerator, used to tell if the user is adding to the form or not
        enum EditMode
        {
            Adding,
            Unchanged
        }
        //instance of EditMode Enumerator
        private EditMode editing;

        //Mainform Parent Property
        private MainForm thisParent
        { get { return (MainForm)MdiParent; } }

        //Location of current row the user is on
        private int _Location;

        //Instance of dataset from the mainform
        private ProjectTrackingDataSet thisProjectTracking
        { get { return thisParent.Tracking; } }

        //Loading Event for form
        private void ProjectsForm_Load(object sender, EventArgs e)
        {
            thisParent.Status = "Projects Form Ready!";
            //if count is more than 0, show the first row and set the controls to their proper values
            if (thisProjectTracking.Employees.Rows.Count > 0)
            {
                _Location = 0;
                ShowRow(_Location);
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                btnNext.Enabled = (_Location < thisProjectTracking.Projects.Rows.Count - 1);

            }
            //else disable controls
            else
            {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                btnDelete.Enabled = false;
                btnLast.Enabled = false;
            }
        }

        //Validation Method
        public bool ValidateForm()
        {
            //Temporary values to evaluate inputs
            bool Valid = true;
            errorProvider1.Clear();
            int managerID;
            DateTime start;
            DateTime end;

            if (txtTitle.Text == "")
            { 
                Valid=false;
                errorProvider1.SetError(txtTitle, "Cannot be null");
            }

            if (txtDescription.Text == "")
            { 
                Valid=false;
                errorProvider1.SetError(txtDescription, "Cannot be null");
            }

            if (txtStart.Text == "")
            { 
                Valid=false;
                errorProvider1.SetError(txtStart, "Cannot be null");
            }
            else if (DateTime.TryParse(txtStart.Text, out start) == false )
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
                        if (end.Date > start.Date)
                        {
                            Valid = false;
                            errorProvider1.SetError(txtEnd, "Must be later than the start date!");
                        }
                    }
                }
            }

            if (txtManager.Text == "")
            { 
                Valid=false;
                errorProvider1.SetError(txtManager, "Cannot be null");
            }

            else if (int.TryParse(txtManager.Text, out managerID) == false)
            {
                Valid = false;
                errorProvider1.SetError(txtManager, "Must be a number!");
            }

            else if (int.TryParse(txtManager.Text, out managerID) == true)
            {
                bool foundEmployee = false;
                foreach (DataRow dr in thisProjectTracking.Employees.Rows)
                {
                    if (dr[0].ToString() == managerID.ToString())
                    {
                        foundEmployee = true;
                        break;
                    }

                }
                if (foundEmployee == false)
                {
                    Valid = false;
                    errorProvider1.SetError(txtManager, "No Employee Found!");
                }
            }

            return Valid;
        }

        //When clicked, move to the first row and display the data. Update controls. 
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

        //When clicked move the the row just before the current row. update controls.
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

        //When clicked move to the next row after the current row. Update controls. 
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

        //When clicked move to the last row. Update controls
        private void btnLast_Click(object sender, EventArgs e)
        {
            _Location = thisProjectTracking.Projects.Rows.Count - 1;
            ShowRow(_Location);
            btnLast.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = true;
            btnFirst.Enabled = true;
        }

        //Method to show the correct values in the conrols based on row location
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
        }

        //method to set control values based on location
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
        
        //method to close the form
        private void btnClose_Click(object sender, EventArgs e)
        {
            thisParent.Status = "Projects Form Closed";
            this.Close();
            
        }

        //Add a new Project
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (editing == EditMode.Adding) 
            {
                bool valid = ValidateForm();
                if (valid == true)
                {
                    DataRow newRow = thisProjectTracking.Projects.NewRow(); 

                    //generate new ID for row, checks for values and makes sure ID does not already exist
                    int newID = thisProjectTracking.Projects.Rows.Count + 1;
                    bool ConfirmID = false;
                    while (ConfirmID == false)
                    {
                        foreach (DataRow dr in thisProjectTracking.Projects.Rows)
                        {
                            if ((int)dr[0] == newID)
                            { newID++; }
                            else
                            { ConfirmID = true; }
                        }
                    }
                    // set values
                    newRow[0] = newID;
                    newRow[1] = txtTitle.Text;
                    newRow[2] = txtDescription.Text;
                    newRow[3] = cbStatus.SelectedValue;
                    newRow[4] = DateTime.Parse(txtStart.Text);
                    newRow[5] = DateTime.Parse(txtEnd.Text);
                    newRow[6] = int.Parse(txtManager.Text);
                    //Add row to database
                    thisProjectTracking.Projects.Rows.Add(newRow);
                    //Set the current location
                    _Location = thisProjectTracking.Projects.Rows.Count - 1;
                    //update control values
                    btnNew.Text = "New";
                    btnDelete.Text = "Delete";
                    if (_Location != 0) 
                    {    
                        btnPrevious.Enabled = true;
                        btnFirst.Enabled = true;
                    }
                    editing = EditMode.Unchanged;
                    ShowRow(_Location);
                    thisParent.Status = "Project Saved!";
                }

            }
            //if not already editing, prepare form to start adding a project
            else if (editing == EditMode.Unchanged) 
            {
                txtID.Clear();
                txtTitle.Clear();
                txtDescription.Clear();
                cbStatus.SelectedIndex = 0;
                txtStart.Clear();
                txtEnd.Clear();
                txtManager.Clear();

                txtTitle.Focus();
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                btnFirst.Enabled = false;
                btnNew.Text = "Save";
                btnDelete.Text = "Cancel";
                editing = EditMode.Adding;
                thisParent.Status = "Ready to create new Project";
            }
        }

        //Method to delete project from database
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (editing == EditMode.Unchanged)
            {
                thisProjectTracking.Projects.Rows[_Location].Delete();

                if (thisProjectTracking.Projects.Rows.Count > 0)
                {
                    if (_Location > 0)
                    { _Location--; }
                    else
                    { _Location++; }

                    ShowRow(_Location);

                    if (_Location == thisProjectTracking.Projects.Rows.Count - 1)
                    {
                        btnPrevious.Enabled = false;
                        btnFirst.Enabled = false;
                        btnNext.Enabled = true;
                        btnLast.Enabled = true;
                    }
                    if (thisProjectTracking.Projects.Rows.Count - 1 == 0)
                    {
                        btnNext.Enabled = false;
                        btnLast.Enabled = false;
                        btnPrevious.Enabled = true;
                        btnFirst.Enabled = true;
                    }
                }
                else
                {
                    txtID.Clear();
                    txtTitle.Clear();
                    txtDescription.Clear();
                    cbStatus.SelectedIndex = 0;
                    txtStart.Clear();
                    txtEnd.Clear();
                    txtManager.Clear();
                    btnDelete.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnLast.Enabled = false;
                    btnFirst.Enabled = false;
                }
            }
            else
            {
                if (thisProjectTracking.Projects.Rows.Count == 0)
                {
                    txtID.Clear();
                    txtTitle.Clear();
                    txtDescription.Clear();
                    cbStatus.SelectedIndex = 0;
                    txtStart.Clear();
                    txtEnd.Clear();
                    txtManager.Clear();
                    btnDelete.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnLast.Enabled = false;
                    btnFirst.Enabled = false;
                }
                else 
                {
                    ShowRow(_Location);

                    if (_Location < thisProjectTracking.Projects.Rows.Count - 1)
                    {
                        btnNext.Enabled = true;
                        btnLast.Enabled = true;
                    }
                    if (_Location > 0)
                    {
                        btnPrevious.Enabled = true;
                        btnFirst.Enabled = true;
                    }
                }
                btnDelete.Text = "Delete";
                btnNew.Text = "Add";
            }
        }
    }
}
