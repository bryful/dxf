using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace dxf
{
	public class JSApp
	{
		public string aaa = "";
		public JSApp() 
		{
			aaa = exeFilePath;
		}
		static public string exeFilePath
		{
			get
			{
				return Environment.ProcessPath ?? string.Empty;
			}
		}
		static public string exeFileName
		{
			get
			{
				return System.IO.Path.GetFileName(Environment.ProcessPath??string.Empty);
			}
		}
		static public string exeFileDir
		{
			get
			{
				return System.IO.Path.GetDirectoryName(Environment.ProcessPath ?? string.Empty)?? string.Empty;
			}
		}
		static public string exeFileNameWithoutExtension
		{
			get
			{
				return System.IO.Path.GetFileNameWithoutExtension(Environment.ProcessPath ?? string.Empty);
			}
		}
		static public string prefFilePath		{
			get
			{
				string d = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				string n = exeFileNameWithoutExtension;
				string dir = System.IO.Path.Combine(d, n);
				if (!System.IO.Directory.Exists(dir))
				{
					System.IO.Directory.CreateDirectory(dir);
				}
				return dir;
			}
		}
		static public string prefFileName
		{
			get
			{
				string filepath = System.IO.Path.Combine(prefFilePath, exeFileNameWithoutExtension + ".pref");
				return filepath;
			}
		}
		static bool openCmd(string command)
		{
			bool ret = false;
			if (string.IsNullOrEmpty(command))
			{
				return ret;
			}
			System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
			psi.FileName = "cmd.exe";
			psi.Arguments = "/C " + command;
			psi.UseShellExecute = false;
			//psi.CreateNoWindow = true;
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo = psi;
			return proc.Start();
		}
		static int callCmd(string command)
		{
			if (string.IsNullOrEmpty(command))
			{
				return -1;
			}
			System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
			psi.FileName = "cmd.exe";
			psi.Arguments = "/C " + command;
			psi.UseShellExecute = false;
			//psi.CreateNoWindow = true;
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo = psi;
			proc.Start();
			proc.WaitForExit();
			return proc.ExitCode;
		}
		static public string callCmdGetStd(string command)
		{
			System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
			psi.FileName = "cmd.exe";
			psi.Arguments = "/C " + command;
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			psi.UseShellExecute = false;
			psi.CreateNoWindow = true;
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo = psi;
			proc.Start();
			string output = proc.StandardOutput.ReadToEnd();
			string error = proc.StandardError.ReadToEnd();
			proc.WaitForExit();
			if (!string.IsNullOrEmpty(error))
			{
				return "ERROR: " + error;
			}
			return output;
		}
		static public bool openProcess(string ps, string arg)
		{
			bool ret = false;
			if (string.IsNullOrEmpty(ps))
			{
				return ret;
			}
			if (!System.IO.File.Exists(ps))
			{
				return ret;
			}
			if (string.IsNullOrEmpty(arg))
			{
				arg = "";
			}
			System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
			psi.FileName = ps;
			psi.Arguments = arg;
			//psi.UseShellExecute = false;
			//psi.CreateNoWindow = true;
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo = psi;
			return proc.Start();
		}
		static public int callProcess(string ps, string arg)
		{
			int ret = -1;
			if (string.IsNullOrEmpty(ps))
			{
				return ret;
			}
			if (!System.IO.File.Exists(ps))
			{
				return ret;
			}
			if (string.IsNullOrEmpty(arg))
			{
				arg = "";
			}
			System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
			psi.FileName = ps;
			psi.Arguments = arg;
			//psi.UseShellExecute = false;
			//psi.CreateNoWindow = true;
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo = psi;
			proc.Start();
			proc.WaitForExit();
			ret = proc.ExitCode;
			return ret;
		}
		static public string callProcessGetStd(string ps,string arg)
		{
			if (string.IsNullOrEmpty(ps))
			{
				return "ERROR: No Process Specified.";
			}
			if (!System.IO.File.Exists(ps))
			{
				return "ERROR: Process Not Found.";
			}
			if (string.IsNullOrEmpty(arg))
			{
				arg = "";
			}
			System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
			psi.FileName = ps;
			psi.Arguments = arg;
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			psi.UseShellExecute = false;
			psi.CreateNoWindow = true;
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo = psi;
			proc.Start();
			string output = proc.StandardOutput.ReadToEnd();
			string error = proc.StandardError.ReadToEnd();
			proc.WaitForExit();
			if (!string.IsNullOrEmpty(error))
			{
				return "ERROR: " + error;
			}
			return output;
		}
		static public string[] commandLine()
		{
			return Environment.GetCommandLineArgs();
		}
		static public string commandLineJson()
		{
			JsonArray ja = new JsonArray();
			foreach(var s in Environment.GetCommandLineArgs())
			{
				ja.Add(s);
			}
			return ja.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
		}
		static public string getEnv(string varName)
		{
			if (string.IsNullOrEmpty(varName))
			{
				return "";
			}
			return Environment.GetEnvironmentVariable(varName) ?? "";
		}
		static public string[] getFileFromPath(string pat)
		{
			if (string.IsNullOrEmpty(pat))
			{
				return Array.Empty<string>();
			}
			string d = getEnv("PATH");
			string[] ds = d.Split(';', StringSplitOptions.RemoveEmptyEntries);
			
			List<string> files = new List<string>();
			foreach (var p in ds)
			{
				if (Directory.Exists(p) == false)
				{
					continue;
				}
				string[] ps = System.IO.Directory.GetFiles(p, pat);
				foreach (var f in ps)
				{
					files.Add(f);
				}
			}
			return files.ToArray();
		}
	}
}
