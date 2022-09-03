using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam2_MustafaSenturk.Model
{
    public class ClientPerson : Person
    {
        Shop Shop { get; set; }

        public ClientPerson(string name, Image image, Shop shop) : base(name, image)
        {
            this.AddDialogBuble(Properties.PublishProfiles.Resources.DialogBuble_x64);
            this.Name = name;
            this.Image = image;
            Shop = shop;
            this.dialogBubble.Text = $"Hi! I'm, {this.Name}";
        }
        public async void Go()
        {
            await this.GoToAvailableStation();
        }
        public async Task GoToAvailableStation()
        {
            await Task.Delay(1000);
            CheckoutStation? station = this.Shop.GetAvailableStationForClients();
            if (station == null)
            {
                await Task.Delay(2000);
                await GoToAvailableStation();
                return;
            }
            this.Shop.SendAssetToSpace(this, station.ClientSpace);
        }
    }
}
