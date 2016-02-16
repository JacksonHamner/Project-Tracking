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
    public partial class EmployeesForm : Form
    {
        //constructor
        public EmployeesForm()
        {
            InitializeComponent();
            editing = EditMode.Unchanged;
        }

        //enumerator to create object to track if the row is
        //currently being edited/ added to or not
        enum EditMode
        {
            Adding,
            Unchanged
        }
        //Create instance of enumerator
        private EditMode editing;


        private MainForm thisParent
        { get { return (MainForm)MdiParent; } }

        private int _Location;

        private ProjectTrackingDataSet thisProjectTracking
        { get { return thisParent.Tracking; } }

        //Validate Method, returns true or false
        public bool ValidateForm()
        {
            errorProvider1.Clear();
            bool Valid = true;
            if (txtFirstName.Text == "")
            {
                Valid = false;
                errorProvider1.SetError(txtFirstName, "Cannot be Null");
            }
            if(txtLastName.Text == "")
            {
                Valid = false;
                errorProvider1.SetError(txtLastName, "Cannot be Null");
            }
            if (txtTitle.Text == "")
            {
                Valid = false;
                errorProvider1.SetError(txtTitle, "Cannot be Null");
            }
            if(Valid == false)
            {
                thisParent.Status="Save Error!";
            }
            return Valid;
        
        }
        
        private void btnNew_Click(object sender, EventArgs e)
        {
            //if we are adding to the Employees database
            if (editing == EditMode.Adding)
            {
                //validate entries
                bool Valid = ValidateForm();
                //if all is good
                if (Valid == true)
                {
                    //Create new datarow
                    DataRow newRow = thisProjectTracking.Employees.NewRow(); 
                    //Create ID for new Row
                    int newID = thisProjectTracking.Employees.Rows.Count + 1;
                    bool ConfirmID = false;
                    while (ConfirmID == false)
                    {
                        foreach (DataRow dr in thisProjectTracking.Employees.Rows)
                        {
                            if ((int)dr[0] == newID)
                            { newID++; }
                            else
                            { ConfirmID = true; }
                        }
                    }
                    //Fill row with data from controls
                    newRow[0] = newID;
                    newRow[1] = txtFirstName.Text;
                    newRow[2] = txtLastName.Text;
                    newRow[3] = txtTitle.Text;
                    //Add row to Employee's table
                    thisProjectTracking.Employees.Rows.Add(newRow);
                    //update location to last row
                    _Location = thisProjectTracking.Employees.Rows.Count - 1;
                    // update Buttons
                    btnNew.Text = "New";
                    btnDelete.Text = "Delete";
                    // if there are more than 0 rows
                    if (_Location > 0) 
                    {   //enable the first and previous buttons
                        btnPrevious.Enabled = true;
                        btnFirst.Enabled = true;
                    }
                    //Update status label
                    thisParent.Status = "Employee Saved!";
                    //Set to unchanged
                    editing = EditMode.Unchanged;
                }
            }
                //If not adding yet
            else if (editing == EditMode.Unchanged)
            {
                //Clear / diable Controls
                txtEmployeeID.Clear();
                txtFirstName.Clear();
                txtLastName.Clear();
                txtTitle.Clear();                
                txtFirstName.Focus();
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                btnFirst.Enabled = false;
                //set new values to buttons
                btnNew.Text = "Save";
                btnDelete.Text = "Cancel";
                // Now Adding
                editing = EditMode.Adding;
                //Update Status Label
                thisParent.Status = "Ready to create new Employee";
            }
        }

        //Delete Method
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //if not adding
            if (editing == EditMode.Unchanged)
            {
                // Delete Employee row from the dataset 
                thisProjectTracking.Employees.Rows[_Location].Delete();
                // if there are more than 0 rows in the Employees table
                if (thisProjectTracking.Employees.Rows.Count > 0)
                {
                    //if location is more than 0 go back
                    if (_Location > 0)
                    { _Location--; }
                    //else move forward
                    else
                    { _Location++; }
                    //Show row and new current location
                    ShowRow(_Location);
                    //if the last row
                    if (_Location == thisProjectTracking.Employees.Rows.Count - 1)
                    {
                        // Update Buttons
                        btnPrevious.Enabled = false;
                        btnFirst.Enabled = false;
                        btnNext.Enabled = true;
                        btnLast.Enabled = true;
                    }
                    //if there are 0 rows
                    if (thisProjectTracking.Employees.Rows.Count - 1 == 0)
                    {
                        btnNext.Enabled = false;
                        btnLast.Enabled = false;
                        btnPrevious.Enabled = true;
                        btnFirst.Enabled = true;
                    }
                }
                       //clear controls and disable buttons
                else 
                {
                    txtEmployeeID.Clear();
                    txtFirstName.Clear();
                    txtLastName.Clear();
                    txtTitle.Clear();

                    btnDelete.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnLast.Enabled = false;
                    btnFirst.Enabled = false;
                }
            }
                // cancel adding
            else
            {   // if there are 0 rows
                if (thisProjectTracking.Employees.Rows.Count == 0)
                {
                    //clear controls, disable buttons
                    txtEmployeeID.Clear();
                    txtFirstName.Clear();
                    txtLastName.Clear();
                    txtTitle.Clear();
                    btnDelete.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnLast.Enabled = false;
                    btnFirst.Enabled = false;
                }
                    //if not 0
                else 
                {
                    //show row
                    ShowRow(_Location);
                    //if location is less than the last row
                    if (_Location < thisProjectTracking.Employees.Rows.Count - 1)
                    {
                        //enable buttons
                        btnNext.Enabled = true;
                        btnLast.Enabled = true;
                    }
                    //if location is greater than 0
                    if (_Location > 0)
                    {
                        //enable buttons
                        btnPrevious.Enabled = true;
                        btnFirst.Enabled = true;
                    }
                }
                
                btnDelete.Text = "Delete";
                btnNew.Text = "Add";
            }
        }

        // move to the first row on click
        private void btnFirst_Click(object sender, EventArgs e)
        {
            _Location = 0;
            ShowRow(_Location);            
            btnPrevious.Enabled = false;
            btnFirst.Enabled = false;
            if (thisProjectTracking.Employees.Rows.Count > 1)
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
            }
        }
        // more to the previous row on click
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

        //move to the next button on click
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

        //move to the last row on click
        private void btnLast_Click(object sender, EventArgs e)
        {
            _Location = thisProjectTracking.Employees.Rows.Count -1;
            ShowRow(_Location);
            btnLast.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = true;
            btnFirst.Enabled = true;
            
        }
        //close form and update status label
        private void btnClose_Click(object sender, EventArgs e)
        {
            thisParent.Status = "Employees Form Closed";
            this.Close();
            
        }

        //load form
        private void EmployeesForm_Load(object sender, EventArgs e)
        {
            //update status label
            thisParent.Status = "Employee Form Ready!";
            // if number of rows are greater than 0
            if (thisProjectTracking.Employees.Rows.Count > 0)
            {
                //set location to 0
                _Location = 0;
                //show row
                ShowRow(_Location);
                // update buttons
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                btnNext.Enabled = (_Location < thisProjectTracking.Employees.Rows.Count - 1);
                btnLast.Enabled = (_Location < thisProjectTracking.Employees.Rows.Count - 1);
            }
            else {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
                btnLast.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        //display row at current location
        private void ShowRow(int location)
        {
            DataRow dr = thisProjectTracking.Employees.Rows[location];
            txtEmployeeID.Text = dr[0].ToString();
            txtFirstName.Text = dr[1].ToString();
            txtLastName.Text = dr[2].ToString();
            txtTitle.Text = dr[3].ToString();
        }

        // get data from controls, send to datarow
        private void getRow(int location)
        {
            DataRow dr = thisProjectTracking.Employees.Rows[location];
            dr[1] = txtFirstName.Text;
            dr[2] = txtLastName.Text;
            dr[3] = txtTitle.Text;
        }
    }
}
