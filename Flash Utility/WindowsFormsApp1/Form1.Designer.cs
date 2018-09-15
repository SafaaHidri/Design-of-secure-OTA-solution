namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.serial = new System.Windows.Forms.GroupBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tcp = new System.Windows.Forms.GroupBox();
            this.txtB_Port = new System.Windows.Forms.TextBox();
            this.txtB_ipaddress = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ble = new System.Windows.Forms.GroupBox();
            this.FileName = new System.Windows.Forms.TextBox();
            this.FilePath = new System.Windows.Forms.Button();
            this.ChoiceBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.openFileDLG = new System.Windows.Forms.OpenFileDialog();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.serial.SuspendLayout();
            this.tcp.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.CausesValidation = false;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(172, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = " Interface";
            // 
            // serial
            // 
            this.serial.Controls.Add(this.comboBox2);
            this.serial.Controls.Add(this.comboBox1);
            this.serial.Controls.Add(this.label3);
            this.serial.Controls.Add(this.label2);
            this.serial.Location = new System.Drawing.Point(488, 190);
            this.serial.Name = "serial";
            this.serial.Size = new System.Drawing.Size(210, 202);
            this.serial.TabIndex = 4;
            this.serial.TabStop = false;
            this.serial.Text = "Serial Communication";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "9600",
            "115200"});
            this.comboBox2.Location = new System.Drawing.Point(84, 119);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 3;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(84, 38);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 2;
           
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Baud Rate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Port";
            // 
            // tcp
            // 
            this.tcp.Controls.Add(this.txtB_Port);
            this.tcp.Controls.Add(this.txtB_ipaddress);
            this.tcp.Controls.Add(this.label5);
            this.tcp.Controls.Add(this.label4);
            this.tcp.Location = new System.Drawing.Point(26, 190);
            this.tcp.Name = "tcp";
            this.tcp.Size = new System.Drawing.Size(210, 202);
            this.tcp.TabIndex = 5;
            this.tcp.TabStop = false;
            this.tcp.Text = "TCP/IP";
            // 
            // txtB_Port
            // 
            this.txtB_Port.Location = new System.Drawing.Point(85, 127);
            this.txtB_Port.Name = "txtB_Port";
            this.txtB_Port.Size = new System.Drawing.Size(100, 20);
            this.txtB_Port.TabIndex = 10;
            // 
            // txtB_ipaddress
            // 
            this.txtB_ipaddress.Location = new System.Drawing.Point(85, 43);
            this.txtB_ipaddress.Name = "txtB_ipaddress";
            this.txtB_ipaddress.Size = new System.Drawing.Size(100, 20);
            this.txtB_ipaddress.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "IP Address";
            // 
            // ble
            // 
            this.ble.Location = new System.Drawing.Point(267, 190);
            this.ble.Name = "ble";
            this.ble.Size = new System.Drawing.Size(210, 202);
            this.ble.TabIndex = 8;
            this.ble.TabStop = false;
            this.ble.Text = "BLE";
            // 
            // FileName
            // 
            this.FileName.Location = new System.Drawing.Point(28, 42);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(481, 20);
            this.FileName.TabIndex = 9;
           
            // 
            // FilePath
            // 
            this.FilePath.Location = new System.Drawing.Point(593, 34);
            this.FilePath.Name = "FilePath";
            this.FilePath.Size = new System.Drawing.Size(82, 28);
            this.FilePath.TabIndex = 10;
            this.FilePath.Text = "Browse File";
            this.FilePath.UseVisualStyleBackColor = true;
            this.FilePath.Click += new System.EventHandler(this.button2_Click);
            // 
            // ChoiceBox
            // 
            this.ChoiceBox.FormattingEnabled = true;
            this.ChoiceBox.Items.AddRange(new object[] {
            "TCP/IP",
            "BLE",
            "UART"});
            this.ChoiceBox.Location = new System.Drawing.Point(291, 10);
            this.ChoiceBox.Name = "ChoiceBox";
            this.ChoiceBox.Size = new System.Drawing.Size(205, 21);
            this.ChoiceBox.TabIndex = 11;
            this.ChoiceBox.SelectedIndexChanged += new System.EventHandler(this.ChoiceBox_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(569, 412);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Download";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FileName);
            this.groupBox1.Controls.Add(this.FilePath);
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(686, 100);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ChoiceBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(691, 45);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Communication";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 447);
            this.Controls.Add(this.serial);
            this.Controls.Add(this.tcp);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ble);
            this.Name = "Form1";
            this.Text = "BootLoader Utility";
            this.serial.ResumeLayout(false);
            this.serial.PerformLayout();
            this.tcp.ResumeLayout(false);
            this.tcp.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox serial;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox tcp;
        private System.Windows.Forms.TextBox txtB_Port;
        private System.Windows.Forms.TextBox txtB_ipaddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox ble;
        private System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.Button FilePath;
        private System.Windows.Forms.ComboBox ChoiceBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.OpenFileDialog openFileDLG;
        private System.IO.Ports.SerialPort serialPort1;
    }
}

