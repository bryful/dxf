namespace dxf
{
	partial class InputDialog
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
			textBox = new System.Windows.Forms.TextBox();
			btnOK = new System.Windows.Forms.Button();
			btnCancel = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			SuspendLayout();
			// 
			// textBox
			// 
			textBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			textBox.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			textBox.Location = new System.Drawing.Point(26, 30);
			textBox.Name = "textBox";
			textBox.ReadOnly = true;
			textBox.Size = new System.Drawing.Size(360, 29);
			textBox.TabIndex = 0;
			// 
			// btnOK
			// 
			btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
			btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btnOK.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F);
			btnOK.Location = new System.Drawing.Point(313, 65);
			btnOK.Name = "btnOK";
			btnOK.Size = new System.Drawing.Size(75, 24);
			btnOK.TabIndex = 1;
			btnOK.Text = "OK";
			btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
			btnCancel.BackColor = System.Drawing.SystemColors.Control;
			btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnCancel.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F);
			btnCancel.Location = new System.Drawing.Point(232, 65);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(75, 24);
			btnCancel.TabIndex = 2;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = false;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
			label1.Location = new System.Drawing.Point(12, 6);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(52, 21);
			label1.TabIndex = 3;
			label1.Text = "label1";
			label1.Visible = false;
			// 
			// InputDialog
			// 
			AcceptButton = btnOK;
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			CancelButton = btnCancel;
			ClientSize = new System.Drawing.Size(413, 101);
			Controls.Add(label1);
			Controls.Add(btnCancel);
			Controls.Add(btnOK);
			Controls.Add(textBox);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			Name = "InputDialog";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "InputDialog";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
	}
}