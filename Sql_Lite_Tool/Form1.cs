using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
using System.Data.Sql;
using System.Data.SqlClient;


namespace Sql_Lite_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void findToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Create form explore and have selection passed back...

            List<string> sqlList = new List<string>();

            DataTable dt = SqlDataSourceEnumerator.Instance.GetDataSources();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["InstanceName"].Equals(string.Empty))
                {
                    sqlList.Add(string.Concat(dr["ServerName"], "\\" , Environment.NewLine));
                }
                else
                {
                    sqlList.Add(string.Concat(dr["ServerName"], "\\", dr["InstanceName"], Environment.NewLine));
                }
            }

            richTextBox1.WordWrap = false;

            foreach (string sqlSrvLst in sqlList)
            {
                richTextBox1.AppendText(sqlSrvLst);
            }

            richTextBox1.Select(0, richTextBox1.Lines[0].Length);
        }

        //

        private void exploreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //select * from master.sys.databases
            //execute us based upon item from the generated list from the above outcome
            //SELECT name FROM Sysobjects where xtype = 'u'

            //add sql connect and query below...

            getTopDataBase();

        }

        private void getTopDataBase()
        {
            textBox1.Text = richTextBox1.SelectedText.Replace(Environment.NewLine, "");
            //textBox1.Text = richTextBox1.Lines[0];
            textBox1.Refresh();

            using (var con = new SqlConnection("Data Source=" +  @textBox1.Text + "; Integrated Security=True;"))
            {
                try
                {
                    con.Open();
                    DataTable databases = con.GetSchema("Databases");
                    con.Close();

                    foreach (DataRow database in databases.Rows)
                    {
                        richTextBox2.AppendText(database.Field<String>("database_name") + Environment.NewLine);
                    }
                }
                catch (Exception e)
                {
                    richTextBox2.Clear();
                    richTextBox2.Text = e.ToString();
                }
            } 
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //select * from master.sys.databases
            //execute us based upon item from the generated list from the above outcome
            //SELECT name FROM Sysobjects where xtype = 'u'

            //add sql connect and query below...

            getTopDataBase();
        }

        private void schemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // get the select DB name
            richTextBox2.Select(0, richTextBox2.Lines[0].Length);

            // connect and get the schema then close...
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                List<string> holdit = new List<string>();
                holdit = grabSchema(textBox3.Text, textBox4.Text);

                foreach (string element in holdit)
                {
                    richTextBox1.AppendText(element + "\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private List<string> grabSchema(string DTBToGrabSchemaFor, string grabDB)
        {
            String ConnectionString;
            int countIT = 0;

            String[] holdit;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            String catstring = "";
            String sqlcommand = "";

            try
            {
                List<string> sBase = new List<string>();

                ConnectionString = @"server=PAUL-HEI-LAP\SQLEXPRESS;user=" + textBox2.Text + ";password=" + textBox3.Text + ";Integrated Security = True;database=" + textBox4.Text;
                SqlConnection cn = new SqlConnection(ConnectionString);
                cn.Open();

                sqlcommand = "exec sp_columns @table_name = " + DTBToGrabSchemaFor;

                SqlDataAdapter transactions_1 = new SqlDataAdapter(sqlcommand, cn);
                transactions_1.Fill(dt);

                // Loop through all entries
                foreach (DataRow drRow in dt.Rows)
                {
                    foreach (var item in drRow.ItemArray)
                    {
                        catstring = catstring + item + " ";
                    }

                    holdit = catstring.Split(' ');

                    sBase.Add(holdit[3]);

                    countIT += 1;

                    catstring = "";
                }

                cn.Close();
                return sBase;

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        private void recordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void schemaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string ConnectionString = @"server=PAUL-HEI-LAP\SQLEXPRESS;user=" + textBox2.Text + ";password=" + textBox3.Text + ";Integrated Security = True;database=" + textBox4.Text;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                // Connect to the database then retrieve the schema information.
                string[] restrictions = new string[4];
                restrictions[2] = "pB_Acct";

                connection.Open();
                DataTable table = connection.GetSchema("Tables", restrictions);

                // Display the contents of the table.
                DisplayData(table);
            }
        }

        private void recordsToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void DisplayData(System.Data.DataTable table)
        {
            foreach (System.Data.DataRow row in table.Rows)
            {
                foreach (System.Data.DataColumn col in table.Columns)
                {
                    richTextBox2.AppendText(col.ColumnName + Environment.NewLine);
                }
            }
        }
    }
}
