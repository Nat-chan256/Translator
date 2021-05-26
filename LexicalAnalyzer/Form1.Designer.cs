namespace LexicalAnalyzer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.rtbSourceCode = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.rtbLexemes = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiOpenSourceFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveSourceFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveOutputFile = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.sfdSaveSourceCode = new System.Windows.Forms.SaveFileDialog();
            this.bExecute = new System.Windows.Forms.Button();
            this.sfdSaveOutputFile = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvDividers = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvConstants = new System.Windows.Forms.DataGridView();
            this.dgvOperators = new System.Windows.Forms.DataGridView();
            this.dgvServiceWords = new System.Windows.Forms.DataGridView();
            this.dgvIdentifiersTable = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.bExecuteRPN = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.rtbSourceCodeRPN = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.rtbRPN = new System.Windows.Forms.RichTextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnExecuteBasicTranslationTab = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.rtbRPNBasicTranslationTab = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.rtbBasic = new System.Windows.Forms.RichTextBox();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDividers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConstants)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperators)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServiceWords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIdentifiersTable)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.rtbSourceCode);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.rtbLexemes);
            this.splitContainer1.Size = new System.Drawing.Size(1055, 454);
            this.splitContainer1.SplitterDistance = 518;
            this.splitContainer1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 20);
            this.label6.TabIndex = 3;
            this.label6.Text = "Код программы";
            // 
            // rtbSourceCode
            // 
            this.rtbSourceCode.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbSourceCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbSourceCode.Location = new System.Drawing.Point(0, 23);
            this.rtbSourceCode.Name = "rtbSourceCode";
            this.rtbSourceCode.Size = new System.Drawing.Size(518, 431);
            this.rtbSourceCode.TabIndex = 0;
            this.rtbSourceCode.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Top;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Лексемы";
            // 
            // rtbLexemes
            // 
            this.rtbLexemes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLexemes.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbLexemes.Location = new System.Drawing.Point(0, 23);
            this.rtbLexemes.Name = "rtbLexemes";
            this.rtbLexemes.Size = new System.Drawing.Size(640, 431);
            this.rtbLexemes.TabIndex = 0;
            this.rtbLexemes.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1069, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiOpenSourceFile,
            this.tsmiSaveSourceFile,
            this.tsmiSaveOutputFile});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // tsiOpenSourceFile
            // 
            this.tsiOpenSourceFile.Name = "tsiOpenSourceFile";
            this.tsiOpenSourceFile.Size = new System.Drawing.Size(223, 22);
            this.tsiOpenSourceFile.Text = "Открыть исходный файл";
            this.tsiOpenSourceFile.Click += new System.EventHandler(this.tsiOpenSourceFile_Click);
            // 
            // tsmiSaveSourceFile
            // 
            this.tsmiSaveSourceFile.Name = "tsmiSaveSourceFile";
            this.tsmiSaveSourceFile.Size = new System.Drawing.Size(223, 22);
            this.tsmiSaveSourceFile.Text = "Сохранить исходный файл";
            this.tsmiSaveSourceFile.Click += new System.EventHandler(this.tsmiSaveSourceFile_Click);
            // 
            // tsmiSaveOutputFile
            // 
            this.tsmiSaveOutputFile.Name = "tsmiSaveOutputFile";
            this.tsmiSaveOutputFile.Size = new System.Drawing.Size(223, 22);
            this.tsmiSaveOutputFile.Text = "Сохранить выходной файл";
            this.tsmiSaveOutputFile.Click += new System.EventHandler(this.tsmiSaveOutputFile_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.выходToolStripMenuItem.Text = "Выход";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // bExecute
            // 
            this.bExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bExecute.Location = new System.Drawing.Point(924, 463);
            this.bExecute.Name = "bExecute";
            this.bExecute.Size = new System.Drawing.Size(131, 34);
            this.bExecute.TabIndex = 2;
            this.bExecute.Text = "Выполнить";
            this.bExecute.UseVisualStyleBackColor = true;
            this.bExecute.Click += new System.EventHandler(this.bExecute_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1069, 549);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Controls.Add(this.bExecute);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1061, 523);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Лексический анализатор";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.dgvDividers);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.dgvConstants);
            this.tabPage2.Controls.Add(this.dgvOperators);
            this.tabPage2.Controls.Add(this.dgvServiceWords);
            this.tabPage2.Controls.Add(this.dgvIdentifiersTable);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1061, 523);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Служебные таблицы";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(985, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(147, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Таблица констант";
            // 
            // dgvDividers
            // 
            this.dgvDividers.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvDividers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDividers.Location = new System.Drawing.Point(735, 48);
            this.dgvDividers.Name = "dgvDividers";
            this.dgvDividers.ReadOnly = true;
            this.dgvDividers.Size = new System.Drawing.Size(225, 468);
            this.dgvDividers.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(8, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(219, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Таблица идентификаторов";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(238, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Таблица служебных слов";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(488, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Таблица операторов";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(731, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Таблица разделителей";
            // 
            // dgvConstants
            // 
            this.dgvConstants.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvConstants.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConstants.Location = new System.Drawing.Point(989, 48);
            this.dgvConstants.Name = "dgvConstants";
            this.dgvConstants.Size = new System.Drawing.Size(260, 468);
            this.dgvConstants.TabIndex = 3;
            // 
            // dgvOperators
            // 
            this.dgvOperators.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvOperators.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOperators.Location = new System.Drawing.Point(492, 48);
            this.dgvOperators.Name = "dgvOperators";
            this.dgvOperators.ReadOnly = true;
            this.dgvOperators.Size = new System.Drawing.Size(226, 468);
            this.dgvOperators.TabIndex = 2;
            // 
            // dgvServiceWords
            // 
            this.dgvServiceWords.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvServiceWords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServiceWords.Location = new System.Drawing.Point(242, 48);
            this.dgvServiceWords.Name = "dgvServiceWords";
            this.dgvServiceWords.ReadOnly = true;
            this.dgvServiceWords.Size = new System.Drawing.Size(228, 468);
            this.dgvServiceWords.TabIndex = 1;
            // 
            // dgvIdentifiersTable
            // 
            this.dgvIdentifiersTable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvIdentifiersTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIdentifiersTable.Location = new System.Drawing.Point(8, 48);
            this.dgvIdentifiersTable.Name = "dgvIdentifiersTable";
            this.dgvIdentifiersTable.ReadOnly = true;
            this.dgvIdentifiersTable.Size = new System.Drawing.Size(215, 468);
            this.dgvIdentifiersTable.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.bExecuteRPN);
            this.tabPage3.Controls.Add(this.splitContainer2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1061, 523);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Перевод в ОПЗ";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // bExecuteRPN
            // 
            this.bExecuteRPN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bExecuteRPN.Location = new System.Drawing.Point(921, 462);
            this.bExecuteRPN.Name = "bExecuteRPN";
            this.bExecuteRPN.Size = new System.Drawing.Size(132, 36);
            this.bExecuteRPN.TabIndex = 1;
            this.bExecuteRPN.Text = "Выполнить";
            this.bExecuteRPN.UseVisualStyleBackColor = true;
            this.bExecuteRPN.Click += new System.EventHandler(this.bExecuteRPN_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.rtbSourceCodeRPN);
            this.splitContainer2.Panel1.Controls.Add(this.label8);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label9);
            this.splitContainer2.Panel2.Controls.Add(this.rtbRPN);
            this.splitContainer2.Size = new System.Drawing.Size(1061, 456);
            this.splitContainer2.SplitterDistance = 500;
            this.splitContainer2.TabIndex = 0;
            // 
            // rtbSourceCodeRPN
            // 
            this.rtbSourceCodeRPN.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbSourceCodeRPN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbSourceCodeRPN.Location = new System.Drawing.Point(0, 23);
            this.rtbSourceCodeRPN.Name = "rtbSourceCodeRPN";
            this.rtbSourceCodeRPN.Size = new System.Drawing.Size(500, 433);
            this.rtbSourceCodeRPN.TabIndex = 1;
            this.rtbSourceCodeRPN.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 20);
            this.label8.TabIndex = 0;
            this.label8.Text = "Код программы";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(215, 20);
            this.label9.TabIndex = 1;
            this.label9.Text = "Обратная польская запись";
            // 
            // rtbRPN
            // 
            this.rtbRPN.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbRPN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbRPN.Location = new System.Drawing.Point(0, 23);
            this.rtbRPN.Name = "rtbRPN";
            this.rtbRPN.Size = new System.Drawing.Size(557, 433);
            this.rtbRPN.TabIndex = 0;
            this.rtbRPN.Text = "";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnExecuteBasicTranslationTab);
            this.tabPage4.Controls.Add(this.splitContainer3);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1061, 523);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Перевод в Basic";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnExecuteBasicTranslationTab
            // 
            this.btnExecuteBasicTranslationTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExecuteBasicTranslationTab.Location = new System.Drawing.Point(904, 471);
            this.btnExecuteBasicTranslationTab.Name = "btnExecuteBasicTranslationTab";
            this.btnExecuteBasicTranslationTab.Size = new System.Drawing.Size(149, 38);
            this.btnExecuteBasicTranslationTab.TabIndex = 1;
            this.btnExecuteBasicTranslationTab.Text = "Выполнить";
            this.btnExecuteBasicTranslationTab.UseVisualStyleBackColor = true;
            this.btnExecuteBasicTranslationTab.Click += new System.EventHandler(this.btnExecuteBasicTranslationTab_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.rtbRPNBasicTranslationTab);
            this.splitContainer3.Panel1.Controls.Add(this.label10);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.rtbBasic);
            this.splitContainer3.Panel2.Controls.Add(this.label11);
            this.splitContainer3.Size = new System.Drawing.Size(1061, 465);
            this.splitContainer3.SplitterDistance = 474;
            this.splitContainer3.TabIndex = 0;
            // 
            // rtbRPNBasicTranslationTab
            // 
            this.rtbRPNBasicTranslationTab.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbRPNBasicTranslationTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbRPNBasicTranslationTab.Location = new System.Drawing.Point(0, 23);
            this.rtbRPNBasicTranslationTab.Name = "rtbRPNBasicTranslationTab";
            this.rtbRPNBasicTranslationTab.Size = new System.Drawing.Size(474, 442);
            this.rtbRPNBasicTranslationTab.TabIndex = 1;
            this.rtbRPNBasicTranslationTab.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Top;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(0, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(215, 20);
            this.label10.TabIndex = 0;
            this.label10.Text = "Обратная польская запись";
            // 
            // rtbBasic
            // 
            this.rtbBasic.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbBasic.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbBasic.Location = new System.Drawing.Point(0, 23);
            this.rtbBasic.Name = "rtbBasic";
            this.rtbBasic.Size = new System.Drawing.Size(583, 442);
            this.rtbBasic.TabIndex = 1;
            this.rtbBasic.Text = "";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(0, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(255, 20);
            this.label11.TabIndex = 0;
            this.label11.Text = "Текст программы на языке Basic";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 573);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Лексический анализатор";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDividers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConstants)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperators)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServiceWords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIdentifiersTable)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox rtbSourceCode;
        private System.Windows.Forms.RichTextBox rtbLexemes;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsiOpenSourceFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveSourceFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveOutputFile;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog sfdSaveSourceCode;
        private System.Windows.Forms.Button bExecute;
        private System.Windows.Forms.SaveFileDialog sfdSaveOutputFile;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvIdentifiersTable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvConstants;
        private System.Windows.Forms.DataGridView dgvOperators;
        private System.Windows.Forms.DataGridView dgvServiceWords;
        private System.Windows.Forms.DataGridView dgvDividers;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button bExecuteRPN;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox rtbSourceCodeRPN;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RichTextBox rtbRPN;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.RichTextBox rtbRPNBasicTranslationTab;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RichTextBox rtbBasic;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnExecuteBasicTranslationTab;
    }
}

