using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Exam2_MustafaSenturk.Model
{
    public class Person : IAsset
    {
        public string Name { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public Direction direction { get; set; } = Direction.Down;
        public double PaseLength { get; set; }
        public Image Image { get; set; }

        public string Speech { get; set; } = string.Empty;

        public Person(string name, Image image)
        {
            this.Name = name;
            this.Image = image;
        }

        protected void Say(string speech)
        {
            Speech += speech;
        }

        protected void ClearSpeech()
        {
            Speech = string.Empty;
        }
        public void TurnRight()
        {
            
            this.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            switch (this.direction)
            { 
                case Direction.Down:
                    direction = Direction.Left;
                    break;
                case Direction.Left:
                    direction = Direction.Top;
                    break;
                case Direction.Top:
                    direction = Direction.Right;
                    break;
                case Direction.Right:
                    direction = Direction.Down;
                    break;
            }
        }
        public void TurnLeft()
        {
            this.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            switch (this.direction)
            {
                case Direction.Down:
                    direction = Direction.Right;
                    break;
                case Direction.Right:
                    direction = Direction.Top;
                    break;
                case Direction.Top:
                    direction = Direction.Left;
                    break;
                case Direction.Left:
                    direction = Direction.Down;
                    break;
            }
        }
    }

    public interface IAsset
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public Direction direction { get; set; }
        public double PaseLength { get; set; }
        public Image Image { get; set; }
        public void TurnRight();
        public void TurnLeft();
    }

    public enum Direction
    {
        Down,
        Left,
        Top,
        Right,
    }
}
