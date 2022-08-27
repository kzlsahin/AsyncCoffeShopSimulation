using System;
using System.Threading.Tasks;
using Exam2_MustafaSenturk.Model;
using Exam2_MustafaSenturk.Data;

namespace Exam2_MustafaSenturk
{
    class CoffeeShop
    {

        public static List<Order> OrdersInProgress = new List<Order>();
        public static List<int> DeliveredOrderIds = new List<int>();
        public static Products Products = new Products();
        public static List<ShopWorker> IdleShopWorkers = new List<ShopWorker>();
        public static List<CheckoutStation> AvailableStations = new List<CheckoutStation>();
        public static List<CheckoutStation> EmptyStations = new List<CheckoutStation>();
        public static List<Task> RunningTasks = new();

        static async Task Main(string[] args)
        {
            ShopWorker worker1 = new ShopWorker("Mali");
            ShopWorker worker2 = new ShopWorker("Turgut");
            ShopWorker worker3 = new ShopWorker("Harun");
            ShopWorker worker4 = new ShopWorker("Kemal");
            ShopWorker worker5 = new ShopWorker("Mustafa");

            CheckoutStation station1 = new();
            CheckoutStation station2 = new();
            CheckoutStation station3 = new();

            worker1.TakeControlOfCheckoutStation(station1);
            worker2.TakeControlOfCheckoutStation(station2);
            worker3.TakeControlOfCheckoutStation(station3);

            IdleShopWorkers.Add(worker4);
            IdleShopWorkers.Add(worker5);


            await Task.Delay(500);
            RunningTasks.Add(AcceptNewClient());

            Task prompter = PromptStatus(500);

            while (RunningTasks.Count() > 0)
            {
                Task finishedTask = await Task.WhenAny(RunningTasks);
                RunningTasks.Remove(finishedTask);
            }

        }

        private static async Task PromptStatus(int eachMilisecond)
        {
            int orderCount = OrdersInProgress.Count();
            ValueTuple<int, int> curosrPos = Console.GetCursorPosition();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($" == {orderCount} == orders are in progress");
            Console.SetCursorPosition(curosrPos.Item1, curosrPos.Item2);
            await Task.Delay(eachMilisecond);
            PromptStatus(eachMilisecond);

        }
        private static async Task AcceptNewClient()
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            bool isCancel = false;
            bool isOrderTaken = false;
            if (AvailableStations.Count() > 0)
            {
                CheckoutStation activeStation = AvailableStations[0];
                Console.WriteLine("< You found an available station to make your order >\n");

                activeStation._shopWorker.requestAttention();
                isOrderTaken = true;
            }
            if (!isOrderTaken)
            {
                Console.WriteLine("There is not available person to take care of you, you are waiting!");
            }
            if (!isCancel)
            {
                // new client comes each 5 seconds
                await Task.Delay(5000);
                RunningTasks.Add(AcceptNewClient());
            }

        }

        public static void HandleOrder(Order order, ShopWorker worker)
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

        public static void DeliverOrder(Order order)
        {
            DeliveredOrderIds.Add(order.OrderId);
            OrdersInProgress.Remove(order);
        }


    }


}