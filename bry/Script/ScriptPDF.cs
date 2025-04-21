using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Drawing;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;


namespace bry
{
	public class ScriptPDF
	{
		private PdfDocument doc=null;
		private PdfPage page =null;
		private XGraphics gfx = null;
		private double Widthmm = 210;
		private double Heightmm = 297;
		[BryScript]
		public PdfDocument Document
		{
			get { return doc; }
		}
		[BryScript]
		public PdfPage Page
		{
			get { return page; }
		}
		[BryScript]
		public XGraphics Gfx
		{
			get { return gfx; }
		}

		[BryScript]
		public SizeF Size
		{
			get{ return new SizeF((float)page.Width.Millimeter, (float)page.Height.Millimeter); }
			set 
			{
				page.Width = XUnit.FromMillimeter(value.Width);
				page.Height = XUnit.FromMillimeter(value.Height);
				Widthmm = value.Width;
				Heightmm = value.Height;
				double cx = page.Width.Millimeter;
				double cy = page.Height.Millimeter;
				gfx.TranslateTransform(cx, cy);
			}
		}
		public ScriptPDF()
		{
			Init();
		}
		public void Init()
		{
			Init(PageSize.A4);
		}
		public void Init(PageSize sz)
		{
			doc = new PdfDocument();
			page = doc.AddPage();
			page.Size = sz;
			page.Orientation = PageOrientation.Portrait;
			Widthmm = page.Width.Millimeter;
			Heightmm = page.Height.Millimeter;
			double cx = page.Width.Point / 2;
			double cy = page.Height.Point / 2;
			gfx = XGraphics.FromPdfPage(page);
			gfx.TranslateTransform(cx, cy);
		}
		public void Init(double w,double h)
		{
			doc = new PdfDocument();
			page = doc.AddPage();
			page.Width = XUnit.FromMillimeter(w);
			page.Height = XUnit.FromMillimeter(h);
			Widthmm = page.Width.Millimeter;
			Heightmm = page.Height.Millimeter;
			page.Orientation = PageOrientation.Portrait;
			double cx = page.Width.Millimeter;
			double cy = page.Height.Millimeter;
			gfx = XGraphics.FromPdfPage(page);
			gfx.TranslateTransform(cx, cy);
		}
		[BryScript]
		public bool Save(string fn)
		{
			bool ret = false;
			try
			{
				if (File.Exists(fn))
				{
					File.Delete(fn);
				}
				doc.Save(fn);
				ret = File.Exists(fn);
			}
			catch
			{
				ret = false;
			}
			return ret;
		}
		[BryScript]
		public int RGBInt(int r,int g, int b)
		{
			return Color.FromArgb(r, g, b).ToArgb();
		}
		[BryScript]
		public void DrawLine(XPen p , double x0, double y0, double x1, double y1)
		{
			gfx.DrawLine(p, x0, y0, x1, y1);
		}
		public XPoint[] ToXPoints(object ob)
		{
			ScriptObject soo = (ScriptObject)ob;
			List<XPoint> a = new List<XPoint>();
			if (ob.GetType().ToString().IndexOf("Array") >= 0)
			{
				foreach (int idx in soo.PropertyIndices)
				{
					object w = soo.GetProperty(idx);
					if (w.GetType().ToString().IndexOf("Array") >= 0)
					{
						ScriptObject soo2 = (ScriptObject)w;

						List<double> b = new List<double>();
						foreach (int idx2 in soo2.PropertyIndices)
						{
							object w2 = soo2.GetProperty(idx2);
							if ((w2 is double)|| (w2 is float) )
							{
								b.Add((double)w2);
							}else if (w2 is int)
							{
								int g = (int)w2;
								b.Add((double)g);
							}

						}
						if (b.Count>=2)
						{
							XPoint pp = new XPoint(
								XUnit.FromMillimeter(b[0]),
								XUnit.FromMillimeter(b[1]));
							a.Add(pp);
						}
					}

				}
			}
			return a.ToArray();
		}
		[BryScript]
		public void DrawLines(XPen p, object ob)
		{
			
			XPoint[] a = ToXPoints(ob);
			gfx.DrawLines(p, a);
		}
		[BryScript]
		public void DrawPolygon(XPen p, object ob)
		{

			XPoint[] a = ToXPoints(ob);
			gfx.DrawPolygon(p, a);
		}
		[BryScript]
		public void FillPolygon(XPen p, XBrush b, object ob)
		{

			XPoint[] a = ToXPoints(ob);
			gfx.DrawPolygon(p, a);

		}
		[BryScript]
		public void DrawEllipse(XPen p, double x, double y, double w, double h)
		{
			double ww = XUnit.FromMillimeter(w);
			double hh = XUnit.FromMillimeter(h);
			XPoint cc = new XPoint(
				XUnit.FromMillimeter(x) - ww / 2,
				XUnit.FromMillimeter(y) - hh / 2
				);
			
			gfx.DrawEllipse(p, cc.X,cc.Y,ww,hh);
		}
		[BryScript]
		public void DrawRectangle(XPen p, double x, double y, double w, double h)
		{
			double ww = XUnit.FromMillimeter(w);
			double hh = XUnit.FromMillimeter(h);
			XPoint cc = new XPoint(
				XUnit.FromMillimeter(x) - ww / 2,
				XUnit.FromMillimeter(y) - hh / 2
				);

			gfx.DrawRectangle(p, cc.X, cc.Y, ww, hh);
		}
	}
}
