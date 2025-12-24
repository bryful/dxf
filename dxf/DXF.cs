using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using netDxf.Entities;
using System.IO;
using System.Windows;
namespace dxf
{
	public class DXF
	{
		private DxfDocument m_dxf = new DxfDocument();
		public DxfDocument DxfDocument
		{
			get { return m_dxf; }
		}
		public DXF() 
		{ 
		}
		public bool save(string fn)
		{
			bool ret = false;
				try
				{
					if (File.Exists(fn))
					{
						File.Delete(fn);
					}
					ret = m_dxf.Save(fn);
				}
				catch
				{
					ret = false;
				}
			return ret;
		}
		static public Vector2[] toVector2(PointD[] a)
		{
			var polygonPoints = new Vector2[a.Length];
			for (int i = 0; i < a.Length; i++)
			{
				polygonPoints[i] = a[i].toVector2();
			}
			return polygonPoints;
		}
		public void drawLine(double x0, double y0, double x1, double y1)
		{
			PointD[] pointDs = new PointD[2];
			pointDs[0] = new PointD(x0, y0);
			pointDs[1] = new PointD(x1, y1);

			Polyline2D t1 = new Polyline2D(toVector2(pointDs));
			t1.IsClosed = false;
			m_dxf.Entities.Add(t1);
		}
		public void drawLine(PointD[] pa)
		{
			if (pa.Length > 1)
			{
				Polyline2D t1 = new Polyline2D(toVector2(pa));
				t1.IsClosed = false;
				m_dxf.Entities.Add(t1);
			}
		}
		public void drawPolygon(PointD[] pnts)
		{
			if (pnts.Length > 1)
			{
				Polyline2D t1 = new Polyline2D(toVector2(pnts));
				t1.IsClosed = true;
				m_dxf.Entities.Add(t1);
			}
		}
		public void drawPolygon(List<PointD[]> pa)
		{
			if (pa.Count > 1)
			{
				for (int i = 0; i < pa.Count; i++)
				{
					drawPolygon(pa[i]);
				}
			}
			
		}
		
		public void drawEllipse(PointD cp, float radius)
		{
			if (radius > 0)
			{
				Ellipse ellipse = new Ellipse(cp.toVector2(), radius*2, radius*2);
				m_dxf.Entities.Add(ellipse);
			}
		}
		public void drawSemiCircle(PointD cp,float radius,float startAngle, float endAngle)
		{
			Arc semiCircle = new Arc(cp.toVector2(), radius, -startAngle - endAngle, -startAngle);
			m_dxf.Entities.Add(semiCircle);
		}


		static public PointD aryCenter(PointD[] a)
		{
			if (a.Length == 0)
				return new PointD(0, 0);
			double sumX = 0;
			double sumY = 0;
			for (int i = 0; i < a.Length; i++)
			{
				sumX += a[i].X;
				sumY += a[i].Y;
			}
			return new PointD(sumX / a.Length, sumY / a.Length);
		}
		/// <summary>
		/// 線分配列を回転する
		/// </summary>
		/// <param name="pa">PointF Array</param>
		/// <param name="cp">Anchor Point</param>
		/// <param name="angleDegrees">Rotaion</param>
		/// <returns></returns>
		static public PointD[] rotAry(PointD[] pa, PointD cp, double angleDegrees)
		{
			PointD[] result = new PointD[pa.Length];
			double angleRadians = angleDegrees * Math.PI / 180.0;

			for (int i = 0; i < pa.Length; i++)
			{
				double dx = pa[i].X - cp.X;
				double dy = pa[i].Y - cp.Y;

				double rotatedX = dx * Math.Cos(angleRadians) - dy * Math.Sin(angleRadians);
				double rotatedY = dx * Math.Sin(angleRadians) + dy * Math.Cos(angleRadians);

				result[i] = new PointD((rotatedX + cp.X), (rotatedY + cp.Y));
			}

			return result;
		}

		/// <summary>
		/// PointFを線分でミラーリングする
		/// </summary>
		/// <param name="point"></param>
		/// <param name="lineStart"></param>
		/// <param name="lineEnd"></param>
		/// <returns></returns>
		static public PointD mirrorPoint(PointD point, PointD lineStart, PointD lineEnd)
		{
			if (lineStart == lineEnd)
				return point; // 線が無効な場合はそのまま返す

			double dx = lineEnd.X - lineStart.X;
			double dy = lineEnd.Y - lineStart.Y;
			double lenSq = dx * dx + dy * dy;

			double px = point.X - lineStart.X;
			double py = point.Y - lineStart.Y;

			double t = (px * dx + py * dy) / lenSq;

			double projX = lineStart.X + t * dx;
			double projY = lineStart.Y + t * dy;

			double mirrorX = 2 * projX - point.X;
			double mirrorY = 2 * projY - point.Y;

			return new PointD(mirrorX, mirrorY);
		}
		/// <summary>
		/// PointF Arrayを線分でミラーリングする
		/// </summary>
		/// <param name="pa"></param>
		/// <param name="lineStart"></param>
		/// <param name="lineEnd"></param>
		/// <returns></returns>
		static public PointD[] mirrorAry(PointD[] pa, PointD lineStart, PointD lineEnd)
		{
			if (pa.Length == 0)
				return pa; // 空の配列の場合はそのまま返す

			PointD[] result = new PointD[pa.Length];
			for (int i = 0; i < pa.Length; i++)
			{
				result[i] = mirrorPoint(pa[i], lineStart, lineEnd);
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
		static public PointD[] scaleAry(PointD[] pa, PointD cp, double sx, double sy)
		{
			PointD[] result = new PointD[pa.Length];

			for (int i = 0; i < pa.Length; i++)
			{
				double dx = pa[i].X - cp.X;
				double dy = pa[i].Y - cp.Y;

				double sdX = dx * sx / 100;
				double sdY = dy * sy / 100;

				result[i] = new PointD((sdX + cp.X), (sdY + cp.Y));
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
		static public PointD[] moveAry(PointD[] pa, double dx, double dy)
		{
			PointD[] result = new PointD[pa.Length];

			for (int i = 0; i < pa.Length; i++)
			{

				result[i] = new PointD((pa[i].X + dx), (pa[i].Y + dy));
			}

			return result;
		}

		static public List<PointD[]> clipping(
			List<PointD[]> subjectPolygons,
			List<PointD[]> clipPolygons,
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
		static public double getAngleAtVertex(PointD point0, PointD point1, PointD point2)
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



		static public PointD[] createRect(double x, double y, double w, double h)
		{
			PointD[] pa = new PointD[4];
			pa[0] = new PointD(x, y);
			pa[1] = new PointD(x + w, y);
			pa[2] = new PointD(x + w, y + h);
			pa[3] = new PointD(x, y + h);
			return pa;
		}
		static public PointD[] createRect(PointD cp, double w, double h)
		{
			return createRect(
				cp.X - w / 2,
				cp.Y - h / 2,
				w,
				h);
		}

		static public PointD[] createTriangle(PointD cp, int count, double radius)
		{
			if (count < 3)
			{
				count = 3;
			}
			PointD[] pa = new PointD[count];
			for (int i = 0; i < count; i++)
			{
				double angle = 2 * Math.PI * i / count;
				pa[i] = new PointD(
					(radius * Math.Cos(angle) + cp.X),
					(radius * Math.Sin(angle)) + cp.Y);
			}
			return pa;
		}

	}
}
