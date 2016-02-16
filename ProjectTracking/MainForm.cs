using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/* 
 * Author:  Powell Hamner
 * Project: Project Tacking
 * Class:   CIS 266
 * Date:    6/17/2014
 * Description:
 
This application is to track projects, tasks and the time employees spend working on each task.
 
Project information includes project name, description and status 
(pending, underway, delayed, and completed), start date, end date, and project manager.
 
Projects are made up of tasks. Each task is part of a project, and has a name, description, 
start date, end date, and status (pending, underway, delayed, completed).
 
Employee information includes name, title, and employee ID. Any employee can manage a project. 
All employees can work on tasks.

Employees work on tasks; information needed includes which task, which employee, date, and hours worked. 
Employees (including managers) need to be able to enter their name, select a project, select a task, and 
enter the date and number of hours spent working on that task. Employees should only be able to select from 
projects and tasks that are designated as underway. Employees should also have the ability to change the time 
spent on a specific project for a particular day; a task should have only one entry per employee per day. 
Managers need to be able to enter/edit employees, projects and associated tasks, and change project and task status. 
Managers also need to be able to view information in several ways (using forms).
 */

namespace ProjectTracking
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //Create a new instance of the DataSet
            _tracking = new ProjectTrackingDataSet();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Ticker to update the time
            tsTime.Text = DateTime.Now.ToLocalTime().ToString();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            timer1_Tick(null, null);
            Status = "Form Loaded";
        }

        //Method for showing a form in the main form
        private void ShowForm(Form display)
        {
            display.MdiParent = this;
            display.Show();
        }

        //DataSet Property
        private ProjectTrackingDataSet _tracking;
        public ProjectTrackingDataSet Tracking
        { get { return _tracking; } }

        //Status label
        public string Status
        { set { tsStatus.Text = value; } }

        //Close the Application
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        { this.Close(); }

        //Open EmployeeForm
        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        { ShowForm(new EmployeesForm()); }

        //Open ProjectsForm
        private void projectsToolStripMenuItem_Click(object sender, EventArgs e)
        { ShowForm(new ProjectsForm()); }

        //Open TasksForm
        private void tasksToolStripMenuItem_Click(object sender, EventArgs e)
        { ShowForm(new TasksForm()); }

        //Open HoursForm
        private void hoursToolStripMenuItem_Click(object sender, EventArgs e)
        { ShowForm(new HoursForm()); }

        //Open form to view Projects
        private void viewProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        { ShowForm(new ProjectTasksView()); }

        //Open form to view Projects and related employees
        private void viewProjectsAndEmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        { ShowForm(new ProjectEmployeeTasksView()); }

        //Open form to view Employees and related Projects
        private void viewEmployeesAndProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        { ShowForm(new EmployeeProjectsView()); }

        //Run the 'Save All' method in the dataset
        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        { _tracking.SaveAll(); }
    }
}
