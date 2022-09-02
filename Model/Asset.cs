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
        public float PosX { get; set; }
        public float PosY { get; set; }
        public Direction direction { get; set; } = Direction.Down;
        public double PaceLength { get; set; } = 2;
        public Image Image { get; set; }

        public string Speech { get; set; } = string.Empty;

        private DialogBuble dialogBubble;

        public Person(string name, Image image)
        {
            this.Name = name;
            this.Image = image;
        }

        public void AddDialogBuble()
        {
            dialogBubble = new DialogBuble(this, 45, -128);
        }
        protected void Say(string speech)
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
            gfx.DrawImage(Image, new RectangleF(PosX, PosY, Image.Width, Image.Height));
            dialogBubble?.Render(gfx);
        }
    }

    public class DialogBuble : IAsset, ITextHolder
    {
        public float PosX { get; set; }
        public float PosY { get; set; }

        public Point Offset = new(0, 0);
        public Direction direction { get; set; }
        public double PaceLength { get; set; } = 0;
        public Image Image { get; set; } = Properties.Resources.DialogBuble_x128;

        IAsset? BondedAsset = null;
        public string Text { get; set; } = "   initted";
        public float FontSize { get; set; } = 10f;
        public SolidBrush brush { get; set; } = new(Color.Black);
        
        StringFormat stringFormat = new StringFormat();

        public DialogBuble()
        {
            SetStringFormat();
        }
        public DialogBuble(int offsetX, int offsetY)
        {
            SetStringFormat();
            Offset.X = offsetX;
            Offset.Y = offsetY;
        }
        public DialogBuble(Image image, IAsset asset, int offsetX, int offsetY)
        {
            SetStringFormat();
            this.Image = image;
            BondedAsset = asset;
            Offset.X = offsetX;
            Offset.Y = offsetY;
        }

        private void SetStringFormat()
        {
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
        }
        
        public DialogBuble(IAsset asset, int offsetX, int offsetY)
        {
            BondedAsset = asset;
            Offset.X = offsetX;
            Offset.Y = offsetY;
        }
        public void BondAsset(IAsset asset, int offsetX, int offsetY)
        {
            BondedAsset = asset;
            Offset.X = offsetX;
            Offset.Y = offsetY;
        }

        public void BondAsset(Image image, int offsetX, int offsetY)
        {
            this.Image = image;
            Offset.X = offsetX;
            Offset.Y = offsetY;
        }

        public void ChangeOffsets(int offsetX, int offsetY)
        {
            Offset.X = offsetX;
            Offset.Y = offsetY;
        }

        public void UpdatePosition()
        {
            if(BondedAsset != null)
            {
                this.PosX = BondedAsset.PosX + Offset.X;
                this.PosY = BondedAsset.PosY + Offset.Y;
            }
            else
            {
                this.PosX = Offset.X;
                this.PosY = Offset.Y;
            }
        }

        public void Move(float destX, float destY)
        {
            UpdatePosition();
        }
        public void Render(Graphics? gfx)
        {
            UpdatePosition();
            gfx?.DrawImage(Image, new Point((int)this.PosX, (int)this.PosY));
            gfx?.DrawString(this.Text, new Font("sagoe ui", FontSize), brush, new RectangleF(new Point((int)PosX, (int)PosY), Image.Size), stringFormat);
        }
        public void TurnRight()
        {
            return;   
        }
        public void TurnLeft()
        {
            return;
        }
    }

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
}
