using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace dxf
{
	public class JSFileDialog
	{
		private string m_initialDirectory = "";
		private string m_name = "";
		private string m_filter = "Text(*.txt)|*.txt|all(*.*)|*.*";
		private int m_filterIndex = 1;
		private string m_title = "";
		private string m_defExt = ".txt";
		private bool m_multiSelect = false;
		public string fileName
		{
			get
			{
				return Path.Combine(m_initialDirectory, m_name);
			}
			set
			{
				string n = System.IO.Path.GetFileName(value);
				string dir = System.IO.Path.GetDirectoryName(value) ?? "";
				m_initialDirectory = dir;
				m_name = n;
			}
		}

		public bool multiSelect
		{
			get { return m_multiSelect; }
			set
			{
				m_multiSelect = value;
			}
		}

		public string filter
		{ 	
			get { return m_filter; }
			set
			{
				m_filter = value;
			}
		}
		public int filterIndex
		{
			get { return m_filterIndex; }
			set
			{
				m_filterIndex = value;
			}
		}
		public string title
		{
			get { return m_title; }
			set
			{
				m_title = value;
			}
		}
		public string defaultExt
		{
			get { return m_defExt; }
			set
			{
				m_defExt = value;
			}
		}
		public string initialDirectory
		{
			get { return m_initialDirectory; }
			set
			{
				m_initialDirectory = value;
			}
		}
		public string name
		{
			get { return m_name; }
			set
			{
				fileName = value;
			}
		}
		public JSFileDialog()
		{
		}
		public string? openDialog()
		{
			string? result = null;
			using (OpenFileDialog dlg = new OpenFileDialog())
			{
				if (m_initialDirectory != "")
				{
					dlg.InitialDirectory = m_initialDirectory;
				}
				if (m_name != "")
				{
					dlg.FileName = m_name;
				}
				if (m_filter != "")
				{
					dlg.Filter = m_filter;

				}
				if (m_title != "")
				{
					dlg.Title = m_title;
				}
				else
				{
					dlg.Title = "Open";
				}
				if (m_defExt != "")
				{
					dlg.DefaultExt = m_defExt;
				}
				dlg.Multiselect = m_multiSelect;
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					if (dlg.Multiselect)
					{
						JsonArray arr = new JsonArray();
						if (dlg.FileNames.Length == 1)
						{
							result = dlg.FileNames[0];
							fileName = dlg.FileNames[0];
						}
						else if (dlg.FileNames.Length > 1)
						{
							foreach (var f in dlg.FileNames)
							{
								arr.Add(f);
							}
							result = arr.ToJsonString(new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
						}
					}
					else
					{
						result = dlg.FileName;
						fileName = dlg.FileName;
					}
				}
			}
			return result;
		}
		public string? openDialog(string fname)
		{
			fileName = fname;
			return openDialog();
		}
		public string? openDialog(string fname,string t)
		{
			fileName = fname;
			title = t;
			return openDialog();
		}
		
		public string? saveDialog()
		{
			string? result = null;
			using (SaveFileDialog dlg = new SaveFileDialog())
			{
				if (m_initialDirectory != "")
				{
					dlg.InitialDirectory = m_initialDirectory;
				}
				if (m_name != "")
				{
					dlg.FileName = m_name;
				}
				if (m_filter != "")
				{
					dlg.Filter = m_filter;

				}
				if (m_title != "")
				{
					dlg.Title = m_title;
				}
				else
				{
					dlg.Title = "Save";
				}
				if (m_defExt != "")
				{
					dlg.DefaultExt = m_defExt;
				}
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					result = dlg.FileName;
					fileName = dlg.FileName;
				}
			}
			return result;
		}
		public string? saveDialog(string fname)
		{
			fileName = fname;
			return saveDialog();
		}
		public string? saveDialog(string fname,string t)
		{
			fileName = fname;
			title = t;
			return saveDialog();
		}

		// モダンなフォルダ選択ダイアログ
		public string? folderDialog()
		{
			string? result = null;
			
			using (var dlg = new CommonOpenFileDialog())
			{
				dlg.IsFolderPicker = true;
				dlg.Title = string.IsNullOrEmpty(m_title) ? "Select Folder" : m_title;
				
				if (!string.IsNullOrEmpty(m_initialDirectory) && Directory.Exists(m_initialDirectory))
				{
					dlg.InitialDirectory = m_initialDirectory;
				}
				
				if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
				{
					result = dlg.FileName;
					m_initialDirectory = result;
				}
			}
			
			return result;
		}

		public string? folderDialog(string title)
		{
			m_title = title;
			return folderDialog();
		}

		public string? folderDialog(string initialDir, string title)
		{
			m_initialDirectory = initialDir;
			m_title = title;
			return folderDialog();
		}
	}
}
