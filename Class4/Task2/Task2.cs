using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

namespace Task2
{
    
    public class Task2
    {
        //Âûâîä îøèáêè â êîíñîëü
        public static void PrintError(string textError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(textError);
            Console.ForegroundColor = ConsoleColor.White;
        }

        //Îøèáêà êîäèðîâêè
        public static void CodingError(string codingName)
        {
            PrintError($"Îøèáêà! Êîäèðîâêà \"{codingName}\" íå íàéäåíà!");
        }

        //Îøèáêà ôàéëà
        public static void FileError(string fileName)
        {
            PrintError($"Îøèáêà! Ôàéëà \"{fileName}\" íå ñóùåñòâóåò!");
        }

        //Ïðîâåðêà ñóùåñòâîâàíèÿ êîäèðîâêè
        public static bool CheckCoding(string codingName, System.Text.EncodingInfo[] allCodings)
        {
            if (!allCodings.Any(code => code.Name == codingName))
            {
                CodingError(codingName);
                return false;
            }

            return true;
        }

        //Ïðîâåðêà ñóùåñòâîâàíèÿ ôàéëà
        public static bool CheckFile(string fileName)
        {
            var fileExists = File.Exists(fileName);

            if (!fileExists)
                FileError(fileName);

            return fileExists;
        }

        public static void Main(string[] args)
        {
            var fileName = args[0];
            var codingIn = args[1];
            var codingOut = args[2];
            var allCodings = Encoding.GetEncodings();

            //Ïðîâåðêà ïðàâèëüíîñòè âõîäíûõ äàííûõ
            if (CheckFile(fileName) &
                CheckCoding(codingIn, allCodings) &
                CheckCoding(codingOut, allCodings))
            {
                string textFile = "";

                //×òåíèå ôàéëà
                using (var reader = new StreamReader(fileName, Encoding.GetEncoding(codingIn)))
                {
                    textFile = reader.ReadToEnd();
                }

                //Çàïèñü â ôàéë
                using (var writer = new StreamWriter(fileName, false, Encoding.GetEncoding(codingOut)))
                {
                    writer.Write(textFile);
                }
            }
        }
    }
}
            

