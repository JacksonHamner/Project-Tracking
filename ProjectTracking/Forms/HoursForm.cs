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
    public partial class HoursForm : Form
    {
        //constructor
        public HoursForm()
        { InitializeComponent(); }

        private ProjectTrackingDataSet Tracking
        { get { return thisParent.Tracking; } }

        private MainForm thisParent
        { get { return (MainForm)MdiParent; } }
        
        //Fill TreeView with Projects and related Tasks
        private void FillTreeView()
        {
            tvProjects.Nodes.Clear();
            foreach (DataRow dr in Tracking.Projects.Rows)
            {
                string project = dr[1].ToString();
                TreeNode projNode = new TreeNode(project); 
                foreach (DataRow TaskRow in Tracking.ProjectTasks.Rows)
                {
                    string projectid = dr[0].ToString();
                    string taskProjectID = TaskRow[1].ToString();

                    if (projectid == taskProjectID)
                    {
                        TreeNode TaskNode = new TreeNode(TaskRow[2].ToString());
                        TaskNode.Tag = TaskRow[0].ToString(); 

                        if (TaskRow[6].ToString() == "Underway")
                        { projNode.Nodes.Add(TaskNode); }
                    }
                }
                if (dr[3].ToString() == "Underway")
                { tvProjects.Nodes.Add(projNode); }
            }
        }
        // Get the Task Hours data from the projects ID
        private void GetTaskHours(int projectTaskID)
        {
            foreach (DataRow drtskEmp in Tracking.TaskEmployees.Rows)
            {
                if ((int)drtskEmp[0] == projectTaskID)
                {
                    ListViewItem itmTaskEmployees = new ListViewItem(drtskEmp[1].ToString());
                    itmTaskEmployees.SubItems.Add(drtskEmp[2].ToString()); 
                    itmTaskEmployees.SubItems.Add(drtskEmp[3].ToString());
                    lvWorkedTasks.Items.Add(itmTaskEmployees);
                }
            }
        }        
        //Load Method
        private void HoursForm_Load(object sender, EventArgs e)
        {
            lvWorkedTasks.Columns.Add("Employee ID", 100);
            lvWorkedTasks.Columns.Add("Date", 100);
            lvWorkedTasks.Columns.Add("Hours", 100);
            lvWorkedTasks.View = View.Details;
            FillTreeView();
            thisParent.Status = "Hours Form Ready!";
            isLoaded = true;

        }
        // CLose form method, update Status Label
        private void btnClose_Click(object sender, EventArgs e)
        {
            thisParent.Status = "Hours Form Closed";
            this.Close();
            
        }
        //Is Loaded Variable, set to true after load form is done
        bool isLoaded = false;

        //After selecting an object from the treeview
        private void tvProjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if form is finished loading
            if (isLoaded)
            {
                // if the node tag is not null
                if (e.Node.Tag != null)
                {
                    //if task ID is parseable into an integer
                    int taskid;
                    if (int.TryParse(e.Node.Tag.ToString(), out taskid))
                    {
                        //Clear Listview
                        lvWorkedTasks.Items.Clear();
                        // fill listview
                        GetTaskHours(taskid);
                    }
                }
                //enable controls
                txtName.Enabled = true;
                txtHours.Enabled = true;
                dtpDate.Enabled = true;                
            }
        }

        //after submit button is clicked
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //create datarow for a new row in the dataset
                DataRow newRow = Tracking.TaskEmployees.NewRow(); 

                TreeViewEventArgs tve = new TreeViewEventArgs(tvProjects.SelectedNode);
                int projectTaskId;
                if(int.TryParse(tve.Node.Tag.ToString(), out projectTaskId))
                {
                    // set row data to controls
                    newRow[0] = projectTaskId;
                    newRow[1] = txtName.Text;
                    newRow[2] = dtpDate.Value;
                    newRow[3] = txtHours.Text;
                    // add row to taskemployees dataset
                    Tracking.TaskEmployees.Rows.Add(newRow);
                    //create instance of a new treeview item
                    ListViewItem itmTaskEmployees = new ListViewItem(txtName.Text);
                    //add items to task employees
                    itmTaskEmployees.SubItems.Add(dtpDate.Value.ToString());
                    itmTaskEmployees.SubItems.Add(txtHours.Text);
                    //add item to listview
                    lvWorkedTasks.Items.Add(itmTaskEmployees);
                }
            //clear controls and update statuslabel
                txtHours.Clear();
                txtName.Clear();
                thisParent.Status = "Hours Submitted!";
        }        
    }
}
