using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace Exam2_MustafaSenturk.Model
{
    static class Dialogs
    {
        public static void RequestEntry(string message, out double userInput)
        {
            Console.WriteLine(message);
            Console.WriteLine("sayısal bir değer giriniz");

            while (!double.TryParse(Console.ReadLine(), NumberStyles.AllowDecimalPoint, null, out userInput))
            {
                Console.WriteLine("Girdiniz sayısal bir değer değil. sayısal bir değer giriniz (!! ondalık ayraç olarak nokta kulanın !!)");
            }

            Console.WriteLine(userInput);
        }

        public static int RequestEntry(string messege, int[] cases)
        {
            Console.WriteLine(messege);
            Console.WriteLine("sayısal bir değer giriniz (!! ondalık ayraç olarak nokta kulanın !!)");
            int userInput;

            while (!int.TryParse(Console.ReadLine(), out userInput) || !cases.Contains(userInput))
            {
                Console.Write("lütfen");
                foreach (int i in cases) Console.Write(", " + i + " ");
                Console.WriteLine("seçeneklerinden birini giriniz ");
            }

            Console.WriteLine(userInput);
            return userInput;
        }
    }
}
