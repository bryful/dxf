using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.IO;

namespace dxf
{
	public class Tetrahedron
	{
		private double _TopWidth = 16;
		private double _TopDepth = 11;
		private double _CubeHeight = 17;

		private double _RotFront = 10;
		private double _RotRear = 0;
		private double _RotRight = 7;
		private double _RotLeft = 7;

		private bool _TopHEnable = true;
		public bool TopHEnable { get { return _TopHEnable; } }
		private double _TopHWidth = 3;
		private double _TopHHeight = 5;
		private double _TopHTop = 2.7;
		private double _TopHLeft = 6.5;

		private bool _RightHEnable = true;
		public bool RightHEnable { get { return _RightHEnable; } }
		private double _RightHWidth = 3;
		private double _RightHHeight = 5;
		private double _RightHTop = 2.2;
		private double _RightHLeft = 3.5;

		private bool _LeftHEnable = true;
		public bool LeftHEnable { get { return _LeftHEnable; } }
		private double _LeftHWidth = 3;
		private double _LeftHHeight = 5;
		private double _LeftHTop = 2.2;
		private double _LeftHLeft = 3.5;
		// ******************************************************
		public PointF[] _CubeTop = new PointF[4];
		public PointF[] _CubeFront = new PointF[4];
		public PointF[] _CubeRight = new PointF[4];
		public PointF[] _CubeLeft = new PointF[4];
		public PointF[] _CubeRear = new PointF[4];
		public PointF[] _CubeBottom = new PointF[4];
		public PointF[] _HoleTop = new PointF[4];
		public PointF[] _HoleRight = new PointF[4];
		public PointF[] _HoleLeft = new PointF[4];
		// ******************************************************
		public PointF[] Outline = new PointF[0];
		public PointF[] OriLine1 = new PointF[0];
		public PointF[] OriLine2 = new PointF[0];
		public PointF[] TopHole = new PointF[0];
		public PointF[] RightHole = new PointF[0];
		public PointF[] LeftHole = new PointF[0];
		public Tetrahedron() 
		{
		
		}
		private PointF[] CreateSurf()
		{
			PointF[] pnts = new PointF[4];
			pnts[0] = new PointF(0, 0);
			pnts[1] = new PointF(0, 0);
			pnts[2] = new PointF(0, 0);
			pnts[3] = new PointF(0, 0);
			return pnts;
		}
		public bool FromJson(string js)
		{
			bool ret = false;
			try
			{
				JsonObject json = JsonNode.Parse(js).AsObject();
				_TopWidth = json["TopWidth"].GetValue<double>();
				_TopDepth = json["TopDepth"].GetValue<double>();
				_CubeHeight = json["CubeHeight"].GetValue<double>();
				_RotFront = json["RotFront"].GetValue<double>();
				_RotRear = json["RotRear"].GetValue<double>();
				_RotRight = json["RotRight"].GetValue<double>();
				_RotLeft = json["RotLeft"].GetValue<double>();

				_TopHWidth = json["TopHWidth"].GetValue<double>();
				_TopHHeight = json["TopHHeight"].GetValue<double>();
				_TopHTop = json["TopHTop"].GetValue<double>();
				_TopHLeft = json["TopHLeft"].GetValue<double>();

				_RightHWidth = json["RightHWidth"].GetValue<double>();
				_RightHHeight = json["RightHHeight"].GetValue<double>();
				_RightHTop = json["RightHTop"].GetValue<double>();
				_RightHLeft = json["RightHLeft"].GetValue<double>();

				_LeftHWidth = json["LeftHWidth"].GetValue<double>();
				_LeftHHeight = json["LeftHHeight"].GetValue<double>();
				_LeftHTop = json["LeftHTop"].GetValue<double>();
				_LeftHLeft = json["LeftHLeft"].GetValue<double>();


				ret = true;
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
		public string ToJson()
		{
			JsonObject json = new JsonObject();
			json["TopWidth"] = _TopWidth;
			json["TopDepth"] = _TopDepth;
			json["CubeHeight"] = _CubeHeight;
			json["RotFront"] = _RotFront;
			json["RotRear"] = _RotRear;
			json["RotRight"] = _RotRight;
			json["RotLeft"] = _RotLeft;

			json["TopHWidth"] = _TopHWidth;
			json["TopHHeight"] = _TopHHeight;
			json["TopHTop"] = _TopHTop;
			json["TopHLeft"] = _TopHLeft;

			json["RightHWidth"] = _RightHWidth;
			json["RightHHeight"] = _RightHHeight;
			json["RightHTop"] = _RightHTop;
			json["RightHLeft"] = _RightHLeft;

			json["LeftHWidth"] = _LeftHWidth;
			json["LeftHHeight"] = _LeftHHeight;
			json["LeftHTop"] = _LeftHTop;
			json["LeftHLeft"] = _LeftHLeft;

			var options = new JsonSerializerOptions
			{
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
				WriteIndented = true
			};
			return json.ToJsonString(options);
		}
		public bool WriteToFile(string fileName)
		{
			bool ret = false;
			try
			{
				string json = ToJson();
				if (File.Exists(fileName))
				{
					System.IO.File.Delete(fileName);
				}
				System.IO.File.WriteAllText(fileName, json);

				ret = File.Exists(fileName);
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
		public bool ReadFromFile(string fileName)
		{
			bool ret = false;
			try
			{
				if (File.Exists(fileName) == true)
				{
					string json = System.IO.File.ReadAllText(fileName);
					ret = FromJson(json);
				}
			}
			catch
			{
				ret = false;
			}
			return ret;
		}

		public void SetTopSurf(
			float width,
			float height,
			float depth,
			float frontrot,
			float rearrot,
			float rightrot,
			float leftrot)
		{
			_TopDepth = depth;
			_TopWidth = width;
			_CubeHeight = height;
			_RotFront = frontrot;
			_RotRear = rearrot;
			_RotRight = rightrot;
			_RotLeft = leftrot;


		}

		public PointF[] CubeHoleTop(PointF[] f)
		{
			PointF[] ret = new PointF[4];
			if ((_TopHWidth > 0) && (_TopHHeight > 0))
			{
				_TopHEnable = true;
				ret[0] = new PointF(f[0].X + (float)_TopHLeft, f[0].Y + (float)_TopHTop);
				ret[1] = new PointF((float)(ret[0].X + _TopHWidth), ret[0].Y);
				ret[2] = new PointF(ret[1].X, ret[1].Y + (float)_TopHHeight);
				ret[3] = new PointF(ret[0].X, ret[2].Y);
			}
			else
			{
				_TopHEnable = false;
				ret[0] = ret[1] = ret[2] = ret[3] = new PointF(0, 0);
			}
			return ret;
		}
		public PointF[] CubeHoleRight(PointF[] f)
		{
			PointF[] ret = new PointF[4];
			if ((_RightHWidth > 0) && (_RightHHeight > 0))
			{
				_RightHEnable = true;
				float h2 = (float)(1 / Math.Cos(_RotRight * Math.PI / 180));

				ret[0] = new PointF(f[1].X - (float)_RightHTop * h2, f[1].Y + (float)_RightHLeft);
				ret[1] = new PointF(ret[0].X, (float)(ret[0].Y + _RightHWidth));
				ret[2] = new PointF(ret[1].X - (float)_RightHHeight * h2, ret[1].Y);
				ret[3] = new PointF(ret[2].X, ret[0].Y);
			}
			else
			{
				_RightHEnable = false;
				ret[0] = ret[1] = ret[2] = ret[3] = new PointF(0, 0);
			}

			return ret;
		}
		public PointF[] CubeHoleLeft(PointF[] f)
		{
			PointF[] ret = new PointF[4];
			if ((_LeftHWidth > 0) && (_LeftHHeight > 0))
			{
				_LeftHEnable = true;

				float h2 = (float)(1 / Math.Cos(_RotLeft * Math.PI / 180));

				ret[0] = new PointF(f[0].X + (float)_LeftHTop * h2, f[0].Y + (float)_LeftHLeft);
				ret[1] = new PointF(ret[0].X, (float)(ret[0].Y + _LeftHWidth));
				ret[2] = new PointF(ret[1].X + (float)_LeftHHeight * h2, ret[1].Y);
				ret[3] = new PointF(ret[2].X, ret[0].Y);
			}
			else
			{
				_LeftHEnable = false;
				ret[0] = ret[1] = ret[2] = ret[3] = new PointF(0, 0);
			}
			return ret;
		}
		public PointF[] CubeFront()
		{
			PointF[] ret = new PointF[4];
			float cw = (float)_TopWidth / 2;
			float cd = (float)_TopDepth / 2;
			float h2 = (float)(_CubeHeight / Math.Cos(_RotFront * Math.PI / 180));
			float wl = (float)(h2 * Math.Tan(_RotLeft * Math.PI / 180));
			float wr = (float)(h2 * Math.Tan(_RotRight * Math.PI / 180));

			ret[0] = new PointF(-cw, cd);
			ret[1] = new PointF(cw, cd);
			ret[2] = new PointF(cw + wl, cd + h2);
			ret[3] = new PointF(-cw - wr, cd + h2);
			return ret;
		}
		public PointF[] CubeRight()
		{
			PointF[] ret = new PointF[4];
			float cw = (float)_TopWidth / 2;
			float cd = (float)_TopDepth / 2;
			float h2 = (float)(_CubeHeight / Math.Cos(_RotRight * Math.PI / 180));
			float wl = (float)(h2 * Math.Tan(_RotRear * Math.PI / 180));
			float wr = (float)(h2 * Math.Tan(_RotFront * Math.PI / 180));

			ret[0] = new PointF(-cw - h2, -cd - wl);
			ret[1] = new PointF(-cw, -cd);
			ret[2] = new PointF(-cw, cd);
			ret[3] = new PointF(-cw - h2, cd + wr);
			return ret;
		}
		public PointF[] CubeLeft()
		{
			PointF[] ret = new PointF[4];
			float cw = (float)_TopWidth / 2;
			float cd = (float)_TopDepth / 2;
			float h2 = (float)(_CubeHeight / Math.Cos(_RotLeft * Math.PI / 180));
			float wl = (float)(h2 * Math.Tan(_RotRear * Math.PI / 180));
			float wr = (float)(h2 * Math.Tan(_RotFront * Math.PI / 180));

			ret[0] = new PointF(cw, -cd);
			ret[1] = new PointF(cw + h2, -cd - wl);
			ret[2] = new PointF(cw + h2, cd + wr);
			ret[3] = new PointF(cw, cd);
			return ret;
		}
		public PointF[] CubeRear()
		{
			PointF[] ret = new PointF[4];
			float cw = (float)_TopWidth / 2;
			float cd = (float)_TopDepth / 2;
			float h2 = (float)(_CubeHeight / Math.Cos(_RotRear * Math.PI / 180));
			float wl = (float)(h2 * Math.Tan(_RotLeft * Math.PI / 180));
			float wr = (float)(h2 * Math.Tan(_RotRight * Math.PI / 180));

			ret[0] = new PointF(-cw - wr, -cd - h2);
			ret[1] = new PointF(cw + wl, -cd - h2);
			ret[2] = new PointF(cw, -cd);
			ret[3] = new PointF(-cw, -cd);
			return ret;
		}
		public PointF[] CubeBottom()
		{
			PointF[] ret = new PointF[4];
			PointF[] f = CubeFront();
			PointF[] r = CubeRight();

			ret[0] = new PointF(f[3].X, f[3].Y);
			ret[1] = new PointF(f[2].X, f[2].Y);
			float ll = r[3].Y - r[0].Y;
			ret[2] = new PointF(f[2].X, f[2].Y + ll);
			ret[3] = new PointF(f[3].X, f[3].Y + ll);
			return ret;
		}
		public PointF[] CubeBottom(PointF[] f, PointF[] r)
		{
			PointF[] ret = new PointF[4];

			ret[0] = new PointF(f[3].X, f[3].Y);
			ret[1] = new PointF(f[2].X, f[2].Y);
			float ll = r[3].Y - r[0].Y;
			ret[2] = new PointF(f[2].X, f[2].Y + ll);
			ret[3] = new PointF(f[3].X, f[3].Y + ll);
			return ret;
		}
	}
}
