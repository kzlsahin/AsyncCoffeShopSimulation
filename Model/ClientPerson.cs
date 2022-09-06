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

        CheckoutStation? CheckoutStation = null;
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
            await Task.Delay(1500);
            var station = await this.GoToAvailableStation();
            this.CheckoutStation = station;
            string answer = String.Empty;
            string[] options = new string[] { "yes!"};
            if (this.CheckoutStation != null)
            {
                answer = await Ask(station._shopWorker, "Hi!\n May I ask", options);

                switch (answer)
                {
                    case "yes!":
                        Say("Thank youu!");
                        StartOrdering(station._shopWorker);
                        break;
                    case "Sorry!":
                        Say(Dialogs.GetRaction(Dialogs.Reactions.Annoyed));
                        break;
                }
            }
        }

        public async void StartOrdering(ShopWorker worker)
        {
            await worker.requestAttention(this);
            //when ordering is finished
            Shop.SendPersonToSpace(this, new Space(this.PosX - 100, this.PosY + 300, SpaceStatus.Free));
            CheckoutStation.ClientSpace.Status = SpaceStatus.Free;
            CheckoutStation = null;
            Say("Thanks !");
        }

        public async Task<CheckoutStation?> GoToAvailableStation()
        {
            await Task.Delay(1000);
            CheckoutStation? station = this.Shop.GetAvailableStationForClients();
            if (station == null)
            {
                return await GoToAvailableStation();
                
            }
            this.Shop.SendPersonToSpace(this, station.ClientSpace);
            return station;
        }
    }
}
