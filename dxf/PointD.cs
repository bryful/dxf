using System;
using netDxf;

namespace dxf
{
    public struct PointD
    {
        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;
		public double R { get; set; } = 0;

		public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
		public PointD(double x, double y,double r)
		{
			X = x;
			Y = y;
            R = r;
		}

		// 加算演算子のオーバーロード
		public static PointD operator +(PointD a, PointD b)
        {
            return new PointD(a.X + b.X, a.Y + b.Y);
        }

        // 減算演算子のオーバーロード
        public static PointD operator -(PointD a, PointD b)
        {
            return new PointD(a.X - b.X, a.Y - b.Y);
        }

        // 乗算演算子のオーバーロード(スカラー倍)
        public static PointD operator *(PointD p, double scalar)
        {
            return new PointD(p.X * scalar, p.Y * scalar);
        }

        public static PointD operator *(double scalar, PointD p)
        {
            return new PointD(p.X * scalar, p.Y * scalar);
        }

		// 除算演算子のオーバーロード
		public static PointD operator /(PointD p, double scalar)
        {
            return new PointD(p.X / scalar, p.Y / scalar);
        }

        // 等価演算子のオーバーロード
        public static bool operator ==(PointD a, PointD b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(PointD a, PointD b)
        {
            return !(a == b);
        }

        // PointF から PointD への暗黙的変換
        public static implicit operator PointD(System.Drawing.PointF pf)
        {
            return new PointD(pf.X, pf.Y);
        }

        // PointD から PointF への明示的変換
        public static explicit operator System.Drawing.PointF(PointD pd)
        {
            return new System.Drawing.PointF((float)pd.X, (float)pd.Y);
        }

        // Equals と GetHashCode のオーバーライド(==, != をオーバーロードする場合は必須)
        public override bool Equals(object? obj)
        {
            if (obj is PointD other)
            {
                return this == other;
            }
            return false;
        }
		private const double EPSILON = 1e-10;
		public bool equals(PointD other, double epsilon = EPSILON)
		{
			return Math.Abs(X - other.X) < epsilon &&
				   Math.Abs(Y - other.Y) < epsilon;
		}
		public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

		public string toString()
		{
			return $"({X}, {Y})";
		}
		public System.Drawing.PointF toPointF()
        {
            return new System.Drawing.PointF((float)X, (float)Y);
		}
		public Vector2 toVector2()
		{
			// dxfの座標系に合わせてY軸を反転
			return new Vector2((float)X, (float)-Y);
		}
		public double distanceTo(PointD other)
		{
			double dx = X - other.X;
			double dy = Y - other.Y;
			return Math.Sqrt(dx * dx + dy * dy);
		}
	}
}