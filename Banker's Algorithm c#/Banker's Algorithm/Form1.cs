using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Banker_s_Algorithm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int n, m;
        int num = 0;
        List<List<int>> allocation = new List<List<int>>();
        List<List<int>> max = new List<List<int>>();
        List<List<int>> need = new List<List<int>>();
        List<int> available = new List<int>();
        List<int> available2 = new List<int>();


        Label allocation_label = new Label();
        TextBox allocation_text = new TextBox();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedIndex = 1;
            if (num == 0)
            {
                n = int.Parse(textBox1.Text);  //number of processes
                m = int.Parse(textBox2.Text);  // number of resources

                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;

                label11.Visible = true;
                label11.Text = "Enter Allocation" + Environment.NewLine + "Matrix: ";
                textBox6.Visible = true;
                textBox6.AutoSize = false;
                textBox6.Size = new System.Drawing.Size(m * 20, n * 18);

                label12.Visible = true;
                label12.Text = "Enter Max" + Environment.NewLine  + "Matrix: ";
                textBox7.Visible = true;
                textBox7.AutoSize = false;
                textBox7.Size = new System.Drawing.Size(m * 20, n * 18); //width, height

                label10.Visible = true;
                label10.Text = "Enter available resources vector";
                textBox5.Visible = true;

                num++;
            }
            else if(num == 1)
            {
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                textBox7.ReadOnly = true;

                string textbox_string = textBox6.Text;
                string[] strings = Regex.Split(textbox_string, Environment.NewLine);

                foreach (string x in strings)
                {
                    string[] row_string = x.Split(' ');
                    int[] row_int = Array.ConvertAll(row_string, int.Parse);
                    List<int> row = row_int.ToList();
                    allocation.Add(row);
                }

                string textbox_string1 = textBox7.Text;
                string[] strings1 = Regex.Split(textbox_string1, Environment.NewLine);

                foreach (string x in strings1)
                {
                    string[] row_string = x.Split(' ');
                    int[] row_int = Array.ConvertAll(row_string, int.Parse);
                    List<int> row = row_int.ToList();
                    max.Add(row);
                    Console.WriteLine(String.Join("; ", row));
                }

                string[] row_string1 = textBox5.Text.Split(' ');
                int[] row_int1 = Array.ConvertAll(row_string1, int.Parse);
                available = row_int1.ToList();
                available2 = row_int1.ToList();
                Console.WriteLine(String.Join("; ", available));


                calculate_need_matrix();
                print_need_matrix();

                label3.Visible = true;
                label3.Text = "Do you want safe state ?";
                comboBox1.Visible = true;
                num++;
            }
            else if(num == 2)
            {
                string need_safe_or_request = comboBox1.SelectedText;
                selectedIndex = comboBox1.SelectedIndex; // 0: yes , 1: no
                string safe_string = "";
                if (selectedIndex == 0)
                {
                    List<int> safe_vector = new List<int>();
                    
                    selectedIndex = is_safe(safe_vector);
                    if(selectedIndex == 0)
                    {
                        safe_string = "Not Safe";
                        Console.WriteLine("Not Safe");
                    }
                    else
                    {
                        safe_string = "Yes, Safe state <";
                        for (int i = 0; i < n; i++)
                        {
                            safe_string += "P";
                            safe_string += safe_vector[i].ToString();
                            if (i != n - 1)
                            {
                                safe_string += ",";
                            }
                        }
                        safe_string += ">";
                    }
                    
                }
                print_safe_vector(safe_string, 0);
                Console.WriteLine(safe_string);
                label3.Visible = false;
                comboBox1.Visible = false;

                label4.Visible = true;
                label4.Text = "Do you want immediate requests ?";
                comboBox2.Visible = true;
                num++;
            }
            else if(num == 3)
            {
                selectedIndex = comboBox2.SelectedIndex; //yes:0, no:1
                if(selectedIndex == 0)
                {
                    label5.Visible = true;
                    label6.Visible = true;
                    label5.Text = "Enter number of process: ";
                    label6.Text = "Enter requested resources:";
                    textBox3.Visible = true;
                    textBox4.Visible = true;
                }
                else
                {
                    label7.Visible = true;
                    label7.Text = "Do you want to repeat again ?";
                    comboBox3.Visible = true;
                    num++;
                }
                num++;
            }
            else if(num == 4)
            {
                int requested_process = int.Parse(textBox3.Text);
                List<int> request = new List<int>();
                string[] row_string = textBox4.Text.Split(' ');
                int[] row_int = Array.ConvertAll(row_string, int.Parse);
                request = row_int.ToList();
                resource_request(request, requested_process);
         

                label7.Visible = true;
                label7.Text = "Do you want to repeat again ?";
                comboBox3.Visible = true;
                num++;
            }
            else if(num == 5)
            {
                selectedIndex = comboBox3.SelectedIndex; //yes:0, no:1
                if(selectedIndex == 0)
                {
                    foreach (Label _label in this.Controls.OfType<Label>())
                        _label.Text = string.Empty;
                    label4.Visible = false;
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = false;
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    textBox7.Clear();
                    textBox3.Visible = false;
                    textBox4.Visible = false;
                    textBox5.Visible = false;
                    textBox6.Visible = false;
                    textBox7.Visible = false;

                    comboBox2.Visible = false;
                    comboBox3.Visible = false;

                    allocation_label.Visible = false;
                    allocation_text.Visible = false;

                    label1.Visible = true;
                    label2.Visible = true;
                    label1.Text = "Enter number of processes:";
                    label2.Text = "Enter number of resources:";
                    textBox1.Visible = true;
                    textBox2.Visible = true;

                    textBox1.ReadOnly = false;
                    textBox2.ReadOnly = false;
                    textBox3.ReadOnly = false;
                    textBox4.ReadOnly = false;
                    textBox5.ReadOnly = false;
                    textBox6.ReadOnly = false;
                    textBox7.ReadOnly = false;

                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;
                    comboBox3.SelectedIndex = -1;

                    available.Clear();
                    available2.Clear();
                    allocation.Clear();
                    max.Clear();
                    need.Clear();
                    allocation.Clear();

                    num = 0;
                }
                else
                    this.Close();
            }
        }

        private void resource_request(List<int> request, int process)
        {
            bool requesting = true;
            string request_string = "";

            for (int i = 0; i < m; i++)
            {
                if (request[i] > need[process][i])
                {
                    requesting = false;
                    break;
                }
            }

            if (requesting)
            {
                for (int i = 0; i < m; i++)
                {
                    if (request[i] > available[i])
                    {
                        requesting = false;
                        break;
                    }
                }
            }
            else
            {
                request_string = "Process has exceeded its maximum claim";
                return;
            }

            if (requesting)
            {
                for (int i = 0; i < m; i++)
                {
                    available2[i] -= request[i];
                    allocation[process][i] += request[i];
                    need[process][i] -= request[i];
                }
                List<int> safe_vector = new List<int>();
                int safe = is_safe(safe_vector);
                if (safe == 0)
                {
                    for (int i = 0; i < m; i++)
                    {
                        available[i] += request[i];
                        allocation[process][i] -= request[i];
                        need[process][i] += request[i];
                    }
                    request_string = "Request can not be granted as no safe state";
                }
                else
                {
                    request_string = "Yes request can be granted with safe state, Safe state <P";
                    request_string += process.ToString();
                    request_string += "req,";
                    for (int i = 0; i < n; i++)
                    {
                        request_string += "P";
                        request_string += safe_vector[i].ToString();
                        if (i != n - 1)
                        {
                            request_string += ",";
                        }
                    }
                    request_string += ">";
                }
            }
            else
                request_string = "Process must wait resources not available";

            print_safe_vector(request_string, 1);
        }

        private int is_safe(List<int> safe)
        {
            bool found = true;
            List<int> finish = new List<int>(n);
            finish.AddRange(Enumerable.Repeat(0, n));
            for (int i = 0; i < m; i++)
            {
                available[i] = available2[i];
            }
            while(found)
            {
                found = false;
                for (int i = 0; i < n; i++)
                {
                    if (finish[i] == 0)
                    {
                        found = true;

                        for (int j = 0; j < m; j++)
                        {
                            if (need[i][j] > available[j])
                            {
                                found = false;
                            }
                        }
                        if (found)
                        {
                            safe.Add(i);
                            for (int j = 0; j < m; j++)
                            {
                                available[j] = available[j] + allocation[i][j];
                            }
                            finish[i] = 1;
                        }
                    }
                }

            }
            int x = safe.Count();
            return x;
        }

        private void calculate_need_matrix()
        {
            int num;
            for(int i = 0; i < n; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < m; j++)
                {
                    num = max[i][j] - allocation[i][j];
                    row.Add(num);
                }
                need.Add(row);
            }
            Console.WriteLine(String.Join("; ", need[0]));
        }

        private void print_need_matrix()
        {
            label9.Visible = true;
            label9.Text = "Need Matrix:";

            for (int i = 0; i < m; i++)
            {
                Label R = new Label();
                R.Text = "R" + i;
                R.Location = new Point(620 + i*20, 130);
                R.AutoSize = true;
                R.Font = new Font("Calibri", 10);
                R.ForeColor = Color.Black;
                R.Visible = true;
                this.Controls.Add(R);
            }

            for (int i = 0; i < n; i++)
            {
                Label P = new Label();
                P.Text = "P" + i;
                P.Location = new Point(600, 150 + i * 20);
                P.AutoSize = true;
                P.Font = new Font("Calibri", 10);
                P.ForeColor = Color.Black;
                P.Visible = true;
                this.Controls.Add(P);

                for (int j = 0; j < m; j++)
                {
                    Label need0 = new Label();
                    need0.Text = need[i][j].ToString();
                    need0.Location = new Point(625 + j * 20, 150 + i * 20);
                    need0.AutoSize = true;
                    need0.Font = new Font("Calibri", 10);
                    need0.ForeColor = Color.Black;
                    need0.Visible = true;
                    this.Controls.Add(need0);
                }
            }
        }

        private void print_safe_vector(string safe, int num)
        {
            Label safe_label = new Label();
            safe_label.Text = safe;
            safe_label.Location = new Point(600, 190 + m*20 + num *20);
            safe_label.AutoSize = true;
            safe_label.Font = new Font("Calibri", 10);
            safe_label.ForeColor = Color.Black;
            safe_label.Visible = true;


            // Adding this control to the form
            this.Controls.Add(safe_label);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

    }
}
