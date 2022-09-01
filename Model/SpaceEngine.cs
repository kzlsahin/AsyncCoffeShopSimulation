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

        private Task mainTask;

        private bool _isRunning = false;


        public void InitAssetPoses(double PosX, double PosY)
        {

        }

        public void AddSpace(ISpace space, double PosX, double PosY)
        {
            space.PosX = PosX;
            space.PosY = PosY;
            this.spaces.Add(space);
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

    }

    public interface ISpace
    {
        public double PosX { get; set; }
        public double PosY { get; set; }
    }

}
