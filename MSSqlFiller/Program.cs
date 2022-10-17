using System;
using System.IO;
using System.Linq;
namespace MSSqlFiller
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            string[] Names = { "Vasia", "Petia", "Max", "Masha", "Igor", "Davud", "Aleksei" };
            Console.WriteLine("Enter file path: ");
            string path = Console.ReadLine();
            string[] readText = File.ReadAllLines(path);
            string tableName = "";
            foreach (string str in readText)
            {
                if(str.Split(' ')[0] == "CREATE" && str.Split(' ')[1] == "TABLE")
                {
                    tableName = str.Split(' ')[2];
                }
            }
            if(tableName[tableName.Length - 1] == '(')
            {
                tableName = tableName.Remove(tableName.Length - 1);
            }
            Console.WriteLine(tableName);
            


            string[] colNames;
            string[] typeNames;
            int count = 0;
            int j = 0;
            for (int i = 0; i < readText.Length; i++)
            {
                if (i > 0 && readText[i] != ");" && readText[i] != ")" && readText[i] != "(" && readText[i] != "")
                {
                    count++;
                }
            }
            colNames = new string[count];
            typeNames = new string[count];
            for (int i = 0; i < readText.Length; i++)
            {
                if (i > 0 && readText[i] != "(" && readText[i] != ");" && readText[i] != ")" && readText[i] != "")
                {
                    colNames[j] = readText[i].Split(' ').First();
                    typeNames[j] = readText[i].Split(' ')[1].Split(',')[0];
                    
                    j++;
                }
            }

            
            Console.WriteLine("Enter number of inserts: ");
            int insertCount = int.Parse(Console.ReadLine());
            string[] inserts = new string[insertCount];
            int insertsIndex = 0;

            for (int f = 0; f < insertCount; f++)
            {
                string insertStr = "INSERT INTO " + tableName + "( ";
                foreach (string name in colNames)
                {
                    if (name.ToLower() != "id" && name.ToLower() != "[id]" && name.ToLower() != "\tid" && name.ToLower() != "\t[id]")
                        insertStr = insertStr + name + ", ";
                }
                insertStr = insertStr.Remove(insertStr.Length - 2, 2);
                insertStr += " )VALUES( ";
                for (int i = 1; i < typeNames.Length; i++)
                {
                    if (typeNames[i].ToUpper() == "INT")
                    {
                        insertStr = insertStr + rnd.Next(1, 45) + ", ";
                    }
                    else if (typeNames[i].ToUpper().Split('(')[0] == "VARCHAR" || typeNames[i].ToUpper().Split('(')[0] == "NVARCHAR")
                    {
                        insertStr = insertStr + "\'" + Names[rnd.Next(0, Names.Length - 1)] + "\'" + ", ";
                    }
                    else if (typeNames[i].ToUpper() == "FLOAT")
                    {
                        insertStr = insertStr + rnd.NextDouble().ToString().Replace(',', '.') + ", ";
                    }
                    else if (typeNames[i].ToUpper() == "DATE")
                    {
                        insertStr = insertStr + "\'" + RandomDay(rnd).ToShortDateString() + "\'" + ", ";
                    }
                    else if (typeNames[i].ToUpper() == "BINARY")
                    {
                        insertStr = insertStr + rnd.Next(0, 1) + ", ";
                    }
                    else if (typeNames[i].ToUpper() == "CHAR")
                    {
                        insertStr = insertStr + "\'" + GetLetter(rnd) + "\'" + ", ";
                    }
                    else if (typeNames[i].ToUpper() == "DATETIME" || typeNames[i].ToUpper() == "DATETIME2")
                    {
                        insertStr = insertStr + "\'" + DateTime.Now.AddDays(rnd.Next(1000)).ToString() + "\'" + ", ";
                    }

                }
                insertStr = insertStr.Remove(insertStr.Length - 2, 2);

                insertStr = insertStr + ");";
                inserts[insertsIndex] = insertStr;
                insertsIndex++;
                Console.WriteLine(insertStr);
                
            }
            try
            {
                StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\inserts.txt");
                for(int i = 0; i < inserts.Length;i++)
                {
                    sw.WriteLine(inserts[i]);
                }
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
            Console.WriteLine($"Path to your file: {Environment.CurrentDirectory + "\\inserts.txt"}");
        }
        static DateTime RandomDay(Random rnd)
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rnd.Next(range));
        }
        public static char GetLetter(Random rnd)
        {
            int num = rnd.Next(0, 26);
            char let = (char)('a' + num);
            return let;
        }
    }
}


//C:\Users\User\Documents\SQL Server Management Studio\TestTable.sql