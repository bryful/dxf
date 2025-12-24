/*
 * 円弧
 * 
 */
DXF dxf = new DXF();

PointF cp = new PointF(0, 0);
dxf.DrawSemiCircle(cp, 100f, 0,270f);
dxf.DrawEllipse(cp, 80f);

var fn = "Ellipse.dxf";
if (dxf.Save(fn)==true)
{
	Console.WriteLine(fn +" created");
}
else
{
	Console.WriteLine(fn + " not created");
}