using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam2_MustafaSenturk.Data;
using Exam2_MustafaSenturk.Model;

namespace Exam2_MustafaSenturk.Model
{
    public class Order
    {
        private static int _id = 0;
        public int OrderId { get;}
        public List<Product> Products { get; } = new List<Product>();

        public string OwnerName { get; set; } = "emptyName";
        public Order()
        {
            OrderId = _id++;
        }
        public double Price()
        {
            double price = 0;

            foreach(Product product in Products)
            {
                price += product.Price;
            }
            return price;
        }
    }

}
