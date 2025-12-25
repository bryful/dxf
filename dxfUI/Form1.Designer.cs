using System;
using System.Drawing;
using System.Windows.Forms;

namespace dxf
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			InputBox = new TextBox();
			OutputBox = new TextBox();
			btnExec = new Button();
			menuStrip1 = new MenuStrip();
			fileMenu = new ToolStripMenuItem();
			openMenu = new ToolStripMenuItem();
			saveMenu = new ToolStripMenuItem();
			quitMenu = new ToolStripMenuItem();
			scriptToolStripMenuItem = new ToolStripMenuItem();
			execMenu = new ToolStripMenuItem();
			btnClear = new Button();
			menuStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// InputBox
			// 
			InputBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			InputBox.Font = new Font("Yu Gothic UI", 12F);
			InputBox.Location = new Point(10, 27);
			InputBox.Multiline = true;
			InputBox.Name = "InputBox";
			InputBox.ScrollBars = ScrollBars.Both;
			InputBox.Size = new Size(944, 193);
			InputBox.TabIndex = 0;
			// 
			// OutputBox
			// 
			OutputBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			OutputBox.Font = new Font("Yu Gothic UI", 12F);
			OutputBox.Location = new Point(12, 272);
			OutputBox.Multiline = true;
			OutputBox.Name = "OutputBox";
			OutputBox.ReadOnly = true;
			OutputBox.ScrollBars = ScrollBars.Both;
			OutputBox.Size = new Size(944, 255);
			OutputBox.TabIndex = 1;
			// 
			// btnExec
			// 
			btnExec.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnExec.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
			btnExec.Location = new Point(802, 235);
			btnExec.Name = "btnExec";
			btnExec.Size = new Size(126, 31);
			btnExec.TabIndex = 2;
			btnExec.Text = "Exec";
			btnExec.UseVisualStyleBackColor = true;
			// 
			// menuStrip1
			// 
			menuStrip1.Items.AddRange(new ToolStripItem[] { fileMenu, scriptToolStripMenuItem });
			menuStrip1.Location = new Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new Size(966, 24);
			menuStrip1.TabIndex = 3;
			menuStrip1.Text = "menuStrip1";
			// 
			// fileMenu
			// 
			fileMenu.DropDownItems.AddRange(new ToolStripItem[] { openMenu, saveMenu, quitMenu });
			fileMenu.Name = "fileMenu";
			fileMenu.Size = new Size(37, 20);
			fileMenu.Text = "File";
			// 
			// openMenu
			// 
			openMenu.Name = "openMenu";
			openMenu.ShortcutKeys = Keys.Control | Keys.O;
			openMenu.Size = new Size(145, 22);
			openMenu.Text = "Open";
			// 
			// saveMenu
			// 
			saveMenu.Name = "saveMenu";
			saveMenu.ShortcutKeys = Keys.Control | Keys.S;
			saveMenu.Size = new Size(145, 22);
			saveMenu.Text = "Save";
			// 
			// quitMenu
			// 
			quitMenu.Name = "quitMenu";
			quitMenu.ShortcutKeys = Keys.Control | Keys.Q;
			quitMenu.Size = new Size(145, 22);
			quitMenu.Text = "Quit";
			// 
			// scriptToolStripMenuItem
			// 
			scriptToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { execMenu });
			scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
			scriptToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.E;
			scriptToolStripMenuItem.Size = new Size(49, 20);
			scriptToolStripMenuItem.Text = "Script";
			// 
			// execMenu
			// 
			execMenu.Name = "execMenu";
			execMenu.ShortcutKeys = Keys.Control | Keys.E;
			execMenu.Size = new Size(154, 22);
			execMenu.Text = "Execute";
			// 
			// btnClear
			// 
			btnClear.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
			btnClear.Location = new Point(12, 235);
			btnClear.Name = "btnClear";
			btnClear.Size = new Size(126, 31);
			btnClear.TabIndex = 4;
			btnClear.Text = "Clear";
			btnClear.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(966, 539);
			Controls.Add(btnClear);
			Controls.Add(btnExec);
			Controls.Add(OutputBox);
			Controls.Add(InputBox);
			Controls.Add(menuStrip1);
			MainMenuStrip = menuStrip1;
			Name = "Form1";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "DxfUI";
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private TextBox InputBox;
		private TextBox OutputBox;
		private Button btnExec;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem fileMenu;
		private ToolStripMenuItem openMenu;
		private ToolStripMenuItem saveMenu;
		private ToolStripMenuItem quitMenu;
		private Button btnClear;
		private ToolStripMenuItem scriptToolStripMenuItem;
		private ToolStripMenuItem execMenu;
	}
}
