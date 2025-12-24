using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using DXF;
using System.IO;
using System.Security.Policy;

namespace graph_fw
{
	internal class Program
	{
		/*
		static void Main(string[] args)
		{
		}
		*/
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		static extern int MessageBoxW(int hWnd, [MarshalAs(UnmanagedType.LPWStr)] string lpText, [MarshalAs(UnmanagedType.LPWStr)] string lpCaption, uint uType);

		public static void Usage()
		{
			string msg = "Usage: graph.exe  <filename1> <filename2> .. [-arg] [arg1] [arg2]...\n";
			
			msg += "  -arg 以前が読み込むスクリプトファイル名、以後が引数として受け取れる\n";
			Console.WriteLine(msg);
		}
		[STAThread]
		static int Main(string[] args)
		{
			int re = -1;

			if (args.Length == 0)
			{
				Usage();
				return -1;
			}	

			List<string> filenames = new List<string>();
			List<string> commandArgs = new List<string>();

			bool isArg = false;
			for (int i = 0; i < args.Length; i++)
			{
				string cm = args[i].ToLower();
				if (
					(cm.IndexOf("-arg")==0)|| (cm.IndexOf("/arg")==0))
				{
					isArg = true;
					continue;
				}
				if (isArg==false)
				{
					string fn = args[i];
					string e = Path.GetExtension(fn).ToLower();
					if (e == "")
					{
						fn += ".cs";
					}
					if ((e == ".cs") || (e == ".csx") || (e == ".csinc"))
					{
						if (File.Exists(fn))
						{
							filenames.Add(fn);
						}
					}
				}
				else
				{
					commandArgs.Add(args[i]);
				}
			}


			if (filenames.Count >0)
			{
				try
				{
					Scripts scripts = new Scripts();
					scripts.CommandLineArgs = commandArgs.ToArray();
					string result = scripts.ExecuteFile(filenames.ToArray());
					Console.WriteLine(result);
					re = 0;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					re = -1;
				}
			}
			else
			{
				Usage();
				re = -1;
			}
			return re;
		}
	}
}
