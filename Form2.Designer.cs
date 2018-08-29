namespace FridgeShop
{
    partial class Selling
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
            this.salling_gb_frego = new System.Windows.Forms.GroupBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.salling_cb_fridges = new System.Windows.Forms.ComboBox();
            this.selling_frigo_quantity = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selling_frigo_info = new System.Windows.Forms.Label();
            this.salling_gb_frego.SuspendLayout();
            this.SuspendLayout();
            // 
            // salling_gb_frego
            // 
            this.salling_gb_frego.Controls.Add(this.selling_frigo_info);
            this.salling_gb_frego.Controls.Add(this.label1);
            this.salling_gb_frego.Controls.Add(this.selling_frigo_quantity);
            this.salling_gb_frego.Controls.Add(this.salling_cb_fridges);
            this.salling_gb_frego.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.salling_gb_frego.Location = new System.Drawing.Point(34, 121);
            this.salling_gb_frego.Name = "salling_gb_frego";
            this.salling_gb_frego.Size = new System.Drawing.Size(460, 165);
            this.salling_gb_frego.TabIndex = 13;
            this.salling_gb_frego.TabStop = false;
            this.salling_gb_frego.Text = "Холодильники";
            // 
            // comboBox2
            // 
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(172, 69);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(320, 28);
            this.comboBox2.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(30, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Покупатель";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(30, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Продавец";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(172, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(320, 28);
            this.comboBox1.TabIndex = 9;
            // 
            // salling_cb_fridges
            // 
            this.salling_cb_fridges.FormattingEnabled = true;
            this.salling_cb_fridges.Location = new System.Drawing.Point(6, 25);
            this.salling_cb_fridges.Name = "salling_cb_fridges";
            this.salling_cb_fridges.Size = new System.Drawing.Size(260, 28);
            this.salling_cb_fridges.TabIndex = 0;
            // 
            // selling_frigo_quantity
            // 
            this.selling_frigo_quantity.Enabled = false;
            this.selling_frigo_quantity.Location = new System.Drawing.Point(397, 25);
            this.selling_frigo_quantity.Name = "selling_frigo_quantity";
            this.selling_frigo_quantity.Size = new System.Drawing.Size(57, 26);
            this.selling_frigo_quantity.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(283, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Количество";
            // 
            // selling_frigo_info
            // 
            this.selling_frigo_info.AutoSize = true;
            this.selling_frigo_info.Location = new System.Drawing.Point(6, 69);
            this.selling_frigo_info.Name = "selling_frigo_info";
            this.selling_frigo_info.Size = new System.Drawing.Size(51, 20);
            this.selling_frigo_info.TabIndex = 3;
            this.selling_frigo_info.Text = "label4";
            // 
            // Selling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 394);
            this.Controls.Add(this.salling_gb_frego);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Name = "Selling";
            this.Text = "Selling";
            this.salling_gb_frego.ResumeLayout(false);
            this.salling_gb_frego.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox salling_gb_frego;
        private System.Windows.Forms.ComboBox salling_cb_fridges;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label selling_frigo_info;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox selling_frigo_quantity;
    }
}