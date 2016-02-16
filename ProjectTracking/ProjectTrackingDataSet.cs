using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracking
{
    public class ProjectTrackingDataSet : DataSet
    {
        //Default File Name
        private const string DEFAULT_FILENAME = "ProjectTracking.mdf";

        //Select Statements
        private const string EmployeesSELECT = "SELECT emp_ID,emp_FirstName,emp_LastName,emp_Title FROM Employees";
        private const string ProjectsSELECT = "SELECT pro_ID, pro_Name, pro_Description,pro_Status,pro_StartDate,pro_EndDate, pro_Manager FROM Projects";
        private const string ProjectTasksSELECT = "SELECT protsk_ID, protsk_Project, protsk_Name, protsk_Description,protsk_StartDate,protsk_EndDate, protsk_Status FROM ProjectTasks";
        private const string TaskEmployeesSELECT = "SELECT tskemp_ID,tskemp_Employee,tskDate,tskemp_Hours FROM TaskEmployees";

        //Table Names
        private const string EMPLOYEES_TABLENAME = "Employees";
        private const string PROJECTS_TABLENAME = "Projects";
        private const string PROJECTTASKS_TABLENAME = "ProjectTasks";
        private const string TASKEMPLOYEES_TABLENAME = "TaskEmployees";

        // Table indexes
        private int EmployeeTableIndex;
        private int ProjectsTableIndex;
        private int ProjectTasksTableIndex;
        private int TaskEmployeesTableIndex;

        //Filename Property
        private string _filename;
        public string FileName
        {
            get { return _filename; }
            set { _filename = value; }
        }

        //Constructor
        public ProjectTrackingDataSet()
        {
            FileName = DEFAULT_FILENAME;
            LoadDataSet();
        }

        //overloaded constructor - Accepts a filename
        public ProjectTrackingDataSet(string filename)
        {
            FileName = filename;
            LoadDataSet();
        }

        //Load the Dataset
        public void LoadDataSet()
        {
            //Clear the Tables and Relationships
            this.Relations.Clear();
            this.Tables.Clear();

            //Add tables tot he dataset
            AddTable(EmployeesSELECT, EMPLOYEES_TABLENAME);
            AddTable(ProjectsSELECT, PROJECTS_TABLENAME);
            AddTable(ProjectTasksSELECT, PROJECTTASKS_TABLENAME);
            AddTable(TaskEmployeesSELECT, TASKEMPLOYEES_TABLENAME);

            //set table indexes of tables
            EmployeeTableIndex = Tables.IndexOf(EMPLOYEES_TABLENAME);
            ProjectsTableIndex = Tables.IndexOf(PROJECTS_TABLENAME);
            ProjectTasksTableIndex = Tables.IndexOf(PROJECTTASKS_TABLENAME);
            TaskEmployeesTableIndex = Tables.IndexOf(TASKEMPLOYEES_TABLENAME);
            
            //add Primary keys
            AddPrimaryKey(Employees, Employees.Columns[0]);
            AddPrimaryKey(Projects, Projects.Columns[0]);
            AddPrimaryKey(ProjectTasks, ProjectTasks.Columns[0]);

            //create primary key for this table using multipule columns
            DataColumn[] TaskEmployeesPrimaryKeys = new DataColumn[3];
            TaskEmployeesPrimaryKeys[0] = ProjectTasks.Columns[0];
            TaskEmployeesPrimaryKeys[1] = ProjectTasks.Columns[1];
            TaskEmployeesPrimaryKeys[2] = ProjectTasks.Columns[2];

            AddPrimaryKey(TaskEmployees, TaskEmployeesPrimaryKeys);
            
            //create relationships between tables
            AddRelation(Employees, Projects, 6);
            AddRelation(Projects, ProjectTasks, 1);
            AddRelation(ProjectTasks, TaskEmployees, 0);
            AddRelation(Employees, TaskEmployees, 1);

            
        }

        //Employees DataTable Propery
        public DataTable Employees
        { get { return Tables[EmployeeTableIndex]; } }

        //Projects DataTable Propery
        public DataTable Projects
        { get { return Tables[ProjectsTableIndex]; } }

        //Project Tasks DataTable Propery
        public DataTable ProjectTasks
        { get { return Tables[ProjectTasksTableIndex]; } }

        //Task Employees DataTable Propery
        public DataTable TaskEmployees
        { get { return Tables[TaskEmployeesTableIndex]; } }

        //Add Table Method
        private void AddTable(string select, string tableName)
        { Tables.Add(DataServices.GetTable(FileName, select, tableName)); }

        //Add Primay Key method
        private void AddPrimaryKey(DataTable table, DataColumn column)
        {
            DataColumn[] dc = new DataColumn[1];
            dc[0] = column;
            table.PrimaryKey = dc;
        }

        // Overloaded Add Primary Keys Method
        // Accepts an array instead of a single column
        private void AddPrimaryKey(DataTable table, DataColumn[] columns)
        {
            try
            { table.PrimaryKey = columns; }
            catch (ArgumentException aEx)
            { Console.Write(aEx.Message); }
        }

        //Add Relation Method
        private void AddRelation(DataTable parent, DataTable child, int childColumnIndex)
        {
            string relationName = string.Format("FK_{0}_{1}", child.TableName, parent.TableName);
            DataColumn[] PrimaryKey = new DataColumn[1];
            DataColumn[] ForeignKey = new DataColumn[1];
            PrimaryKey = parent.PrimaryKey;
            ForeignKey[0] = child.Columns[childColumnIndex];
            try
            {
                DataRelation dr = new DataRelation(relationName, PrimaryKey, ForeignKey);
                Relations.Add(dr);
            }
            catch (Exception ex)
            { Console.Write(ex.Message); }
        }

        //Save DataSet Method
        public void SaveAll()
        {
            // if there are changes
            if (this.HasChanges())
            {
                if (Employees.GetChanges(DataRowState.Added) != null ||
                    Employees.GetChanges(DataRowState.Modified) != null)
                {
                    try
                    {
                        DataServices.SaveTable(FileName, EmployeesSELECT,
                            Employees.GetChanges(DataRowState.Added));
                    }
                    catch (Exception ex)
                    { Console.WriteLine(ex.Message); }

                    try
                    {
                        DataServices.SaveTable(FileName, EmployeesSELECT,
                            Employees.GetChanges(DataRowState.Modified));
                    }
                    catch (Exception ex)
                    { Console.WriteLine(ex.Message); }
                }

                // if there are rows to add or update
                if (TaskEmployees.GetChanges(DataRowState.Added) != null ||
                    TaskEmployees.GetChanges(DataRowState.Modified) != null)
                {
                    try
                    {
                        DataServices.SaveTable(FileName, TaskEmployeesSELECT,
                        TaskEmployees.GetChanges(DataRowState.Added));
                    }
                    catch (Exception ex)
                    { Console.WriteLine(ex.Message); }

                    try
                    {
                        DataServices.SaveTable(FileName, TaskEmployeesSELECT,
                            TaskEmployees.GetChanges(DataRowState.Modified));
                    }
                    catch (Exception ex)
                    { Console.WriteLine(ex.Message); }
                }

                // if there are rows to write to database
                if (ProjectTasks.GetChanges() != null)
                {
                    try
                    {
                        DataServices.SaveTable(FileName, ProjectTasksSELECT,
                            ProjectTasks.GetChanges());
                    }
                    catch (Exception ex)
                    { Console.WriteLine(ex.Message); }

                    try
                    {
                        DataServices.SaveTable(FileName, ProjectTasksSELECT,
                            ProjectTasks.GetChanges(DataRowState.Modified));
                    }
                    catch (Exception ex)
                    { Console.WriteLine(ex.Message); }
                }

                if (Projects.GetChanges(DataRowState.Deleted) != null)
                {
                    try
                    {
                        DataServices.SaveTable(FileName, ProjectsSELECT,
                            Projects.GetChanges(DataRowState.Deleted));
                    }
                    catch (Exception ex)
                    { Console.WriteLine(ex.Message); }

                    try
                    {
                        DataServices.SaveTable(FileName, ProjectsSELECT,
                            Projects.GetChanges(DataRowState.Modified));
                    }
                    catch (Exception ex)
                    { Console.WriteLine(ex.Message); }
                }
            }
        }
    }
}
