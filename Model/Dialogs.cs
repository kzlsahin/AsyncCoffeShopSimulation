using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace Exam2_MustafaSenturk.Model
{
    static class Dialogs
    {
        public static Random random { get; } = new Random();
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

        public static string GetRaction(Reactions reaction)
        {
            string[] options = new string[0];
            switch (reaction)
            {
                case Reactions.Surprise:
                    options = _surpriseReactions;
                    break;
                    
            }
            if (options.Length == 0) return String.Empty;

            int i = random.Next(options.Length);
            return options[i];
        }

        public enum Reactions
        {
            Surprise,
            Annoyed,
        }

        private static string[] _surpriseReactions = new string[]
        {
            "Oh! really!",
            "No way!",
            "Are you serious?"
        };

        private static string[] _annoyedReactions = new string[]
        {
            "I don't care",
            "Get off!",
            "Hah, so an annoying "
        };
    }
}
