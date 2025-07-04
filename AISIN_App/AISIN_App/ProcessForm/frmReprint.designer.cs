namespace AISIN_App
{
    partial class frmReprint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReprint));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblHeader = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageWithPartBarcode = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBinBarcode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.txtPartBarcode = new System.Windows.Forms.TextBox();
            this.tabPageWithOutPartBarcode = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGet = new System.Windows.Forms.Button();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.btnCloseWithOutPart = new System.Windows.Forms.Button();
            this.btnPrintWithOutPart = new System.Windows.Forms.Button();
            this.btnResetWithOutPart = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.BinBarcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.txtKanBan = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbLineNo = new System.Windows.Forms.ComboBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageWithPartBarcode.SuspendLayout();
            this.tabPageWithOutPartBarcode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Cambria", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(684, 27);
            this.lblHeader.TabIndex = 6;
            this.lblHeader.Text = "REPRINT";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.lblMessage);
            this.panel1.Location = new System.Drawing.Point(6, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(672, 417);
            this.panel1.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageWithPartBarcode);
            this.tabControl1.Controls.Add(this.tabPageWithOutPartBarcode);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(670, 383);
            this.tabControl1.TabIndex = 187;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPageWithPartBarcode
            // 
            this.tabPageWithPartBarcode.Controls.Add(this.label1);
            this.tabPageWithPartBarcode.Controls.Add(this.txtBinBarcode);
            this.tabPageWithPartBarcode.Controls.Add(this.label6);
            this.tabPageWithPartBarcode.Controls.Add(this.btnClose);
            this.tabPageWithPartBarcode.Controls.Add(this.btnPrint);
            this.tabPageWithPartBarcode.Controls.Add(this.btnReset);
            this.tabPageWithPartBarcode.Controls.Add(this.txtPartBarcode);
            this.tabPageWithPartBarcode.Location = new System.Drawing.Point(4, 22);
            this.tabPageWithPartBarcode.Name = "tabPageWithPartBarcode";
            this.tabPageWithPartBarcode.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWithPartBarcode.Size = new System.Drawing.Size(662, 357);
            this.tabPageWithPartBarcode.TabIndex = 0;
            this.tabPageWithPartBarcode.Text = "WITH PART BARCODE";
            this.tabPageWithPartBarcode.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 19);
            this.label1.TabIndex = 188;
            this.label1.Text = "Bin Barcode";
            // 
            // txtBinBarcode
            // 
            this.txtBinBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBinBarcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtBinBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtBinBarcode.Location = new System.Drawing.Point(136, 63);
            this.txtBinBarcode.Name = "txtBinBarcode";
            this.txtBinBarcode.ReadOnly = true;
            this.txtBinBarcode.Size = new System.Drawing.Size(520, 22);
            this.txtBinBarcode.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(4, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 19);
            this.label6.TabIndex = 159;
            this.label6.Text = "Scan Part Barcode";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(72)))), ((int)(((byte)(146)))));
            this.btnClose.Image = global::AISIN_App.Properties.Resources.Delete;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClose.Location = new System.Drawing.Point(594, 298);
            this.btnClose.Margin = new System.Windows.Forms.Padding(5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 55);
            this.btnClose.TabIndex = 174;
            this.btnClose.Text = "&Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(72)))), ((int)(((byte)(146)))));
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrint.Location = new System.Drawing.Point(439, 298);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(69, 56);
            this.btnPrint.TabIndex = 186;
            this.btnPrint.Text = "&Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.BackColor = System.Drawing.Color.Transparent;
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatAppearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnReset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(72)))), ((int)(((byte)(146)))));
            this.btnReset.Image = global::AISIN_App.Properties.Resources._1336028501_001_39;
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnReset.Location = new System.Drawing.Point(518, 298);
            this.btnReset.Margin = new System.Windows.Forms.Padding(5);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(66, 55);
            this.btnReset.TabIndex = 173;
            this.btnReset.Text = "&Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // txtPartBarcode
            // 
            this.txtPartBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPartBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtPartBarcode.Location = new System.Drawing.Point(136, 22);
            this.txtPartBarcode.Name = "txtPartBarcode";
            this.txtPartBarcode.Size = new System.Drawing.Size(520, 22);
            this.txtPartBarcode.TabIndex = 0;
            this.txtPartBarcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPartBarcode_KeyPress);
            // 
            // tabPageWithOutPartBarcode
            // 
            this.tabPageWithOutPartBarcode.Controls.Add(this.label7);
            this.tabPageWithOutPartBarcode.Controls.Add(this.label3);
            this.tabPageWithOutPartBarcode.Controls.Add(this.btnGet);
            this.tabPageWithOutPartBarcode.Controls.Add(this.dtpToDate);
            this.tabPageWithOutPartBarcode.Controls.Add(this.label5);
            this.tabPageWithOutPartBarcode.Controls.Add(this.dtpFromDate);
            this.tabPageWithOutPartBarcode.Controls.Add(this.label11);
            this.tabPageWithOutPartBarcode.Controls.Add(this.btnCloseWithOutPart);
            this.tabPageWithOutPartBarcode.Controls.Add(this.btnPrintWithOutPart);
            this.tabPageWithOutPartBarcode.Controls.Add(this.btnResetWithOutPart);
            this.tabPageWithOutPartBarcode.Controls.Add(this.dgv);
            this.tabPageWithOutPartBarcode.Controls.Add(this.label2);
            this.tabPageWithOutPartBarcode.Controls.Add(this.txtKanBan);
            this.tabPageWithOutPartBarcode.Controls.Add(this.label9);
            this.tabPageWithOutPartBarcode.Controls.Add(this.cmbLineNo);
            this.tabPageWithOutPartBarcode.Location = new System.Drawing.Point(4, 22);
            this.tabPageWithOutPartBarcode.Name = "tabPageWithOutPartBarcode";
            this.tabPageWithOutPartBarcode.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWithOutPartBarcode.Size = new System.Drawing.Size(662, 357);
            this.tabPageWithOutPartBarcode.TabIndex = 1;
            this.tabPageWithOutPartBarcode.Text = "WITHOUT  PART BARCODE";
            this.tabPageWithOutPartBarcode.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(97, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 19);
            this.label7.TabIndex = 226;
            this.label7.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(97, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 19);
            this.label3.TabIndex = 224;
            this.label3.Text = "*";
            // 
            // btnGet
            // 
            this.btnGet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(166)))));
            this.btnGet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGet.FlatAppearance.BorderSize = 0;
            this.btnGet.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnGet.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnGet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGet.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGet.ForeColor = System.Drawing.Color.White;
            this.btnGet.Location = new System.Drawing.Point(567, 171);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(87, 27);
            this.btnGet.TabIndex = 223;
            this.btnGet.Text = "Get";
            this.btnGet.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGet.UseVisualStyleBackColor = false;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // dtpToDate
            // 
            this.dtpToDate.CalendarFont = new System.Drawing.Font("Calibri", 12F);
            this.dtpToDate.CustomFormat = "dd-MMM-yyyy";
            this.dtpToDate.Font = new System.Drawing.Font("Calibri", 12F);
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(326, 52);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(142, 27);
            this.dtpToDate.TabIndex = 222;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 12F);
            this.label5.Location = new System.Drawing.Point(262, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 19);
            this.label5.TabIndex = 221;
            this.label5.Text = "To Date";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CalendarFont = new System.Drawing.Font("Calibri", 12F);
            this.dtpFromDate.CustomFormat = "dd-MMM-yyyy";
            this.dtpFromDate.Font = new System.Drawing.Font("Calibri", 12F);
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(119, 51);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(137, 27);
            this.dtpFromDate.TabIndex = 220;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri", 12F);
            this.label11.Location = new System.Drawing.Point(23, 52);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 19);
            this.label11.TabIndex = 219;
            this.label11.Text = "From Date";
            // 
            // btnCloseWithOutPart
            // 
            this.btnCloseWithOutPart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseWithOutPart.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseWithOutPart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseWithOutPart.FlatAppearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnCloseWithOutPart.FlatAppearance.BorderSize = 0;
            this.btnCloseWithOutPart.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCloseWithOutPart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCloseWithOutPart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCloseWithOutPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseWithOutPart.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseWithOutPart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(72)))), ((int)(((byte)(146)))));
            this.btnCloseWithOutPart.Image = global::AISIN_App.Properties.Resources.Delete;
            this.btnCloseWithOutPart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCloseWithOutPart.Location = new System.Drawing.Point(593, 297);
            this.btnCloseWithOutPart.Margin = new System.Windows.Forms.Padding(5);
            this.btnCloseWithOutPart.Name = "btnCloseWithOutPart";
            this.btnCloseWithOutPart.Size = new System.Drawing.Size(66, 55);
            this.btnCloseWithOutPart.TabIndex = 191;
            this.btnCloseWithOutPart.Text = "&Close";
            this.btnCloseWithOutPart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCloseWithOutPart.UseVisualStyleBackColor = false;
            this.btnCloseWithOutPart.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrintWithOutPart
            // 
            this.btnPrintWithOutPart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintWithOutPart.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintWithOutPart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPrintWithOutPart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrintWithOutPart.FlatAppearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnPrintWithOutPart.FlatAppearance.BorderSize = 0;
            this.btnPrintWithOutPart.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrintWithOutPart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintWithOutPart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintWithOutPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintWithOutPart.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintWithOutPart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(72)))), ((int)(((byte)(146)))));
            this.btnPrintWithOutPart.Image = ((System.Drawing.Image)(resources.GetObject("btnPrintWithOutPart.Image")));
            this.btnPrintWithOutPart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrintWithOutPart.Location = new System.Drawing.Point(438, 297);
            this.btnPrintWithOutPart.Name = "btnPrintWithOutPart";
            this.btnPrintWithOutPart.Size = new System.Drawing.Size(69, 56);
            this.btnPrintWithOutPart.TabIndex = 192;
            this.btnPrintWithOutPart.Text = "&Print";
            this.btnPrintWithOutPart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrintWithOutPart.UseVisualStyleBackColor = false;
            this.btnPrintWithOutPart.Click += new System.EventHandler(this.btnPrintWithOutPart_Click);
            // 
            // btnResetWithOutPart
            // 
            this.btnResetWithOutPart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetWithOutPart.BackColor = System.Drawing.Color.Transparent;
            this.btnResetWithOutPart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResetWithOutPart.FlatAppearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnResetWithOutPart.FlatAppearance.BorderSize = 0;
            this.btnResetWithOutPart.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnResetWithOutPart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnResetWithOutPart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnResetWithOutPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetWithOutPart.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetWithOutPart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(72)))), ((int)(((byte)(146)))));
            this.btnResetWithOutPart.Image = global::AISIN_App.Properties.Resources._1336028501_001_39;
            this.btnResetWithOutPart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnResetWithOutPart.Location = new System.Drawing.Point(517, 297);
            this.btnResetWithOutPart.Margin = new System.Windows.Forms.Padding(5);
            this.btnResetWithOutPart.Name = "btnResetWithOutPart";
            this.btnResetWithOutPart.Size = new System.Drawing.Size(66, 55);
            this.btnResetWithOutPart.TabIndex = 190;
            this.btnResetWithOutPart.Text = "&Reset";
            this.btnResetWithOutPart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnResetWithOutPart.UseVisualStyleBackColor = false;
            this.btnResetWithOutPart.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dgv.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Garamond", 12.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BinBarcode,
            this.PrintOn});
            this.dgv.EnableHeadersVisualStyles = false;
            this.dgv.GridColor = System.Drawing.Color.AliceBlue;
            this.dgv.Location = new System.Drawing.Point(6, 202);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.dgv.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(650, 92);
            this.dgv.StandardTab = true;
            this.dgv.TabIndex = 189;
            this.dgv.TabStop = false;
            // 
            // BinBarcode
            // 
            this.BinBarcode.DataPropertyName = "BinBarcode";
            this.BinBarcode.HeaderText = "Bin Barcode";
            this.BinBarcode.Name = "BinBarcode";
            this.BinBarcode.ReadOnly = true;
            // 
            // PrintOn
            // 
            this.PrintOn.DataPropertyName = "PrintOn";
            this.PrintOn.HeaderText = "Print Date";
            this.PrintOn.Name = "PrintOn";
            this.PrintOn.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 19);
            this.label2.TabIndex = 181;
            this.label2.Text = "Scan Kanban";
            // 
            // txtKanBan
            // 
            this.txtKanBan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKanBan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtKanBan.Location = new System.Drawing.Point(118, 90);
            this.txtKanBan.Multiline = true;
            this.txtKanBan.Name = "txtKanBan";
            this.txtKanBan.Size = new System.Drawing.Size(538, 76);
            this.txtKanBan.TabIndex = 180;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 12F);
            this.label9.Location = new System.Drawing.Point(23, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 19);
            this.label9.TabIndex = 179;
            this.label9.Text = "Select Line";
            // 
            // cmbLineNo
            // 
            this.cmbLineNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLineNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLineNo.Font = new System.Drawing.Font("Calibri", 12F);
            this.cmbLineNo.FormattingEnabled = true;
            this.cmbLineNo.Location = new System.Drawing.Point(120, 13);
            this.cmbLineNo.Name = "cmbLineNo";
            this.cmbLineNo.Size = new System.Drawing.Size(536, 27);
            this.cmbLineNo.TabIndex = 178;
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.LightCoral;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(0, 383);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(670, 32);
            this.lblMessage.TabIndex = 139;
            this.lblMessage.Text = "test";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMessage.DoubleClick += new System.EventHandler(this.lblMessage_DoubleClick);
            // 
            // frmReprint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(166)))));
            this.ClientSize = new System.Drawing.Size(684, 466);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmReprint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "REPRINT";
            this.Load += new System.EventHandler(this.frmModelMaster_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageWithPartBarcode.ResumeLayout(false);
            this.tabPageWithPartBarcode.PerformLayout();
            this.tabPageWithOutPartBarcode.ResumeLayout(false);
            this.tabPageWithOutPartBarcode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtPartBarcode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageWithPartBarcode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBinBarcode;
        private System.Windows.Forms.TabPage tabPageWithOutPartBarcode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtKanBan;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbLineNo;
        private System.Windows.Forms.Button btnCloseWithOutPart;
        private System.Windows.Forms.Button btnPrintWithOutPart;
        private System.Windows.Forms.Button btnResetWithOutPart;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblMessage;
        public System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn BinBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrintOn;
    }
}