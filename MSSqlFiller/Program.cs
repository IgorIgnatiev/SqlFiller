using System;
using System.IO;
namespace MSSqlFiller
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] Names = { "Vasia", "Petia", "Max", "Masha", "Igor", "Davud", "Aleksei" };
            Console.WriteLine("Enter file path: ");
            string path = Console.ReadLine();
            string[] readText = File.ReadAllLines(path);
            string tableName ="";
            foreach (string str in readText)
            {
                if(str.Split(' ')[0] == "CREATE" && str.Split(' ')[1] == "TABLE")
                {
                    tableName = str.Split(' ')[2];
                }
            }
            Console.WriteLine(tableName);
        }
    }
}
