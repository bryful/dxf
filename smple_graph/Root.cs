using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.CodeAnalysis.Scripting;
using System.Diagnostics;

namespace DXF
{
	public class Root
	{
		private string[] _Args = new string[0];
		public string[] CommandLineArgs
		{
			get
			{
				return _Args;
			}
		}
		public void SetCommandLineArgs(string[] args)
		{
			_Args = args;
		}
		//public PDF PDF = new PDF();
		//public DXF DXF = new DXF();
		//public GR GR = new GR(0, 0);
		public Root()
		{
			_Args = Environment.GetCommandLineArgs();
		}

		static public RectangleF AryRect(PointF[] a )
		{
			if (a.Length == 0)
				return new RectangleF(0, 0, 0, 0);
			float minX = a[0].X;
			float minY = a[0].Y;
			float maxX = a[0].X;
			float maxY = a[0].Y;
			for (int i = 1; i < a.Length; i++)
			{
				if (a[i].X < minX) minX = a[i].X;
				if (a[i].Y < minY) minY = a[i].Y;
				if (a[i].X > maxX) maxX = a[i].X;
				if (a[i].Y > maxY) maxY = a[i].Y;
			}
			return new RectangleF(minX, minY, maxX - minX, maxY - minY);
		}
		static public PointF AryCenter(PointF[] a)
		{
			if (a.Length == 0)
				return new PointF(0, 0);
			float sumX = 0;
			float sumY = 0;
			for (int i = 0; i < a.Length; i++)
			{
				sumX += a[i].X;
				sumY += a[i].Y;
			}
			return new PointF(sumX / a.Length, sumY / a.Length);
		}
		/// <summary>
		/// 線分配列を回転する
		/// </summary>
		/// <param name="pa">PointF Array</param>
		/// <param name="cp">Anchor Point</param>
		/// <param name="angleDegrees">Rotaion</param>
		/// <returns></returns>
		static public PointF[] RotAry(PointF[] pa, PointF cp, float angleDegrees)
		{
			PointF[] result = new PointF[pa.Length];
			double angleRadians = angleDegrees * Math.PI / 180.0;

			for (int i = 0; i < pa.Length; i++)
			{
				double dx = pa[i].X - cp.X;
				double dy = pa[i].Y - cp.Y;

				double rotatedX = dx * Math.Cos(angleRadians) - dy * Math.Sin(angleRadians);
				double rotatedY = dx * Math.Sin(angleRadians) + dy * Math.Cos(angleRadians);

				result[i] = new PointF((float)(rotatedX + cp.X), (float)(rotatedY + cp.Y));
			}

			return result;
		}
		/// <summary>
		/// PointF ArrayをRectangleFに変換する
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		static public PointF[] RectToAry(RectangleF rect)
		{

			PointF[] pa = new PointF[4];
			pa[0] = new PointF(rect.Left, rect.Top);
			pa[1] = new PointF(rect.Right, rect.Top);
			pa[2] = new PointF(rect.Right, rect.Bottom);
			pa[3] = new PointF(rect.Left, rect.Bottom);

			return pa;
		}
		/// <summary>
		/// PointFを線分でミラーリングする
		/// </summary>
		/// <param name="point"></param>
		/// <param name="lineStart"></param>
		/// <param name="lineEnd"></param>
		/// <returns></returns>
		static public PointF MirrorPoint(PointF point, PointF lineStart, PointF lineEnd)
		{
			if (lineStart == lineEnd)
				return point; // 線が無効な場合はそのまま返す

			float dx = lineEnd.X - lineStart.X;
			float dy = lineEnd.Y - lineStart.Y;
			float lenSq = dx * dx + dy * dy;

			float px = point.X - lineStart.X;
			float py = point.Y - lineStart.Y;

			float t = (px * dx + py * dy) / lenSq;

			float projX = lineStart.X + t * dx;
			float projY = lineStart.Y + t * dy;

			float mirrorX = 2 * projX - point.X;
			float mirrorY = 2 * projY - point.Y;

			return new PointF(mirrorX, mirrorY);
		}
		/// <summary>
		/// PointF Arrayを線分でミラーリングする
		/// </summary>
		/// <param name="pa"></param>
		/// <param name="lineStart"></param>
		/// <param name="lineEnd"></param>
		/// <returns></returns>
		static public PointF[] MirrorAry(PointF[] pa, PointF lineStart, PointF lineEnd)
		{
			if (pa.Length == 0)
				return pa; // 空の配列の場合はそのまま返す

			PointF[] result = new PointF[pa.Length];
			for (int i = 0; i < pa.Length; i++)
			{
				result[i] = MirrorPoint(pa[i], lineStart, lineEnd);
			}
			return result;
		}
		/// <summary>
		/// PointF Arrayをスケールする
		/// </summary>
		/// <param name="pa"></param>
		/// <param name="cp"></param>
		/// <param name="sx"></param>
		/// <param name="sy"></param>
		/// <returns></returns>
		static public PointF[] ScaleAry(PointF[] pa, PointF cp, float sx, float sy)
		{
			PointF[] result = new PointF[pa.Length];

			for (int i = 0; i < pa.Length; i++)
			{
				double dx = pa[i].X - cp.X;
				double dy = pa[i].Y - cp.Y;

				double sdX = dx * sx/100;
				double sdY = dx * sy/100;

				result[i] = new PointF((float)(sdX + cp.X), (float)(sdY + cp.Y));
			}

			return result;
		}
		/// <summary>
		/// PointF Arrayを移動する
		/// </summary>
		/// <param name="pa"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		static public PointF[] MoveAry(PointF[] pa,  float dx, float dy)
		{
			PointF[] result = new PointF[pa.Length];

			for (int i = 0; i < pa.Length; i++)
			{

				result[i] = new PointF((float)(pa[i].X + dx), (float)(pa[i].Y + dy));
			}

			return result;
		}
		/// <summary>
		/// PointF ArrayをRectangleFでクリッピングする
		/// </summary>
		/// <param name="rs"></param>
		/// <param name="mask"></param>
		/// <returns></returns>
		static public List<PointF[]> ClippingRect(List<PointF[]> rs, RectangleF mask)
		{
			return PolygonClipper.MaskRect(rs, mask);
		}
		static public List<PointF[]> Clipping(
			List<PointF[]> subjectPolygons,
			List<PointF[]> clipPolygons,
			ClipOperation operation)
		{
			return PolygonClipper.Execute(
				subjectPolygons,
				clipPolygons,
				operation);
		}
		/// <summary>
		/// 線分point0,point1,point2の頂点point1での角度を計算する
		/// </summary>
		/// <param name="point0"></param>
		/// <param name="point1"></param>
		/// <param name="point2"></param>
		/// <returns></returns>
		static public double GetAngleAtVertex(PointF point0, PointF point1, PointF point2)
		{
			// ベクトルA = Point1 - Point0
			double ax = point0.X - point1.X;
			double ay = point0.Y - point1.Y;

			// ベクトルB = Point2 - Point1
			double bx = point2.X - point1.X;
			double by = point2.Y - point1.Y;

			// 内積とベクトル長を計算
			double dot = ax * bx + ay * by;
			double magA = Math.Sqrt(ax * ax + ay * ay);
			double magB = Math.Sqrt(bx * bx + by * by);

			if (magA == 0 || magB == 0) return 0; // ゼロ除算防止

			double cosTheta = dot / (magA * magB);

			// 丸め誤差対策：cos値を [-1, 1] に制限
			cosTheta = Math.Max(-1.0, Math.Min(1.0, cosTheta));

			// 弧度 → 度 に変換
			double angleRad = Math.Acos(cosTheta);
			double angleDeg = angleRad * 180.0 / Math.PI;

			return angleDeg;
		}

		public string SaveFileDialog(string path ="",string title="SaveDialog", string filter="*.*|*.*")
		{
			string ret = "";
			using (SaveFileDialog dlg = new SaveFileDialog())
			{
				if(string.IsNullOrEmpty(path))
				{
					dlg.InitialDirectory= Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
				}
				else
				{
					if (File.Exists(path))
					{
						dlg.InitialDirectory = Path.GetDirectoryName(path);
						dlg.FileName = Path.GetFileName(path);
					}else if (Directory.Exists(path))
					{
						dlg.InitialDirectory = path;
					}
					else
					{
						dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
					}
				}

					dlg.FileName = path;
				dlg.Title = title;
				dlg.Filter = filter;
				dlg.RestoreDirectory = true;
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					ret = dlg.FileName;
				}
			}
			return ret;
		}
		public string OpenFileDialog(string path = "", string title = "OpenDialog", string filter = "*.*|*.*")
		{
			string ret = "";
			using (OpenFileDialog dlg = new OpenFileDialog())
			{
				if (string.IsNullOrEmpty(path))
				{
					dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
				}
				else
				{
					if (File.Exists(path))
					{
						dlg.InitialDirectory = Path.GetDirectoryName(path);
						dlg.FileName = Path.GetFileName(path);
					}
					else if (Directory.Exists(path))
					{
						dlg.InitialDirectory = path;
					}
					else
					{
						dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
					}
				}

				dlg.FileName = path;
				dlg.Title = title;
				dlg.Filter = filter;
				dlg.RestoreDirectory = true;
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					ret = dlg.FileName;
				}
			}
			return ret;
		}
		public string InputDialog(string txt = "", string title = "InputDialog")
		{
			string ret = "";
			using (InputDialog dlg = new InputDialog())
			{
				dlg.Text = txt;
				dlg.Title = title;
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					ret = dlg.Text;
				}
			}
			return ret;
		}
		public async Task<double> CalcTextAsync(string s)
		{
			return await CSharpScript.EvaluateAsync<double>(s);
		}
		public string Call(string[] args)
		{
			string arg = "";
			string app = "";
			if (args.Length > 0)
			{
				foreach (string s in args)
				{
					string s1 = s.Trim();
					if (arg != "")
					{
						arg += " ";
					}
					if (s1.IndexOf(" ") >= 0)
					{
						s1 = "\"" + s1 + "\"";
					}
					if(app=="")
					{
						app = s1;
					}
					else
					{
						arg += s1;
					}
				}
			}
			ProcessStartInfo processInfo = new ProcessStartInfo
			{
				FileName = app,
				Arguments = arg,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				WorkingDirectory = Environment.CurrentDirectory,
				RedirectStandardError = true,
			};

			var output = new StringBuilder();
			var timeout = TimeSpan.FromMinutes(2); // 2分だけ待つ
			using (var process = Process.Start(processInfo))
			{
				var stdout = new StringBuilder();
				var stderr = new StringBuilder();

				process.OutputDataReceived += (sender, e) => { if (e.Data != null) { stdout.AppendLine(e.Data); } }; // 標準出力に書き込まれた文字列を取り出す
				process.ErrorDataReceived += (sender, e) => { if (e.Data != null) { stderr.AppendLine(e.Data); } }; // 標準エラー出力に書き込まれた文字列を取り出す
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();

				var isTimedOut = false;

				if (!process.WaitForExit((int)timeout.TotalMilliseconds))
				{
					isTimedOut = true;
					process.Kill();
				}
				process.CancelOutputRead();
				process.CancelErrorRead();

				output.AppendLine(stdout.ToString());
				output.AppendLine(stderr.ToString());
				if (isTimedOut)
					output.AppendLine("TIMEOUT AT " + DateTimeOffset.Now);
			}
			return output.ToString();  // 
			/*
			Process process = Process.Start(processInfo);
			process.WaitForExit(5000);

			return process.StandardOutput.ReadToEnd();
			*/
		}
		static public PointF[] CreateRect(float x,float y, float w,float h)
		{
			PointF[] pa = new PointF[4];
			pa[0] = new PointF(x, y);
			pa[1] = new PointF(x + w, y);
			pa[2] = new PointF(x + w, y + h);
			pa[3] = new PointF(x, y + h);
			return pa;
		}
		static public PointF[] CreateRect(RectangleF rect)
		{
			PointF[] pa = new PointF[4];
			pa[0] = new PointF(rect.Left, rect.Top);
			pa[1] = new PointF(rect.Right, rect.Top);
			pa[2] = new PointF(rect.Right, rect.Bottom);
			pa[3] = new PointF(rect.Left, rect.Bottom);
			return pa;
		}
		static public PointF[] CreateTriangle(int count, float radius)
		{
			if(count < 3)
			{
				count = 3;
			}
			PointF[] pa = new PointF[count];
			for (int i = 0; i < count; i++)
			{
				double angle = 2 * Math.PI * i / count;
				pa[i] = new PointF(
					(float)(radius * Math.Cos(angle)),
					(float)(radius * Math.Sin(angle)));
			}
			return pa;
		}
		}
}