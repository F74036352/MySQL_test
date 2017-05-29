using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;//must

namespace MySQL_test
{    
    public partial class Form1 : Form
    {
        string host;
        string id;
        string pw;
        string db;
        public Form1()
        {
            InitializeComponent();
            label6.Visible = false;
            label7.Visible = false;
            textBox5.Visible = false;
            comboBox1.Visible = false;
            dataGridView1.Visible = false;
            button3.Visible = false;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //建立MySql Connection 
            var conn = new MySql.Data.MySqlClient.MySqlConnection
            {
                ConnectionString = "server=" + textBox1.Text + ";uid=" + textBox2.Text + ";pwd=" + textBox3.Text + ";database=" + textBox4.Text
            };
            conn.Open();

            MessageBox.Show(conn.State == ConnectionState.Open ? "This connection is right.\n You can press another button to start. " : "Connect Error! \n There are something wrong.");
            //關閉
            conn.Close();
        }      

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            using (MySqlConnection conn = new MySqlConnection())
            {
                host = textBox1.Text;
                id = textBox2.Text;
                pw = textBox3.Text;
                db = textBox4.Text;
                conn.ConnectionString = "server=" + host + ";uid=" + id + ";pwd=" + pw + ";database=" + db;                
                conn.Open();
                MessageBox.Show(conn.State == ConnectionState.Open ? "Connect Success " : "Connect Error!");
                
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                label6.Visible = true;
                label7.Visible = true;
                textBox5.Visible = true;
                comboBox1.Visible = true;
                dataGridView1.Visible = true;
                button3.Visible = true;
                conn.Close();                
            }                       

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            using (MySqlConnection conn = new MySqlConnection())
            {                
                conn.ConnectionString = "server=" + host + ";uid=" + id + ";pwd=" + pw + ";database=" + db;
                conn.Open();
                //MessageBox.Show(conn.State == ConnectionState.Open ? "Connect Success " : "Connect Error!");                
                //conn.Close();                
                using (MySqlCommand command = conn.CreateCommand())
                {
                    if (comboBox1.SelectedItem.ToString() == "MYSQL")
                    {
                        command.CommandText = textBox5.Text;
                        textBox5.Clear();
                    }
                    else if (comboBox1.SelectedItem.ToString() == "professor\'s salary(>=N)")
                    {
                        string sql;
                        sql="select * from professor where salary >= ";
                        command.CommandText = sql + textBox5.Text + ";";
                        textBox5.Clear();
                    }
                    else if (comboBox1.SelectedItem.ToString() == "student\'s id")
                    {
                        string sql;
                        sql = "select * from student where id=\"" + textBox5.Text + "\" ;";
                        command.CommandText = sql;
                        textBox5.Clear();
                    }
                    else if (comboBox1.SelectedItem.ToString() == "delete professor(pid)")
                    {
                        string sql;
                        sql="delete from professor where pid=\"" + textBox5.Text +"\";";
                        command.CommandText = sql;
                        textBox5.Clear();
                    }
                    else if (comboBox1.SelectedItem.ToString() == "insert professor(pid,pname,p_dep,salary)")
                    {
                        char[] cut = {' ','\t','.',':',','};
                        string[] professor=textBox5.Text.Split(cut);
                        string sql="insert into professor (pid,pname,p_dep,salary) values (\'" + professor[0] + "\'," + "\'" + professor[1] + "\'," + "\'" + professor[2] + "\'," + professor[3] + ");";
                        command.CommandText = sql;
                        textBox5.Clear();
                    }
                    else if (comboBox1.SelectedItem.ToString() == "update student\'s club(club,id)")
                    {
                        char[] cut = {'\t','.',':',','};
                        string[] club=textBox5.Text.Split(cut);
                        string sql;
                        sql = "update student set contact_club = " + "\'" + club[0] + "\' " + "where id = " + "\'" + club[1] + "\';";
                        command.CommandText = sql;
                        textBox5.Clear();
                    }
                    else if (comboBox1.SelectedItem.ToString() == "department location in(dep_location)")
                    {
                        char[] cut = { '\t', '.', ':', ',' };
                        string[] location = textBox5.Text.Split(cut);
                        string sql="select * from department where dep_location in (" + "\'" + location[0] +"\'";
                        for (int i=1;i<location.Length;++i)
                        {
                            if (location[i] != null )
                            {
                                sql = sql + ",\'" + location[i] + "\'";
                            }
                        }
                        sql = sql + ");";
                        command.CommandText = sql;
                        textBox5.Clear();
                    }
                    else if (comboBox1.SelectedItem.ToString() == "department location not in(dep_location)")
                    {
                        char[] cut = { '\t', '.', ':', ',' };
                        string[] location = textBox5.Text.Split(cut);
                        string sql = "select * from department where dep_location not in (" + "\'" + location[0] + "\'";
                        for (int i = 1; i < location.Length; ++i)
                        {
                            if (location[i] != null)
                            {
                                sql = sql + ",\'" + location[i] + "\'";
                            }
                        }
                        sql = sql + ");";
                        command.CommandText = sql;
                        textBox5.Clear();
                    }
                    else if (comboBox1.SelectedItem.ToString() == "EXISTS(>=N)")
                    {
                        string sql;
                        sql = "select * from student where EXISTS (select * from club where club_memberN >=" + textBox5.Text + ");";
                        command.CommandText = sql;
                        textBox5.Clear();
                    }
                    else if (comboBox1.SelectedItem.ToString() == "NOT EXISTS(>=N)")
                    {
                        string sql;
                        sql = "select * from student where NOT EXISTS (select * from club where club_memberN >=" + textBox5.Text + ");";
                        command.CommandText = sql;
                        textBox5.Clear();
                    }
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    try
                    {
                        da.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0].Copy();
                        this.dataGridView1.DataSource = dt;
                    }
                    catch
                    {
                        
                    }
                }
                conn.Close(); 
            }
            /*
            string tmp;
            tmp = textBox5.Text.ToLower();
            if (tmp.Contains("select") )
            {
                DataTable dt = new DataTable();
                dt = ds.Tables[0].Copy();
                this.dataGridView1.DataSource = dt;
            }
            else
            {
                dataGridView1.DataSource = null;
            }
             */
             
        }
              

    }
}
