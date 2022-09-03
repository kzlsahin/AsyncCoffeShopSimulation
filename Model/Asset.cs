using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Exam2_MustafaSenturk.Model
{   
    public interface IAsset
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public Direction direction { get; set; }
        public Image Image { get; set; }
        public double PaceLength { get; set; }
        public void Render(Graphics gfx);
        public void TurnRight();
        public void TurnLeft();
        public ISpace CurrentSpace { get; set; }
        public bool Visible { get; set; }
        public void Move(float destX, float destY);
    }

    public interface ITextHolder
    {
        public string Text { get; set; }
        public float FontSize { get; set; }
        public SolidBrush brush { get; set; }
    }
    public enum Direction
    {
        Down,
        Left,
        Top,
        Right,
    }

    public static class Directions
    {
        public static PointF GetDirection(float posX, float posY, float destX,float destY)
        {
            PointF direction = new();
            double diffX = destX - posX;
            double diffY = destY - posY;
            double magnitute = Math.Sqrt( Math.Pow(diffX, 2) + Math.Pow(diffY, 2) );

            direction.X = (float)(diffX / magnitute);
            direction.Y = (float)(diffY / magnitute);
            return direction;
        }
    }

    public class Space : ISpace
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public SpaceStatus Status { get; set; } = SpaceStatus.None;

        public Space(float PosX, float PosY, SpaceStatus status)
        {
            this.PosX = PosX;
            this.PosY = PosY;
            Status = status;
        }

        public static Space EmptySpace
        {
            get { return new Space(0, 0, SpaceStatus.None); }
        }
    }
    public interface ISpace
    {
        public float PosX { get; set; }
        public float PosY { get; set; }
        public SpaceStatus Status { get; set; }
    }

    public enum SpaceStatus
    {
        None,
        Free,
        Reserved,
        Entrence,
    }
}
