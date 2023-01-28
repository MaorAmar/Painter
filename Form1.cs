using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using FIGURES;
 
namespace my1st
{
    [Serializable()]
    public partial class Form1 : Form
    {
        public Form1()
        {
           InitializeComponent();
           loadPictureBox1();
        }
        ColorDialog colorpickerBackgroundcolor = new ColorDialog();
        ColorDialog colorPicker =new ColorDialog();
        FigureList pts = new FigureList();
        Bitmap paintImage;
        Graphics paintGraphics;
        Point lastPoint;
        bool isMouseDown = false;
        Image openedFile;
        int curIndex=-1;
        int futureKind = -1; 
        int click=0;
        int counter=-1;

        private void loadPictureBox1()
        {
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            paintImage = new Bitmap(width, height);
            paintGraphics = Graphics.FromImage(paintImage);
            pictureBox1.Image = paintImage;
        }
        private void DrawLineInCanvas(Point currentPoint)
        {
            Pen pen = new Pen(colorPicker.Color, trackBar1.Value);
            if (curIndex <0 )
            {
                paintGraphics.DrawLine(pen, lastPoint, currentPoint);   
            }
            lastPoint = currentPoint;
            pictureBox1.Refresh();
        }
        private void DrawEraser(Point currentPoint)
        {
            Pen penEraser = new Pen(pictureBox1.BackColor, 10);
            paintGraphics.DrawLine(penEraser, lastPoint, currentPoint);
            lastPoint = currentPoint;
            pictureBox1.Refresh();
        }
        static Point set_point(PictureBox pb,Point pt)
        {
            float px = 1f * pb.Image.Width / pb.Width;
            float py = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * px), (int)(pt.Y * py));
        }
        private void validate(Bitmap bm,Stack<Point>sp,int x,int y,Color oldColor,Color newColor)
        {
            Color cX = bm.GetPixel(x, y);
            if (cX.Name == "ff000000")
                cX = Color.Black;
            if (cX == oldColor)
            {
                 sp.Push(new Point(x, y));
                 bm.SetPixel(x, y, newColor);   
            }
            return;
        }
        public void Fill(Bitmap bm, int x, int y)
        {
            Color newColor = colorPicker.Color; ;
            Color oldColor = bm.GetPixel(x, y);
            if (oldColor.Name == "ff000000")
                oldColor = Color.Black;
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, newColor);
            if (oldColor.Name != newColor.Name)
            {
                   while (pixel.Count > 0)
                   {
                       Point pt = (Point)pixel.Pop();
                       if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                       {
                           validate(bm, pixel, pt.X - 1, pt.Y, oldColor, newColor);
                           validate(bm, pixel, pt.X, pt.Y - 1, oldColor, newColor);
                           validate(bm, pixel, pt.X + 1, pt.Y, oldColor, newColor);
                           validate(bm, pixel, pt.X, pt.Y + 1, oldColor, newColor);
                       }
                   }
            }
            else
                return;
            pictureBox1.Refresh(); 
         }
        private void Fill_MouseClick(object sender, MouseEventArgs e)
        {
                if (isMouseDown && click == 3)
                {
                    Point point = set_point(pictureBox1, e.Location);
                    Fill(paintImage, point.X, point.Y);
                }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
            isMouseDown = true;
            for (int i = 0; i < pts.NextIndex; i++ )
            {                           
                if ( pts[i].isInside(e.X,e.Y))
                {
                    curIndex = i;
                    string s = e.Button.ToString();
                    if (s == "Right") 
                    {
                        pts.Remove(curIndex);
                        curIndex = -1;
                        pictureBox1.Invalidate();
                        return;
                    }
                    break;
                }
            }
            if (curIndex < 0 && click==0)
            {
                switch (futureKind)
                {
                    case 0:
                       pts[pts.NextIndex] = new myCircle(e.X, e.Y, 5);
                        break;
                    case 1:
                        pts[pts.NextIndex] = new Ellipse(e.X, e.Y, 5);
                        break;
                    case 2:
                        pts[pts.NextIndex] = new myRectangle(e.X, e.Y, 20, 10);
                        break;
                    case 3:
                        pts[pts.NextIndex] = new Square (e.X, e.Y, 15);
                        break;
                    case 4:
                        pts[pts.NextIndex] = new myParallelogram(e.X, e.Y,10,10);
                        break;
                }
                curIndex = pts.NextIndex - 1;
                pictureBox1.Invalidate();
                counter++;
            }          
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown && click ==1)
            {
                  DrawLineInCanvas(e.Location);
            }
            if (isMouseDown && click == 2)
            { 
                DrawEraser(e.Location);
            }
            if (curIndex >= 0)
            {
                Figure c = (Figure)pts[curIndex];
                c.X = e.X;
                c.Y = e.Y;
                pictureBox1.Invalidate();
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            curIndex = -1;
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        { 
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            pts.DrawAll(g); 
        }
        //Parallelogram
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            click = 0;
            futureKind = 4;
        }
        //Square
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            click = 0;
            futureKind = 3;
        }
        //Rectangle
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            click = 0;
            futureKind = 2;
        }
        //Ellipse
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            click = 0;
            futureKind = 1;
        }
        //Circle
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            click = 0;
            futureKind = 0;
        }
        //Save
        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();// + "..\\myModels";
            saveFileDialog1.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, pts);
                    MessageBox.Show ("Successfully Saved!");
                }
            }
        }
        //Open
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();// + "..\\myModels";
            openFileDialog1.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = File.Open(openFileDialog1.FileName, FileMode.Open);
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                pts = (FigureList)binaryFormatter.Deserialize(stream);
                pictureBox1.Image= openedFile;
            }
        }
        private void Backgroundcolor(object sender, EventArgs e)
        {
            if (colorpickerBackgroundcolor.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = colorpickerBackgroundcolor.Color;
            }
        }
        private void ColorPickerButton_Click(object sender, EventArgs e)
        {
            colorPicker.ShowDialog();
        }

        private void Pen_Click(object sender, EventArgs e)
        {
            click = 1;
            futureKind = 0;
        }
        //Eraser
        private void button6_Click(object sender, EventArgs e)
        {
            click = 2;
        }
        //new page
        private void button7_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i <= counter; i++)
            {
                pts.Remove(0);
              
            }
            loadPictureBox1();

        }
        //Fill
        private void button8_Click(object sender, EventArgs e)
        {
            click = 3;
        }
    }
}