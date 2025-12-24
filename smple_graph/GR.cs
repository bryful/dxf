using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using PdfSharp.Pdf;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;

namespace DXF
{
	public enum GRType
	{
		PDF,
		DXF,
		BMP
	}
	public class GR
	{
		private DXF dxf = new DXF();
		private PDF pdf = new PDF();
		private BMP bmp = new BMP();
		public GR(int w, int h)
		{
			Init(w, h);
		}
		public void Init(int w, int h)
		{
			pdf.Init(false, w, h);
			bmp.Init(w, h);
		}
		public bool Save(string fn, GRType gR)
		{
			bool ret = false;
			switch (gR)
			{
				case GRType.PDF:
					ret = pdf.Save(fn);
					break;
				case GRType.DXF:
					ret = dxf.Save(fn);
					break;
				case GRType.BMP:
					ret = bmp.Save(fn);
					break;
			}
			return ret;
		}
		private XPen XPen(Pen p)
		{
			XPen xp = new XPen(
				XColor.FromArgb(p.Color.A, p.Color.R, p.Color.G, p.Color.B),
				p.Width);
			switch (p.DashStyle)
			{
				case DashStyle.Solid:
					xp.DashStyle = XDashStyle.Solid;
					break;
				case DashStyle.Dash:
					xp.DashStyle = XDashStyle.Dash;
					break;
				case DashStyle.Dot:
					xp.DashStyle = XDashStyle.Dot;
					break;
				case DashStyle.DashDot:
					xp.DashStyle = XDashStyle.DashDot;
					break;
				case DashStyle.DashDotDot:
					xp.DashStyle = XDashStyle.DashDotDot;
					break;
			}
			return xp;
		}
		private XBrush XBrush(SolidBrush b)
		{
			XBrush xb = new XSolidBrush(
				XColor.FromArgb(b.Color.A, b.Color.R, b.Color.G, b.Color.B));
			return xb;
		}
		public void DrawLine(Pen p, double x0, double y0, double x1, double y1)
		{
			bmp.DrawLine(p, x0, y0, x0, y1);
			pdf.DrawLine(XPen(p), x0, y0, x1, y1);
			dxf.DrawLine(x0, y0, x1, y1);
		}
		public void DrawLines(Pen p, PointF[] pa)
		{
			if (pa.Length >= 0)
			{
				bmp.DrawLines(p, pa);
				pdf.DrawLines(XPen(p), pa);
				dxf.DrawLines(pa);
			}
		}
		public void DrawLines(Pen p, List<PointF[]> pa)
		{
			if (pa.Count > 0)
			{
				XPen xp = XPen(p);
				foreach (PointF[] pp in pa)
				{
					bmp.DrawLines(p, pp);
					pdf.DrawLines(xp, pp);
					dxf.DrawLines(pp);
				}
			}
		}
		public void DrawPolygon(Pen p, PointF[] pnts)
		{
			bmp.DrawPolygon(p, pnts);
			pdf.DrawPolygon(XPen(p), pnts);
			dxf.DrawPolygon(pnts);
		}
		public void DrawPolygon(Pen p, List<PointF[]> pnts)
		{
			if (pnts.Count > 0)
			{
				foreach (PointF[] pp in pnts)
				{
					DrawPolygon(p, pp);
				}
			}
		}
		public void DrawPolygon(SolidBrush b, PointF[] pnts)
		{
			if (pnts.Length > 0)
			{
				XBrush xb = XBrush(b);
				bmp.DrawPolygon(b, pnts);
				pdf.DrawPolygon(xb, pnts);
				dxf.DrawPolygon(pnts);
			}
		}
		public void DrawPolygon(SolidBrush b, List<PointF[]> pnts)
		{
			if (pnts.Count > 0)
			{
				foreach (PointF[] pp in pnts)
				{
					DrawPolygon(b, pp);
				}
			}
		}
		public void DrawPolygon(Pen p, SolidBrush b, PointF[] pnts)
		{
			if (pnts.Length > 0)
			{
				XPen xp = XPen(p);
				XBrush xb = XBrush(b);
				bmp.DrawPolygon(p, b, pnts);
				pdf.DrawPolygon(xp, xb, pnts);
				dxf.DrawPolygon(pnts);
			}
		}
		public void DrawPolygon(Pen p, SolidBrush b, List<PointF[]> pnts)
		{
			if (pnts.Count > 0)
			{
				foreach (PointF[] pp in pnts)
				{
					DrawPolygon(p, b, pp);
				}
			}
		}
		public void DrawEllipse(Pen p, RectangleF rct)
		{
			bmp.DrawEllipse(p, rct);
			pdf.DrawEllipse(XPen(p), rct);
			dxf.DrawEllipse(rct);
		}
		public void DrawEllipse(SolidBrush b, RectangleF rct)
		{
			bmp.DrawEllipse(b, rct);
			pdf.DrawEllipse(XBrush(b), rct);
			dxf.DrawEllipse(rct);
		}
		public void DrawEllipse(Pen p,SolidBrush b, RectangleF rct)
		{
			bmp.DrawEllipse(p,b, rct);
			pdf.DrawEllipse(XPen(p),XBrush(b), rct);
			dxf.DrawEllipse(rct);
		}
		public void DrawEllipse(Pen p,  PointF cp, float radius)
		{
			bmp.DrawEllipse(p, cp,radius);
			pdf.DrawEllipse(XPen(p), cp, radius);
			dxf.DrawEllipse(cp,radius);
		}
		public void DrawEllipse(SolidBrush b, PointF cp, float radius)
		{
			bmp.DrawEllipse(b, cp, radius);
			pdf.DrawEllipse(XBrush(b), cp, radius);
			dxf.DrawEllipse(cp, radius);
		}
		public void DrawEllipse(Pen p,SolidBrush b, PointF cp, float radius)
		{
			bmp.DrawEllipse(p,b, cp, radius);
			pdf.DrawEllipse(XPen(p),XBrush(b), cp, radius);
			dxf.DrawEllipse(cp, radius);
		}
		public void DrawSemiCircle(Pen pen, PointF center, float radius, double startAngle, double sweepAngle)
		{
			bmp.DrawSemiCircle(pen, center, radius, startAngle, sweepAngle);
			pdf.DrawSemiCircle(XPen(pen), center, radius, startAngle, sweepAngle);
			dxf.DrawSemiCircle(center, radius, (float)startAngle, (float)sweepAngle);
		}
	}
}
