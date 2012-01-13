namespace SuperProgram {
	partial class FormMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.tabCtrlContours = new System.Windows.Forms.TabControl();
            this.pbDrawField = new System.Windows.Forms.PictureBox();
            this.btnCompleteInput = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnNetReculc = new System.Windows.Forms.Button();
            this.btnGetSingleContour = new System.Windows.Forms.Button();
            this.btnTriangulate = new System.Windows.Forms.Button();
            this.btnSolve = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRenumerator = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawField)).BeginInit();
            this.SuspendLayout();
            // 
            // tabCtrlContours
            // 
            this.tabCtrlContours.Location = new System.Drawing.Point(807, 0);
            this.tabCtrlContours.Name = "tabCtrlContours";
            this.tabCtrlContours.SelectedIndex = 0;
            this.tabCtrlContours.Size = new System.Drawing.Size(235, 394);
            this.tabCtrlContours.TabIndex = 0;
            // 
            // pbDrawField
            // 
            this.pbDrawField.BackColor = System.Drawing.SystemColors.Window;
            this.pbDrawField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbDrawField.Location = new System.Drawing.Point(0, 0);
            this.pbDrawField.Name = "pbDrawField";
            this.pbDrawField.Size = new System.Drawing.Size(800, 600);
            this.pbDrawField.TabIndex = 1;
            this.pbDrawField.TabStop = false;
            this.pbDrawField.Paint += new System.Windows.Forms.PaintEventHandler(this.pbDrawField_Paint);
            // 
            // btnCompleteInput
            // 
            this.btnCompleteInput.Location = new System.Drawing.Point(807, 429);
            this.btnCompleteInput.Name = "btnCompleteInput";
            this.btnCompleteInput.Size = new System.Drawing.Size(235, 23);
            this.btnCompleteInput.TabIndex = 2;
            this.btnCompleteInput.Text = "Завершить ввод";
            this.btnCompleteInput.UseVisualStyleBackColor = true;
            this.btnCompleteInput.Click += new System.EventHandler(this.btnCompleteInput_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(806, 490);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Максимальный шаг сетки:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(955, 487);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(87, 20);
            this.textBox1.TabIndex = 4;
            // 
            // btnNetReculc
            // 
            this.btnNetReculc.Location = new System.Drawing.Point(807, 513);
            this.btnNetReculc.Name = "btnNetReculc";
            this.btnNetReculc.Size = new System.Drawing.Size(235, 23);
            this.btnNetReculc.TabIndex = 5;
            this.btnNetReculc.Text = "Пересчитать сетку";
            this.btnNetReculc.UseVisualStyleBackColor = true;
            this.btnNetReculc.Click += new System.EventHandler(this.btnNetReculc_Click);
            // 
            // btnGetSingleContour
            // 
            this.btnGetSingleContour.Location = new System.Drawing.Point(808, 458);
            this.btnGetSingleContour.Name = "btnGetSingleContour";
            this.btnGetSingleContour.Size = new System.Drawing.Size(234, 23);
            this.btnGetSingleContour.TabIndex = 6;
            this.btnGetSingleContour.Text = "Сделать область односвязной";
            this.btnGetSingleContour.UseVisualStyleBackColor = true;
            this.btnGetSingleContour.Click += new System.EventHandler(this.btnGetSingleContour_Click);
            // 
            // btnTriangulate
            // 
            this.btnTriangulate.Location = new System.Drawing.Point(809, 542);
            this.btnTriangulate.Name = "btnTriangulate";
            this.btnTriangulate.Size = new System.Drawing.Size(233, 23);
            this.btnTriangulate.TabIndex = 7;
            this.btnTriangulate.Text = "Триангулировать область";
            this.btnTriangulate.UseVisualStyleBackColor = true;
            this.btnTriangulate.Click += new System.EventHandler(this.btnTriangulate_Click);
            // 
            // btnSolve
            // 
            this.btnSolve.Location = new System.Drawing.Point(809, 600);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(234, 23);
            this.btnSolve.TabIndex = 8;
            this.btnSolve.Text = "Решить задачу в области";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(809, 400);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(233, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Очистить";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnRenumerator
            // 
            this.btnRenumerator.Location = new System.Drawing.Point(809, 571);
            this.btnRenumerator.Name = "btnRenumerator";
            this.btnRenumerator.Size = new System.Drawing.Size(233, 23);
            this.btnRenumerator.TabIndex = 10;
            this.btnRenumerator.Text = "Перенумеровать по Катхилла-Макку";
            this.btnRenumerator.UseVisualStyleBackColor = true;
            this.btnRenumerator.Click += new System.EventHandler(this.btnRenumerator_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 627);
            this.Controls.Add(this.btnRenumerator);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSolve);
            this.Controls.Add(this.btnTriangulate);
            this.Controls.Add(this.btnGetSingleContour);
            this.Controls.Add(this.btnNetReculc);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCompleteInput);
            this.Controls.Add(this.pbDrawField);
            this.Controls.Add(this.tabCtrlContours);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "Задача теплопроводности";
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawField)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabCtrlContours;
		private System.Windows.Forms.PictureBox pbDrawField;
		private System.Windows.Forms.Button btnCompleteInput;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnNetReculc;
		private System.Windows.Forms.Button btnGetSingleContour;
		private System.Windows.Forms.Button btnTriangulate;
		private System.Windows.Forms.Button btnSolve;
		private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRenumerator;
	}
}

