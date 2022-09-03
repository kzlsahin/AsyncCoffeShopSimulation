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
            Say($"Tezgahta bekliyorum");
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
                Say("Kasada Bekliyorum");
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
                Say("Kasayı Bıraktım");
                return true;
            }        
            return false;
        }
        public void requestAttention()
        {
            RequestIntention();
        }

        public void GoToKitchen()
        {
            leaveControlOFCheckoutStation();
            if (this.Shop.GoToFreeSpace(this))
            {
                Say("Tezgahta Bekliyorum");
            }
        }

        private CheckoutStation? CheckForEmptyStation()
        {
            CheckoutStation? station = this.Shop.GetEmptyStation();
            return station;

        }
        private void RequestIntention(bool afterUnresolvedAnswer = false)
        {
            int choice;
            string choices;
            string clientName = "noName";

            if (afterUnresolvedAnswer)
            {
                Console.WriteLine("I'm sorry, may you repeat");
            }
            else
            {
                Console.WriteLine("    GREETİNGS! Hi! :))  ");
                Console.WriteLine($"   my name is {this.Name}.\n   How can I help you?");
                Console.WriteLine("   May I have your NAME ?");
                clientName = Console.ReadLine() ?? "noName";
            }

            choices = "\n 1. I'd like to order \n 2. Never mind, thank you. \n";
            choice = Dialogs.RequestEntry(choices, new int[] { 1, 2 });
            switch (choice)
            {
                case 1:
                    TakeOrder(clientName);
                    break;
                case 2:
                    Console.WriteLine("No problem, see you!");
                    break;
                default:
                    RequestIntention(true);
                    break;
            }
        }

        private void TakeOrder(string clientName)
        {
            _order = new Order();
            _order.OwnerName = clientName;
            AskProduct();
            GetPayment();
            HandleOrder();
        }

        private void AskProduct()
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            int choice;
            string choices = "";

            Console.WriteLine("  We have these products.\n   Which one would you like?");

            int counter = 0;
            List<ProductType> productChoices = new();

            foreach (KeyValuePair<ProductType, double> product in Shop.Products.productList)
            {
                choices += $"\n{counter++}. {product.Key} with price {product.Value} \n";
                productChoices.Add(product.Key);
            }
            int[] choiceIndicators = Enumerable.Range(0, counter).ToArray();
            choice = Dialogs.RequestEntry(choices, choiceIndicators);

            Product slectedProduct = Shop.Products.GetProduct(productChoices[choice]);

            _order.Products.Add(slectedProduct);
            ProceedAdditive(slectedProduct);
            CheckoutStation.RegisterNewOrder(_order, this);
        }

        private void ProceedAdditive(Product product)
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            if (_order == null)
            {
                return;
            }
            int choice;
            string choices = "";

            Console.WriteLine("And Additives?");
            Console.WriteLine("Would you like additives?");

            int counter = 0;
            List<AdditiveType> additiveChoices = new();
            foreach (KeyValuePair<AdditiveType, double> additive in Shop.Products.additiveList)
            {
                choices += $"\n {counter++}. {additive.Key} with price {additive.Value} \n";
                additiveChoices.Add(additive.Key);
            }
            int[] choiceIndicators = Enumerable.Range(0, counter).ToArray();
            choice = Dialogs.RequestEntry(choices, choiceIndicators);

            Additive slectedAdditive = Shop.Products.GetAdditive(additiveChoices[choice]);

            product.AddAdditive(slectedAdditive);
        }

        private void GetPayment()
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            if (_order == null)
            {
                return;
            }
            Console.WriteLine($"I'm taking cash. The price is {_order.Price()} ");
            CheckoutStation.PayCheck(_order.Price());
            Console.WriteLine("Thank you! You can wait fo your order there.");
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
            await Task.Delay(2000);
            Say($"order for Me or You is ready to be taken");
            DeliverOrder(order);
            TogleIdleStatus();
            CheckForEmptyStation();
        }
        public async Task PrepareOrder()
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
            await Task.Delay(2000);
            Say($"\n    order is Ruined \n Auuugh !!!!");
            await Task.Delay(4000);
            Say($"\n    order for Me or You is ready to be taken");
            TogleIdleStatus();
            CheckForEmptyStation();
        }

        private void DeliverOrder(Order order)
        {
            Shop.DeliverOrder(order);
            Console.WriteLine($"worker {this.Name} complated order {order.OrderId}");
        }


    }
}
