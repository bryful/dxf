using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.CodeAnalysis.Scripting;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace DXF
{
	public class Scripts
	{
		public string[] CommandLineArgs { get; set; } = new string[0];		
		public Scripts() 
		{
		}
		// *********************************************************
		public string ExecuteFile(string code)
		{
			string result = "error!";
			if (string.IsNullOrEmpty(code))
			{
				return "empty code!";
			}
			try
			{
				string script = File.ReadAllText(code);
				result = ExecuteCode(script);
			}
			catch (Exception ex)
			{
				result = "Error2!\n" + ex.Message;
			}
			return result;
		}
		public string ExecuteFile(string[] codes)
		{
			string result = "error!";
			if (codes.Length<=0)
			{
				return "empty code!";
			}
			try
			{
				string script = "";
				foreach (string code in codes)
				{
					if (File.Exists(code))
					{
						string s= File.ReadAllText(code);
						if(s.Length > 0)
						{
							script += s;
						}
					}
				}
				result = ExecuteCode(script);
			}
			catch (Exception ex)
			{
				result = "Error2!\n" + ex.Message;
			}
			return result;
		}

		public string ExecuteCode(string code)
		{
			Root root= new Root();
			root.SetCommandLineArgs(CommandLineArgs);
			string result = "error!";
			try
			{

				if (string.IsNullOrEmpty(code))
				{
					return "empty code";
				}
				List<Assembly> assembly = new List<Assembly>()
			{
				//Assembly.Load("System"),
				//Assembly.Load("System.IO"),
				//Assembly.Load("System.Drawing"),
				//Assembly.Load("System.Windows.Form"),
				//Assembly.Load("PDFSharp"),
				//Assembly.Load("netDxf"),

				//Assembly.GetAssembly(typeof(System.Dynamic.DynamicObject)),  // System.Code
				//Assembly.GetAssembly(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo)),  // Microsoft.CSharp
				//Assembly.GetAssembly(typeof(System.Dynamic.ExpandoObject)),
				//Assembly.GetAssembly(typeof(System.Data.DataTable)),
				//Assembly.GetAssembly(typeof(System.Object)),
				//Assembly.GetAssembly(typeof(PDF)),
				//Assembly.GetAssembly(typeof(Root)),
				//Assembly.GetAssembly(typeof(DXF)),
				Assembly.GetExecutingAssembly()


			};

				List<string> import = new List<string>()
			{
				"System",
				"System.Dynamic",
				"System.Linq",
				"System.Text",
				"System.IO",
				"System.Collections.Generic",
				"System.Data",
				"System.Drawing",
				"System.Drawing.Drawing2D",
				"System.Drawing.Imaging",
				"System.Windows.Forms",
				"PdfSharp",
				"PdfSharp.Drawing",
				"netDxf",
				"graph.DXF",
				"graph.PDF",
				"graph.GR",
				"graph",

			};

				var opt = ScriptOptions.Default.AddReferences(assembly).AddImports(import);
				var script = CSharpScript.Create(code, opt,typeof(Root));
				script.RunAsync(root);
				result = "";
			}
			catch (CompilationErrorException ex)
			{
				Console.WriteLine(code);
				result = "compile Error!\n" + ex.Message;
			}
			catch (Exception ex)
			{
				result = "Error1!\n" + ex.Message;
			}

			return result;
		}
		public Bitmap MakeBitMap(int w,int h)
		{
			Bitmap bitmap = new Bitmap(w, h);
			Graphics g = Graphics.FromImage(bitmap);
			g.Clear(Color.White);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.Bilinear;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			bitmap.Save("test.png", System.Drawing.Imaging.ImageFormat.Png);
			return bitmap;
		}
		
	}
}
