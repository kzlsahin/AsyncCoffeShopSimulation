using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam2_MustafaSenturk.Model
{
    public class CheckoutStation
    {
        private double _cashAmount = 0;
        public ShopWorker? _shopWorker = null;
        private Order _order = new Order();
        public bool TakeControl(ShopWorker shopWorker)
        {
            if(_shopWorker == null)
            {
                this._shopWorker = shopWorker;
                CoffeeShop.AvailableStations.Add(this);
                CoffeeShop.EmptyStations.Remove(this);
                return true;
            }
            return false;            
        }

        public bool LeaveControl(ShopWorker shopWorker)
        {
            if (_shopWorker == shopWorker)
            {
                this._shopWorker = null;
                CoffeeShop.AvailableStations.Remove(this);
                CoffeeShop.EmptyStations.Add(this);
                return true;
            }
            return false;
        }

        public bool RegisterNewOrder(Order order, ShopWorker shopWorker)
        {
            if(shopWorker  == _shopWorker)
            {
                _order = order;
                return true;
            }
            return false;
        }

        public bool PayCheck(double cash)
        {
            if(cash < _order.Price())
            {
                return false;
            }
            _cashAmount += cash - _order.Price();
            return true;
        }

    }
}
