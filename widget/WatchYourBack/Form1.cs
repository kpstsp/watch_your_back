using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WatchYourBack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PopulateRemoteConnections();
            RefreshUserList();
            // Start a timer to refresh the user list every 10 seconds
            var timer = new System.Windows.Forms.Timer();
            timer.Tick += (s, ev) => RefreshUserList();
            timer.Interval = 10000; // 10 seconds
            timer.Start();

            
        }

        private void RefreshUserList()
        {
            //var activeUsers = GetActiveUsers();

            //var activeUsers = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            List<string> userNames = new List<string>();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string userName = obj["UserName"] as string;
                        if (!string.IsNullOrEmpty(userName))
                        {
                            userNames.Add(userName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("OOPs error here");
                userNames.Add("NIHUA!");
            }
            DisplayActiveUsers(userNames);
        }
        private void DisplayActiveUsers(List<string> users)
        {
            userListBox.Items.Clear();
            userListBox.Items.AddRange(users.ToArray());
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            // Refresh the DataGridView every tick
            PopulateRemoteConnections();
        }

        private void PopulateRemoteConnections()
        {
            // Clear existing rows
            sharesDataGridView.Rows.Clear();

            try
            {
                var sharedFolders = GetSharedFolders();
                // Run the "net session" command to get remote sessions
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = "cmd.exe";
                processStartInfo.Arguments = "/c net session";
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;

                using (Process process = Process.Start(processStartInfo))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // Parse the output and add to DataGridView
                            if (line.StartsWith("\\"))
                            {
                                string[] parts = line.Split(new[] { ' ', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                                if (parts.Length >= 4)
                                {
                                    string remoteHost = parts[0];
                                    string username = parts[1];
                                    //string clientType = parts[2];
                                    string sharedFolder = FindSharedFolder(remoteHost, sharedFolders);
                                    //string sharedFolder = parts[3];

                                    sharesDataGridView.Rows.Add(username, remoteHost, sharedFolder);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving remote connections: " + ex.Message);
            }
        }

        private List<string> GetSharedFolders()
        {
            List<string> sharedFolders = new List<string>();

            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = "cmd.exe";
                processStartInfo.Arguments = "/c net share";
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;

                using (Process process = Process.Start(processStartInfo))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // Parse the output to extract shared folder names
                            if (line.StartsWith("    "))
                            {
                                sharedFolders.Add(line.TrimStart().Split(' ')[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving shared folders: " + ex.Message);
            }

            return sharedFolders;
        }

        private string FindSharedFolder(string remoteHost, List<string> sharedFolders)
        {
            // Match the shared folder with the given remote host
            foreach (var folder in sharedFolders)
            {
                if (remoteHost.Equals(GetRemoteHostFromPath(folder), StringComparison.OrdinalIgnoreCase))
                {
                    return folder;
                }
            }
            return "Unknown";
        }

        private string GetRemoteHostFromPath(string path)
        {
            // Extracts the remote host name from a shared folder path
            int index = path.IndexOf('\\', 2);
            if (index != -1)
            {
                return path.Substring(2, index - 2);
            }
            return path;
        }
    }
}
