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
        public EmployeesForm()
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

        private MainForm thisParent
        { get { return (MainForm)MdiParent; } }

        private int _Location;

        private ProjectTrackingDataSet thisProjectTracking
        { get { return thisParent.Tracking; } }


        private void btnNew_Click(object sender, EventArgs e)
        {
            if (editing == EditMode.Adding) // in add process
            {
                // save row to datatable
                // create row for recipe
                DataRow newRow = thisProjectTracking.Employees.NewRow(); // row is NOT in table
                // set values for new row fields

                //Generate new ID
                int newID = thisProjectTracking.Employees.Rows.Count + 1;
                bool ConfirmID = false;
                while (ConfirmID == false)
                {
                    foreach (DataRow dr in thisProjectTracking.Employees.Rows)
                    {
                        if ((int)dr[0] == newID)
                        {
                            newID++;
                        }
                        else
                        {
                            ConfirmID = true;
                        }
                    }
                }

                newRow[0] = newID;
                newRow[1] = txtFirstName.Text;
                newRow[2] = txtLastName.Text;
                newRow[3] = txtTitle.Text;

                // add to table
                thisProjectTracking.Employees.Rows.Add(newRow);
                //MyRecipes.Recipes.Rows.Add(newRow);

                // set row location to current
                _Location = thisProjectTracking.Employees.Rows.Count - 1;

                // update form
                btnNew.Text = "New";
                btnDelete.Text = "Delete";
                if (_Location != 0) // if not at first row enable previous button; no need to
                {    // enable next, because at last row
                    btnPrevious.Enabled = true;
                    btnFirst.Enabled = true;
                }
                editing = EditMode.Unchanged;
            }
            else if (editing == EditMode.Unchanged) // start add process
            {
                // clear for new entry
                txtEmployeeID.Clear();
                txtFirstName.Clear();
                txtLastName.Clear();
                txtTitle.Clear();

                txtFirstName.Focus();
                // disable buttons
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                btnFirst.Enabled = false;
                // change edit buttons
                btnNew.Text = "Save";
                btnDelete.Text = "Cancel";
                // set edit mode
                editing = EditMode.Adding;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // if not adding, delete current row
            if (editing == EditMode.Unchanged)
            {
                // delete current row
                // MyRecipes.Recipes.Rows.RemoveAt(Location); -- take from datatable
                thisProjectTracking.Employees.Rows[_Location].Delete();


                // if there are still rows in table, show prior row
                if (thisProjectTracking.Employees.Rows.Count > 0)
                {
                    // if location isn't for first row, decrease by one
                    if (_Location > 0)
                        _Location--;
                    else
                    { _Location++; }

                    // show row
                    ShowRow(_Location);
                    if (_Location == thisProjectTracking.Employees.Rows.Count - 1)
                    {
 

                        btnPrevious.Enabled = false;
                        btnFirst.Enabled = false;
                        btnNext.Enabled = true;
                        btnLast.Enabled = true;
                    }
                    if (thisProjectTracking.Employees.Rows.Count - 1 == 0)
                    {
                        btnNext.Enabled = false;
                        btnLast.Enabled = false;
                        btnPrevious.Enabled = true;
                        btnFirst.Enabled = true;
                    }
                }
                else // no rows in table
                {
                    // clear controls
                    txtEmployeeID.Clear();
                    txtFirstName.Clear();
                    txtLastName.Clear();
                    txtTitle.Clear();
                    // disable all buttons except add
                    btnDelete.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnLast.Enabled = false;
                    btnFirst.Enabled = false;
                }
            }
            else // user started add process
            {
                // cancel adding a row
                // no rows in table
                if (thisProjectTracking.Employees.Rows.Count == 0)
                {
                    // clear controls
                    txtEmployeeID.Clear();
                    txtFirstName.Clear();
                    txtLastName.Clear();
                    txtTitle.Clear();
                    // disable all buttons except add
                    btnDelete.Enabled = false;
                    btnNext.Enabled = false;
                    btnPrevious.Enabled = false;
                    btnLast.Enabled = false;
                    btnFirst.Enabled = false;
                }
                else // show current row
                {
                    ShowRow(_Location);
                    // determine if next button should be enabled
                    if (_Location < thisProjectTracking.Employees.Rows.Count - 1)
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

                // reset delete text
                btnDelete.Text = "Delete";
                btnNew.Text = "Add";
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            _Location = 0;
            ShowRow(_Location);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            // get changes
            getRow(_Location);
            // decrease location to represent prior row
            _Location--;
            // show row at current location
            ShowRow(_Location);
            // if at first row disable previous button
            if (_Location == 0)
            {
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
            }
            // enable next button
            btnNext.Enabled = true;
        }

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
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            _Location = thisProjectTracking.Employees.Rows.Count -1;
            ShowRow(_Location);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EmployeesForm_Load(object sender, EventArgs e)
        {
            thisParent.Status = "Employee Form Ready!";
            if (thisProjectTracking.Employees.Rows.Count > 0)
            {
                _Location = 0;
                ShowRow(_Location);
                btnPrevious.Enabled = false;
               // btnFirst.Enabled = false;
                btnNext.Enabled = (_Location < thisProjectTracking.Employees.Rows.Count - 1);

            }
            else {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void ShowRow(int location)
        {
            DataRow dr = thisProjectTracking.Employees.Rows[location];

            txtEmployeeID.Text = dr[0].ToString();
            txtFirstName.Text = dr[1].ToString();
            txtLastName.Text = dr[2].ToString();
            txtTitle.Text = dr[3].ToString();
        }

        private void getRow(int location)
        {
            DataRow dr = thisProjectTracking.Employees.Rows[location];

            dr[1] = txtFirstName.Text;
            dr[2] = txtLastName.Text;
            dr[3] = txtTitle.Text;
        }

    }
}
