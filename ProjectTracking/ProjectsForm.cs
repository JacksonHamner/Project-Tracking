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

        private void ProjectsForm_Load(object sender, EventArgs e)
        {
            thisParent.Status = "Projects Form Ready!";
            if (thisProjectTracking.Employees.Rows.Count > 0)
            {
                _Location = 0;
                ShowRow(_Location);
                btnPrevious.Enabled = false;
                // btnFirst.Enabled = false;
                btnNext.Enabled = (_Location < thisProjectTracking.Projects.Rows.Count - 1);

            }
            else
            {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                btnDelete.Enabled = false;
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

            if (_Location + 1 == thisProjectTracking.Projects.Rows.Count)
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;

            }
            btnPrevious.Enabled = true;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            _Location = thisProjectTracking.Projects.Rows.Count - 1;
            ShowRow(_Location);
        }

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

        private void getRow(int location)
        {
            DataRow dr = thisProjectTracking.Projects.Rows[location];
            dr[0] = txtID.Text;
            dr[1] = txtTitle.Text;
            dr[2] = txtDescription.Text;
            dr[3] = cbStatus.Text;
            dr[4] = txtStart.Text;

            if (!string.IsNullOrEmpty(txtEnd.Text))
            {
                dr[5] = txtEnd.Text;
            }

            dr[6] = txtManager.Text;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }
    }
}
