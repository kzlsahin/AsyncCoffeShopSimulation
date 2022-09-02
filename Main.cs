using Exam2_MustafaSenturk.Model;
using Exam2_MustafaSenturk.Data;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ExplorerBar;

namespace Exam2_MustafaSenturk
{
    public partial class Main : Form
    {
        public List<IAsset> assets {get; set;}

        Shop coffeeShop;

        System.Windows.Forms.Timer graphicsTimer;

        
        int counter = 0;

        
        public Main()
        {

            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle( ControlStyles.OptimizedDoubleBuffer, true);

            coffeeShop = new Shop(this);

            this.BackgroundImage = Properties.Resources.mekan;
            this.ClientSize = new Size(BackgroundImage.Width, BackgroundImage.Height);

            graphicsTimer = new System.Windows.Forms.Timer();
            graphicsTimer.Interval = 1000/60;
            graphicsTimer.Tick += TimeTicked;
            graphicsTimer.Start();
        }

        public void Init()
        {           
        }
        public void AddAssetToScene(IAsset asset)
        {
            assets.Add(asset);
        }

        private void RenderAssets(Graphics gfx)
        {
            foreach(IAsset asset in assets)
            {
                asset.Render(gfx);
            }
        }


        private void TimeTicked(object sender, EventArgs e)
        {
            counter++;
            coffeeShop.TimeNext();
            Invalidate();
        }
       
        private void Main_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Main_Paint(object sender, PaintEventArgs e)
        {
            RenderAssets(e.Graphics);
        }
    }
}
