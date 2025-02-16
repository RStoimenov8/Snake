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
    public partial class Form1 : Form
    {
        private int _score = 0;
        private bool _fail = false;
        private string _failMessage = string.Empty;

        #region Properties
        private CSnake _snake;
        public CSnake Snake
        {
            get 
            {
                if (_snake == null)
                    _snake = new CSnake();
                return _snake; 
            }
        }

        private Random _rnd;
        protected Random rnd
        {
            get
            {
                if (_rnd == null)
                    _rnd = new Random(DateTime.Now.Millisecond);
                return _rnd;
            }
        }

        private CFood _food = null;
        public CFood Food
        {
            get { return _food; }
            set { _food = value; }
        }
        #endregion

        public Form1()
        {
            InitializeComponent();

            addFood();
        }

        #region Helper Methods
        protected void addFood()
        {
            int x = 0, y = 0;
            int buffer = CSnakePart.Size * 4;
            bool conflicted = true;
            while (conflicted)
            {
                x = rnd.Next(this.Width - buffer - 25);
                if (x == 0)
                    x += buffer + 10;
                y = rnd.Next(this.Height - buffer - 25);
                if (y == 0)
                    y += buffer + 10;

                // check for conflict with snake
                CFood f = new CFood(x, y);
                conflicted = Snake.HitSelf(f.DrawRect);
            }
            _food = new CFood(x, y);
        }
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!this.DesignMode)
            {
                Graphics g = e.Graphics;
                Snake.Draw(g);

                if (_food != null)
                    _food.Draw(g);

                // fail message
                if (_fail)
                {
                    SolidBrush br = new SolidBrush(Color.DarkRed);

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    Rectangle centered = e.ClipRectangle;

                    Font font = this.Font;
                    font = new Font(font.Name, 18.0f,
                        font.Style, font.Unit,
                        font.GdiCharSet, font.GdiVerticalFont);

                    centered.Offset(0, (int)(e.ClipRectangle.Height - e.Graphics.MeasureString(_failMessage, font).Height) / 2);

                    g.DrawString(_failMessage, font, br, centered, sf);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_fail && !_gameTimer.Enabled)
                _gameTimer.Start();

            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (Snake.Direction != CSnake.EDirection.Right)
                        Snake.Direction = CSnake.EDirection.Left;
                    break;
                case Keys.Right:
                    if (Snake.Direction != CSnake.EDirection.Left)
                        Snake.Direction = CSnake.EDirection.Right;
                    break;
                case Keys.Up:
                    if (Snake.Direction != CSnake.EDirection.Down)
                        Snake.Direction = CSnake.EDirection.Up;
                    break;
                case Keys.Down:
                    if (Snake.Direction != CSnake.EDirection.Up)
                        Snake.Direction = CSnake.EDirection.Down;
                    break;
            }
        }

        private void _gameTimer_Tick(object sender, EventArgs e)
        {
            // The game loop
            Snake.Update();

            if ((_food != null) && (Snake.CanEatFood(_food)))
            {
                Snake.Add(5);
                addFood();
                _score+=1;
                _lbl.Text = string.Format("Резултат: {0}", _score);
            }
            else
            {
                _lbl.Text = string.Format("Резултат: {0}", _score);

                if (Snake.HitSelf())
                {
                    _failMessage = "Загуба! Ти се удари в себе си.";
                    _fail = true;
                    _gameTimer.Stop();
                }
                if (Snake.HitWall(this.Width, this.Height))
                {
                    _failMessage = "Загуба!Ти се удари в стена.";
                    _fail = true;
                    _gameTimer.Stop();
                }
            }
            this.Refresh();
        }
    }
}