using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam2_MustafaSenturk.Model
{
    public class DialogBuble : IAsset, ITextHolder
    {
        public float PosX { get; set; }
        public float PosY { get; set; }

        public Point Offset = new(0, 0);
        public Direction direction { get; set; }
        public double PaceLength { get; set; } = 0;
        public Image Image { get; set; } = Properties.PublishProfiles.Resources.DialogBuble_x128;

        public ISpace CurrentSpace { get; set; } = Space.EmptySpace;

        IAsset? BondedAsset = null;
        public string Text { get; set; } = "   initted";
        public float FontSize { get; set; } = 8f;
        public SolidBrush brush { get; set; } = new(Color.Black);

        StringFormat stringFormat = new StringFormat();
        public bool Visible { get; set; } = true;

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
        public DialogBuble(Image image, int offsetX, int offsetY)
        {
            this.Image = image;
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
            if (BondedAsset != null)
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

            if (this.Visible == false) return;

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

}
