using System;
using System.IO;
using System.Text;
using Lexicon.Core.Entity;
using Lexicon.Infrastructure;
using Bus = Lexicon.Business.Business;
using System.Diagnostics;

namespace Lexicon.Console
{
    class Program
    {
        private static Bus _business;
        private static StringHelper _sth = new StringHelper();

        static void Main(string[] args)
        {
            string fileAddress = System.Console.ReadLine();
            string text = System.IO.File.ReadAllText(fileAddress,Encoding.GetEncoding("windows-1254"));
            System.Console.WriteLine("Dosya alındı. Kelimeler alınıyor.");
            string[] words = text.Split(',');
            System.Console.WriteLine(words.Length + " kelime alındı. Anlamlar sorgulanıyor.");
            StringBuilder str = new StringBuilder();

            foreach (string word in words)
            {
                _business = new Bus(new TdkGts { Word = word });
                str.Append(_business.GetDescriptionsAsText());
                str.AppendLine();
                str.AppendLine();
                System.Console.Write('.');
            }
            System.Console.WriteLine("Sorgulama tamamlandı. Dosyaya yazılıyor.");

            string descriptionFile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Description.txt";

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(descriptionFile))
            {
                outputFile.Write(str);
            }

            Process.Start(descriptionFile); 
        }
        
    }
}
