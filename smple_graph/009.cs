/*
 * DXFを描画保存するサンプル
 * ゼブラ模様
 */
DXF dxf2 = new DXF();
//ゼブラ模様を描く
var w = 50;
var h = 1500;

PointF[] dot = new PointF[]{
	new PointF(-w/2, -h/2),
	new PointF(w/2, -h/2),
	new PointF(w/2, h/2),
	new PointF(-w/2, h/2),
};

var rep = 51;
var repOffset = -25;
var moveX = 100;

dot =RotAry(dot, new PointF(0, 0), 45);

List<PointF[]> pa = new List<PointF[]>();
for (var i = 0; i < rep; i++)
{
	var paa = MoveAry(dot, moveX * (i + repOffset), 0);
	pa.Add(paa);
}

//クリッピング
List<PointF[]> pa2 = ClippingRect(pa,new RectangleF(-500,-500,1000,1000));

dxf2.DrawPolygon(pa2);

var fn = "Zebra.dxf";
if (dxf2.Save(fn)==true)
{
	Console.WriteLine(fn +" created");
}
else
{
	Console.WriteLine(fn + " not created");
}