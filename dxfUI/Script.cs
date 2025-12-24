using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Jint;

namespace dxf
{
	public class Script
	{
		private string[] m_ScriptDefaultFolder = new string[] {""};
		public string[] scriptDefaultFolder
		{
			get { return m_ScriptDefaultFolder; }
		}
		private bool m_UseConsole = false;
		public bool UseConsole { get { return m_UseConsole; } }
		public void SetUseConsole(bool useConsole)
		{
			m_UseConsole = useConsole;
		}
		private TextBox? m_OutputBox = null;
		public TextBox? OutputBox
		{
			get { return m_OutputBox; }
			set { m_OutputBox = value; }
		}
		private string m_Result = "";
		public string Result
		{
			get { return m_Result; }
		}
		private string m_Error = "";
		public string Error
		{
			get { return m_Error; }
		}
		Engine engine = new Engine(cfg => cfg
			.LimitRecursion(100)
			.MaxStatements(10_000)
			.AllowClr(typeof(JSFileItem).Assembly)
		);
		private void ScanScriptDefaultFolder()
		{
			List<string> ll = new List<string>();
			string s = "";
			s = Path.Combine (JSApp.prefFilePath, "scripts");
			if (!Directory.Exists(s))
			{
				Directory.CreateDirectory(s);
			}
			ll.Add(s);
			// 実行ファイルと同じフォルダ内の「<実行ファイル名>_script」フォルダ
			s = Path.Combine(JSApp.exeFileDir, "scripts");
			if (Directory.Exists(s))
			{
				ll.Add(s);
			}
			m_ScriptDefaultFolder = ll.ToArray();
		}
		public Script()
		{
			ScanScriptDefaultFolder();
			Init();
		}
		public void Init(int r = 100, int ms = 10_000)
		{
			engine = new Engine(cfg => cfg
			.LimitRecursion(r)
			.MaxStatements(ms)
			.AllowClr(typeof(JSFileItem).Assembly)); // CLR型へのアクセスを許可

			engine.SetValue("write", Write);
			engine.SetValue("writeln", Writeln);
			engine.SetValue("cls", Cls);
			engine.SetValue("scriptDefaultFolder", scriptDefaultFolder);
			engine.SetValue("alert", new Action<string, string>(Alert));
			engine.SetValue("answerDialog", new Func<string, string, bool>(AnswerDialog));
			engine.SetValue("inputBox", new Func<string, string, string, string?>(InputDialog));
			engine.SetValue("calc", new Func<string, double>(Calculate));
			engine.SetValue("ls", new Func<string,string>(JSFileItem.ls));
			engine.Execute(@"
				var FileItem = importNamespace('dxf').JSFileItem;
				var FileDialog = importNamespace('dxf').JSFileDialog;
				var App = importNamespace('dxf').JSApp;
				var Dxf = importNamespace('dxf').DXF;
				var PointD = importNamespace('dxf').PointD;

			");

		}
		// JavaScriptコードを実行
		public bool Execute(string jsCode)
		{
			bool ret = false;
			m_Error = "";
			m_Result = "";
			try
			{
				engine.Execute(jsCode);
				ret = true;
			}
			catch (Jint.Runtime.JavaScriptException jsEx)
			{
				// JavaScriptエラーの詳細情報を取得
				var location = jsEx.Location;
				string er = "\r\n";
				er += $"JavaScript Error: {jsEx.Error}\r\n";
				er += $"Line: {location.Start.Line}, Column: {location.Start.Column}\r\n";
				if (!string.IsNullOrEmpty(jsEx.JavaScriptStackTrace))
				{
					er += $"Stack Trace:\n{jsEx.JavaScriptStackTrace}\r\n";
				}
				m_Error = er;
				ret = false;
			}
			catch (Jint.Runtime.StatementsCountOverflowException)
			{
				m_Error = "Error: 実行ステートメント数が上限(10,000)を超えました";
				ret = false;
			}
			catch (Jint.Runtime.RecursionDepthOverflowException)
			{
				m_Error = "Error: 再帰の深さが上限(100)を超えました";
				ret = false;
			}
			catch (Exception ex)
			{
				m_Error = $"Error: {ex.Message}";
				ret = false;
			}
			return ret;
		}

		public bool ExecteFile(string filename)
		{
			bool ret = false;
			string filename2 = filename;
			if(Path.GetExtension(filename2) == "")
			{
				filename2 += ".js";
			}
			string s = Path.GetDirectoryName(filename2)?? string.Empty;
			if (string.IsNullOrEmpty(s))
			{
				filename2 = Path.Combine(Directory.GetCurrentDirectory(), filename2);
			}
			//まず普通に
			if (!File.Exists(filename2))
			{
				string n = Path.GetFileName(filename2);

				for (int i = 0; i < m_ScriptDefaultFolder.Length; i++)
				{
					string fn = Path.Combine(m_ScriptDefaultFolder[i], n);
					if (File.Exists(fn))
					{
						filename2 = fn;
						break;
					}
				}
			}
			if (File.Exists(filename2))
			{
				ret = Execute(File.ReadAllText(filename2));
			}
			else
			{
				m_Error += $"Error: ファイルが見つかりません: {filename}";
			}
			return ret;
		}

		// JavaScriptから値を取得
		public object? GetValue(string variableName)
		{
			return engine.GetValue(variableName).ToObject();
		}

		// C#のオブジェクトをJavaScriptに公開
		public void SetObject(string name, object obj)
		{
			engine.SetValue(name, obj);
		}

		public void Write(string msg)
		{
			if (m_UseConsole)
			{
				Console.Write(msg);
			}
			else
			{
				if (m_OutputBox != null)
				{
					m_OutputBox.AppendText(msg);
				}
			}
		}
		public void Writeln(string msg)
		{
			Write(msg + "\r\n");
		}
		public void Cls()
		{
			if (m_UseConsole)
			{
				Console.Write("\f");
			}
			else
			{
				if (m_OutputBox != null)
				{
					m_OutputBox.Text = "";
				}
			}
		}

		public void Alert(string msg, string cap)
		{
			try
			{
				if (string.IsNullOrEmpty(cap))
				{
					cap = "alert";
				}
				using (InputDialog dlg = new InputDialog())
				{
					dlg.DialogType = InputDialogType.ALERT;
					dlg.Caption = cap;
					dlg.Text = msg;
					dlg.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				// MessageBoxが使えない環境の場合の代替処理
				string errorMsg = $"[{cap}] {msg}";
				m_Error = $"Alert Error: {ex.Message}";
			}
		}
		public bool AnswerDialog(string msg, string cap)
		{
			bool ret = false;
			try
			{
				if (string.IsNullOrEmpty(cap))
				{
					cap = "Answer";
				}
				using (InputDialog dlg = new InputDialog())
				{
					dlg.DialogType = InputDialogType.YESNO;
					dlg.Caption = cap;
					dlg.Text = msg;
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						ret = true;
					}
				}
			}
			catch (Exception ex)
			{
				// MessageBoxが使えない環境の場合の代替処理
				string errorMsg = $"[{cap}] {msg}";
				m_Error = $"AnswerDialog Error: {ex.Message}";
			}
			return ret;
		}
		public string? InputDialog(string msg, string cap2,string cap)
		{
			string? ret = null;
			try
			{
				if (string.IsNullOrEmpty(cap2))
				{
					cap2 = "Input Please";
				}
				if (string.IsNullOrEmpty(cap))
				{
					cap = "Input";
				}
				using (InputDialog dlg = new InputDialog())
				{
					dlg.DialogType = InputDialogType.INPUTBOX;
					dlg.Text = msg;
					dlg.Caption2 = cap2;
					dlg.Caption = cap;
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						ret = dlg.Text;
					}
				}
			}
			catch (Exception ex)
			{
				// MessageBoxが使えない環境の場合の代替処理
				string errorMsg = $"[{cap}] {msg}";
				m_Error = $"InputDialog Error: {ex.Message}";
			}
			return ret;
		}
		
		public double Calculate(string expression)
		{
			try
			{
				// 数学関数名を大文字に変換（NCalcの標準形式）
				expression = System.Text.RegularExpressions.Regex.Replace(
					expression,
					@"\b(sin|cos|tan|sqrt|abs|asin|acos|atan|log|log10|exp|floor|ceiling|ceil|round|min|max|pow|sign|truncate)\b",
					m => char.ToUpper(m.Value[0]) + m.Value.Substring(1),
					System.Text.RegularExpressions.RegexOptions.IgnoreCase
				);
				
				var e = new NCalc.Expression(expression);
				
				// カスタム関数のサポート（必要に応じて）
				e.EvaluateFunction += (name, args) =>
				{
					switch (name.ToLower())
					{
						case "sin":
							args.Result = Math.Sin(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "cos":
							args.Result = Math.Cos(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "tan":
							args.Result = Math.Tan(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "sqrt":
							args.Result = Math.Sqrt(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "abs":
							args.Result = Math.Abs(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "asin":
							args.Result = Math.Asin(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "acos":
							args.Result = Math.Acos(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "atan":
							args.Result = Math.Atan(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "log":
							args.Result = Math.Log(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "log10":
							args.Result = Math.Log10(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "exp":
							args.Result = Math.Exp(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "floor":
							args.Result = Math.Floor(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "ceiling":
						case "ceil":
							args.Result = Math.Ceiling(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "round":
							args.Result = Math.Round(Convert.ToDouble(args.Parameters[0].Evaluate()));
							break;
						case "pow":
							args.Result = Math.Pow(
								Convert.ToDouble(args.Parameters[0].Evaluate()),
								Convert.ToDouble(args.Parameters[1].Evaluate())
							);
							break;
					}
				};
				
				return Convert.ToDouble(e.Evaluate());
			}
			catch (Exception ex)
			{
				m_Error = $"Calc Error: {ex.Message}";
				return 0;
			}
		}
	}
}
