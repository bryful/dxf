/*
 * PDFを描画保存するサンプル
 * 
 */
// pdf作成
DXF dxf = new DXF();

//基本の形
// パスで四角形
var w = 2f;
var h = 8f;
PointF[] dot = new PointF[]{
	new PointF(-w/2f, -h/2f),
	new PointF(w/2f, -h/2f),
	new PointF(w/2f, h/2f),
	new PointF(-w/2f, h/2),
};

//上に200px移動
dot = MoveAry(dot, 0, -200f);


//回転角度2度で複製描画
var rot = 2f;
var rotCount = 360f / rot;

for (var i=0; i < rotCount; i++)
{
	var angle = rot * i;
	var pa = RotAry(dot, new PointF(0, 0), angle);
	dxf.DrawPolygon(pa);
}

var fn = "Rots.dxf";
if (dxf.Save(fn)==true)
{
	Console.WriteLine(fn +" created");
}
else
{
	Console.WriteLine(fn + " not created");
}