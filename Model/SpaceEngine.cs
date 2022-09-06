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
        public void InitAssetPoses(float PosX, float PosY)
        {

        }

        public void AddAsset(IAsset asset) 
        {
            assets.Add(asset);
        }

        public void AddAssets(List<IAsset> assets)
        {
            foreach(IAsset asset in assets)
            {
                AddAsset(asset);
            }
        }
        public void AddSpace(ISpace space, float PosX, float PosY)
        {
            space.PosX = PosX;
            space.PosY = PosY;
            this.spaces.Add(space);
        }
        public void AddSpace(ISpace space)
        {
            this.spaces.Add(space);
        }
        public void AddSpaces(List<ISpace> spaces)
        {
            foreach(ISpace space in spaces)
            {
                this.spaces.Add(space);
            }            
        }
        public void AddDestination(IAsset asset, ISpace space)
        {
            Movements.Add(asset, space);
        }

        private void MoveAssets()
        {
            ISpace destination;
            foreach (IAsset asset in assets)
            {
                if (!Movements.ContainsKey(asset)) continue;
                destination = Movements[asset];
                if(Math.Abs(asset.PosX - destination.PosX) <= asset.PaceLength && Math.Abs(asset.PosY - destination.PosY) <= asset.PaceLength)
                {
                    asset.CurrentSpace = destination;
                    Movements.Remove(asset);
                    if(asset is Person)
                    {
                        ((Person)asset).IsOnWay = false;
                    }
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


}
