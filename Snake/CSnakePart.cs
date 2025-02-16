using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Snake
{
    public class CSnakePart
    {
        protected static int _size = 4;
        public static int Size
        {
            get
            {
                return _size;
            }
        }

        public Rectangle DrawRect = Rectangle.Empty;

        private Point _location = Point.Empty;
        public Point Location
        {
            get { return _location; }
            set
            {
                _location = value;
                // create our rectangle body
                DrawRect = new Rectangle(_location.X - _size, _location.Y - _size, _size * 2, _size * 2);
            }
        }

        public CSnakePart(int __x, int __y)
        {
            Location = new Point(__x, __y);
        }

        public void Update(int __x, int __y)
        {
            Location = new Point(__x, __y);
        }

        public void Draw(Graphics __g)
        {
            Brush b = Brushes.Black;
            __g.FillRectangle(b, DrawRect);
        }
    }
}
