using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using PdfSharp.Pdf;
using System.Drawing;
using System.Drawing.Drawing2D;

using System.Windows;
using System.IO;
using System.Drawing.Imaging;

namespace DXF
{
	public class BMP
	{

		private Bitmap bmp = null;
		private Graphics g = null;

		private float centerX = 0;
		private float centerY = 0;
		public BMP()
		{
			Init(1000,1000);
		}
		public void Init(int w, int h)
		{
			bmp = new Bitmap(w, h,PixelFormat.Format32bppArgb);
			g = Graphics.FromImage(bmp);
			centerX = w / 2;
			centerY = h / 2;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.Bilinear;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;

			g.Clear(Color.Transparent);
		}
		public bool Save(string fn)
		{
			bool ret = false;
			if (bmp != null)
			{
				try
				{
					if (File.Exists(fn))
					{
						File.Delete(fn);
					}
					bmp.Save(fn);
					ret = File.Exists(fn);
				}
				catch
				{
					ret = false;
				}
			}
			return ret;
		}
		public void DrawLine(Pen p, double x0, double y0, double x1, double y1)
		{
			if (g != null)
			{
				g.DrawLine(p,
					(float)x0 +centerX,
					(float)y0 +centerY,
					(float)x1 + centerY,
					(float)y1 + centerY
				);
			}
		}
		public PointF[] ToCenter(PointF[] a)
		{
			PointF[] pnts = new PointF[a.Length];
			for (int i = 0; i < a.Length; i++)
			{
				pnts[i] = new PointF(
					a[i].X + centerX,
					a[i].Y + centerY);
			}
			return pnts;
		}
		public List<PointF[]> ToCenter(List<PointF[]> a)
		{
			List<PointF[]> pnts = new List<PointF[]>();
			for (int i = 0; i < a.Count; i++)
			{
				pnts.Add(ToCenter(a[i]));
			}
			return pnts;
		}
		public RectangleF ToCenter(RectangleF a)
		{
			return new RectangleF(
				a.X + centerX,
				a.Y + centerY,
				a.Width,
				a.Height);
		}
		public void DrawLines(Pen p, PointF[] pa)
		{
			if ((g != null) && (pa.Length >= 0))
			{
				g.DrawLines(p, ToCenter(pa));
			}
		}
		public void DrawLines(Pen p, List<PointF[]> pas)
		{
			if ((g != null) && (pas.Count >= 0))
			{
				for (int i = 0; i < pas.Count; i++)
				{
					DrawLines(p, pas[i]);
				}
			}
		}
		public void DrawPolygon(SolidBrush b, PointF[] pnts)
		{
			if ((g != null) && (pnts.Length >= 0))
			{
				g.FillPolygon(b, ToCenter(pnts));
			}
		}
		public void DrawPolygon(SolidBrush b, List<PointF[]> pnts)
		{
			if ((g != null) && (pnts.Count >= 0))
			{
				for (int i = 0; i < pnts.Count; i++)
				{
					DrawPolygon(b, pnts[i]);
				}
			}
		}
		public void DrawPolygon(Pen p, PointF[] pnts)
		{
			if ((g != null) && (pnts.Length >= 0))
			{
				g.DrawPolygon(p, ToCenter(pnts));
			}
		}
		public void DrawPolygon(Pen p, List<PointF[]> pnts)
		{
			if ((g != null) && (pnts.Count >= 0))
			{
				for (int i = 0; i < pnts.Count; i++)
				{
					DrawPolygon(p, pnts[i]);
				}
			}
		}
		public void DrawPolygon(Pen p, SolidBrush b, PointF[] pnts)
		{
			if ((g != null) && (pnts.Length >= 0))
			{
				g.FillPolygon(b, ToCenter(pnts));
				g.DrawPolygon(p, ToCenter(pnts));
			}
		}
		public void DrawPolygon(Pen p, Brush b, List<PointF[]> pnts)
		{
			if ((g != null) && (pnts.Count >= 0))
			{
				for (int i = 0; i < pnts.Count; i++)
				{
					g.FillPolygon(b, ToCenter(pnts[i]));
					g.DrawPolygon(p, ToCenter(pnts[i]));
				}
			}
		}
		public void DrawEllipse(Pen p, RectangleF rct)
		{
			if (g != null)
			{
				g.DrawEllipse(p, ToCenter(rct));
			}
		}
		public void DrawEllipse(SolidBrush b, RectangleF rct)
		{
			if (g != null)
			{
				g.FillEllipse(b, ToCenter(rct));
			}
		}
		public void DrawEllipse(Pen p, Brush b, RectangleF rct)
		{
			if (g != null)
			{
				g.DrawEllipse(p, ToCenter(rct));
				g.FillEllipse(b, ToCenter(rct));
			}
		}
		public void DrawEllipse(Pen p, PointF cp, float radius)
		{
			if (g != null)
			{
				g.DrawEllipse(p,
					cp.X+centerX -radius,
					cp.Y + centerY - radius,
					radius * 2, radius * 2);
			}
		}
		public void DrawEllipse(SolidBrush b, PointF cp, float radius)
		{
			if (g != null)
			{
				g.FillEllipse(b,
					cp.X + centerX - radius,
					cp.Y + centerY - radius,
					radius * 2, radius * 2);
			}
		}
		public void DrawEllipse(Pen p, SolidBrush b, PointF cp, float radius)
		{
			if (g != null)
			{
				g.FillEllipse(b,
					cp.X + centerX - radius,
					cp.Y + centerY - radius,
					radius * 2, radius * 2);
				g.DrawEllipse(p,
					cp.X + centerX - radius,
					cp.Y + centerY - radius,
				radius * 2, radius * 2);
			}
		}
		public void DrawSemiCircle(Pen pen, PointF center, float radius, double startAngle, double sweepAngle)
		{
			if (g != null)
			{
				// 半円を描画するための矩形領域を計算
				var rect = new RectangleF(
					center.X - radius +centerX,
					center.Y - radius + centerY,
					radius * 2,
					radius * 2
				);

				// 半円を描画
				g.DrawArc(pen, rect, (Single)startAngle, (Single)sweepAngle);
			}
		}
	}
}
