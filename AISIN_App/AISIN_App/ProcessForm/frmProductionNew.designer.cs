namespace AISIN_App
{
    partial class frmProductionNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProductionNew));
            this.lblShift = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnDeleteBin = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblBackNo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbPartBarcode = new System.Windows.Forms.ListBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnServicePart = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.lblLineNo = new System.Windows.Forms.Label();
            this.lblScanQty = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnFraction = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblBinQty = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPartBarcode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtKanbanBarcode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblShift
            // 
            this.lblShift.BackColor = System.Drawing.Color.Transparent;
            this.lblShift.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblShift.Font = new System.Drawing.Font("Cambria", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShift.ForeColor = System.Drawing.Color.White;
            this.lblShift.Location = new System.Drawing.Point(0, 0);
            this.lblShift.Name = "lblShift";
            this.lblShift.Size = new System.Drawing.Size(974, 27);
            this.lblShift.TabIndex = 6;
            this.lblShift.Text = "PRODUCTION";
            this.lblShift.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlMain
            // 
            this.pnlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Controls.Add(this.btnDeleteBin);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.lblBackNo);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.lbPartBarcode);
            this.pnlMain.Controls.Add(this.lblMessage);
            this.pnlMain.Controls.Add(this.btnServicePart);
            this.pnlMain.Controls.Add(this.label10);
            this.pnlMain.Controls.Add(this.lblLineNo);
            this.pnlMain.Controls.Add(this.lblScanQty);
            this.pnlMain.Controls.Add(this.label14);
            this.pnlMain.Controls.Add(this.btnClose);
            this.pnlMain.Controls.Add(this.btnFraction);
            this.pnlMain.Controls.Add(this.btnReset);
            this.pnlMain.Controls.Add(this.lblBinQty);
            this.pnlMain.Controls.Add(this.label8);
            this.pnlMain.Controls.Add(this.txtPartBarcode);
            this.pnlMain.Controls.Add(this.label7);
            this.pnlMain.Controls.Add(this.txtKanbanBarcode);
            this.pnlMain.Controls.Add(this.label6);
            this.pnlMain.Location = new System.Drawing.Point(6, 38);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(962, 439);
            this.pnlMain.TabIndex = 8;
            // 
            // btnDeleteBin
            // 
            this.btnDeleteBin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteBin.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteBin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteBin.FlatAppearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnDeleteBin.FlatAppearance.BorderSize = 0;
            this.btnDeleteBin.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDeleteBin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDeleteBin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDeleteBin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteBin.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteBin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(72)))), ((int)(((byte)(146)))));
            this.btnDeleteBin.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteBin.Image")));
            this.btnDeleteBin.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDeleteBin.Location = new System.Drawing.Point(830, 173);
            this.btnDeleteBin.Margin = new System.Windows.Forms.Padding(5);
            this.btnDeleteBin.Name = "btnDeleteBin";
            this.btnDeleteBin.Size = new System.Drawing.Size(125, 69);
            this.btnDeleteBin.TabIndex = 205;
            this.btnDeleteBin.Text = "Delete old Part Barcode";
            this.btnDeleteBin.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteBin.UseVisualStyleBackColor = false;
            this.btnDeleteBin.Click += new System.EventHandler(this.btnDeleteBin_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(855, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 33);
            this.label2.TabIndex = 203;
            this.label2.Text = "BackNo";
            // 
            // lblBackNo
            // 
            this.lblBackNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBackNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(166)))));
            this.lblBackNo.Font = new System.Drawing.Font("Cambria", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBackNo.ForeColor = System.Drawing.Color.White;
            this.lblBackNo.Location = new System.Drawing.Point(850, 43);
            this.lblBackNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBackNo.Name = "lblBackNo";
            this.lblBackNo.Size = new System.Drawing.Size(106, 46);
            this.lblBackNo.TabIndex = 202;
            this.lblBackNo.Text = "YS10";
            this.lblBackNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 249);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 19);
            this.label1.TabIndex = 201;
            this.label1.Text = "Part Barcode";
            // 
            // lbPartBarcode
            // 
            this.lbPartBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPartBarcode.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold);
            this.lbPartBarcode.FormattingEnabled = true;
            this.lbPartBarcode.ItemHeight = 26;
            this.lbPartBarcode.Location = new System.Drawing.Point(3, 276);
            this.lbPartBarcode.Name = "lbPartBarcode";
            this.lbPartBarcode.Size = new System.Drawing.Size(950, 82);
            this.lbPartBarcode.TabIndex = 200;
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.LightCoral;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Font = new System.Drawing.Font("Cambria", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(0, 360);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(960, 77);
            this.lblMessage.TabIndex = 199;
            this.lblMessage.Text = "Enter User Name";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMessage.DoubleClick += new System.EventHandler(this.lblMessage_DoubleClick);
            // 
            // btnServicePart
            // 
            this.btnServicePart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnServicePart.BackColor = System.Drawing.Color.Transparent;
            this.btnServicePart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnServicePart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnServicePart.FlatAppearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnServicePart.FlatAppearance.BorderSize = 0;
            this.btnServicePart.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnServicePart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnServicePart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnServicePart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServicePart.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnServicePart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(72)))), ((int)(((byte)(146)))));
            this.btnServicePart.Image = global::AISIN_App.Properties.Resources.iconfinder_Save_1493294;
            this.btnServicePart.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnServicePart.Location = new System.Drawing.Point(491, 172);
            this.btnServicePart.Name = "btnServicePart";
            this.btnServicePart.Size = new System.Drawing.Size(102, 56);
            this.btnServicePart.TabIndex = 192;
            this.btnServicePart.Text = "&Service Part";
            this.btnServicePart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnServicePart.UseVisualStyleBackColor = false;
            this.btnServicePart.Click += new System.EventHandler(this.btnServicePart_Click);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Maroon;
            this.label10.Location = new System.Drawing.Point(720, 138);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(163, 19);
            this.label10.TabIndex = 191;
            this.label10.Text = "(**Scan again to reject)";
            // 
            // lblLineNo
            // 
            this.lblLineNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLineNo.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLineNo.ForeColor = System.Drawing.Color.Navy;
            this.lblLineNo.Location = new System.Drawing.Point(6, 235);
            this.lblLineNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLineNo.Name = "lblLineNo";
            this.lblLineNo.Size = new System.Drawing.Size(953, 39);
            this.lblLineNo.TabIndex = 188;
            this.lblLineNo.Text = "Line No : ";
            this.lblLineNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScanQty
            // 
            this.lblScanQty.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblScanQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(166)))));
            this.lblScanQty.Font = new System.Drawing.Font("Calibri", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScanQty.ForeColor = System.Drawing.Color.White;
            this.lblScanQty.Location = new System.Drawing.Point(356, 158);
            this.lblScanQty.Name = "lblScanQty";
            this.lblScanQty.Size = new System.Drawing.Size(128, 70);
            this.lblScanQty.TabIndex = 173;
            this.lblScanQty.Text = "0";
            this.lblScanQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(382, 134);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 19);
            this.label14.TabIndex = 172;
            this.label14.Text = "Scan Qty";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.btnClose.Location = new System.Drawing.Point(747, 172);
            this.btnClose.Margin = new System.Windows.Forms.Padding(5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 55);
            this.btnClose.TabIndex = 171;
            this.btnClose.Text = "&Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnFraction
            // 
            this.btnFraction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFraction.BackColor = System.Drawing.Color.Transparent;
            this.btnFraction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFraction.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFraction.FlatAppearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnFraction.FlatAppearance.BorderSize = 0;
            this.btnFraction.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnFraction.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFraction.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFraction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFraction.Font = new System.Drawing.Font("Cambria", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFraction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(72)))), ((int)(((byte)(146)))));
            this.btnFraction.Image = global::AISIN_App.Properties.Resources.iconfinder_arrow_right_18140851;
            this.btnFraction.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFraction.Location = new System.Drawing.Point(595, 172);
            this.btnFraction.Name = "btnFraction";
            this.btnFraction.Size = new System.Drawing.Size(82, 56);
            this.btnFraction.TabIndex = 168;
            this.btnFraction.Text = "&Fraction";
            this.btnFraction.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFraction.UseVisualStyleBackColor = false;
            this.btnFraction.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.btnReset.Location = new System.Drawing.Point(684, 172);
            this.btnReset.Margin = new System.Windows.Forms.Padding(5);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(57, 55);
            this.btnReset.TabIndex = 169;
            this.btnReset.Text = "&Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblBinQty
            // 
            this.lblBinQty.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBinQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(166)))));
            this.lblBinQty.Font = new System.Drawing.Font("Calibri", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBinQty.ForeColor = System.Drawing.Color.White;
            this.lblBinQty.Location = new System.Drawing.Point(210, 158);
            this.lblBinQty.Name = "lblBinQty";
            this.lblBinQty.Size = new System.Drawing.Size(133, 70);
            this.lblBinQty.TabIndex = 166;
            this.lblBinQty.Text = "0";
            this.lblBinQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(216, 134);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 19);
            this.label8.TabIndex = 164;
            this.label8.Text = "Standard Bin Qty";
            // 
            // txtPartBarcode
            // 
            this.txtPartBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPartBarcode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPartBarcode.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPartBarcode.Location = new System.Drawing.Point(197, 80);
            this.txtPartBarcode.Name = "txtPartBarcode";
            this.txtPartBarcode.Size = new System.Drawing.Size(649, 47);
            this.txtPartBarcode.TabIndex = 1;
            this.txtPartBarcode.TextChanged += new System.EventHandler(this.txtPartBarcode_TextChanged);
            this.txtPartBarcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPartBarcode_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(57, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 19);
            this.label7.TabIndex = 161;
            this.label7.Text = "Scan Part Barcode";
            // 
            // txtKanbanBarcode
            // 
            this.txtKanbanBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKanbanBarcode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtKanbanBarcode.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKanbanBarcode.Location = new System.Drawing.Point(197, 3);
            this.txtKanbanBarcode.Multiline = true;
            this.txtKanbanBarcode.Name = "txtKanbanBarcode";
            this.txtKanbanBarcode.Size = new System.Drawing.Size(649, 75);
            this.txtKanbanBarcode.TabIndex = 0;
            this.txtKanbanBarcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKanbanBarcode_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(5, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(186, 19);
            this.label6.TabIndex = 159;
            this.label6.Text = "Scan Kanban/Bin Barcode";
            // 
            // frmProductionNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(166)))));
            this.ClientSize = new System.Drawing.Size(974, 488);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.lblShift);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmProductionNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PRODUCTION";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmModelMaster_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lblShift;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.TextBox txtPartBarcode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtKanbanBarcode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblBinQty;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnFraction;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblScanQty;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblLineNo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnServicePart;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbPartBarcode;
        private System.Windows.Forms.Label lblBackNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDeleteBin;
    }
}