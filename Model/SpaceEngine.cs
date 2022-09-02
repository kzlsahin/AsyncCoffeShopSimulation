using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam2_MustafaSenturk.Model
{
    public class SpaceEngine
    {
        List<IAsset> assets = new();
        List<ISpace> spaces = new();

        public Dictionary<IAsset, ISpace> Movements = new();

        private Task mainTask;

        private bool _isRunning = false;

        int counter = 0;
        public void InitAssetPoses(double PosX, double PosY)
        {

        }

        public void AddAsset(IAsset asset) 
        {
            assets.Add(asset);
        }


        public void AddSpace(ISpace space, double PosX, double PosY)
        {
            space.PosX = PosX;
            space.PosY = PosY;
            this.spaces.Add(space);
        }
        public void AddDestination(IAsset asset, ISpace space)
        {
            Movements.Add(asset, space);
        }

        private void MoveAssets()
        {
            ISpace destination;
            int i = 0;
            foreach (IAsset asset in assets)
            {
                if (!Movements.ContainsKey(asset)) return;
                destination = Movements[asset];
                if(Math.Abs(asset.PosX - destination.PosX) < 2*asset.PaceLength && Math.Abs(asset.PosY - destination.PosY) < 2 * asset.PaceLength)
                {
                    counter++;
                    //Movements.Remove(asset);
                    Movements[asset] = spaces[(counter + i) % spaces.Count()];
                    asset.Move((float)destination.PosX, (float)destination.PosY);
                }
                else
                {
                    asset.Move((float)destination.PosX, (float)destination.PosY);
                }
            }
        }
        public async Task Run()
        {
            mainTask = _mainLoop();
        }

        private async Task _mainLoop()
        {
            while (_isRunning)
            {


                await Task.Delay(500);
            }

        }

        public void Next()
        {
            MoveAssets();
        }
    }

    public interface ISpace
    {
        public double PosX { get; set; }
        public double PosY { get; set; }
    }

}
