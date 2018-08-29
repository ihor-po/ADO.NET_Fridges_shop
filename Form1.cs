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
using System.Configuration;

namespace FridgeShop
{
    public partial class main_form : Form
    {
        private string dbLink;
        private SqlConnection conn;
        private SqlDataAdapter adapter;
        private DataSet ds;
        private SqlCommandBuilder cmd;

        public main_form()
        {
            InitializeComponent();

            this.Load += Main_form_Load;
        }

        private void Main_form_Load(object sender, EventArgs e)
        {
            dbLink = ConfigurationManager.ConnectionStrings["LocalConnectionString"].ConnectionString;
            conn = new SqlConnection(dbLink);

            main_form_dv.CellDoubleClick += Main_form_dv_CellDoubleClick;
            main_form_createBtn.Click += Main_form_createBtn_Click;

            GetCashVouchers();
            
        }

        private void Main_form_createBtn_Click(object sender, EventArgs e)
        {
            Selling sellingForm = new Selling(dbLink);

            if (sellingForm.ShowDialog() == DialogResult.OK)
            {
                GetCashVouchers();
            }
        }

        /// <summary>
        /// Двойной клик по ячейке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_form_dv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow currentRow = main_form_dv.CurrentRow;
            
            if (currentRow?.Cells[0]?.Value is int)
            {
                string sql;
                string items = "\tКупленный товар:\n";

                int cashId = Convert.ToInt16(currentRow?.Cells[0]?.Value);
                string info = $"\t\tНомер чека: {currentRow?.Cells[0]?.Value}\n\n";
                info += $"\tПродавец: {currentRow?.Cells[2]?.Value}\n";
                info += $"\tПокупатель: {currentRow?.Cells[3]?.Value}\n";

                sql = "SELECT * FROM cash_voucer_item WHERE cash_voucher_id = " + cashId;

                try
                {
                    DataSet tmp = new DataSet();
                    conn.Open();
                    adapter = new SqlDataAdapter(sql, conn);

                    cmd = new SqlCommandBuilder(adapter);
                    adapter.Fill(tmp);
                    DataRowCollection rows = tmp.Tables[0].Rows;

                    foreach(DataRow row in rows)
                    {
                        int fregId = Convert.ToInt16(row[2]);
                        int quantity = Convert.ToInt16(row[3]);

                        sql = "SELECT artikle, mark, model, price FROM fridges WHERE id = " + fregId;
                         
                        conn?.Close();
                        conn.Open();
                        adapter = new SqlDataAdapter(sql, conn);
                        cmd = new SqlCommandBuilder(adapter);
                        adapter.Fill(tmp, "fre" + fregId);
                        DataRowCollection fRows = tmp.Tables["fre" + fregId].Rows;

                        items += $"\t\t {fRows[0][0]}   {fRows[0][1]}   {fRows[0][2]}   {quantity} X {fRows[0][3]}\n";
                    }

                    info += items;
                                   }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
                }
                conn?.Close();

                MessageBox.Show(info, this.Text);
            }
        }

        /// <summary>
        /// Получение списка всех чеков
        /// </summary>
        private void GetCashVouchers()
        {
            string sql = "SELECT " +
                "cash_voucher.id as 'ID', cash_voucher.date as 'Дата продажи', " +
                "slr.fio as 'Продавец', " +
                "brs.fio as 'Покупатель', brs.phone as 'Телефон покупателя', " +
                "SUM(frdg.price * cvi.quantity) as 'Цена чека' " +
                "FROM cash_voucher " +
                "INNER JOIN sallers as slr ON cash_voucher.seller_id = slr.id " +
                "INNER JOIN buyers as brs ON cash_voucher.buyer_id = brs.id " +
                "INNER JOIN cash_voucer_item as cvi ON cash_voucher.id = cvi.cash_voucher_id " +
                "INNER JOIN fridges as frdg ON cvi.id_fridge = frdg.id " +
                "GROUP BY cash_voucher.id, cash_voucher.date, slr.fio, brs.fio, brs.phone";
            try
            {
                ds = new DataSet();
                conn.Open();
                adapter = new SqlDataAdapter(sql, conn);
                main_form_dv.DataSource = null;

                cmd = new SqlCommandBuilder(adapter);
                adapter.Fill(ds, "CachVoucher");

                main_form_dv.DataSource = ds.Tables["CachVoucher"];
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
