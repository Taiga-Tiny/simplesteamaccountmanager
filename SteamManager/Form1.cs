using Microsoft.Win32;
using System.Diagnostics;

namespace SteamManager
{
    public partial class Form1 : Form
    {
        string path =@"example";//Path for your stored user and pass file ex: d:data.txt
        struct user
        {
            public user(string user, string pass)
            {
                username = user;
                password = pass;
            }
            public string username { get; set; }
            public string password { get; set; }
        }
        List<user> users = new List<user>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Username and Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        
            }
            else
            {
                List<string> lines = File.ReadAllLines(path).ToList();
                var user = new user(textBox1.Text, textBox2.Text);
                users.Add(user);
                lines.Add(user.username + "," + user.password);
                File.WriteAllLines(path, lines);

                comboBox1.Items.Clear();
                textBox1.Clear();
                textBox2.Clear();

                foreach (user v in users)
                {
                    comboBox1.Items.Add(v.username);
                }  
                MessageBox.Show("New user have been added.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            if (!File.Exists(path))
            {
                StreamWriter sw = File.CreateText(path);
                sw.Flush();
                sw.Dispose();
            }
            List<string> lines = File.ReadAllLines(path).ToList();
            foreach (var line in lines)
            {
                string[] entries = line.Split(',');
                user user = new user(entries[0], entries[1]);
                comboBox1.Items.Add(entries[0]);
                users.Add(user);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("steam"))
            {
                process.Kill();
            }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamExe", null);
            startInfo.Arguments = " -noreactlogin -login "+ users[comboBox1.SelectedIndex].username + " " + users[comboBox1.SelectedIndex].password;          
            Process.Start(startInfo);
        }

     
    }
}