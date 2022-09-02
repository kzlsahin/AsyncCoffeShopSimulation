using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exam2_MustafaSenturk
{
    class Program
    {
        public static Main mainForm = new();
        [STAThread]
        static async Task Main(string[] args)
        {            
            ApplicationConfiguration.Initialize();
            Application.Run(mainForm);
        }       
    }
}