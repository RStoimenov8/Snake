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
    public class CFood
    {
        protected const int _size = 4;
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

        public CFood(int __x, int __y)
        {
            Location = new Point(__x, __y);
        }

        public void Draw(Graphics __g)
        {
            Brush b = Brushes.DarkSlateGray;
            __g.FillRectangle(b, DrawRect);
        }
    }
}
