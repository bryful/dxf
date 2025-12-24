//grクラス　単位はポイント
GR gr = new GR(1000,1000);

PointF[] rct = new PointF[]{
	new PointF(-100,-100),
	new PointF(100,-100),
	new PointF(100,100),
	new PointF(-100,100),
};

Pen p = new Pen(Color.Black, 0.5f);
gr.DrawPolygon(p, rct);

PointF cp = new PointF(0, 0);
gr.DrawEllipse(p, cp,100);
gr.DrawSemiCircle(p, cp, 150, 90, 45);

gr.Save("test.png", GRType.BMP);
gr.Save("test.dxf", GRType.DXF);
gr.Save("test.pdf", GRType.PDF);

