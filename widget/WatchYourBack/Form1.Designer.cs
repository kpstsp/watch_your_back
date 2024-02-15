namespace WatchYourBack
{
    partial class Form1
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
            this.userListBox = new System.Windows.Forms.ListBox();
            this.currentUsersLabel = new System.Windows.Forms.Label();
            this.sharesDataGridView = new System.Windows.Forms.DataGridView();
            this.username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remoteHost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sharedFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PinButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sharesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // userListBox
            // 
            this.userListBox.FormattingEnabled = true;
            this.userListBox.ItemHeight = 16;
            this.userListBox.Location = new System.Drawing.Point(12, 34);
            this.userListBox.Name = "userListBox";
            this.userListBox.Size = new System.Drawing.Size(400, 260);
            this.userListBox.TabIndex = 0;
            // 
            // currentUsersLabel
            // 
            this.currentUsersLabel.AutoSize = true;
            this.currentUsersLabel.Location = new System.Drawing.Point(12, 9);
            this.currentUsersLabel.Name = "currentUsersLabel";
            this.currentUsersLabel.Size = new System.Drawing.Size(88, 16);
            this.currentUsersLabel.TabIndex = 1;
            this.currentUsersLabel.Text = "Current Users";
            // 
            // sharesDataGridView
            // 
            this.sharesDataGridView.AllowUserToAddRows = false;
            this.sharesDataGridView.AllowUserToDeleteRows = false;
            this.sharesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sharesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.username,
            this.remoteHost,
            this.sharedFolder});
            this.sharesDataGridView.Location = new System.Drawing.Point(15, 343);
            this.sharesDataGridView.Name = "sharesDataGridView";
            this.sharesDataGridView.ReadOnly = true;
            this.sharesDataGridView.RowHeadersWidth = 51;
            this.sharesDataGridView.RowTemplate.Height = 24;
            this.sharesDataGridView.Size = new System.Drawing.Size(552, 272);
            this.sharesDataGridView.TabIndex = 2;
            // 
            // username
            // 
            this.username.HeaderText = "username";
            this.username.MinimumWidth = 6;
            this.username.Name = "username";
            this.username.ReadOnly = true;
            this.username.Width = 125;
            // 
            // remoteHost
            // 
            this.remoteHost.HeaderText = "remoteHost";
            this.remoteHost.MinimumWidth = 6;
            this.remoteHost.Name = "remoteHost";
            this.remoteHost.ReadOnly = true;
            this.remoteHost.Width = 125;
            // 
            // sharedFolder
            // 
            this.sharedFolder.HeaderText = "sharedFolder";
            this.sharedFolder.MinimumWidth = 6;
            this.sharedFolder.Name = "sharedFolder";
            this.sharedFolder.ReadOnly = true;
            this.sharedFolder.Width = 125;
            // 
            // PinButton
            // 
            this.PinButton.Location = new System.Drawing.Point(794, 12);
            this.PinButton.Name = "PinButton";
            this.PinButton.Size = new System.Drawing.Size(119, 23);
            this.PinButton.TabIndex = 3;
            this.PinButton.Text = "PIN/UNPIN";
            this.PinButton.UseVisualStyleBackColor = true;
            this.PinButton.Click += new System.EventHandler(this.PinButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 742);
            this.Controls.Add(this.PinButton);
            this.Controls.Add(this.sharesDataGridView);
            this.Controls.Add(this.currentUsersLabel);
            this.Controls.Add(this.userListBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sharesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox userListBox;
        private System.Windows.Forms.Label currentUsersLabel;
        private System.Windows.Forms.DataGridView sharesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn username;
        private System.Windows.Forms.DataGridViewTextBoxColumn remoteHost;
        private System.Windows.Forms.DataGridViewTextBoxColumn sharedFolder;
        private System.Windows.Forms.Button PinButton;
    }
}

