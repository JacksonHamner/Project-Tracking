namespace ProjectTracking
{
    partial class EmployeeProjectsView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label emp_IDLabel;
            System.Windows.Forms.Label emp_FirstNameLabel;
            System.Windows.Forms.Label emp_LastNameLabel;
            System.Windows.Forms.Label emp_TitleLabel;
            this.txtEmpID = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lvProjects = new System.Windows.Forms.ListView();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            emp_IDLabel = new System.Windows.Forms.Label();
            emp_FirstNameLabel = new System.Windows.Forms.Label();
            emp_LastNameLabel = new System.Windows.Forms.Label();
            emp_TitleLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // emp_IDLabel
            // 
            emp_IDLabel.AutoSize = true;
            emp_IDLabel.Location = new System.Drawing.Point(33, 15);
            emp_IDLabel.Name = "emp_IDLabel";
            emp_IDLabel.Size = new System.Drawing.Size(70, 13);
            emp_IDLabel.TabIndex = 1;
            emp_IDLabel.Text = "Employee ID:";
            // 
            // emp_FirstNameLabel
            // 
            emp_FirstNameLabel.AutoSize = true;
            emp_FirstNameLabel.Location = new System.Drawing.Point(33, 41);
            emp_FirstNameLabel.Name = "emp_FirstNameLabel";
            emp_FirstNameLabel.Size = new System.Drawing.Size(60, 13);
            emp_FirstNameLabel.TabIndex = 3;
            emp_FirstNameLabel.Text = "First Name:";
            // 
            // emp_LastNameLabel
            // 
            emp_LastNameLabel.AutoSize = true;
            emp_LastNameLabel.Location = new System.Drawing.Point(33, 67);
            emp_LastNameLabel.Name = "emp_LastNameLabel";
            emp_LastNameLabel.Size = new System.Drawing.Size(61, 13);
            emp_LastNameLabel.TabIndex = 5;
            emp_LastNameLabel.Text = "Last Name:";
            // 
            // emp_TitleLabel
            // 
            emp_TitleLabel.AutoSize = true;
            emp_TitleLabel.Location = new System.Drawing.Point(33, 93);
            emp_TitleLabel.Name = "emp_TitleLabel";
            emp_TitleLabel.Size = new System.Drawing.Size(30, 13);
            emp_TitleLabel.TabIndex = 7;
            emp_TitleLabel.Text = "Title:";
            // 
            // txtEmpID
            // 
            this.txtEmpID.Enabled = false;
            this.txtEmpID.Location = new System.Drawing.Point(123, 12);
            this.txtEmpID.Name = "txtEmpID";
            this.txtEmpID.Size = new System.Drawing.Size(55, 20);
            this.txtEmpID.TabIndex = 2;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Enabled = false;
            this.txtFirstName.Location = new System.Drawing.Point(123, 38);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(121, 20);
            this.txtFirstName.TabIndex = 4;
            // 
            // txtLastName
            // 
            this.txtLastName.Enabled = false;
            this.txtLastName.Location = new System.Drawing.Point(123, 64);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(121, 20);
            this.txtLastName.TabIndex = 6;
            // 
            // txtTitle
            // 
            this.txtTitle.Enabled = false;
            this.txtTitle.Location = new System.Drawing.Point(123, 90);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(121, 20);
            this.txtTitle.TabIndex = 8;
            // 
            // lvProjects
            // 
            this.lvProjects.Location = new System.Drawing.Point(11, 174);
            this.lvProjects.Name = "lvProjects";
            this.lvProjects.Size = new System.Drawing.Size(291, 145);
            this.lvProjects.TabIndex = 9;
            this.lvProjects.UseCompatibleStateImageBehavior = false;
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(231, 141);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(44, 23);
            this.btnLast.TabIndex = 66;
            this.btnLast.Text = ">]";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(164, 141);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(44, 23);
            this.btnNext.TabIndex = 65;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(99, 141);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(44, 23);
            this.btnPrevious.TabIndex = 64;
            this.btnPrevious.Text = "<<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(37, 141);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(44, 23);
            this.btnFirst.TabIndex = 63;
            this.btnFirst.Text = "[<";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(112, 325);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 67;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // EmployeeProjectsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 356);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.lvProjects);
            this.Controls.Add(emp_IDLabel);
            this.Controls.Add(this.txtEmpID);
            this.Controls.Add(emp_FirstNameLabel);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(emp_LastNameLabel);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(emp_TitleLabel);
            this.Controls.Add(this.txtTitle);
            this.Name = "EmployeeProjectsView";
            this.Text = "EmployeeProjectsView";
            this.Load += new System.EventHandler(this.EmployeeProjectsView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEmpID;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.ListView lvProjects;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnClose;

    }
}