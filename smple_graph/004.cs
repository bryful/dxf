PDF pdf = new PDF();
pdf.Init(true,216,310);

float w = 48f;
float h = 38f;
float d = 27f;

PointF [] outline = new PointF[4];
outline[0] = new PointF(-w / 2f, -h / 2f);
outline[1] = new PointF(w / 2f, -h / 2f);
outline[2] = new PointF(w / 2f, h / 2f + d);
outline[3] = new PointF(-w / 2f, h / 2f + d);

XPen p = new XPen(XColors.Black, 0.5f);
pdf.DrawPolygon(p, outline);

PointF[] oriline = new PointF[2];
outline[0] = new PointF(-w / 2f, h / 2f);
outline[1] = new PointF(w / 2f, h / 2f);
p.DashStyle = XDashStyle.Dot;
pdf.DrawLines(p, outline);

RectangleF a = new RectangleF(-17f / 2f, -17f / 2f + d/2f+ h/2f, 17f, 17f);
pdf.DrawEllipse(p, a);

w = 48.4f;
h = 38.4f;
d = 27.4f;

List<PointF> outline2 = new List<PointF>();
outline2.Add(new PointF(-w / 2f, -h / 2f));
outline2.Add(new PointF(-w / 2f, -h * 3f / 2f - d));
outline2.Add(new PointF(w / 2f, -h * 3f / 2f - d));
outline2.Add(new PointF(w / 2f, -h / 2f));
outline2.Add(new PointF(w / 2f + d, -h / 2f));
outline2.Add(new PointF(w / 2f + d, h / 2f));
outline2.Add(new PointF(w / 2f, h / 2f));
outline2.Add(new PointF(-w / 2f, h / 2f));
outline2.Add(new PointF(-w / 2f-d, h / 2f));
outline2.Add(new PointF(-w / 2f - d, -h / 2f));
p.DashStyle = XDashStyle.Solid;
pdf.DrawPolygon(p, outline2.ToArray());
p.DashStyle = XDashStyle.Dot;
outline2.Clear();
outline2.Add(new PointF(-w / 2f, -h / 2f -d));
outline2.Add(new PointF(w / 2f, -h / 2f - d));
pdf.DrawLines(p, outline2.ToArray());

outline2.Clear();
outline2.Add(new PointF(-w / 2f, h / 2f));
outline2.Add(new PointF(-w / 2f, -h / 2f));
outline2.Add(new PointF(w / 2f, -h / 2f));
outline2.Add(new PointF(w / 2f, h / 2f));
pdf.DrawLines(p, outline2.ToArray());



pdf.Save("head.pdf");
