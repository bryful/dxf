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
using System.Windows;
using System.IO;
using System.Diagnostics;
namespace DXF
{
	public class PDF
	{
		private bool _UsedMM = true;
		public bool UnitMM
		{
			get
			{
				return _UsedMM;
			}
		}
		public bool UnitPoint
		{
			get
			{
				return !_UsedMM;
			}
		}
		private PdfDocument doc = null;
		private PdfPage page = null;
		private XGraphics gfx = null;
		private double Widthmm = 210;
		private double Heightmm = 297;
		public PDF()
		{
			Init();
		}
		public PDF(bool usemm,double w, double h)
		{
			Init(usemm, w, h);
		}
		public PDF(double w, double h)
		{
			Init(true,w,h);
		}
		public PDF(int w, int h)
		{
			Init(false, w, h);
		}
		private XUnit PointV(double v)
		{
		if (_UsedMM)
			{
				return XUnit.FromMillimeter(v);
			}
			else
			{
				return XUnit.FromPoint(v);
			}
		}
		private XPoint[] PointA(XPoint[] pnts)
		{
			XPoint[] a = new XPoint[pnts.Length];
			for (int i = 0; i < pnts.Length; i++)
			{
				a[i] = new XPoint(PointV(pnts[i].X).Point, PointV(pnts[i].Y).Point);
			}
			return a;
		}
		private XPoint[] PointA(PointF[] pa)
		{
			List<XPoint> pnts = new List<XPoint>();
			for (int i = 0; i < pa.Length; i++)
			{
				pnts.Add(new XPoint(PointV(pa[i].X).Point, PointV(pa[i].Y).Point));
			}
			return pnts.ToArray();
		}
		private XRect RectV(RectangleF rct)
		{
			return new XRect(
				PointV(rct.X).Point,
				PointV(rct.Y).Point,
				PointV(rct.Width).Point,
				PointV(rct.Height).Point
			);
		}
		private XRect RectV2(PointF cp, float radius)
		{
			return new XRect(
				PointV(cp.X-radius).Point,
				PointV(cp.Y - radius).Point,
				PointV(radius*2).Point,
				PointV(radius * 2).Point
			);
		}
		public void Init(bool UsedMM, double w,double h)
		{
			doc = new PdfDocument();
			page = doc.AddPage();
			_UsedMM = UsedMM;
			page.Width = PointV(w);
			page.Height = PointV(h);
			Widthmm = page.Width.Millimeter;
			Heightmm = page.Height.Millimeter;
			gfx = XGraphics.FromPdfPage(page);
			double cx = page.Width.Point / 2;
			double cy = page.Height.Point / 2;
			gfx.TranslateTransform(cx, cy);
		}
		public void Init()
		{
			doc = new PdfDocument();
			page = doc.AddPage();
			_UsedMM = true;
			page.Size = PageSize.A4;
			Widthmm = page.Width.Millimeter;
			Heightmm = page.Height.Millimeter;
			gfx = XGraphics.FromPdfPage(page);
			double cx = page.Width.Point / 2;
			double cy = page.Height.Point / 2;
			gfx.TranslateTransform(cx, cy);
		}
		public bool Save(string fn)
		{
			bool ret = false;
			if (doc != null)
			{
				try
				{
					if (File.Exists(fn))
					{
						File.Delete(fn);
					}
					doc.Save(fn);
					ret = File.Exists(fn);
				}
				catch
				{
					ret = false;
				}
			}
			return ret;
		}
		public void DrawLine(XPen p, double x0, double y0, double x1, double y1)
		{
			if (gfx != null)
			{
				gfx.DrawLine(p, 
					PointV(x0).Point,
					PointV(y0).Point,
					PointV(x1).Point,
					PointV(y1).Point
				);
			}
		}

		public void DrawLines(XPen p, PointF[] pa)
		{
			if ((gfx != null) && (pa.Length >= 0))
			{
				gfx.DrawLines(p, PointA(pa));
			}
		}
		public void DrawLines(XPen p, List<PointF[]> pas)
		{
			if ((gfx != null) && (pas.Count >= 0))
			{
				for (int i = 0; i < pas.Count; i++)
				{
					DrawLines(p, pas[i]);
				}
			}
		}
		public void DrawPolygon(XBrush b, PointF[] pnts)
		{
			if ((gfx != null) && (pnts.Length >= 0))
			{
				gfx.DrawPolygon(b, PointA(pnts), XFillMode.Winding);
			}
		}
		public void DrawPolygon(XBrush b, List<PointF[]> pnts)
		{
			if ((gfx != null) && (pnts.Count >= 0))
			{
				for (int i = 0; i < pnts.Count; i++)
				{
					DrawPolygon(b, pnts[i]);
				}
			}
		}

		public void DrawPolygon(XPen p, PointF[] pnts)
		{
			if ((gfx != null) && (pnts.Length >= 0))
			{
				gfx.DrawPolygon(p, PointA(pnts));
			}
		}
		public void DrawPolygon(XPen p, List<PointF[]> pnts)
		{
			if ((gfx != null) && (pnts.Count >= 0))
			{
				for (int i = 0; i < pnts.Count; i++)
				{
					DrawPolygon(p, pnts[i]);
				}
			}
		}
		public void DrawPolygon(XPen p, XBrush b, PointF[] pnts)
		{
			if ((gfx != null) && (pnts.Length >= 0))
			{
				gfx.DrawPolygon(p, b, PointA(pnts), XFillMode.Winding);
			}
		}
		public void DrawPolygon(XPen p, XBrush b, List<PointF[]> pnts)
		{
			if ((gfx != null) && (pnts.Count >= 0))
			{
				for (int i = 0; i < pnts.Count; i++)
				{
					DrawPolygon(p,b, pnts[i]);
				}
			}
		}
		public void DrawEllipse(XPen p, RectangleF rct)
		{
			if (gfx != null) 
			{
				double cx = PointV(rct.X ).Point;
				double cy = PointV(rct.Y ).Point;
				double r1 = PointV(rct.Width).Point;
				double r2 = PointV(rct.Height).Point;
				gfx.DrawEllipse(p, cx,cy,r1,r2);
			}
		}
		public void DrawEllipse(XBrush b, RectangleF rct)
		{
			if (gfx != null)
			{
				gfx.DrawEllipse(b, RectV(rct));
			}
		}
		public void DrawEllipse(XPen p,XBrush b, RectangleF rct)
		{
			if (gfx != null)
			{
				gfx.DrawEllipse(p,b, RectV(rct));
			}
		}
		public void DrawEllipse(XPen p, PointF cp,float radius)
		{
			if (gfx != null)
			{
				gfx.DrawEllipse(p,
					PointV(cp.X-radius).Point,
					PointV(cp.Y-radius).Point,
					PointV(radius*2).Point,
					PointV(radius * 2).Point
					);
			}
		}
		public void DrawEllipse(XBrush b, PointF cp, float radius)
		{
			if (gfx != null)
			{
				gfx.DrawEllipse(b,
					RectV2(cp, radius));
			}
		}
		public void DrawEllipse(XPen p, XBrush b, PointF cp, float radius)
		{
			if (gfx != null)
			{
				gfx.DrawEllipse(p,b,
					RectV2(cp, radius)
					);
			}
		}
		public void DrawSemiCircle(XPen pen, PointF center, float radius, double startAngle, double sweepAngle)
		{
			if (gfx != null)
			{
				// 半円を描画するための矩形領域を計算
				var rect = new XRect(
					PointV(center.X - radius).Point,
					PointV(center.Y - radius).Point,
					PointV(radius * 2).Point,
					PointV(radius * 2).Point
				);

				// 半円を描画
				gfx.DrawArc(pen, rect, startAngle, sweepAngle);
				XPen aa =new XPen(XColors.Red, 0.5f);
				aa.DashStyle =XDashStyle.Dot;
			}
		}
	}
}
