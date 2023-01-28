using System.Drawing;

namespace FIGURES
{
	using System;
	using System.Collections;
    using System.Windows.Forms;


    [Serializable] 
   
    public abstract class Figure
    {
        float x;  
        float y;
      
      
        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public abstract void Draw(Graphics g);
        public abstract bool isInside(int xP, int yP);        
    }

    [Serializable] 

    public class myCircle: Figure 
	{
        float radius;  //private
		public myCircle() 
			: this(10,10,10) //!!!!
        {}
		
		public myCircle(float xVal,float yVal,float rVal) 
		{
            X=xVal;
            Y=yVal;
            radius =rVal;
		}
	    

		public float Radius 
		{
			get 
			{
				return radius;
			}
			set 
			{
				radius = value;
			}
		}

        public override void Draw(Graphics g)
        {
            SolidBrush br = new SolidBrush(Color.White);
            Pen pen = new Pen(Color.Black, 1);
            g.FillEllipse(br, X - radius, Y - radius, 2 * radius, 2 * radius);
            g.DrawEllipse(pen, X - radius, Y - radius, 2 * radius, 2 * radius);
        }
        public override bool isInside(int xP, int yP)
        {
            return Math.Sqrt((xP - X) * (xP - X) + (yP - Y) * (yP - Y)) < radius;
        }

        ~myCircle() {}
	   
	}

    [Serializable] 

    public class Ellipse : myCircle
    {
        public Ellipse()
            : this(10, 10, 5) //!!!!
        { }

        public Ellipse(float xVal, float yVal, float rVal)
        {
            X = xVal;
            Y = yVal;
            Radius = rVal;
        }
        public void DrawOuter(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 1);
           
            g.DrawEllipse(pen, X - Radius, Y - Radius+5, 2 * Radius + 10, 2 * Radius-2);

        }
        public override void Draw(Graphics g)
        {
            SolidBrush br = new SolidBrush(Color.White);
            Pen pen = new Pen(Color.Black,1);
            g.FillEllipse(br, X - Radius-5, Y - Radius, 2 * Radius + 20, 2 * Radius+7);
            g.DrawEllipse(pen, X - Radius-5, Y - Radius, 2 * Radius + 20, 2 * Radius+7);
            DrawOuter(g);
        }
       
        public override bool isInside(int xP, int yP)
        {
            return Math.Sqrt((xP - X) * (xP - X) + (yP - Y) * (yP - Y)) < Radius;
        }

        ~Ellipse() { }
    }

    [Serializable]  

    public class myRectangle: Figure
    {
        float width;
        float height;
        public myRectangle()
            : this(10, 10, 10,20) //!!!!!! 
        { } 

        public myRectangle(float xVal, float yVal, float wVal, float hVal)
        {
            X = xVal;
            Y = yVal;
            width = wVal;
            height=hVal;
        }

        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
      
        public override void Draw(Graphics g)
        {
           
            SolidBrush br = new SolidBrush(Color.White);
            Pen pen = new Pen(Color.Black, 1);
            g.FillRectangle(br, X - width/2, Y - height/2, width, height);
            g.DrawRectangle(pen, X - width/2, Y - height/2, width, height);
        }
        public override bool isInside(int xP, int yP)
        {
            return Math.Abs(xP - X)<=width/2 && Math.Abs(yP - Y) <= height / 2;
        }
        ~myRectangle() { }
    }

    [Serializable]
    
    public class Square : myRectangle
    {
        public Square()
            : this(10, 10, 10) 
        {}

        public Square(float xVal, float yVal, float hVal)
        {
            X = xVal;
            Y = yVal;
            Height = hVal;
        }
        public void DrawlineSquare(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 1);
            g.DrawLine(pen, X - Height / 2, Y - Height / 2, X + Height / 2, Y + Height / 2);
            g.DrawLine(pen, X + Height / 2, Y - Height / 2, X - Height / 2, Y + Height / 2);
        }
        public override void Draw(Graphics g)
        {
            SolidBrush br = new SolidBrush(Color.White);
            Pen pen = new Pen(Color.Black, 1);
            g.FillRectangle(br, X - Height / 2, Y - Height / 2, Height, Height);
            g.DrawRectangle(pen, X - Height / 2, Y - Height / 2, Height, Height);
            DrawlineSquare(g);
        }
        public override bool isInside(int xP, int yP)
        {
            return Math.Abs(xP - X) <= Width / 2 && Math.Abs(yP - Y) <= Height / 2;
        }

        ~Square() { }
    }

    [Serializable]

    public class myParallelogram : myRectangle
    {
  
        public myParallelogram()
            : this(10, 20,10,20) //!!!!!! 
        { }

        public myParallelogram(float xVal, float yVal, float wVal, float hVal)
        {
            X = xVal;
            Y = yVal;
            Height = hVal;
            Width = wVal;
        }
        public void DrawlineParallelogram(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 1);
            g.DrawLine(pen, X - Width + 5, Y - Height / 2, X + Width - 5, Y + Height / 2);
            g.DrawLine(pen, X - Width - 5, Y + Height / 2, X + Width + 5, Y - Height / 2);
        }
        public override void Draw(Graphics g)
        {
            Point p1 = new Point((int)X - (int)Width + 5 , (int)Y - (int)Height /2);
            Point p2 = new Point((int)X - (int)Width - 5, (int)Y + (int)Height / 2);
            Point p3 = new Point((int)X + (int)Width - 5, (int)Y + (int)Height / 2);
            Point p4 = new Point((int)X + (int)Width + 5, (int)Y - (int)Height / 2);

            Point[] pointArray = { p1, p2, p3, p4 };
            SolidBrush br = new SolidBrush(Color.White);
            Pen pen = new Pen(Color.Black, 1);
            g.FillPolygon(br, pointArray);
            g.DrawPolygon(pen, pointArray);
            DrawlineParallelogram(g);
        }
        public override bool isInside(int xP, int yP)
        {
            return Math.Abs(xP - X) <= Width / 2 && Math.Abs(yP - Y) <= Height / 2;
        }

        ~myParallelogram() { }
    }

    [Serializable] 
   
    public class FigureList 
	{
		protected SortedList figures;
	
		public FigureList() 
		{
			figures = new SortedList();
		}
		public int NextIndex 
		{
			get 
			{
				return figures.Count;
			}
		}
		public Figure this[int index] 
		{
			get 
			{
				if (index >= figures.Count)
                    return (Figure)null;
				                      
                return (Figure)figures.GetByIndex(index);
			}
			set 
			{
				if ( index <= figures.Count )
					figures[index] = value; //!!!		
			}
		}

        public void Remove(int element) 
		{
            if (element >= 0 && element < figures.Count)
            {
                for (int i = element; i < figures.Count - 1; i++)
                    figures[i] = figures[i + 1];
                figures.RemoveAt(figures.Count - 1);
            }
		}
        public void DrawAll(Graphics g)
        {
            Figure prev, cur;
            for (int i = 1; i < figures.Count; i++)
            {
                prev = (Figure)figures[i-1];
                cur = (Figure)figures[i];
                g.DrawLine( Pens.Black, prev.X,prev.Y, cur.X, cur.Y );

                ((Figure)figures[i]).Draw(g);
            }
            for (int i = 0; i < figures.Count; i++)
                ((Figure)figures[i]).Draw(g);
        }
    }
}





