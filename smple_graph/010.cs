Bitmap bm = new Bitmap(1200, 1200);
Graphics g = Graphics.FromImage(bm);
g.Clear(Color.White);
g.SmoothingMode = SmoothingMode.AntiAlias;
g.InterpolationMode = InterpolationMode.Bilinear;
g.PixelOffsetMode = PixelOffsetMode.HighQuality;

//基本の形
// パスで四角形
var w = 2;
var h = 8;
PointF[] dot = new PointF[]{
	new PointF(-w/2, -h/2),
	new PointF(w/2, -h/2),
	new PointF(w/2, h/2),
	new PointF(-w/2, h/2),
};

//中心に移動して上に移動
dot = MoveAry(dot, 600, 600-400);


//回転角度2度で複製描画
var rot = 2;
var rotCount = 360 / rot;
Pen p = new Pen(Color.Black, 0.5f);
SolidBrush b = new SolidBrush(Color.Red);

for (var i = 0; i < rotCount; i++)
{
	var angle = rot * i;
	var pa = RotAry(dot, new PointF(600, 600), (float)angle);
	g.FillPolygon(b, pa);
	g.DrawPolygon(p, pa);
}

bm.Save("bitmap.png", ImageFormat.Png);