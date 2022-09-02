using Exam2_MustafaSenturk.Data;
using Exam2_MustafaSenturk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam2_MustafaSenturk.Model
{
    public class Shop
    {
        public List<Order> OrdersInProgress = new List<Order>();
        public List<int> DeliveredOrderIds = new List<int>();
        public Products Products = new Products();
        public List<ShopWorker> IdleShopWorkers = new List<ShopWorker>();
        public List<CheckoutStation> AvailableStations = new List<CheckoutStation>();
        public List<CheckoutStation> EmptyStations = new List<CheckoutStation>();
        public List<Task> RunningTasks = new();

        public static SpaceEngine spaceEngine = new SpaceEngine();

        Main mainForm;
        public Shop(Main mainForm)
        {
            this.mainForm = mainForm;

            ShopWorker worker1 = new ShopWorker("Mali", Image.FromFile(@"images\Person.png"), this);
            ShopWorker worker2 = new ShopWorker("Turgut", Image.FromFile(@"images\Person.png"), this);
            ShopWorker worker3 = new ShopWorker("Harun", Image.FromFile(@"images\Person.png"), this);
            ShopWorker worker4 = new ShopWorker("Kemal", Image.FromFile(@"images\Person.png"), this);
            ShopWorker worker5 = new ShopWorker("Mustafa", Image.FromFile(@"images\Person.png"), this);


            List<IAsset> assets = new List<IAsset>
            {
                worker1, worker2, worker3
            };

            mainForm.assets = assets;

            worker1.AddDialogBuble();
            worker2.AddDialogBuble();
            worker3.AddDialogBuble();

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

            CheckoutStation station1 = new(this);
            CheckoutStation station2 = new(this);
            CheckoutStation station3 = new(this);
            CheckoutStation station4 = new(this);

            spaceEngine.AddSpace(station1, 80, 120);
            spaceEngine.AddSpace(station2, 280, 120);
            spaceEngine.AddSpace(station3, 360, 240);
            spaceEngine.AddSpace(station4, 80, 260);

            worker1.TakeControlOfCheckoutStation(station1);
            worker2.TakeControlOfCheckoutStation(station2);
            worker3.TakeControlOfCheckoutStation(station3);
            worker3.TakeControlOfCheckoutStation(station3);

            IdleShopWorkers.Add(worker4);
            IdleShopWorkers.Add(worker5);


            spaceEngine.AddAsset(worker1);
            spaceEngine.AddAsset(worker2);
            spaceEngine.AddAsset(worker3);
            spaceEngine.AddDestination(worker1, station1);
            spaceEngine.AddDestination(worker2, station2);
            spaceEngine.AddDestination(worker3, station3);

            Task.Run(() => worker1.PrepareOrder());
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

        public void TimeNext()
        {
            spaceEngine.Next();
        }

    }
}
