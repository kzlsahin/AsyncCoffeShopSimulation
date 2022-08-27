using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam2_MustafaSenturk.Model;

namespace Exam2_MustafaSenturk.Data
{
    public class Products
    {
        public Dictionary<ProductType, double> productList = new Dictionary<ProductType, double>();
        public Dictionary<AdditiveType, double> additiveList = new Dictionary<AdditiveType, double>();
        public Products()
        {
            productList.Add(ProductType.Expresso, 25.2);
            productList.Add(ProductType.Americano, 27.40);
            productList.Add(ProductType.Cappuccino, 30.85);

            additiveList.Add(AdditiveType.AdditionalSugar, 1.50);
            additiveList.Add(AdditiveType.AdditionalCaramel, 1.50);
            additiveList.Add(AdditiveType.AdditionalMilk, 1.50);
        }

        public string GetDescriptionOfProduct(ProductType productType)
        {
            string Description = "";

            switch (productType)
            {
                case ProductType.Expresso:
                    Description = Expresso.Description;
                    break;
                case ProductType.Cappuccino:
                    Description = Cappuccino.Description;
                    break;
                case ProductType.Americano:
                    Description = Cappuccino.Description;
                    break;
            }
            return Description;
        }

        public Product GetProduct(ProductType productType, string willBePreparedFor = "")
        {
            Product product;
            switch (productType)
            {
                case ProductType.Expresso:
                    product = new Expresso(willBePreparedFor, productList[productType] );
                    break;
                case ProductType.Cappuccino:
                    product = new Cappuccino(willBePreparedFor, productList[productType]);
                    break;
                case ProductType.Americano:
                    product = new Americano(willBePreparedFor, productList[productType]);
                    break;
                default:
                    throw (new Exception("not available product"));
                    break;
            }
            return product;
        }

        public Additive GetAdditive(AdditiveType additive)
        {
            return new Additive(additive.ToString(), TimeSpan.FromSeconds(15), additiveList[additive]);
        }
    }
    public enum ProductType
    {
        Expresso,
        Americano,
        Cappuccino,
    }

    public enum AdditiveType
    {
        AdditionalSugar,
        AdditionalCaramel,
        AdditionalMilk,
    }
    
}
