using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam2_MustafaSenturk.Model
{
    public class CheckoutStation : ISpace
    {
        Shop Shop { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public Space ClientSpace { get; set; }

        public ShopWorker? _shopWorker = null;

        private double _cashAmount = 0;

        private Order _order = new Order();
        public SpaceStatus Status { get; set; } = SpaceStatus.Free;
        public CheckoutStation(Shop shop, float PosX, float PosY)
        { 
            Shop = shop;
            this.PosX = PosX;
            this.PosY = PosY;
            this.ClientSpace = new Space(PosX, PosY + 100, SpaceStatus.Free);
        }
        public bool TakeControl(ShopWorker shopWorker)
        {
            if(_shopWorker == null)
            {
                this._shopWorker = shopWorker;
                Shop.AvailableStations.Add(this);
                this.Status = SpaceStatus.Reserved;
                return true;
            }
            return false;            
        }

        public bool LeaveControl(ShopWorker shopWorker)
        {
            if (_shopWorker == shopWorker)
            {
                this._shopWorker = null;
                Shop.AvailableStations.Remove(this);
                this.Status = SpaceStatus.Free;
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
