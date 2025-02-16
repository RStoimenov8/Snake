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
    public class CSnake
    {
        public enum EDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        #region Properties
        private EDirection _direction = EDirection.Right;
        public EDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        private List<CSnakePart> _body = new List<CSnakePart>();
        public List<CSnakePart> Body
        {
            get { return _body; }
        }
        #endregion

        public CSnake()
        {
            Add(5);
        }

        #region Helper Methods
        public void Add(int __section)
        {
            int buffer = CSnakePart.Size * 2;
            for (int i = 0; i < __section; i++)
            {
                if (Body.Count > 0)
                {
                    CSnakePart lastPart = Body[Body.Count - 1];
                    int x = 0;
                    int y = 0;
                    switch (Direction)
                    {
                        case EDirection.Left:
                            x = lastPart.Location.X + buffer;
                            y = lastPart.Location.Y;
                            break;
                        case EDirection.Right:
                            x = lastPart.Location.X - buffer;
                            y = lastPart.Location.Y;
                            break;
                        case EDirection.Up:
                            x = lastPart.Location.X;
                            y = lastPart.Location.Y + buffer;
                            break;
                        case EDirection.Down:
                            x = lastPart.Location.X;
                            y = lastPart.Location.Y - buffer;
                            break;
                        default:
                            break;
                    }
                    Body.Add(new CSnakePart(x, y));
                }
                else
                    Body.Add(new CSnakePart(150, 100));
            }
        }

        public void Update()
        {
            Update(Direction);
        }

        public void Update(EDirection __direction)
        {
            Direction = __direction;
            // everyone else follows the leader
            for (int i = Body.Count - 1; i > 0; i--)
            {
                CSnakePart leader = Body[i - 1];
                Body[i].Update(leader.Location.X, leader.Location.Y);
            }

            // only the head changes direction
            CSnakePart head = Body[0];
            int x = 0, y = 0;
            int buffer = CSnakePart.Size * 2;
            switch (Direction)
            {
                case EDirection.Left:
                    x = head.Location.X - buffer;
                    y = head.Location.Y;
                    break;
                case EDirection.Right:
                    x = head.Location.X + buffer;
                    y = head.Location.Y;
                    break;
                case EDirection.Up:
                    x = head.Location.X;
                    y = head.Location.Y - buffer;
                    break;
                case EDirection.Down:
                    x = head.Location.X;
                    y = head.Location.Y + buffer;
                    break;
                default:
                    break;
            }
            head.Update(x, y);
        }

        public bool CanEatFood(CFood __f)
        {
            CSnakePart head = Body[0];
            return head.DrawRect.IntersectsWith(__f.DrawRect);
        }

        public bool HitSelf()
        {
            CSnakePart head = Body[0];
            bool hit = false;
            for (int i = 1; i < Body.Count; i++)
            {
                hit = head.DrawRect.IntersectsWith(Body[i].DrawRect);
                if (hit)
                    break;
            }
            return hit;
        }

        public bool HitSelf(Rectangle __rect)
        {
            bool hit = false;
            for (int i = 1; i < Body.Count; i++)
            {
                hit = __rect.IntersectsWith(Body[i].DrawRect);
                if (hit)
                    break;
            }
            return hit;
        }

        public bool HitWall(int __maxWidth, int __maxHeight)
        {
            CSnakePart head = Body[0];
            int buffer = CSnakePart.Size;
            if ((head.Location.X <= buffer) || (head.Location.X >= (__maxWidth - buffer)))
                return true;
            if ((head.Location.Y <= buffer) || (head.Location.Y >= (__maxHeight - buffer)))
                return true;
            return false;
        }
        #endregion

        public void Draw(Graphics __g)
        {
            Brush b = Brushes.Black;

            for (int i = 0; i < Body.Count; i++)
            {
                Body[i].Draw(__g);
            }
        }
    }
}
