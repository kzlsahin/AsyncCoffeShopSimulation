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
        List<IAsset> assets = new List<IAsset>();

        System.Windows.Forms.Timer graphicsTimer;

        public List<Order> OrdersInProgress = new List<Order>();
        public List<int> DeliveredOrderIds = new List<int>();
        public Products Products = new Products();
        public List<ShopWorker> IdleShopWorkers = new List<ShopWorker>();
        public List<CheckoutStation> AvailableStations = new List<CheckoutStation>();
        public List<CheckoutStation> EmptyStations = new List<CheckoutStation>();
        public List<Task> RunningTasks = new();

        public static SpaceEngine spaceEngine = new SpaceEngine();
        int counter = 0;

        
        public Main()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle( ControlStyles.OptimizedDoubleBuffer, true);

            this.BackgroundImage = Properties.Resources.mekan;
            this.ClientSize = new Size(BackgroundImage.Width, BackgroundImage.Height);

            graphicsTimer = new System.Windows.Forms.Timer();
            graphicsTimer.Interval = 100;
            graphicsTimer.Tick += TimeTicked;
            graphicsTimer.Start();
        }

        public void Init()
        {

            ShopWorker worker1 = new ShopWorker("Mali", Image.FromFile(@"images\Person.png"));
            ShopWorker worker2 = new ShopWorker("Turgut", Image.FromFile(@"images\Person.png"));
            ShopWorker worker3 = new ShopWorker("Harun", Image.FromFile(@"images\Person.png"));
            ShopWorker worker4 = new ShopWorker("Kemal", Image.FromFile(@"images\Person.png"));
            ShopWorker worker5 = new ShopWorker("Mustafa", Image.FromFile(@"images\Person.png"));


            List<IAsset> assets = new List<IAsset>
            {
                worker1, worker2, worker3, worker4, worker5
            };

            worker1.PosX = 100;
            worker1.PosY = 260;
            worker2.PosX = 200;
            worker2.PosY = 260;
            worker3.PosX = 240;
            worker3.PosY = 260;
            worker4.PosX = 140;
            worker4.PosY = 140;
            worker5.PosX = 240;
            worker5.PosY = 140;


            CheckoutStation station1 = new();
            CheckoutStation station2 = new();
            CheckoutStation station3 = new();

            spaceEngine.AddSpace(station1, 120, 120);
            spaceEngine.AddSpace(station2, 240, 120);
            spaceEngine.AddSpace(station3, 360, 120);

            worker1.TakeControlOfCheckoutStation(station1);
            worker2.TakeControlOfCheckoutStation(station2);
            worker3.TakeControlOfCheckoutStation(station3);

            IdleShopWorkers.Add(worker4);
            IdleShopWorkers.Add(worker5);

            this.AddImageToScene(worker1);
            this.AddImageToScene(worker2);
            this.AddImageToScene(worker3);
            this.AddImageToScene(worker4);
            this.AddImageToScene(worker5);
        }
        public void AddImageToScene(IAsset asset)
        {
            //PictureBox pictureBox = new PictureBox();
            //pictureBox.BackColor = System.Drawing.Color.Transparent;
            //pictureBox.Image = asset.Image;
            //pictureBox.Location = new System.Drawing.Point(asset.PosX, asset.PosY);
            //pictureBox.Name = "pictureBox" + assets.Count();
            //pictureBox.Size = new System.Drawing.Size(93, 93);
            //pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            //pictureBox.TabIndex = 0;
            //pictureBox.TabStop = false;
            //pictureBox.Parent = panel1;
            //this.panel1.Controls.Add(pictureBox);
            //pictureBoxes.Add(pictureBox);
            //panel1.Invalidate();
            //panel1.Update();
            //panel1.Refresh();
            assets.Add(asset);
        }

        private void RenderBackground(Graphics gfx)
        {
            gfx.DrawImage(BackgroundImage, new RectangleF(0, 0, this.Width, this.Height));
        }
        private void RenderAssets(Graphics gfx)
        {
            foreach(IAsset asset in assets)
            {
                gfx.DrawImage(asset.Image, new RectangleF(asset.PosX, asset.PosY, asset.Image.Width, asset.Image.Height));
            }
        }


        private void TimeTicked(object sender, EventArgs e)
        {
            counter++;
            foreach (IAsset asset in assets)
            {
                asset.PosX += 1;
            }
            Invalidate();
        }
        private async Task AcceptNewClient()
        {
        }

        public void HandleOrder(Order order, ShopWorker worker)
        {
            OrdersInProgress.Add(order);
            if (IdleShopWorkers.Count() > 0)
            {
                RunningTasks.Add(IdleShopWorkers[0].PrepareOrder(order));
            }
            else
            {
                RunningTasks.Add(worker.PrepareOrder(order));
            }
        }

        public void DeliverOrder(Order order)
        {
            DeliveredOrderIds.Add(order.OrderId);
            OrdersInProgress.Remove(order);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Main_Paint(object sender, PaintEventArgs e)
        {
            //RenderBackground(e.Graphics);
            RenderAssets(e.Graphics);
        }
    }
}
