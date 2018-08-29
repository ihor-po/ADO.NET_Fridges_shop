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
        private Dictionary<string, string> check;
        private Fridge fridge;
        private double total;

        public Selling(string conString)
        {
            
            InitializeComponent();

            dbLink = conString;

            this.Load += Selling_Load;
        }

        private void Selling_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(dbLink);
            total = 0;

            selling_frigo_info.Text = "";

            salling_cb_fridges.SelectedIndexChanged += Salling_cb_fridges_SelectedIndexChanged;
            selling_frigo_quantity.KeyPress += Selling_frigo_quantity_KeyPress;
            selling_frigo_quantity.KeyUp += Selling_frigo_quantity_KeyUp;
            selling_add_button.Click += Selling_add_button_Click;

            GetFridges();
        }

        /// <summary>
        /// Доавление товара в чек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selling_add_button_Click(object sender, EventArgs e)
        {
            int quantity = Convert.ToInt16(selling_frigo_quantity.Text);
            if (quantity > fridge.Quantity)
            {
                ShowMessage("Выбранная Вами модель,\nв нужном количестве\nотсутсвует на складе!");
            }
        }

        /// <summary>
        /// Обработка нажатия кнопки вверх в поле количество
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selling_frigo_quantity_KeyUp(object sender, KeyEventArgs e)
        {
            string text = selling_frigo_quantity.Text;
            int textLength = text.Length;
            int number;

            if (text != "")
            {
                number = Convert.ToInt16(text);
            }
            else
            {
                number = 0;
            }
            
            if (e.KeyCode == Keys.Back)
            {
                if (text.Length == 1)
                {
                    selling_frigo_quantity.Text = "";
                }
                else
                {
                    selling_frigo_quantity.Text = text.Substring(0, --textLength);
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (text != "")
                {
                    selling_frigo_quantity.Text = (++number).ToString();
                }
                else
                {
                    selling_frigo_quantity.Text = "0";
                }
                
            }
            else if (e.KeyCode == Keys.Down)
            {
                
                if (number > 0)
                {
                    selling_frigo_quantity.Text = (--number).ToString();
                }
                else
                {
                    selling_frigo_quantity.Text = "0";
                }

            }
            selling_frigo_quantity.SelectionStart = textLength;
        }

        /// <summary>
        /// Обработка нажатия кнопок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selling_frigo_quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            char key = e.KeyChar;

            //Для проверки ввода только цифр

            if (!Char.IsDigit(key))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Change selected fridge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Salling_cb_fridges_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (salling_cb_fridges.SelectedItem.ToString() != "")
            {
                selling_frigo_quantity.Enabled = true;
                selling_frigo_quantity.Text = "0";
                selling_frigo_quantity.Focus();
                GetFridgoInfo(salling_cb_fridges.SelectedItem.ToString());
                selling_add_button.Enabled = true;
            }
            else
            {
                selling_frigo_quantity.Enabled = false;
                selling_frigo_quantity.Text = "";
                selling_frigo_info.Text = "";
                selling_add_button.Enabled = false;
            }
            //ShowMessage(salling_cb_fridges.SelectedItem.ToString());
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
                    salling_cb_fridges.Items.Add($"{row[0].ToString()}   {row[1].ToString()}");
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

        /// <summary>
        /// Get Fridgo info
        /// </summary>
        /// <param name="markModel"></param>
        private void GetFridgoInfo(string markModel)
        {
            int index = markModel.IndexOf("   ");
            int strLength = markModel.Length;
            string mark = markModel.Substring(0, index);
            string model = markModel.Substring(index + 3, strLength - (index + 3));

            string sql = $"SELECT " +
                $"fridges.id as 'Id', fridges.mark as 'Mark', fridges.model as 'Model', fridges.artikle as 'Artikle', " +
                $"fridges.price as 'Price', stor.quantity as 'Quantity' " +
                $"FROM fridges " +
                $"JOIN storage as stor ON stor.id_fridge = fridges.id " +
                $"WHERE mark = '{mark}' AND model = '{model}';" ;
            ShowMessage(sql);
            try
            {
                ds = new DataSet();

                conn.Open();

                adapter = new SqlDataAdapter(sql, conn);
                cmd = new SqlCommandBuilder(adapter);
                adapter.Fill(ds, "fre");
                DataRowCollection fRows = ds.Tables["fre"].Rows;

                fridge = new Fridge();

                fridge.Id = Convert.ToInt16(fRows[0][0]);
                fridge.Mark = fRows[0][1].ToString();
                fridge.Model = fRows[0][2].ToString();
                fridge.Artikle = fRows[0][3].ToString();
                fridge.Price = Convert.ToDouble(fRows[0][4]);
                fridge.Quantity = Convert.ToInt16(fRows[0][5]);

                selling_frigo_info.Text = $"\t\tМарка холодильника:\t{fridge.Mark}\n" +
                    $"\t\tМодель холодильника: {fridge.Model}\n" +
                    $"\t\tЦена холодильника: {fridge.Price}\n" +
                    $"\t\tОстаток на складе: {fridge.Quantity}";

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

            conn?.Close();
        }


    }
}
