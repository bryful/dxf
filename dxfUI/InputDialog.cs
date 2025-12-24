using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace dxf
{
	public enum InputDialogType
	{
		ALERT,
		YESNO,
		INPUTBOX
	}
	public partial class InputDialog : Form
	{
		private InputDialogType m_dialogType = InputDialogType.ALERT;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public InputDialogType DialogType
		{
			get { return m_dialogType; }
			set { SetMode(value); }
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string Caption
		{
			get { return base.Text; }
			set { 
				base.Text = value; 
			}
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string Caption2
		{
			get { return label1.Text; }
			set
			{
				label1.Text = value;
			}
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public new string Text
		{
			get 
			{
				return textBox.Text;
			}
			set
			{
				textBox.Text = value;
				textBox.Select(Right, 0);

			}
		}
		public InputDialog()
		{
			InitializeComponent();
			ChkSize();
			label1.Text = "";
		}
		public void SetMode(InputDialogType idt)
		{
			m_dialogType = idt;
			switch (idt)
			{
				case InputDialogType.ALERT:
					label1.Visible = false;
					textBox.ReadOnly = true;
					textBox.Multiline = true;
					btnCancel.Visible = false;
					btnOK.Visible = true;
					btnOK.Text = "OK";
					this.MaximumSize = new Size(3000, 3000);
					break;
				case InputDialogType.YESNO:
					label1.Visible = false;
					textBox.ReadOnly = true;
					textBox.Multiline = false;
					btnCancel.Visible = true;
					btnOK.Visible = true;
					btnCancel.Text = "No";
					btnOK.Text = "Yes";
					this.MaximumSize = new Size(3000, 140);
					break;
				case InputDialogType.INPUTBOX:
					label1.Visible = true;
					textBox.ReadOnly = false;
					textBox.Multiline = false;
					btnCancel.Visible = true;
					btnOK.Visible = true;
					btnCancel.Text = "cancel";
					btnOK.Text = "OK";
					this.MaximumSize = new Size(3000, 160);
					break;
			}
			ChkSize();
		}
		public void ChkSize()
		{
			switch (m_dialogType)
			{
				case InputDialogType.ALERT:
					textBox.Location = new Point(12, 12);
					textBox.Size = new Size(this.ClientSize.Width - 24, this.ClientSize.Height - 12 - 45);
					btnOK.Location = new Point(this.ClientSize.Width - btnOK.Width - 12, this.ClientSize.Height - 30);
					btnCancel.Location = new Point(this.ClientSize.Width - btnOK.Width - btnCancel.Width - 20, this.ClientSize.Height - 30);
					break;
				case InputDialogType.YESNO:
					textBox.Location = new Point(12, 12);
					textBox.Size = new Size(this.ClientSize.Width - 24, this.ClientSize.Height - 12 - 40);
					btnCancel.Location = new Point(this.ClientSize.Width - btnOK.Width - btnCancel.Width - 20, this.ClientSize.Height - 30);
					btnOK.Location = new Point(this.ClientSize.Width - btnOK.Width - 12, this.ClientSize.Height - 30);
					break;
				case InputDialogType.INPUTBOX:
					label1.Location = new Point(12, 6);
					textBox.Location = new Point(12, 30);
					textBox.Size = new Size(this.ClientSize.Width - 24, this.ClientSize.Height - 12 - 40);
					btnCancel.Location = new Point(this.ClientSize.Width - btnOK.Width - btnCancel.Width - 20, this.ClientSize.Height - 30);
					btnOK.Location = new Point(this.ClientSize.Width - btnOK.Width - 12, this.ClientSize.Height - 30);
					break;
			}
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ChkSize();
		}
	}
}
