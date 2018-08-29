using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FridgeShop
{
    public partial class Selling : Form
    {
        private string dbLink;
        private SqlConnection conn;
        private SqlDataAdapter adapter;
        private DataSet ds;
        private SqlCommandBuilder cmd;

        public Selling(string conString)
        {
            
            InitializeComponent();

            dbLink = conString;

            this.Load += Selling_Load;
        }

        private void Selling_Load(object sender, EventArgs e)
        {
          
            conn = new SqlConnection(dbLink);

            GetFridges();
        }

        /// <summary>
        /// Get fridges from DB
        /// </summary>
        private void GetFridges()
        {
            string sql = "SELECT mark, model FROM fridges";

            try
            {
                ds = new DataSet();
                conn.Open();
                adapter = new SqlDataAdapter(sql, conn);

                cmd = new SqlCommandBuilder(adapter);
                adapter.Fill(ds, "fridges");

                DataRowCollection rows = ds.Tables["fridges"].Rows;

                foreach (DataRow row in rows)
                {
                    salling_cb_fridges.Items.Add(row[0].ToString() + "   " + row[1].ToString());
                }


            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

            conn?.Close();
        }

        /// <summary>
        /// Отображение сообщения
        /// </summary>
        /// <param name="msg"></param>
        private void ShowMessage(string msg)
        {
            MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
