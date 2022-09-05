using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Exam2_MustafaSenturk.Model;
using Exam2_MustafaSenturk.Data;

namespace Exam2_MustafaSenturk.Model
{
    public class ShopWorker : Person
    {
        Shop Shop { get; set; }
        private Order? _order = null;

        CheckoutStation? CheckoutStation = null;

        private bool _isIdle = true;
        public bool IsIdle { get => _isIdle; }

        private void TogleIdleStatus()
        {
            _isIdle = !_isIdle;
            if (_isIdle)
            {
                Shop.IdleShopWorkers.Add(this);
            }
            else
            {
                Shop.IdleShopWorkers.Remove(this);
            }
        }

        public ShopWorker(string name, Image image, Shop shop) : base(name, image)
        {
            this.AddDialogBuble(image = Properties.PublishProfiles.Resources.DialogBuble_x64);
            Shop = shop;
            Say($"My name is {Name}");
        }
        public bool TakeControlOfCheckoutStation(CheckoutStation? station = null)
        {
            if (station == null)
            {
                station = CheckForEmptyStation();
            }
            if(station == null)
            {
                return false;
            }
            
            bool isControlTaken = station.TakeControl(this);

            if (isControlTaken)
            {
                CheckoutStation = station;
                this.Shop.SendAssetToSpace(this, station);
                return true;
            }
            return false;
        }

        public bool leaveControlOFCheckoutStation()
        {
            if (CheckoutStation != null)
            {
                CheckoutStation.LeaveControl(this);
                CheckoutStation = null;
                return true;
            }        
            return false;
        }
        public async Task requestAttention(IAnswerer client)
        {
            TogleIdleStatus();
            await Ask(client, "How Can I Help you?", new string[] { "I want to order." } );

            int choice;
            string choices;
            string clientName = "noName";

            clientName = await Ask(client, "How Can I Help you?", new string[] { ((Person)client).Name });
            Order order = new Order();
            this._order = order;
            this._order.OwnerName = clientName;
            this.CheckoutStation.RegisterNewOrder(order, this);
            await AskProduct(client);
            //await GetPayment(client);
            HandleOrder();
            TogleIdleStatus();
        }

        public void GoToKitchen()
        {
            leaveControlOFCheckoutStation();
            if (this.Shop.GoToFreeSpace(this))
            {
                //Say("Tezgahta Bekliyorum");
            }
        }

        private CheckoutStation? CheckForEmptyStation()
        {
            CheckoutStation? station = this.Shop.GetEmptyStation();
            return station;

        }

        private async Task AskProduct(IAnswerer client)
        {
            if (_order == null || CheckoutStation == null)
            {
                return;
            }
            string choice;

            Say("  We have these products.\n   Which one would you like?");
            await Task.Delay(1000);
            List<string> productChoices = new();
            int counter = 1;
            foreach (KeyValuePair<ProductType, double> product in Shop.Products.productList)
            {
                await Task.Delay(800);
                Say($"\n{counter++}. {product.Key} with price {product.Value} \n");
                productChoices.Add(product.Key.ToString());
            }

            choice = await Ask(client, "what is your choice?", productChoices.ToArray() );

            Product slectedProduct = Shop.Products.GetProduct( (ProductType) Enum.Parse(typeof(ProductType), choice));
            Say($"selected product {choice}");
            await Task.Delay(2000);
            this._order.Products.Add(slectedProduct);
            await ProceedAdditive(client, slectedProduct);
            
        }

        private async Task ProceedAdditive(IAnswerer client, Product product)
        {
            if (_order == null)
            {
                return;
            }
            string choice;

            Say("And Additives?");
            Say("Would you like additives?");

            int counter = 1;
            List<string> additiveChoices = new();
            foreach (KeyValuePair<AdditiveType, double> additive in Shop.Products.additiveList)
            {
                Say($"\n {counter++}. {additive.Key} with price {additive.Value} \n");
                additiveChoices.Add(additive.Key.ToString() );
            }
            int[] choiceIndicators = Enumerable.Range(0, counter).ToArray();

            choice = await Ask(client, "what is your choice?", additiveChoices.ToArray());

            Additive slectedAdditive = Shop.Products.GetAdditive((AdditiveType)Enum.Parse(typeof(AdditiveType), choice));

            product.AddAdditive(slectedAdditive);
        }

        private async Task GetPayment(IAnswerer client)
        {
            if (_order == null)
            {
                return;
            }
            Say($"I'm taking cash. The price is {_order.Price()} ");
            CheckoutStation.PayCheck(_order.Price());
            await Ask(client, "You can wait there.", new string[] { "OK! No problem." } );
        }

        private void HandleOrder()
        {
            if (_order != null)
            {
                Shop.HandleOrder(_order, this);
                _order = null;
            }
        }
        public async Task PrepareOrder(Order order)
        {
            if (CheckoutStation != null)
            {
                leaveControlOFCheckoutStation();
            }
            TogleIdleStatus();
            /*foreach (Product product in order.Products)
            {
                Console.WriteLine($"\n < {this.Name} is preparing {product.productName} > \n  for the order of {order.OwnerName}\n");
                await Task.Delay(product.ProductionTime);
            }*/
            GoToKitchen();
            Say($"I'm working for order of {order.OwnerName}");
            await Task.Delay(12000);
            await DeliverOrder(order);
            TogleIdleStatus();
            CheckoutStation? station = CheckForEmptyStation();
            if(station != null)
            {
                TakeControlOfCheckoutStation(station);
            }
        }
        private async Task DeliverOrder(Order order)
        {
            Shop.DeliverOrder(order);
            Say($"complated order {order.OrderId}");
        }


    }
}
