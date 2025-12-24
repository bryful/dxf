namespace dxf
{
	partial class InputDialogSample
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
			SuspendLayout();
			// 
			// InputDialogSample
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			Caption = "Alert";
			ClientSize = new System.Drawing.Size(559, 101);
			DialogType = InputDialogType.INPUTBOX;
			MaximumSize = new System.Drawing.Size(3000, 160);
			Name = "InputDialogSample";
			Text = "InputDialogSample";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
	}
}