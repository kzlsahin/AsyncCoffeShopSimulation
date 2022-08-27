using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam2_MustafaSenturk.Model
{
    public class Expresso : Product
    {
        public static new string Description = "Expresso coffee";
        public Expresso(string preperedFor, double price) : base(preperedFor, price)
        {
            this._productName = "Expresso";
            this.ProductionTime = TimeSpan.FromSeconds(40);

            this.PreperedFor = preperedFor;
        }
    }

    public class Americano : Product
    {
        public static new string Description = "Americano coffee";
        public Americano(string preperedFor, double price) : base(preperedFor, price)
        {
            this._productName = "Americano";
            this.ProductionTime = TimeSpan.FromSeconds(45);
        }
    }

    public class Cappuccino : Product
    {
        public static new string Description = "Cappuccino coffee";
        public Cappuccino(string preperedFor, double price) : base(preperedFor, price)
        {
            this._productName = "Americano";
            this.ProductionTime = TimeSpan.FromSeconds(55);

        }
    }
    public class Product
    {
        static protected int _lastId = 0;

        protected string _productName = "emptyProduct";
        public string productName { get => _productName; }
        public string PreperedFor { get; set; }
        public static string Description { get; set; } = "base product";
        public int Id { get; set; }
        public TimeSpan ProductionTime { get; set; }

        private double _price;
        public double Price
        {
            get => _price;
            init { _price = value; }
        }

        public List<Additive> additives = new List<Additive>();

        public Product(string preperedFor, double price)
        {
            this.PreperedFor = preperedFor;
            Id = ++_lastId;
            Price = price;
        }

        public void AddAdditive(Additive additive)
        {
            this.additives.Add(additive);
            this._price += additive.Price;
            this.ProductionTime.Add(additive.ProductionTime);
        }

    }

    public class Additive
    {
        public string Name { get; set; }

        public double Price { get; }
        public TimeSpan ProductionTime { get; set; }
        public Additive(string name, TimeSpan productionTime, double price)
        {
            Name = name;
            ProductionTime = productionTime;
            Price = price;
        }
    }
}
