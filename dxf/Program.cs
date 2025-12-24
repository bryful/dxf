// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using dxf;

class Program
{
	static void ShowHelp()
	{
		string str = "dxf - dxf file 作成コマンド\r\n";
		str += "\t使い方: dxf <スクリプトファイル>\r\n";
		Console.WriteLine(str);
	}
	// STAThread属性を追加(MessageBox使用に必要)
	[STAThread]
	static void Main(string[] args)
	{
		dxf.Script script = new dxf.Script();
		
		if (args.Length == 0)
		{
			ShowHelp();
		}
		else if (args.Length >= 1)
		{
			string scriptFile = args[0];
			if (script.ExecuteFile(scriptFile) == false)
			{
				Console.WriteLine(script.Error);
			}
		}
	}

}