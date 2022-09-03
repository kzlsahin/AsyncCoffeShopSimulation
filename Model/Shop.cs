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
        public List<CheckoutStation> Stations = new List<CheckoutStation>();
        private List<ClientPerson> _waitingClients = new();
        private List<ISpace> _spaces;
        private List<IAsset> assets = new List<IAsset>();
        private List<ClientPerson> clientPeople = new();
        public List<Task> RunningTasks = new();

        public SpaceEngine spaceEngine = new SpaceEngine();
        public Space Entrance { get; } = new Space(250, 450, SpaceStatus.Entrence);

        Main mainForm;
        public DialogBuble Prompter;

        public Shop(Main mainForm)
        {
            this.mainForm = mainForm;

            ShopWorker worker1 = new ShopWorker("Mali", Image.FromFile(@"images\Person.png"), this);
            ShopWorker worker2 = new ShopWorker("Turgut", Image.FromFile(@"images\Person.png"), this);
            ShopWorker worker3 = new ShopWorker("Harun", Image.FromFile(@"images\Person.png"), this);
            ShopWorker worker4 = new ShopWorker("Kemal", Image.FromFile(@"images\Person.png"), this);
            ShopWorker worker5 = new ShopWorker("Mustafa", Image.FromFile(@"images\Person.png"), this);

            CreateClientPerson("Ahmet", Image.FromFile(@"images\Person.png"), this);
            CreateClientPerson("Mesut", Image.FromFile(@"images\Person.png"), this);

            Prompter = new(Properties.PublishProfiles.Resources.DialogBuble_x64, 0, 0);
            assets = new List<IAsset>
            {
                worker1, worker2, worker3, worker4, worker5, Prompter
            };

            mainForm.AddAssetsToScene(assets);

            worker1.PosX = 100;
            worker1.PosY = 300;
            worker2.PosX = 200;
            worker2.PosY = 300;
            worker3.PosX = 240;
            worker3.PosY = 300;
            worker4.PosX = 140;
            worker4.PosY = 300;
            worker5.PosX = 240;
            worker5.PosY = 300;



            CheckoutStation station1 = new(this, 110, 220);
            CheckoutStation station2 = new(this, 230, 220);
            CheckoutStation station3 = new(this, 340, 220);

            Stations.Add(station1);
            Stations.Add(station2);
            Stations.Add(station3);

            spaceEngine.AddSpace(station1);
            spaceEngine.AddSpace(station2);
            spaceEngine.AddSpace(station3);

            _spaces = new()
            {
                new Space(20, 100, SpaceStatus.Free),
                new Space(120, 100, SpaceStatus.Free),
                new Space(220, 100, SpaceStatus.Free),
                new Space(320, 100, SpaceStatus.Free),
                new Space(420, 100, SpaceStatus.Free)
            };


            spaceEngine.AddSpaces(_spaces);

            spaceEngine.AddAsset(worker1);
            spaceEngine.AddAsset(worker2);
            spaceEngine.AddAsset(worker3);
            spaceEngine.AddAsset(worker4);
            spaceEngine.AddAsset(worker5);

            worker1.TakeControlOfCheckoutStation(station1);
            worker2.TakeControlOfCheckoutStation(station2);
            worker3.TakeControlOfCheckoutStation(station3);

            worker4.GoToKitchen();
            worker5.GoToKitchen();

            IdleShopWorkers.Add(worker4);
            IdleShopWorkers.Add(worker5);

            ActivateClients();

            Task.Run( async () =>
            {
                ClientPerson client;
                await Task.Delay(5000);
                worker3.GoToKitchen();
                await Task.Delay(5000);
                worker4.TakeControlOfCheckoutStation();
                await Task.Delay(5000);
                worker1.GoToKitchen();
                client = CreateClientPerson("Melih", Image.FromFile(@"images\Person.png"), this);
                client.Go();
                await Task.Delay(5000);
                worker2.GoToKitchen();
                await Task.Delay(5000);
                worker4.GoToKitchen();
                await Task.Delay(5000);
                worker3.TakeControlOfCheckoutStation();


            }
            );
        }

        private void ActivateClients()
        {
            foreach(ClientPerson client in clientPeople)
            {
                client.Go();
            }
        }
        private ClientPerson CreateClientPerson(string name, Image image, Shop shop)
        {
            ClientPerson client = new(name, image, shop);
            client.SpawnAt(Entrance);
            clientPeople.Add(client);
            assets.Add(client);
            RegisterAsset(client);
            return client;
        }

        private void RegisterAsset(IAsset asset)
        {
            spaceEngine.AddAsset(asset);
            mainForm.AddAssetToScene(asset);
        }
        public void SendAssetToSpace(IAsset asset, ISpace space)
        {
            spaceEngine.AddDestination(asset, space);
            asset.CurrentSpace.Status = SpaceStatus.Free;
            space.Status = SpaceStatus.Reserved;
        }

        private ISpace? GetFreeSpace()
        {
            foreach (ISpace space in _spaces)
            {
                if (space.Status == SpaceStatus.Free) return space;
            }
            return null;
        }
        public bool GoToFreeSpace(IAsset asset)
        {
            ISpace? space = GetFreeSpace();

            if (space == null) return false;

            SendAssetToSpace(asset, space);

            return true;
        }



        public bool AddClientToWaitingClients(ClientPerson client)
        {
            _waitingClients.Add(client);
            return true;
        }

        public CheckoutStation? GetAvailableStationForClients()
        {
            if (AvailableStations.Count == 0) return null;
            foreach(CheckoutStation station in AvailableStations)
            {
                if(station.ClientSpace.Status == SpaceStatus.Free)
                {
                    return station;
                }
            }
            return null;
        }
        public CheckoutStation? GetEmptyStation()
        {
            if (AvailableStations.Count == 0) return null;
            foreach (CheckoutStation station in Stations)
            {
                if (station._shopWorker == null && station.Status == SpaceStatus.Free)
                {
                    return station;
                }
            }
            return null;
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

        public void TimeNext(int counter = 0)
        {
            Prompter.Text = $"tick counter\n{counter}";
            spaceEngine.Next();
        }


    }
}
