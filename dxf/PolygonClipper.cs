using System;
using System.Collections.Generic;
using Clipper2Lib;

namespace dxf
{
	public enum ClipOperation
	{
		/// <summary>
		/// 交差 (2つの図形の重なり部分)
		/// </summary>
		Intersection,
		/// <summary>
		/// 和 (2つの図形の合成)
		/// </summary>
		Union,
		/// <summary>
		/// 差 (1つの図形からもう1つの図形を引いた部分)
		/// </summary>
		Difference,
		/// <summary>
		/// 排他的論理和 (2つの図形の重なり部分を除いた部分)
		/// </summary>
		Xor
	}

	public static class PolygonClipper
	{
		public static List<PointD[]> Execute(
			List<PointD[]> subjectPolygons,
			List<PointD[]> clipPolygons,
			ClipOperation operation,
			FillRule fillRule = FillRule.NonZero,
			int precision = 2)
		{
			var subject = ConvertToPathsD(subjectPolygons);
			var clip = ConvertToPathsD(clipPolygons);

			PathsD result;

			// C# 7.3 互換用に switch 文を従来の形式に変更
			switch (operation)
			{
				case ClipOperation.Intersection:
					result = Clipper.Intersect(subject, clip, fillRule, precision);
					break;

				case ClipOperation.Union:
					result = Clipper.Union(subject, clip, fillRule, precision);
					break;

				case ClipOperation.Difference:
					result = Clipper.Difference(subject, clip, fillRule, precision);
					break;

				case ClipOperation.Xor:
					result = Clipper.Xor(subject, clip, fillRule, precision);
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(operation), "Unsupported operation");
			}

			return ConvertToPointFArrays(result);
		}

		public static List<PointD[]> ConvertToPointFArrays(PathsD paths)
		{
			var results = new List<PointD[]>();
			foreach (var path in paths)
			{
				var points = new PointD[path.Count];
				for (int i = 0; i < path.Count; i++)
				{
					points[i] = new PointD(path[i].x, path[i].y);
				}
				results.Add(points);
			}
			return results;
		}

		public static PathsD ConvertToPathsD(List<PointD[]> polygons)
		{
			var paths = new PathsD();
			foreach (var polygon in polygons)
			{
				var path = new PathD();
				foreach (var pt in polygon)
				{
					path.Add(new Clipper2Lib.PointD(pt.X, pt.Y));
				}
				paths.Add(path);
			}
			return paths;
		}

		public static System.Drawing.PointF[] CreatePolygon(params (float x, float y)[] points)
		{
			var list = new List<System.Drawing.PointF>();
			foreach (var (x, y) in points)
			{
				list.Add(new System.Drawing.PointF(x, y));
			}
			return list.ToArray();
		}

		/*
		public static List<PointD[]> MaskRect(
			List<PointD[]> subjectPolygons,
			RectangleF mask)
		{
			PointF[] maskPolygon = new PointF[4];
			maskPolygon[0] = new PointD(mask.Left, mask.Top);
			maskPolygon[1] = new PointF(mask.Right, mask.Top);
			maskPolygon[2] = new PointF(mask.Right, mask.Bottom);
			maskPolygon[3] = new PointF(mask.Left, mask.Bottom);
			List<PointF[]> clipPolygons = new List<PointF[]>();
			clipPolygons.Add(maskPolygon);
			List<PointF[]> result = Execute(subjectPolygons, clipPolygons, ClipOperation.Intersection);
			return result;
		}
		*/
	}
}
