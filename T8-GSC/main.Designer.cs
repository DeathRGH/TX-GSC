namespace T8_GSC
{
	partial class main
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
			this.panel_bottom = new System.Windows.Forms.Panel();
			this.panel_top = new System.Windows.Forms.Panel();
			this.panel_left = new System.Windows.Forms.Panel();
			this.panel_right = new System.Windows.Forms.Panel();
			this.topBar = new System.Windows.Forms.Panel();
			this.minimize = new System.Windows.Forms.Button();
			this.exit = new System.Windows.Forms.Button();
			this.panel6 = new System.Windows.Forms.Panel();
			this.title = new System.Windows.Forms.Label();
			this.listBox_gscFiles = new System.Windows.Forms.ListBox();
			this.panel_listBorder = new System.Windows.Forms.Panel();
			this.label_log = new System.Windows.Forms.Label();
			this.progressBar_listFiles = new System.Windows.Forms.ProgressBar();
			this.button_inject = new T8_GSC.ButtonX();
			this.btn_makeDataBase = new T8_GSC.ButtonX();
			this.btn_dumpAllFiles = new T8_GSC.ButtonX();
			this.btn_dumpFile = new T8_GSC.ButtonX();
			this.btn_fetchLoadedFiles = new T8_GSC.ButtonX();
			this.btn_connect = new T8_GSC.ButtonX();
			this.box_ip = new T8_GSC.TextBoxX();
			this.box_hashIn = new T8_GSC.TextBoxX();
			this.btn_hash = new T8_GSC.ButtonX();
			this.box_hashOut = new T8_GSC.TextBoxX();
			this.checkBox_t9 = new System.Windows.Forms.CheckBox();
			this.topBar.SuspendLayout();
			this.panel_listBorder.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel_bottom
			// 
			this.panel_bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.panel_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel_bottom.Location = new System.Drawing.Point(0, 449);
			this.panel_bottom.Name = "panel_bottom";
			this.panel_bottom.Size = new System.Drawing.Size(646, 1);
			this.panel_bottom.TabIndex = 8;
			// 
			// panel_top
			// 
			this.panel_top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel_top.Location = new System.Drawing.Point(0, 0);
			this.panel_top.Name = "panel_top";
			this.panel_top.Size = new System.Drawing.Size(646, 1);
			this.panel_top.TabIndex = 9;
			// 
			// panel_left
			// 
			this.panel_left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.panel_left.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel_left.Location = new System.Drawing.Point(0, 1);
			this.panel_left.Name = "panel_left";
			this.panel_left.Size = new System.Drawing.Size(1, 448);
			this.panel_left.TabIndex = 10;
			// 
			// panel_right
			// 
			this.panel_right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.panel_right.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel_right.Location = new System.Drawing.Point(645, 1);
			this.panel_right.Name = "panel_right";
			this.panel_right.Size = new System.Drawing.Size(1, 448);
			this.panel_right.TabIndex = 11;
			// 
			// topBar
			// 
			this.topBar.Controls.Add(this.minimize);
			this.topBar.Controls.Add(this.exit);
			this.topBar.Controls.Add(this.panel6);
			this.topBar.Controls.Add(this.title);
			this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.topBar.Location = new System.Drawing.Point(1, 1);
			this.topBar.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
			this.topBar.Name = "topBar";
			this.topBar.Size = new System.Drawing.Size(644, 25);
			this.topBar.TabIndex = 12;
			this.topBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.topBar_MouseDown);
			// 
			// minimize
			// 
			this.minimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
			this.minimize.Dock = System.Windows.Forms.DockStyle.Right;
			this.minimize.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
			this.minimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
			this.minimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
			this.minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.minimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.minimize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.minimize.Location = new System.Drawing.Point(600, 0);
			this.minimize.Name = "minimize";
			this.minimize.Size = new System.Drawing.Size(22, 24);
			this.minimize.TabIndex = 16;
			this.minimize.TabStop = false;
			this.minimize.Text = "_";
			this.minimize.UseCompatibleTextRendering = true;
			this.minimize.UseMnemonic = false;
			this.minimize.UseVisualStyleBackColor = false;
			this.minimize.Click += new System.EventHandler(this.minimize_Click);
			// 
			// exit
			// 
			this.exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
			this.exit.Dock = System.Windows.Forms.DockStyle.Right;
			this.exit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
			this.exit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.exit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.exit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.exit.Location = new System.Drawing.Point(622, 0);
			this.exit.Name = "exit";
			this.exit.Size = new System.Drawing.Size(22, 24);
			this.exit.TabIndex = 15;
			this.exit.TabStop = false;
			this.exit.Text = "X";
			this.exit.UseCompatibleTextRendering = true;
			this.exit.UseMnemonic = false;
			this.exit.UseVisualStyleBackColor = false;
			this.exit.Click += new System.EventHandler(this.exit_Click);
			// 
			// panel6
			// 
			this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
			this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel6.Location = new System.Drawing.Point(0, 24);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(644, 1);
			this.panel6.TabIndex = 14;
			// 
			// title
			// 
			this.title.Location = new System.Drawing.Point(0, 0);
			this.title.Margin = new System.Windows.Forms.Padding(0);
			this.title.Name = "title";
			this.title.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.title.Size = new System.Drawing.Size(184, 25);
			this.title.TabIndex = 13;
			this.title.Text = "T8 - DeathRGH";
			this.title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.title_MouseDown);
			// 
			// listBox_gscFiles
			// 
			this.listBox_gscFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBox_gscFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.listBox_gscFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox_gscFiles.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listBox_gscFiles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.listBox_gscFiles.FormattingEnabled = true;
			this.listBox_gscFiles.IntegralHeight = false;
			this.listBox_gscFiles.ItemHeight = 14;
			this.listBox_gscFiles.Location = new System.Drawing.Point(1, 1);
			this.listBox_gscFiles.Margin = new System.Windows.Forms.Padding(1);
			this.listBox_gscFiles.Name = "listBox_gscFiles";
			this.listBox_gscFiles.Size = new System.Drawing.Size(198, 336);
			this.listBox_gscFiles.TabIndex = 41;
			// 
			// panel_listBorder
			// 
			this.panel_listBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel_listBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
			this.panel_listBorder.Controls.Add(this.listBox_gscFiles);
			this.panel_listBorder.Location = new System.Drawing.Point(434, 37);
			this.panel_listBorder.Name = "panel_listBorder";
			this.panel_listBorder.Size = new System.Drawing.Size(200, 338);
			this.panel_listBorder.TabIndex = 42;
			// 
			// label_log
			// 
			this.label_log.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.label_log.Font = new System.Drawing.Font("Courier New", 9F);
			this.label_log.ForeColor = System.Drawing.Color.LimeGreen;
			this.label_log.Location = new System.Drawing.Point(12, 63);
			this.label_log.Margin = new System.Windows.Forms.Padding(3);
			this.label_log.Name = "label_log";
			this.label_log.Size = new System.Drawing.Size(416, 349);
			this.label_log.TabIndex = 44;
			this.label_log.Text = "Idle";
			// 
			// progressBar_listFiles
			// 
			this.progressBar_listFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar_listFiles.Location = new System.Drawing.Point(434, 433);
			this.progressBar_listFiles.Maximum = 1000;
			this.progressBar_listFiles.Name = "progressBar_listFiles";
			this.progressBar_listFiles.Size = new System.Drawing.Size(200, 5);
			this.progressBar_listFiles.TabIndex = 46;
			// 
			// button_inject
			// 
			this.button_inject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.button_inject.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
			this.button_inject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.button_inject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
			this.button_inject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_inject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.button_inject.Location = new System.Drawing.Point(353, 37);
			this.button_inject.Name = "button_inject";
			this.button_inject.Size = new System.Drawing.Size(75, 20);
			this.button_inject.TabIndex = 50;
			this.button_inject.TabStop = false;
			this.button_inject.Text = "Inject";
			this.button_inject.UseCompatibleTextRendering = true;
			this.button_inject.UseMnemonic = false;
			this.button_inject.UseVisualStyleBackColor = false;
			this.button_inject.Click += new System.EventHandler(this.button_inject_Click);
			// 
			// btn_makeDataBase
			// 
			this.btn_makeDataBase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.btn_makeDataBase.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
			this.btn_makeDataBase.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.btn_makeDataBase.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
			this.btn_makeDataBase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_makeDataBase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btn_makeDataBase.Location = new System.Drawing.Point(272, 37);
			this.btn_makeDataBase.Name = "btn_makeDataBase";
			this.btn_makeDataBase.Size = new System.Drawing.Size(75, 20);
			this.btn_makeDataBase.TabIndex = 49;
			this.btn_makeDataBase.TabStop = false;
			this.btn_makeDataBase.Text = "Make DB";
			this.btn_makeDataBase.UseCompatibleTextRendering = true;
			this.btn_makeDataBase.UseMnemonic = false;
			this.btn_makeDataBase.UseVisualStyleBackColor = false;
			this.btn_makeDataBase.Click += new System.EventHandler(this.btn_makeDataBase_Click);
			// 
			// btn_dumpAllFiles
			// 
			this.btn_dumpAllFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_dumpAllFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.btn_dumpAllFiles.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
			this.btn_dumpAllFiles.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.btn_dumpAllFiles.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
			this.btn_dumpAllFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_dumpAllFiles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btn_dumpAllFiles.Location = new System.Drawing.Point(434, 407);
			this.btn_dumpAllFiles.Name = "btn_dumpAllFiles";
			this.btn_dumpAllFiles.Size = new System.Drawing.Size(95, 20);
			this.btn_dumpAllFiles.TabIndex = 48;
			this.btn_dumpAllFiles.TabStop = false;
			this.btn_dumpAllFiles.Text = "Dump All";
			this.btn_dumpAllFiles.UseCompatibleTextRendering = true;
			this.btn_dumpAllFiles.UseMnemonic = false;
			this.btn_dumpAllFiles.UseVisualStyleBackColor = false;
			this.btn_dumpAllFiles.Click += new System.EventHandler(this.btn_dumpAllFiles_Click);
			// 
			// btn_dumpFile
			// 
			this.btn_dumpFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_dumpFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.btn_dumpFile.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
			this.btn_dumpFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.btn_dumpFile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
			this.btn_dumpFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_dumpFile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btn_dumpFile.Location = new System.Drawing.Point(539, 407);
			this.btn_dumpFile.Name = "btn_dumpFile";
			this.btn_dumpFile.Size = new System.Drawing.Size(95, 20);
			this.btn_dumpFile.TabIndex = 47;
			this.btn_dumpFile.TabStop = false;
			this.btn_dumpFile.Text = "Dump";
			this.btn_dumpFile.UseCompatibleTextRendering = true;
			this.btn_dumpFile.UseMnemonic = false;
			this.btn_dumpFile.UseVisualStyleBackColor = false;
			this.btn_dumpFile.Click += new System.EventHandler(this.btn_dumpFile_Click);
			// 
			// btn_fetchLoadedFiles
			// 
			this.btn_fetchLoadedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btn_fetchLoadedFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.btn_fetchLoadedFiles.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
			this.btn_fetchLoadedFiles.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.btn_fetchLoadedFiles.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
			this.btn_fetchLoadedFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_fetchLoadedFiles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btn_fetchLoadedFiles.Location = new System.Drawing.Point(434, 381);
			this.btn_fetchLoadedFiles.Name = "btn_fetchLoadedFiles";
			this.btn_fetchLoadedFiles.Size = new System.Drawing.Size(200, 20);
			this.btn_fetchLoadedFiles.TabIndex = 45;
			this.btn_fetchLoadedFiles.TabStop = false;
			this.btn_fetchLoadedFiles.Text = "List Loaded Files";
			this.btn_fetchLoadedFiles.UseCompatibleTextRendering = true;
			this.btn_fetchLoadedFiles.UseMnemonic = false;
			this.btn_fetchLoadedFiles.UseVisualStyleBackColor = false;
			this.btn_fetchLoadedFiles.Click += new System.EventHandler(this.btn_fetchLoadedFiles_Click);
			// 
			// btn_connect
			// 
			this.btn_connect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.btn_connect.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
			this.btn_connect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.btn_connect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
			this.btn_connect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_connect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btn_connect.Location = new System.Drawing.Point(118, 37);
			this.btn_connect.Name = "btn_connect";
			this.btn_connect.Size = new System.Drawing.Size(75, 20);
			this.btn_connect.TabIndex = 43;
			this.btn_connect.TabStop = false;
			this.btn_connect.Text = "Connect";
			this.btn_connect.UseCompatibleTextRendering = true;
			this.btn_connect.UseMnemonic = false;
			this.btn_connect.UseVisualStyleBackColor = false;
			this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
			// 
			// box_ip
			// 
			this.box_ip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.box_ip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.box_ip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.box_ip.Location = new System.Drawing.Point(12, 37);
			this.box_ip.Name = "box_ip";
			this.box_ip.Size = new System.Drawing.Size(100, 20);
			this.box_ip.TabIndex = 51;
			this.box_ip.TabStop = false;
			this.box_ip.TextChanged += new System.EventHandler(this.box_ip_TextChanged);
			// 
			// box_hashIn
			// 
			this.box_hashIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.box_hashIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.box_hashIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.box_hashIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.box_hashIn.Location = new System.Drawing.Point(12, 418);
			this.box_hashIn.Name = "box_hashIn";
			this.box_hashIn.Size = new System.Drawing.Size(204, 20);
			this.box_hashIn.TabIndex = 52;
			this.box_hashIn.TabStop = false;
			// 
			// btn_hash
			// 
			this.btn_hash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btn_hash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.btn_hash.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
			this.btn_hash.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
			this.btn_hash.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
			this.btn_hash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_hash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.btn_hash.Location = new System.Drawing.Point(222, 418);
			this.btn_hash.Name = "btn_hash";
			this.btn_hash.Size = new System.Drawing.Size(70, 20);
			this.btn_hash.TabIndex = 53;
			this.btn_hash.TabStop = false;
			this.btn_hash.Text = "Hash ->";
			this.btn_hash.UseCompatibleTextRendering = true;
			this.btn_hash.UseMnemonic = false;
			this.btn_hash.UseVisualStyleBackColor = false;
			this.btn_hash.Click += new System.EventHandler(this.btn_hash_Click);
			// 
			// box_hashOut
			// 
			this.box_hashOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.box_hashOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.box_hashOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.box_hashOut.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.box_hashOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.box_hashOut.Location = new System.Drawing.Point(298, 418);
			this.box_hashOut.Name = "box_hashOut";
			this.box_hashOut.Size = new System.Drawing.Size(130, 20);
			this.box_hashOut.TabIndex = 54;
			this.box_hashOut.TabStop = false;
			// 
			// checkBox_t9
			// 
			this.checkBox_t9.AutoSize = true;
			this.checkBox_t9.Location = new System.Drawing.Point(199, 38);
			this.checkBox_t9.Name = "checkBox_t9";
			this.checkBox_t9.Size = new System.Drawing.Size(39, 17);
			this.checkBox_t9.TabIndex = 55;
			this.checkBox_t9.Text = "T9";
			this.checkBox_t9.UseVisualStyleBackColor = true;
			// 
			// main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
			this.ClientSize = new System.Drawing.Size(646, 450);
			this.Controls.Add(this.checkBox_t9);
			this.Controls.Add(this.box_hashOut);
			this.Controls.Add(this.btn_hash);
			this.Controls.Add(this.box_hashIn);
			this.Controls.Add(this.box_ip);
			this.Controls.Add(this.button_inject);
			this.Controls.Add(this.btn_makeDataBase);
			this.Controls.Add(this.btn_dumpAllFiles);
			this.Controls.Add(this.btn_dumpFile);
			this.Controls.Add(this.progressBar_listFiles);
			this.Controls.Add(this.btn_fetchLoadedFiles);
			this.Controls.Add(this.label_log);
			this.Controls.Add(this.btn_connect);
			this.Controls.Add(this.panel_listBorder);
			this.Controls.Add(this.topBar);
			this.Controls.Add(this.panel_right);
			this.Controls.Add(this.panel_left);
			this.Controls.Add(this.panel_top);
			this.Controls.Add(this.panel_bottom);
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "main";
			this.Text = "T7";
			this.topBar.ResumeLayout(false);
			this.panel_listBorder.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel_bottom;
		private System.Windows.Forms.Panel panel_top;
		private System.Windows.Forms.Panel panel_left;
		private System.Windows.Forms.Panel panel_right;
		private System.Windows.Forms.Panel topBar;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Label title;
		private System.Windows.Forms.Button minimize;
		private System.Windows.Forms.Button exit;
		private System.Windows.Forms.ListBox listBox_gscFiles;
		private System.Windows.Forms.Panel panel_listBorder;
		private ButtonX btn_connect;
		private System.Windows.Forms.Label label_log;
		private ButtonX btn_fetchLoadedFiles;
		private System.Windows.Forms.ProgressBar progressBar_listFiles;
		private ButtonX btn_dumpFile;
		private ButtonX btn_dumpAllFiles;
		private ButtonX btn_makeDataBase;
		private ButtonX button_inject;
		private TextBoxX box_ip;
		private TextBoxX box_hashIn;
		private ButtonX btn_hash;
		private TextBoxX box_hashOut;
		private System.Windows.Forms.CheckBox checkBox_t9;
	}
}

