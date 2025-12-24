using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
//using IWshRuntimeLibrary;
namespace dxf
{
	public enum JSFileStyle
	{
		NONE = 0,
		FILE = 1,
		FOLDER = 2,
		SHORTCUT = 3,
	}
	public class JSFileItem
	{
		#region Properties
		private JSFileStyle m_fileStyle = JSFileStyle.NONE;
		private string m_path = "";
		private string m_resolve = "";
		private string m_dir = "";
		private string m_name = "";
		private string m_ext = "";

		private bool m_hidden = false;
		#endregion

		public JSFileItem(string path)
		{
			setFullName(path);
		}
		// ***************************************************
		private bool chkPath()
		{
			bool ret = false;
			m_fileStyle = JSFileStyle.NONE;
			if (m_path != "")
			{
				FileInfo fi = new FileInfo(m_path);
				DirectoryInfo di = new DirectoryInfo(m_path);
				if (di.Exists)
				{
					m_fileStyle = JSFileStyle.FOLDER;
					m_path = di.FullName;
					m_hidden = (di.Attributes & FileAttributes.Hidden) != 0;
					ret = true;
				}
				else if (fi.Exists)
				{
					m_fileStyle = JSFileStyle.FILE;
					m_path = fi.FullName;
					m_hidden = (fi.Attributes & FileAttributes.Hidden) != 0;
					if (fi.Extension.ToLower() == ".lnk")
					{
						m_fileStyle = JSFileStyle.SHORTCUT;
					}
					ret = true;
				}
				m_dir = Path.GetDirectoryName(m_path) ?? "";
				if (m_dir == "")
				{
					m_dir = Directory.GetCurrentDirectory();
				}
				m_name = Path.GetFileName(m_path);
				m_ext = Path.GetExtension(m_path);
				if (m_fileStyle == JSFileStyle.SHORTCUT)
				{
					m_resolve = resolveFromFile(m_path);
				}
			}
			else
			{
				m_dir = "";
				m_name = "";
				m_ext = "";
				m_resolve = "";
				m_hidden = false;
				ret = false;
			}
			return ret;
		}
		public bool setFullName(string path)
		{
			bool ret = false;
			try
			{
				m_path = path;
				ret = chkPath();
			}
			catch (Exception)
			{
				ret = false;
			}
			return ret;
		}
		public bool isDirectory
		{
			get
			{
				return m_fileStyle == JSFileStyle.FOLDER;
			}
		}
		public bool isShortcutDirectory
		{
			get
			{
				bool ret = false;
				if (m_fileStyle == JSFileStyle.SHORTCUT)
				{
					JSFileItem? res = resolve();
					if (res != null)
					{
						ret = res.isDirectory;
					}
				}
				return ret;
			}
		}
		public bool exists
		{
			get
			{
				bool ret = false;
				if (m_path != "")
				{
					ret = chkPath();
				}
				return ret;
			}
		}
		public string fullName
		{
			get
			{
				return m_path;
			}
			set
			{
				setFullName(value);
			}
		}
		public string name
		{
			get
			{
				return m_name;
			}
		}
		public string ext
		{
			get
			{
				return m_ext;
			}
		}
		public string directory
		{
			get
			{
				return m_dir;
			}
		}
		public JSFileItem? parent
		{
			get
			{
				JSFileItem? ret = null;
				if (exists)
				{
					if (m_dir != "")
					{
						ret = new JSFileItem(m_dir);
					}
				}
				return ret;
			}
		}
		public long length
		{
			get
			{
				long ret = 0;
				FileInfo fi = new FileInfo(m_path);
				if (fi.Exists)
				{
					ret = fi.Length;
				}
				return ret;
			}
		}
		public DateTime created
		{
			get
			{
				DateTime ret = DateTime.MinValue;
				if (m_fileStyle == JSFileStyle.FOLDER)
				{
					DirectoryInfo di = new DirectoryInfo(m_path);
					if (di.Exists)
					{
						ret = di.CreationTime;
					}
				}
				else if ((m_fileStyle == JSFileStyle.FILE) || (m_fileStyle == JSFileStyle.SHORTCUT))
				{
					FileInfo fi = new FileInfo(m_path);
					if (fi.Exists)
					{
						ret = fi.CreationTime;
					}
				}
				return ret;
			}
		}
		public DateTime modified
		{
			get
			{
				DateTime ret = DateTime.MinValue;
				if (m_fileStyle == JSFileStyle.FOLDER)
				{
					DirectoryInfo di = new DirectoryInfo(m_path);
					if (di.Exists)
					{
						ret = di.LastWriteTime;
					}
				}
				else if ((m_fileStyle == JSFileStyle.FILE) || (m_fileStyle == JSFileStyle.SHORTCUT))
				{
					FileInfo fi = new FileInfo(m_path);
					if (fi.Exists)
					{
						ret = fi.LastWriteTime;
					}
				}
				return ret;
			}
		}
		public bool remove()
		{
			bool ret = false;
			try
			{
				if (exists)
				{
					if (m_fileStyle == JSFileStyle.FOLDER)
					{
						DirectoryInfo di = new DirectoryInfo(m_path);
						if (di.Exists)
						{
							di.Delete(true);
							ret = true;
						}
					}
					else if ((m_fileStyle == JSFileStyle.FILE) || (m_fileStyle == JSFileStyle.SHORTCUT))
					{
						FileInfo fi = new FileInfo(m_path);
						if (fi.Exists)
						{
							fi.Delete();
							ret = true;
						}
					}
				}
			}
			catch (Exception)
			{
				ret = false;
			}
			return ret;
		}
		public bool rename(string newName)
		{
			bool ret = false;
			try
			{
				if (exists)
				{
					string destPath = Path.GetDirectoryName(newName) ?? "";
					if (string.IsNullOrEmpty(destPath))
					{
						destPath = m_dir;
					}
					destPath = Path.Combine(destPath, Path.GetFileName(newName));
					if (m_path == destPath)
					{
						return false;
					}
					if (m_fileStyle == JSFileStyle.FOLDER)
					{
						DirectoryInfo di = new DirectoryInfo(m_path);
						di.MoveTo(destPath);
						setFullName(destPath);
					}
					else if ((m_fileStyle == JSFileStyle.FILE) || (m_fileStyle == JSFileStyle.SHORTCUT))
					{
						FileInfo di = new FileInfo(m_path);
						di.MoveTo(destPath);
						setFullName(destPath);
					}
					ret = true;
				}
			}
			catch (Exception)
			{
				ret = false;
			}
			return ret;
		}
		public bool copy(string destPath)
		{
			bool ret = false;
			if (string.IsNullOrEmpty(destPath))
			{
				return false;
			}
			try
			{
				if (exists)
				{
					if (m_fileStyle == JSFileStyle.FOLDER)
					{

						FileInfo fi = new FileInfo(m_path);
						string newp = Path.Combine(destPath, m_name);
						fi.CopyTo(newp);
						setFullName(newp);
						ret = true;
					}
				}
			}
			catch (Exception)
			{
				ret = false;
			}
			return ret;
		}
		public bool move(string destPath)
		{
			bool ret = false;
			try
			{
				if (exists)
				{
					if (string.IsNullOrEmpty(destPath))
					{
						return false;
					}
					if (m_path == destPath)
					{
						return false;
					}
					string newp = Path.Combine(destPath, m_name);
					if (m_fileStyle == JSFileStyle.FOLDER)
					{
						DirectoryInfo di = new DirectoryInfo(m_path);
						di.MoveTo(destPath);
						setFullName(destPath);
					}
					else if ((m_fileStyle == JSFileStyle.FILE) || (m_fileStyle == JSFileStyle.SHORTCUT))
					{
						FileInfo fi = new FileInfo(m_path);
						fi.MoveTo(destPath);
						setFullName(destPath);
					}
					ret = true;
				}
			}
			catch (Exception)
			{
				ret = false;
			}
			return ret;
		}
		public override string ToString()
		{
			return m_path;
		}
		public string toString()
		{
			return m_path;
		}
		public bool hidden
		{
			get
			{
				return m_hidden;
			}
			set
			{
				if (m_fileStyle == JSFileStyle.FOLDER)
				{
					DirectoryInfo di = new DirectoryInfo(m_path);
					if (di.Exists)
					{
						if (value)
						{
							di.Attributes |= FileAttributes.Hidden;
						}
						else
						{
							di.Attributes &= ~FileAttributes.Hidden;
						}
						m_hidden = value;
					}
				}
				else if ((m_fileStyle == JSFileStyle.FILE) || (m_fileStyle == JSFileStyle.SHORTCUT))
				{
					FileInfo fi = new FileInfo(m_path);
					if (fi.Exists)
					{
						if (value)
						{
							fi.Attributes |= FileAttributes.Hidden;
						}
						else
						{
							fi.Attributes &= ~FileAttributes.Hidden;
						}
						m_hidden = value;
					}
				}
			}
		}
		public void execute()
		{
			if (m_fileStyle == JSFileStyle.FOLDER)
			{
				Process.Start(new ProcessStartInfo("explorer.exe", m_path) { UseShellExecute = true });
			}
			else if ((m_fileStyle == JSFileStyle.FILE) || (m_fileStyle == JSFileStyle.SHORTCUT))
			{
				Process.Start(new ProcessStartInfo(m_path) { UseShellExecute = true });
			}
		}
		static public string resolveFromFile(string s)
		{
			string ret = "";
			if (Path.GetExtension(s).ToLower() == ".lnk")
			{
				IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
				// ショートカットオブジェクトの取得
				IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(s);

				// ショートカットのリンク先の取得
				ret = shortcut.TargetPath.ToString();
			}
			return ret;
		}
		public String resolvePath()
		{
			return m_resolve;
		}
		public JSFileItem? resolve()
		{
			JSFileItem? ret = null;
			if (m_resolve != "")
			{
				ret = new JSFileItem(m_resolve);
			}
			return ret;
		}
		public JSFileItem[] getFiles()
		{
			JSFileItem[] ret = getFilesFromDir(m_path);
			List<JSFileItem> list = new List<JSFileItem>();
			foreach (var f in ret)
			{
				list.Add(f);
			}
			return list.ToArray();
		}
		public JSFileItem[] getDirectories()
		{
			JSFileItem[] ret = getDirectoriesFromDir(m_path);
			List<JSFileItem> list = new List<JSFileItem>();
			foreach (var f in ret)
			{
				list.Add(f);
			}
			return list.ToArray();
		}
		public bool create()
		{
			bool ret = false;
			if (m_fileStyle == JSFileStyle.NONE)
			{
				if (createFolder(m_path))
				{
					ret = chkPath();
				}
			}
			return ret;
		}
		static public string encode(string str)
		{
			// ExtendScript File.encode互換の実装
			// Uri.EscapeDataStringを使用してRFC 3986準拠のエンコードを実行
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}

			// Uri.EscapeDataStringは最大32766文字の制限があるため、分割処理
			const int limit = 32766;
			StringBuilder result = new StringBuilder();

			for (int i = 0; i < str.Length; i += limit)
			{
				int length = Math.Min(limit, str.Length - i);
				result.Append(Uri.EscapeDataString(str.Substring(i, length)));
			}
			return result.ToString();
		}
		static public string decode(string str)
		{
			return System.Web.HttpUtility.UrlDecode(str);
		}
		static public string? readAllText(string path, string? enc = null)
		{
			string? ret = null;
			if (File.Exists(path) == false)
			{
				return null;
			}
			try
			{
				if (enc != null)
				{
					enc = enc.Trim().ToLower();
				}
				if (enc == null)
				{
					ret = File.ReadAllText(path);
				}
				else if ((enc == "utf-8") || (enc == "utf8"))
				{
					ret = File.ReadAllText(path, Encoding.UTF8);
				}
				else if ((enc == "utf-16") || (enc == "utf16"))
				{
					ret = File.ReadAllText(path, Encoding.Unicode);
				}
				else if ((enc == "shift_jis") || (enc == "shift-jis") || (enc == "sjis"))
				{
					ret = File.ReadAllText(path, Encoding.GetEncoding("shift_jis"));
				}
				else
				{
					ret = File.ReadAllText(path);
				}
			}
			catch (Exception)
			{
				ret = null;
			}
			return ret;
		}
		static public bool writeAllText(string path, string content, string? enc = null)
		{
			bool ret = false;
			try
			{
				if (enc != null)
				{
					enc = enc.Trim().ToLower();
				}
				if (enc == null)
				{
					File.WriteAllText(path, content);
				}
				else if ((enc == "utf-8") || (enc == "utf8"))
				{
					File.WriteAllText(path, content, Encoding.UTF8);
				}
				else if ((enc == "utf-16") || (enc == "utf16"))
				{
					File.WriteAllText(path, content, Encoding.Unicode);
				}
				else if ((enc == "shift_jis") || (enc == "shift-jis") || (enc == "sjis"))
				{
					File.WriteAllText(path, content, Encoding.GetEncoding("shift_jis"));
				}
				else
				{
					File.WriteAllText(path, content);
				}
				ret = true;
			}
			catch (Exception)
			{
				ret = false;
			}
			return ret;
		}
		static public JSFileItem[] getFilesFromDir(string path)
		{
			JSFileItem[] ret = Array.Empty<JSFileItem>();
			try
			{
				var di = new DirectoryInfo(path);
				if (di.Exists)
				{
					var files =
						di.EnumerateFiles("*", SearchOption.TopDirectoryOnly);
					List<JSFileItem> fileList = new List<JSFileItem>();
					foreach (var f in files)
					{
						fileList.Add(new JSFileItem(f.FullName));
					}
					fileList.Sort((a, b) => a.fullName.CompareTo(b.fullName));
					ret = fileList.ToArray();
				}
			}
			catch (Exception)
			{
				ret = Array.Empty<JSFileItem>();
			}
			return ret;
		}
		static public JSFileItem[] getDirectoriesFromDir(string path)
		{
			JSFileItem[] ret = Array.Empty<JSFileItem>();
			try
			{
				var di = new DirectoryInfo(path);
				if (di.Exists)
				{
					var files =
						di.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
					List<JSFileItem> dirList = new List<JSFileItem>();
					foreach (var f in files)
					{
						dirList.Add(new JSFileItem(f.FullName));
					}
					dirList.Sort((a, b) => a.fullName.CompareTo(b.fullName));
					ret = dirList.ToArray();
				}
			}
			catch (Exception)
			{
				ret = Array.Empty<JSFileItem>();
			}
			return ret;
		}
		static public string ls(string path)
		{
			string ret = "";
			if (string.IsNullOrEmpty(path))
			{
				path = Directory.GetCurrentDirectory();
			}
			DirectoryInfo di = new DirectoryInfo(path);
			var entries = getFilesFromDir(di.FullName);
			foreach (var f in entries)
			{
				ret += f.fullName + "\r\n";
			}
			return ret;
		}
		static public string ls()
		{
			string ret = "";
			string path = Directory.GetCurrentDirectory();
			DirectoryInfo di = new DirectoryInfo(path);
			var entries = getFilesFromDir(di.FullName);
			foreach (var f in entries)
			{
				ret += f.fullName + "\r\n";
			}
			return ret;
		}

		static public bool createFolder(string path)
		{
			bool ret = false;
			try
			{
				DirectoryInfo di = new DirectoryInfo(path);
				if (di.Exists == false)
				{
					di.Create();
				}
				ret = true;
			}
			catch (Exception)
			{
				ret = false;
			}
			return ret;
		}

		static public string currentPath
		{
			get 
			{ 
				return Directory.GetCurrentDirectory();
			}
		}
		static public string appDataPath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			}
		}


		static public string localAppDataPath
		{
			get { 
				return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			}
		}

		static public string myDocumentsPath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			}
		}

		static public string desktopPath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			}
		}

		static public string tempPath
		{
			get
			{
				return Path.GetTempPath();
			}
		}

		static public string userProfilePath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			}
		}

		static public string programFilesPath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			}
		}

		static public string systemPath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.System);
			}
		}

		static public string commonAppDataPath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			}
		}

		static public string startupPath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.Startup);
			}
		}

		// 汎用メソッド - 任意の特殊フォルダを取得
		static public string getSpecialFolderPath(Environment.SpecialFolder folder)
		{
			return Environment.GetFolderPath(folder);
		}
	}
}
