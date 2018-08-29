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
        private List<Fridge> check;
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

            check = new List<Fridge>();

            selling_frigo_info.Text = "";
            selling_check_info.Text = "";

            salling_cb_fridges.SelectedIndexChanged += Salling_cb_fridges_SelectedIndexChanged;
            selling_frigo_quantity.KeyPress += Selling_frigo_quantity_KeyPress;
            selling_frigo_quantity.KeyUp += Selling_frigo_quantity_KeyUp;
            selling_add_button.Click += Selling_add_button_Click;
            selling_cansel_btn.Click += Selling_cansel_btn_Click;
            selling_create_btn.Click += Selling_create_btn_Click;

            GetSellersFio();
            GetBuyersFio();
            GetFridges();
        }

        /// <summary>
        /// Продажа холодильников
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selling_create_btn_Click(object sender, EventArgs e)
        {
            if (selling_sellers.SelectedIndex == -1)
            {
                ShowMessage("Вы не выбрали продавца");
            }
            else if (selling_buyers.SelectedIndex == -1)
            {
                ShowMessage("Вы не выбрали покупателя");
            }
            else if (check.Count == 0)
            {
                ShowMessage("Вы не выбрали холодильник для покупки");
            }
            else
            {
                int seller_id;
                int buyer_id;

                seller_id = GetSellerId(selling_sellers.SelectedItem.ToString());
               
                if (seller_id != -1)
                {
                    buyer_id = GetBuyerId(selling_buyers.SelectedItem.ToString());

                    if (buyer_id != -1)
                    {
                        string sql = $"INSERT INTO cash_voucher VALUES ({seller_id}, {buyer_id}, GETDATE())";

                        SqlTransaction transaction = null;

                        try
                        {
                            conn.Open();

                            transaction = conn.BeginTransaction();

                            SqlCommand comm = conn.CreateCommand();
                            comm.Transaction = transaction;

                            comm.CommandText = sql;
                            comm.ExecuteNonQuery();
                            transaction.Commit();
                            conn.Close();

                            SaveCheckItems();

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message);
                            transaction.Rollback();
                        }

                        conn?.Close();
                    }
                    else
                    {
                        ShowMessage("Покупатель не найден!\nПродажа невозможна!");
                    }
                }
                else
                {
                    ShowMessage("Продавец не найден!\nПродажа невозможна!");
                }
            }
        }

        /// <summary>
        /// Отмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selling_cansel_btn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Доавление товара в чек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selling_add_button_Click(object sender, EventArgs e)
        {
            int quantity;

            if (selling_frigo_quantity.Text != "")
            {
                quantity = Convert.ToInt16(selling_frigo_quantity.Text);
            }
            else
            {
                quantity = 0;
            }

            if (quantity == 0)
            {
                ShowMessage("Вы не ввели количестово для покупки!");
            }
            else if (quantity > fridge.Quantity)
            {
                ShowMessage("Выбранная Вами модель,\nв нужном количестве\nотсутсвует на складе!");
            }
            else
            {
                fridge.Quantity = quantity;
                total += quantity * fridge.Price;
                check.Add(fridge);
                selling_check_info.Text += $"A   {fridge.Artikle}                    {fridge.Quantity} x {fridge.Price}\n" +
                    $"{fridge.Mark}   {fridge.Model}   Total: {fridge.Quantity * fridge.Price}\n";
                selling_lb_total.Text = total.ToString();
                salling_cb_fridges.Items.Remove(salling_cb_fridges.SelectedItem);
                selling_frigo_info.Text = "";
                selling_frigo_quantity.Text = "0";
                selling_frigo_quantity.Enabled = false;
                selling_add_button.Enabled = false;
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
                if (text.Length == 1 || text.Length == 0)
                {
                    selling_frigo_quantity.Text = "0";
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

        private void GetSellersFio()
        {
            string sql = "SELECT fio FROM sallers";

            try
            {
                ds = new DataSet();
                conn.Open();
                adapter = new SqlDataAdapter(sql, conn);

                cmd = new SqlCommandBuilder(adapter);
                adapter.Fill(ds, "sellers");

                DataRowCollection rows = ds.Tables["sellers"].Rows;

                foreach (DataRow row in rows)
                {
                    selling_sellers.Items.Add($"{row[0].ToString()}");
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

            conn?.Close();
        }

        private void GetBuyersFio()
        {
            string sql = "SELECT fio FROM buyers";

            try
            {
                ds = new DataSet();
                conn.Open();
                adapter = new SqlDataAdapter(sql, conn);

                cmd = new SqlCommandBuilder(adapter);
                adapter.Fill(ds, "buyers");

                DataRowCollection rows = ds.Tables["buyers"].Rows;

                foreach (DataRow row in rows)
                {
                    selling_buyers.Items.Add($"{row[0].ToString()}");
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

            conn?.Close();
        }

        /// <summary>
        /// Get seller id by fio
        /// </summary>
        /// <param name="fio"></param>
        /// <returns></returns>
        private int GetSellerId(string fio)
        {
            int id = -1;
            string sql = $"SELECT id FROM sallers WHERE fio = '{fio}'";

            try
            {
                ds = new DataSet();
                conn.Open();
                adapter = new SqlDataAdapter(sql, conn);

                cmd = new SqlCommandBuilder(adapter);
                adapter.Fill(ds, "sallers");

                DataRowCollection rows = ds.Tables["sallers"].Rows;

                id = Convert.ToInt16(rows[0][0]);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

            conn?.Close();

            return id;
        }

        /// <summary>
        /// Get seller id by fio
        /// </summary>
        /// <param name="fio"></param>
        /// <returns></returns>
        private int GetBuyerId(string fio)
        {
            int id = -1;
            string sql = $"SELECT id FROM buyers WHERE fio = '{fio}'";

            try
            {
                ds = new DataSet();
                conn.Open();
                adapter = new SqlDataAdapter(sql, conn);

                cmd = new SqlCommandBuilder(adapter);
                adapter.Fill(ds, "buyers");

                DataRowCollection rows = ds.Tables["buyers"].Rows;

                id = Convert.ToInt16(rows[0][0]);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

            conn?.Close();

            return id;
        }

        /// <summary>
        /// Save check item
        /// </summary>
        private void SaveCheckItems()
        {
            int checkId = -1;

            string sql = $"SELECT TOP 1 * FROM cash_voucher ORDER BY id DESC";

            try
            {
                ds = new DataSet();
                conn.Open();
                adapter = new SqlDataAdapter(sql, conn);

                cmd = new SqlCommandBuilder(adapter);
                adapter.Fill(ds, "check");

                DataRowCollection rows = ds.Tables["check"].Rows;

                checkId = Convert.ToInt16(rows[0][0]);

                conn?.Close();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

            if (checkId != -1)
            {
                foreach (Fridge item in check)
                {
                    AddItemsToTable(checkId, item);
                }
            }
            
        }

        private void AddItemsToTable(int checkId, Fridge f)
        {
            string sql = $"INSERT INTO cash_voucer_item VALUES ({checkId}, {f.Id}, {f.Quantity})";

            SqlTransaction transaction = null;

            try
            {
                conn.Open();

                transaction = conn.BeginTransaction();

                SqlCommand comm = conn.CreateCommand();
                comm.Transaction = transaction;

                comm.CommandText = sql;
                comm.ExecuteNonQuery();
                transaction.Commit();
                conn.Close();

                UpdateStorageInfo(f);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                transaction.Rollback();
            }

            conn?.Close();
        }

        private void UpdateStorageInfo(Fridge f)
        {
            string sql = $"UPDATE storage SET quantity = quantity - {f.Quantity} WHERE id_fridge = {f.Id};";
            
            SqlTransaction transaction = null;

            try
            {
                conn.Open();

                transaction = conn.BeginTransaction();

                SqlCommand comm = conn.CreateCommand();
                comm.Transaction = transaction;

                comm.CommandText = sql;
                comm.ExecuteNonQuery();
                transaction.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                transaction.Rollback();
            }

            conn?.Close();
        }
    }
}
