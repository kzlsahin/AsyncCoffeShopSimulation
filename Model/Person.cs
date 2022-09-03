using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam2_MustafaSenturk.Model
{
    public class Person : IAsset
    {
        public string Name { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public Direction direction { get; set; } = Direction.Down;
        public double PaceLength { get; set; } = 2;
        public Image Image { get; set; }
        public ISpace CurrentSpace { get; set; } = Space.EmptySpace;
        public string Speech { get; set; } = string.Empty;

        public bool Visible { get; set; } = true;
        public bool IsOnWay { get; set; } = false;

        protected DialogBuble? dialogBubble;

        public Person(string name, Image image)
        {
            this.Name = name;
            this.Image = image;
        }
        public virtual bool SpawnAt(ISpace space)
        {
            this.PosX = space.PosX;
            this.PosY = space.PosY;
            return true;
        }
        public void AddDialogBuble(Image? image = null)
        {
            if (image != null)
            {
                dialogBubble = new DialogBuble(image, this, this.Image.Width, -this.Image.Height);
            }
            else
            {
                dialogBubble = new DialogBuble(this, this.Image.Width, -this.Image.Height);
            }
        }
        public void Say(string speech)
        {
            Speech += "\n" + speech;
            dialogBubble.Text = speech;
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

        public void Move(float destX, float destY)
        {
            PointF vector = Directions.GetDirection(PosX, PosY, destX, destY);
            this.PosX += (float)(vector.X * this.PaceLength);
            this.PosY += (float)(vector.Y * this.PaceLength);
        }

        public void Render(Graphics gfx)
        {
            if (this.Visible)
            {
            gfx.DrawImage(Image, new RectangleF(PosX, PosY, Image.Width, Image.Height));
            dialogBubble?.Render(gfx);
            }
        }
    }

}
